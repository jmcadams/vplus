namespace EZ_8_Output {
	using System.ComponentModel;

	public partial class SetupDialog {
		private IContainer components;

		#region Windows Form Designer generated code

		private void InitializeComponent() {
			this.components = new Container();
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Text = "SetupDialog";
		}
		#endregion

		protected override void Dispose(bool disposing) {
			if (disposing && (this.components != null)) {
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}
	}
}