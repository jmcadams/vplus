using System.Collections.Generic;
using System.IO;

namespace VixenPlus
{
    internal class TriggerImpl : ITrigger
    {
        private readonly ActivateTriggerDelegate _activateTriggerDelegate;
        private readonly IExecution _executionInterface;
        private readonly Dictionary<string, List<RegisteredResponse>> _registeredTriggerResponses;

        public TriggerImpl()
        {
            _registeredTriggerResponses = new Dictionary<string, List<RegisteredResponse>>();
            _executionInterface = (IExecution) Interfaces.Available["IExecution"];
            _activateTriggerDelegate = ActivateTrigger;
        }

        public void ActivateTrigger(string interfaceTypeName, int index)
        {
            if (Host.InvokeRequired)
            {
                Host.BeginInvoke(_activateTriggerDelegate, new object[] {interfaceTypeName, index});
            }
            else
            {
                List<RegisteredResponse> list;
                if (_registeredTriggerResponses.TryGetValue(string.Format("{0}{1}", interfaceTypeName, index), out list))
                {
                    foreach (RegisteredResponse response in list)
                    {
                        _executionInterface.ExecutePlay(response.EcHandle);
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
            if (!_registeredTriggerResponses.TryGetValue(key, out list))
            {
                list = new List<RegisteredResponse>();
                _registeredTriggerResponses[key] = list;
            }
            int ecHandle = _executionInterface.RequestContext(true, false, null);
            list.Add(new RegisteredResponse(interfaceTypeName, index, ecHandle));
            _executionInterface.SetSynchronousContext(ecHandle, sequence);
            return ecHandle;
        }

        public void ShowRegistrations()
        {
            var dialog = new TriggerResponseRegistrationsDialog(_registeredTriggerResponses);
            dialog.ShowDialog();
            dialog.Dispose();
        }

        public void UnregisterResponse(string interfaceTypeName, int index, int executionContextHandle)
        {
            List<RegisteredResponse> list;
            string key = string.Format("{0}{1}", interfaceTypeName, index);
            if (_registeredTriggerResponses.TryGetValue(key, out list))
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
            _executionInterface.ReleaseContext(executionContextHandle);
        }

        private delegate void ActivateTriggerDelegate(string interfaceTypename, int index);
    }
}