namespace RemoteClient
{
    using System;
    using System.Windows.Forms;
    using System.Xml;
    using Vixen;

    public class RemoteClient : IAddIn, ILoadable, IPlugIn
    {
        private bool m_allowLocal = false;
        private bool m_allowRemote = false;
        private bool m_canBeUsed = true;
        private static ControlClient m_controlClient = null;
        private XmlNode m_dataNode;
        private static ExecutionClient m_executionClient = null;
        private static LocalClient m_localClient = null;

        public bool Execute(EventSequence sequence)
        {
            if (!this.m_canBeUsed)
            {
                MessageBox.Show("The application is not providing the necessary data for the remote client.  The remote client cannot be used.", "Remote Client", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return false;
            }
            ClientUI tui = new ClientUI(m_controlClient, m_executionClient, m_localClient, this.m_dataNode);
            tui.ShowDialog();
            Xml.SetValue(this.m_dataNode, "AllowLocal", tui.AllowLocal.ToString());
            Xml.SetValue(this.m_dataNode, "AllowRemote", tui.AllowRemote.ToString());
            this.m_allowLocal = tui.AllowLocal;
            this.m_allowRemote = tui.AllowRemote;
            tui.Dispose();
            return false;
        }

        public void Loading(XmlNode dataNode)
        {
            object obj2;
            this.m_dataNode = dataNode;
            IExecution execution = null;
            if (Interfaces.Available.TryGetValue("IExecution", out obj2))
            {
                execution = (IExecution) obj2;
            }
            else
            {
                this.m_canBeUsed = false;
                throw new Exception("No execution interface available, the remote client cannot be used.");
            }
            Preference2 userPreferences = null;
            if (Interfaces.Available.TryGetValue("ISystem", out obj2))
            {
                userPreferences = ((ISystem) obj2).UserPreferences;
            }
            else
            {
                this.m_canBeUsed = false;
                throw new Exception("No preferences available, the remote client cannot be used.");
            }
            bool result = false;
            bool.TryParse(Xml.GetNodeAlways(this.m_dataNode, "AllowLocal", bool.FalseString).InnerText, out result);
            this.m_allowLocal = result;
            result = false;
            bool.TryParse(Xml.GetNodeAlways(this.m_dataNode, "AllowRemote", bool.FalseString).InnerText, out result);
            this.m_allowRemote = result;
            string innerText = Xml.GetNodeAlways(this.m_dataNode, "Server").InnerText;
            if (m_controlClient == null)
            {
                m_controlClient = new ControlClient();
                m_controlClient.Server = innerText;
            }
            if (m_localClient == null)
            {
                m_localClient = new LocalClient();
            }
            if (this.m_allowLocal && !m_localClient.Start())
            {
                throw new Exception("Failure trying to start the local client.");
            }
            if (m_executionClient == null)
            {
                m_executionClient = new ExecutionClient();
                m_executionClient.Server = innerText;
            }
            if (this.m_allowRemote && !m_executionClient.Start())
            {
                throw new Exception("Failure trying to start the remote execution client.\nIt may be trying to contact a non-existent server.");
            }
        }

        public void Unloading()
        {
            if (m_executionClient != null)
            {
                m_executionClient.Stop();
            }
            m_executionClient = null;
            if (m_localClient != null)
            {
                m_localClient.Stop();
            }
            m_localClient = null;
        }

        public string Author
        {
            get
            {
                return "Vixen Developers";
            }
        }

        public LoadableDataLocation DataLocationPreference
        {
            get
            {
                return LoadableDataLocation.Application;
            }
        }

        public string Description
        {
            get
            {
                return "Remote client for LAN and Internet connectivity";
            }
        }

        public string Name
        {
            get
            {
                return "Remote client";
            }
        }
    }
}

