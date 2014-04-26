using System.IO.Ports;
using System.Windows.Forms;

namespace Controllers.Common {
    public partial class SetupDialog : UserControl {
        public SetupDialog() {
            InitializeComponent();
        }


        public SerialPort SelectedPort {
            get { return serialSetup1.SelectedPorts; }
            set { serialSetup1.SelectedPorts = value; }
        }


        public bool ValidateSettings() {
            return serialSetup1.ValidateSettings();
        }
    }
}
