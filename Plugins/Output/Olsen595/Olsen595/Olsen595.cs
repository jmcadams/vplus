using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Xml;
using VixenPlus;

namespace Olsen595 {
    // ReSharper disable once UnusedMember.Global
    public class Olsen595 : IEventDrivenOutputPlugIn {
        private readonly PortMapping[] _portMappings = {new PortMapping(1), new PortMapping(2), new PortMapping(3)};
        private XmlNode _setupNode;
        
        [DllImport("inpout32", EntryPoint = "Out32")]
        private static extern void Out(short port, short data);

        public void Initialize(IExecutable executableObject, SetupData setupData, XmlNode setupNode) {
            _setupNode = setupNode;

            InitNodes();
            InitPorts();
        }


        private void InitNodes() {
            InitNode("parallel1");
            InitNode("parallel2");
            InitNode("parallel3");
        }


        private void InitNode(string nodeName) {
            if (_setupNode.SelectSingleNode(nodeName) != null) {
                return;
            }

            Xml.SetAttribute(_setupNode, nodeName, "from", "0");
            Xml.SetAttribute(_setupNode, nodeName, "to", "0");
        }


        private void InitPorts() {
            InitPort(0, "parallel1");
            InitPort(1, "parallel2");
            InitPort(2, "parallel3");
        }


        private void InitPort(int i, string nodeName) {
            var node = _setupNode.SelectSingleNode(nodeName);
            if (node == null || node.Attributes == null) {
                _portMappings[i].From = 0;
                _portMappings[i].To = 0;
                _portMappings[i].Mapped = false;
                return;
            }

            var fromChannel = Convert.ToInt32(node.Attributes["from"].Value);
            _portMappings[i].From = fromChannel;

            var toChannel = Convert.ToInt32(node.Attributes["to"].Value);
            _portMappings[i].To = toChannel;

            _portMappings[i].Mapped = (fromChannel != 0 && toChannel != 0);
        }


        public void Event(byte[] channelValues) {
            foreach (var portMap in _portMappings) {
                if (!portMap.Mapped) {
                    continue;
                }

                var fromChannel = portMap.From - 1;
                var toChannel = Math.Min(channelValues.Length, portMap.To) - 1;
                for (var i = toChannel; i >= fromChannel; i--) {
                    Out(portMap.DataPort, (channelValues[i] > 0) ? ((short) 1) : ((short) 0));
                    Out(portMap.ControlPort, 2);
                    Out(portMap.ControlPort, 3);
                }
                Out(portMap.ControlPort, 1);
                Out(portMap.ControlPort, 3);
            }
        }



        public void Setup() {
            var dialog = new SetupDialog(_setupNode);
            if (dialog.ShowDialog() == DialogResult.OK) {
                InitPorts();
            }
        }


        public void Shutdown() {}

        public void Startup() {}


        public override string ToString() {
            return Name;
        }


        public string Author {
            get {
                return "Vixen and VixenPlus Developers";
            }
        }

        public string Description {
            get {
                return "3 port Olsen 595 plugin for Vixen+";
            }
        }

        public HardwareMap[] HardwareMap {
            get {
                var num = _portMappings.Sum(mapping => mapping.Mapped ? 1 : 0);
                var mapArray = new HardwareMap[num];
                num = 0;
                foreach (var mapping in _portMappings.Where(mapping => mapping.Mapped)) {
                    mapArray[num++] = new HardwareMap("Parallel", mapping.DataPort, "X");
                }
                return mapArray;
            }
        }

        public string Name {
            get {
                return "Olsen 595";
            }
        }
    }
}