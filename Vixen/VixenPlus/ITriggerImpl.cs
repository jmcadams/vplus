namespace Vixen
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.CompilerServices;

    internal class ITriggerImpl : ITrigger
    {
        private ActivateTriggerDelegate m_activateTrigger;
        private IExecution m_executionInterface;
        private Host m_host;
        private Dictionary<string, List<RegisteredResponse>> m_registeredTriggerResponses;

        public ITriggerImpl(Host host)
        {
            this.m_host = host;
            this.m_registeredTriggerResponses = new Dictionary<string, List<RegisteredResponse>>();
            this.m_executionInterface = (IExecution) Interfaces.Available["IExecution"];
            this.m_activateTrigger = new ActivateTriggerDelegate(this.ActivateTrigger);
        }

        public void ActivateTrigger(string interfaceTypeName, int index)
        {
            if (Host.InvokeRequired)
            {
                Host.BeginInvoke(this.m_activateTrigger, new object[] { interfaceTypeName, index });
            }
            else
            {
                List<RegisteredResponse> list;
                if (this.m_registeredTriggerResponses.TryGetValue(string.Format("{0}{1}", interfaceTypeName, index), out list))
                {
                    foreach (RegisteredResponse response in list)
                    {
                        this.m_executionInterface.ExecutePlay(response.EcHandle);
                    }
                }
            }
        }

        public int RegisterResponse(string interfaceTypeName, int index, string responseSequenceFile)
        {
            EventSequence sequence;
            List<RegisteredResponse> list;
            try
            {
                sequence = new EventSequence(Path.Combine(Paths.SequencePath, responseSequenceFile));
            }
            catch
            {
                return 0;
            }
            string key = string.Format("{0}{1}", interfaceTypeName, index);
            if (!this.m_registeredTriggerResponses.TryGetValue(key, out list))
            {
                list = new List<RegisteredResponse>();
                this.m_registeredTriggerResponses[key] = list;
            }
            int ecHandle = this.m_executionInterface.RequestContext(true, false, null);
            list.Add(new RegisteredResponse(interfaceTypeName, index, ecHandle));
            this.m_executionInterface.SetSynchronousContext(ecHandle, sequence);
            return ecHandle;
        }

        public void ShowRegistrations()
        {
            TriggerResponseRegistrationsDialog dialog = new TriggerResponseRegistrationsDialog(this.m_registeredTriggerResponses);
            dialog.ShowDialog();
            dialog.Dispose();
        }

        public void UnregisterResponse(string interfaceTypeName, int index, int executionContextHandle)
        {
            List<RegisteredResponse> list;
            string key = string.Format("{0}{1}", interfaceTypeName, index);
            if (this.m_registeredTriggerResponses.TryGetValue(key, out list))
            {
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i].EcHandle == executionContextHandle)
                    {
                        list.RemoveAt(i);
                        i--;
                    }
                }
            }
            this.m_executionInterface.ReleaseContext(executionContextHandle);
        }

        private delegate void ActivateTriggerDelegate(string interfaceTypename, int index);
    }
}

