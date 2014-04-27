namespace Controllers.Common {
    public partial class SetupDialog {
        private System.ComponentModel.IContainer components = null;

        #region Windows Form Designer generated code



        private void InitializeComponent() {
            this.serialSetup1 = new Controllers.Common.SerialSetup();
            this.SuspendLayout();
            // 
            // serialSetup1
            // 
            this.serialSetup1.Location = new System.Drawing.Point(13, 13);
            this.serialSetup1.Name = "serialSetup1";
            this.serialSetup1.SelectedPorts = null;
            this.serialSetup1.Size = new System.Drawing.Size(306, 144);
            this.serialSetup1.TabIndex = 3;
            // 
            // SetupDialog
            // 
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.serialSetup1);
            this.Name = "SetupDialog";
            this.Size = new System.Drawing.Size(335, 164);
            this.ResumeLayout(false);

        }

        #endregion

        protected override void Dispose(bool disposing) {
            if (disposing && (this.components != null)) {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private Common.SerialSetup serialSetup1;
    }
}
