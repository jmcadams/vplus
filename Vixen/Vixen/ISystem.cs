namespace Vixen
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Windows.Forms;

    public interface ISystem
    {
        int GetExecutingTimerExecutionContextHandle(int executingTimerIndex);
        Form InstantiateForm(ConstructorInfo constructorInfo, params object[] parameters);
        bool InvokeSave(UIBase pluginInstance);
        List<ILoadable> LoadableList(string interfaceName);
        void VerifySequenceHardwarePlugins(EventSequence sequence);

        string[] AudioDevices { get; }

        byte[,] Clipboard { get; set; }

        int ExecutingTimerCount { get; }

        string KnownFileTypesFilter { get; }

        Preference2 UserPreferences { get; }
    }
}

