using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using RSoft.MacroPad.BLL;
using RSoft.MacroPad.BLL.Infrasturture;
using RSoft.MacroPad.BLL.Infrasturture.Configuration;
using RSoft.MacroPad.BLL.Infrasturture.Model;
using RSoft.MacroPad.BLL.Infrasturture.Physical;
using RSoft.MacroPad.BLL.Infrasturture.Protocol;
using RSoft.MacroPad.BLL.Infrasturture.Protocol.Mappers;
using RSoft.MacroPad.BLL.Infrasturture.Protocol.WebHub;
using RSoft.MacroPad.BLL.Infrasturture.UsbDevice;
using RSoft.MacroPad.Infrastructure;
using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.UI.Input.KeyboardAndMouse;

namespace RSoft.MacroPad.Forms
{
    public partial class MainForm : Form
    {
        private KeyboardLayout[] _layouts;
        private LayoutParser _parser = new LayoutParser();
        private IUsb _usb = new HidLibUsb();
        private ConfigurationReader _configReader = new ConfigurationReader();
        private ComposerRepository _composerRepository = new ComposerRepository();

        private static readonly string[] WebHubLedModes = { "Off", "Solid", "Breathing", "On keypress", "Tide" };
        private Label _lblHint;
        private Label _lblSelected;
        private Label _lblOnKeypad;
        private WebHubDeviceState _deviceState;


        public MainForm()
        {
            InitializeComponent();

            InitializeGuidance();
            InitializeLayouts();
            InitializeUsb();
        }

        private void InitializeGuidance()
        {
            tsSend.DropDownItems.Clear();
            tsSend.AutoSize = true;
            tsSend.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;
            tsSend.Text = "Upload to keypad";
            tsSend.ToolTipText = "Write the selected key's setting to the keypad (Ctrl+S)";

            tsLayout.AutoSize = true;
            tsLayout.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;
            tsLayout.Text = "Layout";

            tsAbout.AutoSize = true;
            tsAbout.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;
            tsAbout.Text = "About";

            tsSetParams.Text = "Report ID...";
            tsSetParams.ToolTipText = "Try a different report id if the keypad does not react to uploads";
            menuStrip1.Items.Add(tsSetParams);

            _lblHint = new Label
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                ForeColor = SystemColors.GrayText,
                Padding = new Padding(0, 4, 0, 4),
                Text = "1. Click a button in the picture, or a knob: its centre is the press, its left and right sides are the turns.\n" +
                       "2. Under Key setup, choose what it should do: record a shortcut, pick a media key, or set the LEDs.\n" +
                       "3. Click \"Upload to keypad\"."
            };
            _lblSelected = new Label
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                Font = new Font(Font, FontStyle.Bold),
                Padding = new Padding(0, 4, 0, 4),
            };
            _lblOnKeypad = new Label
            {
                Dock = DockStyle.Right,
                AutoSize = true,
                Padding = new Padding(12, 4, 12, 4),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = SystemColors.Window,
                Visible = false,
            };
            groupBox1.Controls.Add(_lblOnKeypad);
            groupBox1.Controls.Add(_lblSelected);
            groupBox1.Controls.Add(_lblHint);

