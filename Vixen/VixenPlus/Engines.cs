namespace Vixen
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;

    internal static class Engines
    {
        private static EngineDescriptor[] m_loadedEngines = new EngineDescriptor[0];
        private static int m_selectedIndex = -1;

        public static IEngine2 GetInstance()
        {
            if (m_selectedIndex != -1)
            {
                return (IEngine2) Activator.CreateInstance(m_loadedEngines[m_selectedIndex].Type);
            }
            return null;
        }

        public static void LoadFrom(string path)
        {
            List<EngineDescriptor> list = new List<EngineDescriptor>();
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
                                IEngine2 engineInstance = (IEngine2) Activator.CreateInstance(type);
                                list.Add(new EngineDescriptor(engineInstance));
                            }
                        }
                    }
                }
                catch
                {
                }
            }
            m_loadedEngines = list.ToArray();
        }

        public static void Select(int index)
        {
            if (((index >= 0) && (index < m_loadedEngines.Length)) && (m_selectedIndex != index))
            {
                m_selectedIndex = index;
            }
        }
    }
}

