using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Xml;

using AdjustablePreview.Properties;

using VixenPlus;

using VixenPlusCommon;
using VixenPlusCommon.Annotations;

namespace Preview {
    [UsedImplicitly]
    public class AdjustablePreview : IEventDrivenOutputPlugIn {
        private readonly List<Channel> _channels;
        private APPreviewDialog _previewDialog;
        private SetupData _setupData;
        private APSetupDialog _setupDialog;
        private XmlNode _setupNode;
        private int _startChannel;


        public AdjustablePreview() {
            _channels = new List<Channel>();
        }


        public void Event(byte[] channelValues) {
            if (((_previewDialog != null) && !_previewDialog.Disposing) && !_previewDialog.IsDisposed) {
                _previewDialog.UpdateWith(channelValues);
            }
        }


        public void Initialize(IExecutable executableObject, SetupData setupData, XmlNode setupNode) {
            _channels.Clear();
            _channels.AddRange(executableObject.FullChannels);
            _setupData = setupData;
            _setupNode = setupNode;
            if (_setupNode.Attributes != null) {
                _startChannel = Convert.ToInt32(_setupNode.Attributes["from"].Value) - 1;
            }
            setupData.GetBytes(_setupNode, "BackgroundImage", new byte[0]);
        }


        public Control Setup() {
            if (_channels.Count == 0) {
                MessageBox.Show(Resources.NoChannelsInProfile, Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            else {
                _setupDialog = new APSetupDialog(_setupData, _setupNode, _channels, _startChannel);
                _setupDialog.ShowDialog();
                _setupDialog.Dispose();
            }

            return null;
        }


        public void GetSetup() {
        }


        public void CloseSetup() {
        }


        public bool SupportsLiveSetup() {
            return false;
        }


        public void Shutdown() {
            if (_previewDialog != null) {
                if (_previewDialog.InvokeRequired) {
                    _previewDialog.BeginInvoke(new MethodInvoker(_previewDialog.Dispose));
                }
                else {
                    _previewDialog.Dispose();
                }
                _previewDialog = null;
            }
            
            _channels.Clear();
            _setupData = null;
            _setupNode = null;
        }


        public void Startup() {
            if (!_channels.Any()) return;

            var system = (ISystem) Interfaces.Available["ISystem"];
            var constructor = typeof (APPreviewDialog).GetConstructor(new[] {typeof (XmlNode), typeof (List<Channel>), typeof (int)});

            _previewDialog = (APPreviewDialog) system.InstantiateForm(constructor, _setupNode, _channels, _startChannel);
        }


        public override string ToString() {
            return Name;
        }


        public string HardwareMap {
            get { return null; }
        }

        public string Name {
            get { return "Adjustable preview"; }
        }
    }
}
