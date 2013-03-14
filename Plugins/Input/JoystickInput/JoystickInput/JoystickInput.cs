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
            JoystickInput.SetupData.LoadFrom(setupNode);
            List<Input> list = new List<Input>();
            JoystickManager.JoystickManager.RefreshAttachments();
            foreach (Joystick joystick in JoystickManager.JoystickManager.Joysticks)
            {
                foreach (AxisResource resource in joystick.AvailableAxes())
                {
                    list.Add(new JoystickInputResource(this, resource, JoystickInput.SetupData.GetIsIterator(resource.Name)));
                }
                foreach (ButtonResource resource2 in joystick.Buttons)
                {
                    list.Add(new JoystickInputResource(this, resource2, JoystickInput.SetupData.GetIsIterator(resource2.Name)));
                }
                foreach (POVResource resource3 in joystick.AvailablePOV)
                {
                    if (JoystickInput.SetupData.IsQuadDigitalPOV)
                    {
                        list.Add(new JoystickInputResource(this, new DigitalPOVResource(joystick, resource3.Index, DigitalPOVResource.Direction.N), resource3.Name + "_N", JoystickInput.SetupData.GetIsIterator(resource3.Name + "_N")));
                        list.Add(new JoystickInputResource(this, new DigitalPOVResource(joystick, resource3.Index, DigitalPOVResource.Direction.S), resource3.Name + "_S", JoystickInput.SetupData.GetIsIterator(resource3.Name + "_S")));
                        list.Add(new JoystickInputResource(this, new DigitalPOVResource(joystick, resource3.Index, DigitalPOVResource.Direction.E), resource3.Name + "_E", JoystickInput.SetupData.GetIsIterator(resource3.Name + "_E")));
                        list.Add(new JoystickInputResource(this, new DigitalPOVResource(joystick, resource3.Index, DigitalPOVResource.Direction.W), resource3.Name + "_W", JoystickInput.SetupData.GetIsIterator(resource3.Name + "_W")));
                    }
                    else
                    {
                        list.Add(new JoystickInputResource(this, resource3, JoystickInput.SetupData.GetIsIterator(resource3.Name)));
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
            JoystickManager.JoystickManager.ReleaseAll();
        }

        public override void Startup()
        {
            JoystickManager.JoystickManager.AcquireAll();
        }

        public override string Author
        {
            get
            {
                return "K.C. Oaks";
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

