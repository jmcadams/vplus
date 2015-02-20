using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml.Linq;

namespace VixenPlus.Dialogs {
    internal class ProfileManagerNumbers : Rules {
        private const string IsLimitedElement = "IsLimited";
        private const string StartElement = "Start";
        private const string EndElement = "End";
        private const string IncrementElement = "Increment";
        private const string BounceElement = "Bounce";


        public override string BaseName {
            get { return "Numbers"; }
        }

        private string _name = string.Empty;

        public override string Name {
            get { return _name != string.Empty ? _name : BaseName; }
            set { _name = value; }
        }

        public override XElement RuleData {
            get {
                return new XElement(RuleDataElement, new XAttribute(RuleAttribute, BaseName), new XElement(IsLimitedElement, IsLimited),
                    new XElement(StartElement, Start), new XElement(EndElement, End), new XElement(IncrementElement, Increment),
                    new XElement(BounceElement, Bounce));
            }
            set {
                var xElement = value.Element(IsLimitedElement);
                IsLimited = xElement != null && bool.Parse(xElement.Value);

                xElement = value.Element(StartElement);
                Start = xElement != null ? int.Parse(xElement.Value) : 1;

                xElement = value.Element(EndElement);
                End = xElement != null ? int.Parse(xElement.Value) : 1;

                xElement = value.Element(IncrementElement);
                Increment = xElement != null ? int.Parse(xElement.Value) : 1;

                xElement = value.Element(BounceElement);
                Bounce = xElement != null && bool.Parse(xElement.Value);
            }
        }


        private IEnumerable<string> GenerateNames(int count) {
            var result = new List<string>();
            for (var i = 0; i < count; i++) {
                result.Add(GenerateName(i));
                if (Bounce) CheckBounce();
            }
            return result;
        }


        private int _currentCount;


        private void CheckBounce() {
            _currentCount += Math.Abs(Increment);
            if (_currentCount < Iterations) {
                return;
            }

            _currentCount = 0;
            var hold = End;
            End = Start;
            Start = hold;
            Increment = -Increment;
        }


        public override IEnumerable<string> GenerateNames() {
            return GenerateNames(Iterations);
        }


        public override string GenerateName(int count) {
            return (Start + Increment * count).ToString(CultureInfo.InvariantCulture);
        }


        public override int Iterations {
            get { return IsLimited ? (int) Math.Floor((End - Start) / (Double) Increment) + 1 : -1; }
        }

        public override bool IsUnlimited {
            get { return !IsLimited; }
        }


        public override string GenerateDefaultName() {
            return "Default Number Name";
        }


        public static string Prompt {
            get { return "Numbers configuration"; }
        }

        public bool IsLimited { get; set; }
        public int Start { get; set; }
        public int End { get; set; }
        public int Increment { get; set; }
        public bool Bounce { get; set; }
    }
}