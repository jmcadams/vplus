namespace Keyboard
{
	using System;
	using System.Windows.Forms;
	using Vixen;

	internal class VixenInput : Input
	{
		public KeyState CurrentState;
		public Keyboard.Inputs.InputType InputType;
		public Keys Key;
		public bool Toggled;

		public VixenInput(InputPlugin pluginOwner, Keys key, Keyboard.Inputs.InputType inputType, bool isIterator) : base(pluginOwner, key.ToString(), isIterator)
		{
			this.Toggled = false;
			this.CurrentState = KeyState.Released;
			this.Key = key;
			this.InputType = inputType;
		}

		public override byte GetValue()
		{
			byte num = 0;
			if (this.InputType != Keyboard.Inputs.InputType.Latching)
			{
				if (Interop.Keyboard.IsKeyDown(this.Key))
				{
					num = 0xff;
				}
				return num;
			}
			switch (this.CurrentState)
			{
				case KeyState.Released:
					if (Interop.Keyboard.IsKeyDown(this.Key))
					{
						this.CurrentState = KeyState.Pressed;
					}
					break;

				case KeyState.Pressed:
					this.CurrentState = KeyState.WaitingForUp;
					this.Toggled = !this.Toggled;
					break;

				case KeyState.WaitingForUp:
					if (!Interop.Keyboard.IsKeyDown(this.Key))
					{
						this.CurrentState = KeyState.Released;
					}
					break;
			}
			if (this.Toggled)
			{
				num = 0xff;
			}
			return num;
		}

		public void IncrementState()
		{
			if (this.CurrentState == KeyState.WaitingForUp)
			{
				this.CurrentState = KeyState.Released;
			}
			else
			{
				this.CurrentState += 1;
			}
		}

		public override bool Changed { get {return true;} } // TODO: Update logic to make this accurate

		public enum KeyState
		{
			Released,
			WaitingForDown,
			Pressed,
			WaitingForUp
		}
	}
}

