namespace VixenPlus.Dialogs {
    public class ProfileManagerWords : IRules {
        public string BaseName { get { return "Words"; } }

        private string _name = string.Empty;

        public string Name {
            get { return _name != string.Empty ? _name : BaseName; }
            set { _name = value; }
        }

        public string Prompt {
            get { return "Words (One Per Line)"; }
        }

        public string Words { get; set; }
    }
}