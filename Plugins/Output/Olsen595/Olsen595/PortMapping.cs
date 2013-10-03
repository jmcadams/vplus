namespace Olsen595 {
    internal class PortMapping {
        public PortMapping(int parallelPortNumber) {
            From = 0;
            To = 0;
            Mapped = false;

            switch (parallelPortNumber) {
                case 1:
                    DataPort = 0x378;
                    ControlPort = 0x37a;
                    break;

                case 2:
                    DataPort = 0x278;
                    ControlPort = 0x27a;
                    break;

                case 3:
                    DataPort = 0x3bc;
                    ControlPort = 0x3be;
                    break;
            }
        }


        public short ControlPort { get; private set; }

        public short DataPort { get; private set; }

        public int From { get; set; }

        public int To { get; set; }

        public bool Mapped { get; set; }
    }
}

