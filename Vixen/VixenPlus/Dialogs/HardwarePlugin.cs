namespace Vixen.Dialogs
{
    using System;
    using System.Collections.Generic;
    using System.Xml;
    using Vixen;

    internal class HardwarePlugin
    {
        private List<Controller> m_controllers;
        private XmlNode m_dataNode;
        private IOutputPlugIn m_plugin;

        public HardwarePlugin(IOutputPlugIn plugin, XmlNode dataNode)
        {
            this.m_plugin = plugin;
            this.m_dataNode = dataNode;
            this.m_controllers = new List<Controller>();
        }

        public List<Controller> Controllers
        {
            get
            {
                return this.m_controllers;
            }
        }

        public XmlNode DataNode
        {
            get
            {
                return this.m_dataNode;
            }
        }

        public IOutputPlugIn Plugin
        {
            get
            {
                return this.m_plugin;
            }
        }
    }
}

