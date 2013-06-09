using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using NutcrackerEffects;

namespace VixenEditor
{
    public partial class NutcrackerControlDialog: Form
    {
        public NutcrackerControlDialog()
        {
            InitializeComponent();
            PopulateEffects();
        }


        private void PopulateEffects() {
            var dropDown1 = groupBox1.Controls.Find("neControl1", false)[0];
            var a1 = dropDown1 as NutcrackerEffectControl;
            if (a1 != null) {
                var cb1 = a1.Controls.Find("comboBox1", false)[0];
                var c1 = cb1 as ComboBox;
                if (c1 != null) {
                    c1.Items.Add("Testing");
                }
            }
        }
    }
}
