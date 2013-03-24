using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace VixenPlus
{
	internal static class Engines
	{
		private static EngineDescriptor[] _loadedEngines = new EngineDescriptor[0];
		private static int _selectedIndex = -1;

		public static IEngine2 GetInstance()
		{
			if (_selectedIndex != -1)
			{
				return (IEngine2) Activator.CreateInstance(_loadedEngines[_selectedIndex].Type);
			}
			return null;
		}

		public static void LoadFrom(string path)
		{
			var list = new List<EngineDescriptor>();
			foreach (string str in Directory.GetFiles(path, "*.dll", SearchOption.TopDirectoryOnly))
			{
				try
				{
					Assembly assembly = Assembly.LoadFile(str);
					foreach (Type type in assembly.GetExportedTypes())
					{
						foreach (Type type2 in type.GetInterfaces())
						{
							if (type2.Name == "IEngine")
							{
								var engineInstance = (IEngine2) Activator.CreateInstance(type);
								list.Add(new EngineDescriptor(engineInstance));
							}
						}
					}
				}
				catch
				{
				}
			}
			_loadedEngines = list.ToArray();
		}

		public static void Select(int index)
		{
			if (((index >= 0) && (index < _loadedEngines.Length)) && (_selectedIndex != index))
			{
				_selectedIndex = index;
			}
		}
	}
}