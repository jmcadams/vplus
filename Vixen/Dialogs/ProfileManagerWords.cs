using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace VixenPlus.Dialogs {
    internal class ProfileManagerWords : Rules {
        private const string WordsElement = "Words";
        private const string XmlCrLf = "&#10;";

        public override string BaseName { get { return "Words"; } }

        private string _name = string.Empty;
        private string _words = String.Empty;
        private IList<string> _wordArray;
 
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

        public override IEnumerable<string> GenerateNames() {
            return _wordArray;
        }

        public override string GenerateName(int index) {
            return _wordArray[index % _wordArray.Count()];
        }

        public override int Iterations {
            get { return _wordArray.Count(); }
        }

        public override bool IsUnlimited {
            get { return false; }
        }

        public override string GenerateDefaultName() {
            return "Default Word Name";
        }


        public static string Prompt {
            get { return "Words (One Per Line)"; }
        }

        public string Words {
            get { return _words; }
            set {
                _words = value;
                _wordArray = value.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            }
        }
    }
}