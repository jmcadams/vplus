namespace JoystickManager
{
    using Microsoft.DirectX.DirectInput;
    using System;
    using System.Collections.Generic;

    public static class JoystickManager
    {
        private static Joystick[] m_joysticks = null;

        public static void AcquireAll()
        {
            if (m_joysticks == null)
            {
                RefreshAttachments();
            }
            if (m_joysticks != null)
            {
                foreach (Joystick joystick in m_joysticks)
                {
                    joystick.Acquire();
                }
            }
        }

        public static void RefreshAttachments()
        {
            List<Joystick> list = new List<Joystick>();
            List<Guid> list2 = new List<Guid>();
            if (m_joysticks != null)
            {
                foreach (Joystick joystick in m_joysticks)
                {
                    list.Add(joystick);
                    list2.Add(joystick.DeviceGuid);
                }
            }
            foreach (DeviceInstance instance in Manager.GetDevices(DeviceClass.GameControl, EnumDevicesFlags.AttachedOnly))
            {
                if (!list2.Contains(instance.InstanceGuid))
                {
                    list.Add(new Joystick(instance.InstanceGuid));
                }
            }
            m_joysticks = list.ToArray();
        }

        public static void ReleaseAll()
        {
            if (m_joysticks != null)
            {
                foreach (Joystick joystick in m_joysticks)
                {
                    joystick.Release();
                }
            }
        }

        public static Joystick[] Joysticks
        {
            get
            {
                if (m_joysticks == null)
                {
                    RefreshAttachments();
                }
                return m_joysticks;
            }
        }
    }
}

