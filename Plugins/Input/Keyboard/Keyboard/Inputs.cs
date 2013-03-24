namespace Keyboard
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Runtime.CompilerServices;
	using System.Threading;
	using System.Windows.Forms;
	using System.Xml;
	using VixenPlus;

	internal class Inputs
	{
		private const string ATTRIBUTE_IS_ITERATOR = "isIterator";
		private const string INPUT_KEYS = "InputKeys";
		private const string KEY = "Key";
		private XmlNode m_inputKeysNode;
		private XmlNode m_setupNode;

		public Inputs(XmlNode setupNode)
		{
			this.m_setupNode = setupNode;
			this.m_inputKeysNode = this.m_setupNode["InputKeys"];
		}

		public void Clear()
		{
			this.m_inputKeysNode = Xml.GetEmptyNodeAlways(this.m_setupNode, "InputKeys");
		}

		public IEnumerable<KeyInput> ReadAll() {
			return (IEnumerable<KeyInput>)new List<KeyInput>();
		}

		public IEnumerable<KeyInput> ReadAll(Inputs inputs) {
			return (IEnumerable<KeyInput>)new List<KeyInput>();
		}

		//public IEnumerable<KeyInput> ReadAll()
		//{
		//    <ReadAll>d__0 d__ = new <ReadAll>d__0(-2);
		//    d__.<>4__this = this;
		//    return d__;
		//}

		public void Write(Keys key, InputType inputType, bool isIterator)
		{
			if (this.m_inputKeysNode != null)
			{
				Xml.SetAttribute(Xml.SetNewValue(this.m_inputKeysNode, "Key", string.Format("{0},{1}", key.ToString(), inputType.ToString())), "isIterator", isIterator.ToString());
			}
		}

		//[CompilerGenerated]
		//private sealed class <ReadAll>d__0 : IEnumerable<KeyInput>, IEnumerable, IEnumerator<KeyInput>, IEnumerator, IDisposable
		//{
		//    private int <>1__state;
		//    private KeyInput <>2__current;
		//    public Inputs <>4__this;
		//    public IEnumerator <>7__wrap4;
		//    public IDisposable <>7__wrap5;
		//    public XmlNode <inputKeyNode>5__3;
		//    public KeyInput <keyInput>5__2;
		//    public string[] <nameValue>5__1;

		//    [DebuggerHidden]
		//    public <ReadAll>d__0(int <>1__state)
		//    {
		//        this.<>1__state = <>1__state;
		//    }

		//    private bool MoveNext()
		//    {
		//        try
		//        {
		//            switch (this.<>1__state)
		//            {
		//                case 0:
		//                    this.<>1__state = -1;
		//                    if (this.<>4__this.m_inputKeysNode != null)
		//                    {
		//                        this.<>7__wrap4 = this.<>4__this.m_inputKeysNode.ChildNodes.GetEnumerator();
		//                        this.<>1__state = 1;
		//                        while (this.<>7__wrap4.MoveNext())
		//                        {
		//                            this.<inputKeyNode>5__3 = (XmlNode) this.<>7__wrap4.Current;
		//                            this.<nameValue>5__1 = this.<inputKeyNode>5__3.InnerText.Split(new char[] { ',' });
		//                            this.<keyInput>5__2 = new KeyInput();
		//                            this.<keyInput>5__2.Key = (Keys) Enum.Parse(typeof(Keys), this.<nameValue>5__1[0]);
		//                            this.<keyInput>5__2.InputType = (Inputs.InputType) Enum.Parse(typeof(Inputs.InputType), this.<nameValue>5__1[1]);
		//                            this.<keyInput>5__2.IsIterator = bool.Parse(this.<inputKeyNode>5__3.Attributes["isIterator"].Value);
		//                            this.<>2__current = this.<keyInput>5__2;
		//                            this.<>1__state = 2;
		//                            return true;
		//                        Label_0142:
		//                            this.<>1__state = 1;
		//                        }
		//                        this.<>1__state = -1;
		//                        this.<>7__wrap5 = this.<>7__wrap4 as IDisposable;
		//                        if (this.<>7__wrap5 != null)
		//                        {
		//                            this.<>7__wrap5.Dispose();
		//                        }
		//                    }
		//                    break;

		//                case 2:
		//                    goto Label_0142;
		//            }
		//            return false;
		//        }
		//        fault
		//        {
		//            ((IDisposable) this).Dispose();
		//        }
		//    }

		//    [DebuggerHidden]
		//    IEnumerator<KeyInput> IEnumerable<KeyInput>.GetEnumerator()
		//    {
		//        if (Interlocked.CompareExchange(ref this.<>1__state, 0, -2) == -2)
		//        {
		//            return this;
		//        }
		//        Inputs.<ReadAll>d__0 d__ = new Inputs.<ReadAll>d__0(0);
		//        d__.<>4__this = this.<>4__this;
		//        return d__;
		//    }

		//    [DebuggerHidden]
		//    IEnumerator IEnumerable.GetEnumerator()
		//    {
		//        return this.System.Collections.Generic.IEnumerable<Keyboard.KeyInput>.GetEnumerator();
		//    }

		//    [DebuggerHidden]
		//    void IEnumerator.Reset()
		//    {
		//        throw new NotSupportedException();
		//    }

		//    void IDisposable.Dispose()
		//    {
		//        switch (this.<>1__state)
		//        {
		//            case 1:
		//            case 2:
		//                try
		//                {
		//                }
		//                finally
		//                {
		//                    this.<>1__state = -1;
		//                    this.<>7__wrap5 = this.<>7__wrap4 as IDisposable;
		//                    if (this.<>7__wrap5 != null)
		//                    {
		//                        this.<>7__wrap5.Dispose();
		//                    }
		//                }
		//                break;
		//        }
		//    }

		//    KeyInput IEnumerator<KeyInput>.Current
		//    {
		//        [DebuggerHidden]
		//        get
		//        {
		//            return this.<>2__current;
		//        }
		//    }

		//    object IEnumerator.Current
		//    {
		//        [DebuggerHidden]
		//        get
		//        {
		//            return this.<>2__current;
		//        }
		//    }
		//}

		public enum InputType
		{
			First = 1,
			Last = 2,
			Latching = 2,
			Momentary = 1
		}
	}
}

