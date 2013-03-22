namespace Vixen.Dialogs {
	using System;
	using System.ComponentModel;
	using System.Drawing;
	using System.Windows.Forms;

	public partial class TextQueryDialog : Form {
		
		public TextQueryDialog(string caption, string query, string response) {
			this.InitializeComponent();
			this.Text = caption;
			this.labelQuery.Text = query;
			this.textBoxResponse.Text = response;
		}
		
		public string Caption {
			get {
				return this.Text;
			}
			set {
				this.Text = value;
			}
		}

		public string Query {
			get {
				return this.labelQuery.Text;
			}
			set {
				this.labelQuery.Text = value;
			}
		}

		public string Response {
			get {
				return this.textBoxResponse.Text;
			}
			set {
				this.textBoxResponse.Text = value;
			}
		}
	}
}

