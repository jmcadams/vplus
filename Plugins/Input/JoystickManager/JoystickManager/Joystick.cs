namespace JoystickManager
{
	using Microsoft.DirectX.DirectInput;
	using System;
	using System.Collections.Generic;

	public class Joystick
	{
		private bool m_acquired = false;
		private ButtonResource[] m_buttons;
		private Microsoft.DirectX.DirectInput.Device m_device;
		private Guid m_instanceGuid;
		private POVResource[] m_pov;
		private AxisResource m_rxAxis = null;
		private AxisResource m_ryAxis = null;
		private AxisResource m_rzAxis = null;
		private AxisResource m_xAxis = null;
		private AxisResource m_yAxis = null;
		private AxisResource m_zAxis = null;

		internal Joystick(Guid instanceGuid)
		{
			this.m_instanceGuid = instanceGuid;
			this.m_device = new Microsoft.DirectX.DirectInput.Device(this.m_instanceGuid);
			DataFormat df = new DataFormat();
			df.Flags = DataFormatFlags.RelativeAxis;
			this.m_device.SetDataFormat(df); // was SetDataFormat(2);
			this.m_device.get_Properties().set_AxisModeAbsolute(true);
			this.m_device.SetCooperativeLevel(IntPtr.Zero, CooperativeLevelFlags.Background | CooperativeLevelFlags.NonExclusive);
			int num = 0;
			int num2 = 0;
			List<ButtonResource> list = new List<ButtonResource>();
			List<AxisResource> list2 = new List<AxisResource>();
			List<POVResource> list3 = new List<POVResource>();
			foreach (DeviceObjectInstance instance in this.m_device.get_Objects())
			{
				if ((instance.get_ObjectId() & 3) == 0)
				{
					goto Label_01D6;
				}
				string str = instance.get_Name().ToLower();
				this.Device.get_Properties().SetRange(2, instance.get_ObjectId(), new InputRange(0, 0xff));
				switch (str[0])
				{
					case 'x':
						if (!str.Contains("rotation"))
						{
							break;
						}
						this.m_rxAxis = new AxisResource(this, Axis.rX);
						goto Label_0227;

					case 'y':
						if (!str.Contains("rotation"))
						{
							goto Label_018B;
						}
						this.m_ryAxis = new AxisResource(this, Axis.rY);
						goto Label_0227;

					case 'z':
						if (!str.Contains("rotation"))
						{
							goto Label_01C2;
						}
						this.m_rzAxis = new AxisResource(this, Axis.rZ);
						goto Label_0227;

					default:
						goto Label_0227;
				}
				this.m_xAxis = new AxisResource(this, Axis.X);
				goto Label_0227;
			Label_018B:
				this.m_yAxis = new AxisResource(this, Axis.Y);
				goto Label_0227;
			Label_01C2:
				this.m_zAxis = new AxisResource(this, Axis.Z);
				goto Label_0227;
			Label_01D6:
				if ((instance.get_ObjectId() & 12) != 0)
				{
					list.Add(new ButtonResource(this, num++));
				}
				else if ((instance.get_ObjectId() & 0x10) != 0)
				{
					list3.Add(new POVResource(this, num2++));
				}
			Label_0227:;
			}
			this.m_buttons = list.ToArray();
			this.m_pov = list3.ToArray();
		}

		public void Acquire()
		{
			this.m_device.Acquire();
			this.m_acquired = true;
		}

		public AxisResource[] AvailableAxes()
		{
			List<AxisResource> list = new List<AxisResource>();
			if (this.m_xAxis != null)
			{
				list.Add(this.m_xAxis);
			}
			if (this.m_yAxis != null)
			{
				list.Add(this.m_yAxis);
			}
			if (this.m_zAxis != null)
			{
				list.Add(this.m_zAxis);
			}
			if (this.m_rxAxis != null)
			{
				list.Add(this.m_rxAxis);
			}
			if (this.m_ryAxis != null)
			{
				list.Add(this.m_ryAxis);
			}
			if (this.m_rzAxis != null)
			{
				list.Add(this.m_rzAxis);
			}
			return list.ToArray();
		}

		public void Poll()
		{
			this.m_device.Poll();
		}

		public void Release()
		{
			if (this.m_acquired)
			{
				this.m_acquired = false;
				this.m_device.Unacquire();
			}
		}

		public override string ToString()
		{
			return this.Name;
		}

		public POVResource[] AvailablePOV
		{
			get
			{
				return this.m_pov;
			}
		}

		public ButtonResource[] Buttons
		{
			get
			{
				return this.m_buttons;
			}
		}

		internal Microsoft.DirectX.DirectInput.Device Device
		{
			get
			{
				return this.m_device;
			}
		}

		internal Guid DeviceGuid
		{
			get
			{
				return this.m_instanceGuid;
			}
		}

		public string Name
		{
			get
			{
				return this.m_device.get_DeviceInformation().get_InstanceName();
			}
		}

		public enum Axis
		{
			X,
			Y,
			Z,
			rX,
			rY,
			rZ
		}
	}
}

