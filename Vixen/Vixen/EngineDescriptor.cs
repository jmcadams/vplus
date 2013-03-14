namespace Vixen
{
    using System;

    internal class EngineDescriptor
    {
        private string m_name;
        private System.Type m_type;

        public EngineDescriptor(IEngine2 engineInstance)
        {
            this.m_name = engineInstance.Name;
            this.m_type = engineInstance.GetType();
            engineInstance.Dispose();
        }

        public string Name
        {
            get
            {
                return this.m_name;
            }
        }

        public System.Type Type
        {
            get
            {
                return this.m_type;
            }
        }
    }
}

