using System.Xml.Linq;

using VixenPlus.Annotations;

namespace VixenPlus.Dialogs {
    internal abstract class Rules {
        protected internal const string RuleDataElement = "RuleData";
        protected internal const string RuleAttribute = "Type";

        public abstract string BaseName { get; }
        public abstract string Name { [UsedImplicitly] get; set; }
        public abstract XElement RuleData { get; set; }
    }
}
