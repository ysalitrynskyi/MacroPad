using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RSoft.MacroPad.BLL.Infrasturture.Model;
using RSoft.MacroPad.BLL.Infrasturture.Protocol.Mappers;
using RSoft.MacroPad.Infrastructure;

namespace RSoft.MacroPad.Forms
{
    public partial class ManualKeyForm : Form
    {
        public VirtualKey keySelected { get; private set; }
        public Modifier modifier = Modifier.None;
        private bool _updating;

        public Modifier Modifier
        {
            get => modifier;
            set
            {
                modifier = value;
                UpdateControls();
            }
        }

        public ManualKeyForm()
        {
            InitializeComponent();

            // Only the keys a keypad can actually send
            listBox1.DataSource = System.Enum.GetValues(typeof(KeyCode)).Cast<KeyCode>()
                .Where(k => k != KeyCode.None)
                .Select(k => k.Map())
                .Distinct()
                .ToList();

            cbShiftL.Tag = Modifier.LeftShift;
            cbShiftR.Tag = Modifier.RightShift;
            cbAltL.Tag = Modifier.LeftAlt;
            cbAltR.Tag = Modifier.RightAlt;
            cbCtrlL.Tag = Modifier.LeftCtrl;
            cbCtrlR.Tag = Modifier.RightCtrl;
            cbWinL.Tag = Modifier.LeftWin;
            cbWinR.Tag = Modifier.RightWin;

            UpdateControls();
        }

        private void UpdateControls()
        {
            _updating = true;
            foreach (var item in gbModifiers.Controls.As<CheckBox>())
            {
                item.Checked = ((Modifier)item.Tag & modifier) != Modifier.None;
            }
            _updating = false;
        }

        private void ModifierChanged(object sender, EventArgs e)
        {
            if (_updating)
                return;
            var result = Modifier.None;

            foreach (var item in gbModifiers.Controls.As<CheckBox>())
            {
                if (item.Checked)
                    result |= (Modifier)item.Tag;
            }

            modifier = result;
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            keySelected = (VirtualKey)listBox1.SelectedItem;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
