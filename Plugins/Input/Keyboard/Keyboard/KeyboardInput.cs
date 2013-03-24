namespace Keyboard
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Forms;
    using System.Xml;
    using Vixen;

    public class KeyboardInput : InputPlugin
    {
        private Input[] m_inputs = null;
        private Keyboard.Inputs m_inputsObject;
        private XmlNode m_setupNode;

        private void CreateInputs()
        {
            List<VixenInput> list = new List<VixenInput>();
            foreach (KeyInput input in this.m_inputsObject.ReadAll())
            {
                list.Add(new VixenInput(this, input.Key, input.InputType, input.IsIterator));
            }
            this.m_inputs = list.ToArray();
        }

        public override void Initialize(SetupData setupData, XmlNode setupNode)
        {
            this.m_setupNode = setupNode;
            this.m_inputsObject = new Keyboard.Inputs(this.m_setupNode);
            this.CreateInputs();
        }

        public override void Setup()
        {
            SetupDialog dialog = new SetupDialog(this.m_inputsObject);
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                this.CreateInputs();
            }
        }

        public override string Author
        {
            get
            {
                return "Vixen and VixenPlus Developers";
            }
        }

        public override string Description
        {
            get
            {
                return "Allows the keyboard to be used to create event data";
            }
        }

        public override Vixen.HardwareMap[] HardwareMap
        {
            get
            {
                return new Vixen.HardwareMap[0];
            }
        }

        public override Input[] Inputs
        {
            get
            {
                return this.m_inputs;
            }
        }

        public override string Name
        {
            get
            {
                return "Keyboard";
            }
        }
    }
}

