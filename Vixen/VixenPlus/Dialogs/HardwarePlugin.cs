using System.Collections.Generic;
using System.Xml;

namespace Vixen.Dialogs
{
	internal class HardwarePlugin
	{
		private readonly List<Controller> _controllers;
		private readonly XmlNode _dataNode;
		private readonly IOutputPlugIn _plugin;

		public HardwarePlugin(IOutputPlugIn plugin, XmlNode dataNode)
		{
			_plugin = plugin;
			_dataNode = dataNode;
			_controllers = new List<Controller>();
		}

		public List<Controller> Controllers
		{
			get { return _controllers; }
		}

		public XmlNode DataNode
		{
			get { return _dataNode; }
		}

		public IOutputPlugIn Plugin
		{
			get { return _plugin; }
		}
	}
}