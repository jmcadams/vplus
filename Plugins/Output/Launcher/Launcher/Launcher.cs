using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using System.Xml;

using CommonControls;

using VixenPlus;

namespace Launcher {
    public class Launcher : IEventDrivenOutputPlugIn {
        private XmlNode _setupNode;
        private readonly Dictionary<byte, string[]> _targets = new Dictionary<byte, string[]>();


        public void Event(byte[] channelValues) {
            foreach (var channelValue in channelValues) {
                string[] strArray;
                if (!_targets.TryGetValue(channelValue, out strArray)) {
                    continue;
                }

                var process = new Process {StartInfo = new ProcessStartInfo(strArray[0], strArray[1])};
                process.Start();
            }
        }


        public void Initialize(IExecutable executableObject, SetupData setupData, XmlNode setupNode) {
            _setupNode = setupNode;
            if (_setupNode.SelectSingleNode("Programs") == null) {
                Xml.GetNodeAlways(_setupNode, "Programs");
            }
        }


        public void Setup() {
            var programList = _setupNode.SelectNodes("Programs/Program");
            if (programList == null) {
                return;
            }

            var num = 0;
            var programs = new string[programList.Count][];
            foreach (XmlNode node in programList) {
                if (node.Attributes == null) {
                    continue;
                }

                programs[num++] = new[]
                {node.InnerText, node.Attributes["params"].Value, node.Attributes["trigger"].Value};
            }
            var dialog = new SetupDialog(programs);
            if (dialog.ShowDialog() != DialogResult.OK) {
                return;
            }

            var contextNode = _setupNode.SelectSingleNode("Programs");
            if (contextNode == null) {
                return;
            }
            contextNode.RemoveAll();
            foreach (var parameters in dialog.Programs) {
                var programNode = Xml.SetNewValue(contextNode, "Program", parameters[0]);
                Xml.SetAttribute(programNode, "params", parameters[1]);
                Xml.SetAttribute(programNode, "trigger", parameters[2]);
            }
        }


        public void Shutdown() {}


        public void Startup() {
            _targets.Clear();
            var programsNode = _setupNode.SelectNodes("Programs/Program");
            if (programsNode == null) {
                return;
            }

            foreach (XmlNode programNode in programsNode) {
                if (programNode.Attributes == null) {
                    continue;
                }
                var num = (byte) Convert.ToSingle(programNode.Attributes["trigger"].Value).ToValue();
                if (File.Exists(programNode.InnerText)) {
                    _targets[num] = new[] {programNode.InnerText, programNode.Attributes["params"].Value};
                }
            }
        }


        public override string ToString() {
            return Name;
        }


        public string Author {
            get { return "Vixen and VixenPlus Developers"; }
        }

        public string Description {
            get { return "External program launcher"; }
        }

        public HardwareMap[] HardwareMap {
            get { return new HardwareMap[0]; }
        }

        public string Name {
            get { return "Launcher"; }
        }
    }
}
