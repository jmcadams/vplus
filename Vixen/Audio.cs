namespace VixenPlus {
    public class Audio {
        public Audio() {}


        public Audio(string name, string filename, int duration) {
            Name = name;
            FileName = filename;
            Duration = duration;
        }


        public int Duration { get; set; }

        public string FileName { get; set; }

        public string Name { get; set; }


        public override string ToString() {
            return Name;
        }
    }
}