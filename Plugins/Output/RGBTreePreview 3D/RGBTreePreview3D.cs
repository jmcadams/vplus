using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Xml;

using VixenPlus;
using VixenPlus.Annotations;

[UsedImplicitly]
public class RGBTreePreview3D : IEventDrivenOutputPlugIn {
    private RGBTreePreviewDialog _previewDialog;
    private XmlNode _setupNode;
    private readonly List<Channel> _channels;


    public RGBTreePreview3D() {
        _channels = new List<Channel>();
    }


    public void Event(byte[] channelValues) {
        if (((_previewDialog != null) && !_previewDialog.Disposing) && !_previewDialog.IsDisposed) {
            _previewDialog.VixenEvent(channelValues);
        }
    }


    public void Initialize(IExecutable executableObject, SetupData setupData, XmlNode setupNode) {
        _channels.Clear();
        _channels.AddRange(executableObject.FullChannels);
        _setupNode = setupNode;

        if (_setupNode.SelectSingleNode("Settings") == null) {
            Xml.GetNodeAlways(_setupNode, "Settings");
        }
    }


    public Control Setup() {
        var mSetupDialog = new RGBTreeSetupDialog(_setupNode);
        if (mSetupDialog.ShowDialog() == DialogResult.OK) {
            mSetupDialog.Dispose();
        }
        mSetupDialog.Close();

        return null;
    }


    public void GetSetup() {}
    public void CloseSetup() {}


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
        _setupNode = null;
    }


    public void Startup() {
        if (!_channels.Any()) {
            return;
        }

        var system = (ISystem) Interfaces.Available["ISystem"];
        var constructor = typeof (RGBTreePreviewDialog).GetConstructor(new[] {typeof (XmlNode), typeof (List<Channel>)});

        _previewDialog = (RGBTreePreviewDialog) system.InstantiateForm(constructor, _setupNode, _channels);
    }


    //public string Author {
    //    get { return "Emmanuel Miranda (nuelemma)"; }
    //}


    public string Name {
        get { return "RGB Mega Tree 3D Preview"; }
    }

    public string HardwareMap {
        get { return null; }
    }


    public override string ToString() {
        return Name;
    }
}