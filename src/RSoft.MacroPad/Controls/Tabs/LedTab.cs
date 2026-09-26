using System;
using System.Linq;
using System.Windows.Forms;
using RSoft.MacroPad.BLL.Infrasturture.Model;
using RSoft.MacroPad.Infrastructure;

namespace RSoft.MacroPad.Controls.Tabs
{
    public partial class LedTab : UserControl
    {
        private bool colorsSupported;
        private int modeCount = 6;
        private int mode;
        private LedColor color = LedColor.Random;
        private GroupBox gbAdjust;
        private TrackBar tbBrightness;
        private TrackBar tbSpeed;
        private Label lblBrightness;
        private Label lblSpeed;

        /// <summary>
        /// Brightness in percent (5-100)
        /// </summary>
        public byte Brightness => (byte)(tbBrightness.Value * 5);

        /// <summary>
        /// Animation speed, 0-4
        /// </summary>
        public byte Speed => (byte)tbSpeed.Value;

        public bool Adjustable
        {
            get => gbAdjust.Visible;
            set => gbAdjust.Visible = value;
        }

        public int ModeCount
        {
            get => modeCount;
            set
            {
                modeCount = value;
                UpdateControls();
            }
        }
        public bool ColorsSupported
        {
            get => colorsSupported;
            set
            {
                colorsSupported = value;
                UpdateControls();
            }
        }

        public LedMode Mode
        {
            get => (LedMode)mode;
            set
            {
                mode = (int)value;
                UpdateControls();
            }
        }

        public LedColor Color
        {
            get => color;
            set
            {
                color = value;
                UpdateControls();
            }
        }


        /// <summary>
        /// Names the modes the connected keypad uses; null restores the generic "Mode n" labels
        /// </summary>
        public void SetModeNames(string[] names)
        {
            var buttons = new[] { rbMode0, rbMode1, rbMode2, rbMode3, rbMode4, rbMode5 };
            for (var i = 0; i < buttons.Length; i++)
                buttons[i].Text = names != null && i < names.Length ? names[i] : i == 0 ? "Off" : $"Mode {i}";
        }

        public LedTab()
        {
            InitializeComponent();

            rbRed.Tag = LedColor.Red;
            rbOrange.Tag = LedColor.Orange;
            rbYellow.Tag = LedColor.Yellow;
            rbGreen.Tag = LedColor.Green;
            rbCyan.Tag = LedColor.Cyan;
            rbBlue.Tag = LedColor.Blue;
            rbPurple.Tag = LedColor.Purple;
            rbRandom.Tag = LedColor.Random;

            rbMode0.Tag = 0;
            rbMode1.Tag = 1;
            rbMode2.Tag = 2;
            rbMode3.Tag = 3;
            rbMode4.Tag = 4;
            rbMode5.Tag = 5;

            gbAdjust = new GroupBox
            {
                Text = "Brightness and speed",
                Location = new System.Drawing.Point(gbColors.Right + 6, gbColors.Top),
                Size = new System.Drawing.Size(220, gbColors.Height),
                Visible = false,
            };
            lblBrightness = new Label { AutoSize = true, Location = new System.Drawing.Point(6, 18) };
            tbBrightness = new TrackBar { AutoSize = false, Minimum = 1, Maximum = 20, Value = 20, TickFrequency = 2, SmallChange = 1, LargeChange = 4, Location = new System.Drawing.Point(3, 31), Width = 210, Height = 24 };
            lblSpeed = new Label { AutoSize = true, Location = new System.Drawing.Point(6, 60) };
            tbSpeed = new TrackBar { AutoSize = false, Minimum = 0, Maximum = 4, Value = 2, TickFrequency = 1, LargeChange = 1, Location = new System.Drawing.Point(3, 73), Width = 210, Height = 24 };
            tbBrightness.ValueChanged += (s, e) => UpdateAdjustLabels();
            tbSpeed.ValueChanged += (s, e) => UpdateAdjustLabels();
            gbAdjust.Controls.AddRange(new Control[] { lblBrightness, tbBrightness, lblSpeed, tbSpeed });
            Controls.Add(gbAdjust);
            UpdateAdjustLabels();

            UpdateControls();
        }

        private void UpdateAdjustLabels()
        {
            lblBrightness.Text = $"Brightness: {Brightness}% (single colour)";
            lblSpeed.Text = $"Speed: {Speed} (animated effects)";
        }

        private void UpdateControls()
        {
            gbColors.Visible = colorsSupported;

            foreach (var rb in gbModes.Controls.As<RadioButton>())
            {
                rb.Visible = (int)rb.Tag < modeCount;
            }

            var rbMode = gbModes.Controls.As<RadioButton>().FirstOrDefault(r => (int)r.Tag == mode);
            if (rbMode != null) rbMode.Checked = true;

            var rbColor = gbColors.Controls.As<RadioButton>().FirstOrDefault(r => (LedColor)r.Tag == color);
            if(rbColor != null) rbColor.Checked = true;
        }

        public void ModeChanged(object sender, EventArgs e)
        {
            var rbMode = sender as RadioButton;
            mode = (int)rbMode.Tag;
        }

        public void ColorChanged(object sender, EventArgs e)
        {
            var rbColor = sender as RadioButton;
            Color = (LedColor)rbColor.Tag;
        }
    }
}
