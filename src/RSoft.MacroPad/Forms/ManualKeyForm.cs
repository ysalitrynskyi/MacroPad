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
using RSoft.MacroPad.Infrastructure;

namespace RSoft.MacroPad.Forms
{
    public partial class ManualKeyForm : Form
    {
        public VirtualKey keySelected { get; private set; }
        public Modifier modifier = Modifier.None;

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

            listBox1.DataSource = System.Enum.GetValues(typeof(VirtualKey));

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
            foreach (var item in gbModifiers.Controls.As<CheckBox>())
            {
                item.Checked = ((Modifier)item.Tag & modifier) != Modifier.None;
            }
        }

        private void ModifierChanged(object sender, EventArgs e)
        {
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
