using System.IO.Ports;
using System.Windows.Forms;

namespace Controllers.Renard {
    public partial class SetupDialog : UserControl {
        public SetupDialog(SerialPort selectedPort, bool holdPort) {
            InitializeComponent();
            SelectedPort = selectedPort;
            checkBoxHoldPort.Checked = holdPort;
        }


        public bool HoldPort {
            get { return checkBoxHoldPort.Checked; }
        }


        public SerialPort SelectedPort {
            get { return serialSetup1.SelectedPort; }
            private set { serialSetup1.SelectedPort = value; }
        }


        public bool ValidateSettings() {
            return serialSetup1.ValidateSettings();
        }
    }
}
