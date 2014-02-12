namespace VixenPlus.Dialogs {
    internal class ProfileManagerNumbers : IRules {
        public string BaseName { get { return "Numbers"; } }

        private string _name = string.Empty;

        public string Name {
            get { return _name != string.Empty ? _name : BaseName; }
            set { _name = value; }
        }

        public string Prompt {
            get { return "Numbers configuration"; }
        }

        public bool IsLimited { get; set; }
        public int Start { get; set; }
        public int End { get; set; }
        public int Increment { get; set; }
    }
}
