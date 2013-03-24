using System.Collections.Generic;
using System.IO;

namespace Vixen
{
	internal class ITriggerImpl : ITrigger
	{
		private readonly ActivateTriggerDelegate m_activateTrigger;
		private readonly IExecution m_executionInterface;
		private readonly Dictionary<string, List<RegisteredResponse>> m_registeredTriggerResponses;
		private Host m_host;

		public ITriggerImpl(Host host)
		{
			m_host = host;
			m_registeredTriggerResponses = new Dictionary<string, List<RegisteredResponse>>();
			m_executionInterface = (IExecution) Interfaces.Available["IExecution"];
			m_activateTrigger = ActivateTrigger;
		}

		public void ActivateTrigger(string interfaceTypeName, int index)
		{
			if (Host.InvokeRequired)
			{
				Host.BeginInvoke(m_activateTrigger, new object[] {interfaceTypeName, index});
			}
			else
			{
				List<RegisteredResponse> list;
				if (m_registeredTriggerResponses.TryGetValue(string.Format("{0}{1}", interfaceTypeName, index), out list))
				{
					foreach (RegisteredResponse response in list)
					{
						m_executionInterface.ExecutePlay(response.EcHandle);
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
			if (!m_registeredTriggerResponses.TryGetValue(key, out list))
			{
				list = new List<RegisteredResponse>();
				m_registeredTriggerResponses[key] = list;
			}
			int ecHandle = m_executionInterface.RequestContext(true, false, null);
			list.Add(new RegisteredResponse(interfaceTypeName, index, ecHandle));
			m_executionInterface.SetSynchronousContext(ecHandle, sequence);
			return ecHandle;
		}

		public void ShowRegistrations()
		{
			var dialog = new TriggerResponseRegistrationsDialog(m_registeredTriggerResponses);
			dialog.ShowDialog();
			dialog.Dispose();
		}

		public void UnregisterResponse(string interfaceTypeName, int index, int executionContextHandle)
		{
			List<RegisteredResponse> list;
			string key = string.Format("{0}{1}", interfaceTypeName, index);
			if (m_registeredTriggerResponses.TryGetValue(key, out list))
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
			m_executionInterface.ReleaseContext(executionContextHandle);
		}

		private delegate void ActivateTriggerDelegate(string interfaceTypename, int index);
	}
}