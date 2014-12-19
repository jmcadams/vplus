using System.Reflection;
using System.Windows.Forms;

using VixenPlusCommon;

namespace VixenPlus {
    public interface ISystem
    {
        byte[,] Clipboard { get; set; }
        Preference2 UserPreferences { get; }
        Form InstantiateForm(ConstructorInfo constructorInfo, params object[] parameters);
        void InvokeSave(UIBase pluginInstance);
        void InvokeSaveAs(UIBase pluginInstance);
        void InvokeNew(object sender);

        //void InvokeGroupChange(object data);
        //void VerifySequenceHardwarePlugins(EventSequence sequence);
    }
}