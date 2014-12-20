using System.ComponentModel;
using System.Drawing;

namespace Controllers.Common {
    public partial class SetupDialog {
        private IContainer components = null;

        #region Windows Form Designer generated code



        private void InitializeComponent() {
            this.serialSetup1 = new SerialSetup();
            this.SuspendLayout();
            // 
            // serialSetup1
            // 
            this.serialSetup1.Location = new Point(13, 13);
            this.serialSetup1.Name = "serialSetup1";
            this.serialSetup1.SelectedPorts = null;
            this.serialSetup1.Size = new Size(306, 144);
            this.serialSetup1.TabIndex = 3;
            // 
            // SetupDialog
            // 
            this.BackColor = Color.Transparent;
            this.Controls.Add(this.serialSetup1);
            this.Name = "SetupDialog";
            this.Size = new Size(335, 164);
            this.ResumeLayout(false);

        }

        #endregion

        protected override void Dispose(bool disposing) {
            if (disposing && (this.components != null)) {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private SerialSetup serialSetup1;
    }
}