            keyboardVisual1.FunctionSelected += (s, e) => UpdateSelectionText();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.Control | Keys.S) && tsSend.Enabled)
            {
                tsSend_Click(this, EventArgs.Empty);
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private static string ActionName(InputAction action, KeyboardLayout layout)
        {
            if (action >= InputAction.Key1 && action <= InputAction.Key12)
                return $"Button {(byte)action}";
            if (action < InputAction.Knob1Left || action > InputAction.Knob3Right)
                return action.ToString();

            var knob = ((byte)action - (byte)InputAction.Knob1Left) / 3 + 1;
            var part = ((byte)action - (byte)InputAction.Knob1Left) % 3;
            var knobCount = layout?.Controls?.Count(c => c.Actions.Any(a => a >= InputAction.Knob1Left)) ?? 0;
            var name = knobCount > 1 ? $"Knob {knob}" : "Knob";
            return name + (part == 0 ? " turn left" : part == 1 ? " press" : " turn right");
        }

        private IEnumerable<InputAction> LayoutActions()
            => keyboardVisual1.KeyboardLayout?.Controls?.SelectMany(c => c.Actions) ?? Enumerable.Empty<InputAction>();

        private void UpdateSelectionText()
        {
            var action = keyboardVisual1.SelectedAction;
            if (action == InputAction.None)
            {
                _lblSelected.Text = "Nothing selected";
                return;
            }
            var text = $"Selected: {ActionName(action, keyboardVisual1.KeyboardLayout)}";
            if (_deviceState != null && _deviceState.Mappings.TryGetValue(action, out var current))
                text += $"  (on the keypad now: {current})";
            _lblSelected.Text = text;
        }

        private void RefreshDeviceState()
        {
            _deviceState = _usb.ProtocolType == ProtocolType.WebHub
                ? WebHubDeviceState.Read(_usb, keyboardVisual1.Layer)
                : null;

            if (_deviceState == null)
            {
                _lblOnKeypad.Text = "Could not read the keypad's current settings.\nClose other keypad tools and reconnect it.";
                _lblOnKeypad.Visible = _usb.ProtocolType == ProtocolType.WebHub;
            }
            else
            {
                var layout = keyboardVisual1.KeyboardLayout;
                var parts = LayoutActions()
                    .Where(a => _deviceState.Mappings.ContainsKey(a))
                    .Select(a => $"{ActionName(a, layout)}: {_deviceState.Mappings[a]}")
                    .ToList();
                if (_deviceState.Backlight != null)
                    parts.Add($"LEDs: {_deviceState.Backlight}");
                _lblOnKeypad.Text = "On the keypad now:\n" + string.Join("\n", parts);
                _lblOnKeypad.Visible = true;
            }
            UpdateSelectionText();
        }


        #region Init
        private void InitializeUsb()
        {
            var config = _configReader.Read("config.txt");
            if (config != null)
                _usb.SupportedDevices = config.SupportedDevices;

            _usb.OnConnected += (s, e) =>
            {
                var layout = _layouts.FirstOrDefault(l => l.Products.Any(p => p.VendorId == _usb.VendorId && p.ProductId == _usb.ProductId));

                if (layout != null)
                {
                    keyboardVisual1.KeyboardLayout = layout;
                    keyboardFunction1.KeyboardLayout = layout;
                }

                lblCommStatus.Text = $"Connected: ({_usb.VendorId}:{_usb.ProductId}) Protocol: {_usb.ProtocolType}.id{_usb.Version}";

                var webHub = _usb.ProtocolType == ProtocolType.WebHub;
                tsSetParams.Visible = !webHub;
                keyboardFunction1.SetLedModeNames(webHub ? WebHubLedModes : null);
                keyboardFunction1.SetWebHubFeatures(webHub);
                RefreshDeviceState();

                ShowDisclaimerIfNeeded();
            };
        }


        private void ShowDisclaimerIfNeeded()
        {
            if (!TestedProducts.IsTested(_usb.VendorId, _usb.ProductId))
            {
                new DisclaimerForm().ShowDialog();
            }
        }

        private void SetUsbStatus(bool connected)
        {
            lblStatus.Text = connected ? "Connected" : "Disconnected";
            lblStatus.BackColor = connected ? Color.FromArgb(0, 128, 0) : Color.FromArgb(128, 0, 0);
            tsSend.Enabled = connected;
        }

        private void InitializeLayouts()
        {
            _layouts = _parser.Parse("layouts.txt");
            ((ToolStripDropDownMenu)tsLayout.DropDown).ShowImageMargin = false;
            tsLayout.DropDownItems.Clear();
            tsLayout.DropDownItems.AddRange(_layouts.Select(l =>
            {
                var result = new ToolStripMenuItem()
                {
                    Text = l.Name,
                    AutoSize = true,
                    Tag = l
                };

                result.Click += (s, e) =>
                {
                    StopRecording(s, e);
                    keyboardVisual1.KeyboardLayout = l;
                    keyboardFunction1.KeyboardLayout = l;
                };

                return result;

            }).ToArray());
        }

        private void Tick(object sender, EventArgs e)
        {
            SetUsbStatus(_usb.Connect());
        }

        private void StopRecording(object sender, EventArgs e)
        {
            keyboardFunction1.StopRecording();
        }

        #endregion;  

        private void tsSend_Click(object sender, EventArgs e)
        {
            StopRecording(sender, e);
            if (keyboardVisual1.SelectedAction == InputAction.None)
            {
                MessageBox.Show("Please select a key or knob action to map!");
                return;
            }
            var composer = _composerRepository.Get(_usb.ProtocolType, _usb.Version);

            IEnumerable<Report> reports = Enumerable.Empty<Report>();
            switch (keyboardFunction1.Function)
            {
                case Model.SetFunction.LED:
                    reports = composer is WebHubReportComposer webHubLed
                        ? webHubLed.Led(keyboardVisual1.Layer, keyboardFunction1.LedMode, keyboardFunction1.LedColor, keyboardFunction1.LedBrightness, keyboardFunction1.LedSpeed)
                        : composer.Led(keyboardVisual1.Layer, keyboardFunction1.LedMode, keyboardFunction1.LedColor);
                    break;
                case Model.SetFunction.KeypadLed:
                    if (composer is WebHubReportComposer webHubKeypad)
                        reports = webHubKeypad.KeypadLed(keyboardVisual1.SelectedAction, keyboardVisual1.Layer, keyboardFunction1.KeypadLedFunction);
                    break;
                case Model.SetFunction.KeySequence:
                    var currentLayout = PInvoke.GetKeyboardLayout(0);
                    var enUsLayout = PInvoke.LoadKeyboardLayout("00000409", ACTIVATE_KEYBOARD_LAYOUT_FLAGS.KLF_ACTIVATE);

                    reports = composer.Key(keyboardVisual1.SelectedAction, keyboardVisual1.Layer, keyboardFunction1.Delay,
                        keyboardFunction1.KeySequence.Select(s => (
                        KeyCodeMapper.Map((VirtualKey)PInvoke.MapVirtualKeyEx((uint)s.ScanCode, MAP_VIRTUAL_KEY_TYPE.MAPVK_VSC_TO_VK, enUsLayout)),
                        ModifierMapper.Map(s.ShiftL, s.ShiftR, s.AltL, s.AltR, s.CtrlL, s.CtrlR, s.WinL, s.WinR))));
                    PInvoke.ActivateKeyboardLayout(currentLayout, ACTIVATE_KEYBOARD_LAYOUT_FLAGS.KLF_ACTIVATE);
                    break;
                case Model.SetFunction.MediaKey:
                    reports = composer.Media(keyboardVisual1.SelectedAction, keyboardVisual1.Layer, MediaKeyMapper.Map((VirtualKey)keyboardFunction1.MediaKey));
                    break;
                case Model.SetFunction.Mouse:
                    reports = composer.Mouse(keyboardVisual1.SelectedAction, keyboardVisual1.Layer, keyboardFunction1.MouseButton, keyboardFunction1.MouseModifier, keyboardFunction1.MouseScrollAmount);
                    break;
            }
            if (!reports.Any())
            {
                lblCommStatus.Text = $"This function is not supported by the connected keypad [{DateTime.Now.ToString("T")}]";
                return;
            }

            bool success = true;
            HidLog.ClearLog();
            foreach (var report in reports)
            {
                if (!_usb.Write(report))
                {
                    success = false;
                    break;
                }
            }
            lblCommStatus.Text = success
                ? "Writing successful"
                : "Write failed";

            if (success && _usb.ProtocolType == ProtocolType.WebHub)
            {
                RefreshDeviceState();
                if (keyboardFunction1.Function == Model.SetFunction.LED)
                    lblCommStatus.Text = $"Saved to keypad. LEDs: {_deviceState?.Backlight ?? "could not read back"}";
                else if (_deviceState != null && _deviceState.Mappings.TryGetValue(keyboardVisual1.SelectedAction, out var now))
                    lblCommStatus.Text = $"Saved to keypad. {ActionName(keyboardVisual1.SelectedAction, keyboardVisual1.KeyboardLayout)} now does: {now}";
            }
            lblCommStatus.Text += $" [{DateTime.Now.ToString("T")}]";
        }

        private void tsAbout_Click(object sender, EventArgs e)
        {
            StopRecording(sender, e);
            var aboutBox = new AboutBox();
            aboutBox.ShowDialog();
        }

        private void tsSetParams_Click(object sender, EventArgs e)
        {
            var f = new ConnectionForm(_usb);
            f.ShowDialog();
        }
    }
}
