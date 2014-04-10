using System;
using System.Collections.Generic;
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
        private PreviewDialog _previewDialog;
        private SetupData _setupData;
        private SetupDialog _setupDialog;
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


        public void Setup() {
            if (_channels.Count == 0) {
                MessageBox.Show(Resources.NoChannelsInProfile, Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            else {
                _setupDialog = new SetupDialog(_setupData, _setupNode, _channels, _startChannel);
                _setupDialog.ShowDialog();
                _setupDialog.Dispose();
            }
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
            if (_channels.Count == 0) {
                return;
            }
            var system = (ISystem) Interfaces.Available["ISystem"];
            var constructor =
                typeof (PreviewDialog).GetConstructor(new[] {typeof (XmlNode), typeof (List<Channel>), typeof (int)});
            _previewDialog = (PreviewDialog) system.InstantiateForm(constructor, new object[] {_setupNode, _channels, _startChannel});
        }


        public override string ToString() {
            return Name;
        }

        public HardwareMap[] HardwareMap {
            get { return new HardwareMap[0]; }
        }

        public string Name {
            get { return "Adjustable preview"; }
        }
    }
}
