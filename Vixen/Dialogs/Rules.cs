using System.Collections.Generic;
using System.Xml.Linq;

using VixenPlus.Annotations;

namespace VixenPlus.Dialogs {
    internal abstract class Rules {
        protected internal const string RuleDataElement = "RuleData";
        protected internal const string RuleAttribute = "Type";

        public abstract string BaseName { get; }
        public abstract string Name { [UsedImplicitly] get; set; }
        public abstract XElement RuleData { get; set; }
        public abstract IEnumerable<string> GenerateNames();
        public abstract string GenerateName(int count);
        public abstract int Iterations { get; }
        public abstract bool IsUnlimited { get; }
    }
}
