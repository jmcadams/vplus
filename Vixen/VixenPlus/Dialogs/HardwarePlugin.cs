using System.Collections.Generic;
using System.Xml;

namespace Vixen.Dialogs
{
	internal class HardwarePlugin
	{
		private readonly List<Controller> m_controllers;
		private readonly XmlNode m_dataNode;
		private readonly IOutputPlugIn m_plugin;

		public HardwarePlugin(IOutputPlugIn plugin, XmlNode dataNode)
		{
			m_plugin = plugin;
			m_dataNode = dataNode;
			m_controllers = new List<Controller>();
		}

		public List<Controller> Controllers
		{
			get { return m_controllers; }
		}

		public XmlNode DataNode
		{
			get { return m_dataNode; }
		}

		public IOutputPlugIn Plugin
		{
			get { return m_plugin; }
		}
	}
}