using System;
using System.Xml.Linq;

namespace VixenPlus.Dialogs {
    internal class ProfileManagerWords : Rules {
        private const string WordsElement = "Words";
        private const string XmlCrLf = "&#10;";

        public override string BaseName { get { return "Words"; } }

        private string _name = string.Empty;

        public override string Name {
            get { return _name != string.Empty ? _name : BaseName; }
            set { _name = value; }
        }

        public override XElement RuleData {
            get {
                return new XElement(RuleDataElement, 
                    new XAttribute(RuleAttribute, BaseName),
                    new XElement(WordsElement, Words.Replace(Environment.NewLine, XmlCrLf))
                );
            }
            set {
                var xElement = value.Element(WordsElement);
                Words = (xElement != null) ? xElement.Value.Replace(XmlCrLf, Environment.NewLine) : string.Empty;
            }
        }

        public static string Prompt {
            get { return "Words (One Per Line)"; }
        }

        public string Words { get; set; }
    }
}