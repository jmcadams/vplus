namespace JoystickInput
{
    using JoystickManager;
    using System;
    using System.Collections.Generic;
    using System.Windows.Forms;
    using System.Xml;
    using Vixen;

    public class JoystickInput : InputPlugin
    {
        private Input[] m_inputs = null;

        public override void Initialize(Vixen.SetupData setupData, XmlNode setupNode)
        {
            SetupData.LoadFrom(setupNode);
            List<Input> list = new List<Input>();
            JoystickManager.RefreshAttachments();
            foreach (Joystick joystick in JoystickManager.Joysticks)
            {
                foreach (AxisResource resource in joystick.AvailableAxes())
                {
                    list.Add(new JoystickInputResource(this, resource, SetupData.GetIsIterator(resource.Name)));
                }
                foreach (ButtonResource resource2 in joystick.Buttons)
                {
                    list.Add(new JoystickInputResource(this, resource2, SetupData.GetIsIterator(resource2.Name)));
                }
                foreach (POVResource resource3 in joystick.AvailablePOV)
                {
                    if (SetupData.IsQuadDigitalPOV)
                    {
                        list.Add(new JoystickInputResource(this, new DigitalPOVResource(joystick, resource3.Index, DigitalPOVResource.Direction.N), resource3.Name + "_N", SetupData.GetIsIterator(resource3.Name + "_N")));
                        list.Add(new JoystickInputResource(this, new DigitalPOVResource(joystick, resource3.Index, DigitalPOVResource.Direction.S), resource3.Name + "_S", SetupData.GetIsIterator(resource3.Name + "_S")));
                        list.Add(new JoystickInputResource(this, new DigitalPOVResource(joystick, resource3.Index, DigitalPOVResource.Direction.E), resource3.Name + "_E", SetupData.GetIsIterator(resource3.Name + "_E")));
                        list.Add(new JoystickInputResource(this, new DigitalPOVResource(joystick, resource3.Index, DigitalPOVResource.Direction.W), resource3.Name + "_W", SetupData.GetIsIterator(resource3.Name + "_W")));
                    }
                    else
                    {
                        list.Add(new JoystickInputResource(this, resource3, SetupData.GetIsIterator(resource3.Name)));
                    }
                }
            }
            this.m_inputs = list.ToArray();
        }

        public override void Setup()
        {
            SetupDialog dialog = new SetupDialog(this.m_inputs);
            if (dialog.ShowDialog() == DialogResult.OK)
            {
            }
            dialog.Dispose();
        }

        public override void Shutdown()
        {
            JoystickManager.ReleaseAll();
        }

        public override void Startup()
        {
            JoystickManager.AcquireAll();
        }

        public override string Author
        {
            get
            {
                return "Vixen Developers";
            }
        }

        public override string Description
        {
            get
            {
                return "Implements managed DirectX joystick input";
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
                return "Joystick";
            }
        }
    }
}

