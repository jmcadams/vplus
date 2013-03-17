namespace MiniSSC {
	using System;
	using System.Windows.Forms;
	using System.Drawing;
	using System.ComponentModel;
	using System.Collections;

	public partial class Form1 {
		private IContainer components;

		#region Windows Form Designer generated code

		private void InitializeComponent() {
			this.components = new Container();
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Text = "Form1";
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