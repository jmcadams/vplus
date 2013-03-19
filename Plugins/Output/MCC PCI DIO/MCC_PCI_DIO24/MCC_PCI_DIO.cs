namespace MCC_PCI_DIO24
{
    using MccDaq;
    using System;
    using System.Collections.Generic;
    using System.Windows.Forms;
    using System.Xml;
    using Vixen;

    public class MCC_PCI_DIO : IEventDrivenOutputPlugIn, IOutputPlugIn, IHardwarePlugin, IPlugIn, ISetup
    {
        private DigitalPortType[] m_boardPortType;
        private MccBoard[] m_boards;
        private Vixen.HardwareMap[] m_hardwareMap;
        private int[] m_pinBoardIndex;
        private int[] m_pinBoardOffset;
        private int m_pinCount;

        public void Event(byte[] channelValues)
        {
            int index = Math.Min(channelValues.Length, this.m_pinCount);
            while (--index > 0)
            {
                int num2 = this.m_pinBoardIndex[index];
                this.m_boards[num2].DBitOut(this.m_boardPortType[num2], this.m_pinBoardOffset[index], (channelValues[index] > 0) ? ((DigitalLogicState) 1) : ((DigitalLogicState) 0));
            }
        }

        public void Initialize(IExecutable executableObject, SetupData setupData, XmlNode setupNode)
        {
            MccBoard board;
            int num = 0;
            this.m_boards = new MccBoard[GlobalConfig.NumBoards];
            this.m_boardPortType = new DigitalPortType[GlobalConfig.NumBoards];
            int index = 0;
            while (index < GlobalConfig.NumBoards)
            {
                board = new MccBoard(index);
                board.BoardConfig.GetBoardType(out num);
                if (num == 0)
                {
                    break;
                }
                this.m_boards[index] = board;
                this.m_boardPortType[index] = (DigitalPortType)1;
                index++;
            }
            Array.Resize<MccBoard>(ref this.m_boards, index);
            Array.Resize<DigitalPortType>(ref this.m_boardPortType, index);
            this.m_hardwareMap = new Vixen.HardwareMap[index];
            int num3 = 0;
            int num6 = 0;
            Dictionary<string, int> dictionary = new Dictionary<string, int>();
            this.m_pinBoardIndex = new int[0];
            this.m_pinBoardOffset = new int[0];
            for (index = 0; index < this.m_boards.Length; index++)
            {
                Dictionary<string, int> dictionary2;
                string str;
                board = this.m_boards[index];
                if (!dictionary.ContainsKey(board.BoardName))
                {
                    dictionary[board.BoardName] = 0;
                }
                this.m_hardwareMap[index] = new Vixen.HardwareMap(board.BoardName, dictionary[board.BoardName]);
                (dictionary2 = dictionary)[str = board.BoardName] = dictionary2[str] + 1;
                int pinCount = this.m_pinCount;
                board.BoardConfig.GetDiNumDevs(out num3);
                for (int i = 0; i < num3; i++)
                {
                    int num5;
                    board.DioConfig.GetDevType(i, out num5);
                    if (num5 == 10)
                    {
                        this.m_boardPortType[index] = (DigitalPortType)10;
                    }
                    board.DConfigPort((DigitalPortType) num5, DigitalPortDirection.DigitalOut);
                    board.DioConfig.GetNumBits(i, out num6);
                    Array.Resize<int>(ref this.m_pinBoardIndex, this.m_pinBoardIndex.Length + num6);
                    Array.Resize<int>(ref this.m_pinBoardOffset, this.m_pinBoardOffset.Length + num6);
                    for (int j = 0; j < num6; j++)
                    {
                        this.m_pinBoardIndex[j + this.m_pinCount] = index;
                        this.m_pinBoardOffset[j + this.m_pinCount] = (this.m_pinCount + j) - pinCount;
                    }
                    this.m_pinCount += num6;
                }
            }
        }

        public void Setup()
        {
            MessageBox.Show(this.m_pinCount.ToString() + " pins have been setup for output.", "Vixen", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }

        public void Shutdown()
        {
        }

        public void Startup()
        {
        }

        public override string ToString()
        {
            return this.Name;
        }

        public string Author
        {
            get
            {
                return "K.C. Oaks";
            }
        }

        public string Description
        {
            get
            {
                return "Measurement Computing's PCI DIO";
            }
        }

        public Vixen.HardwareMap[] HardwareMap
        {
            get
            {
                return this.m_hardwareMap;
            }
        }

        public string Name
        {
            get
            {
                return "MCC DIO";
            }
        }
    }
}

