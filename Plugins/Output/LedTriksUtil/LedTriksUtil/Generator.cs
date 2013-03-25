namespace LedTriksUtil
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.Drawing.Text;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using System.Xml;

    public class Generator : IDisposable
    {
        public int BoardPixelHeight = 0x10;
        public int BoardPixelWidth = 0x30;
        public int FrameLength = 25;
        public bool IgnoreFontDescent = false;
        private int m_fontHeight;
		//private int m_staticCount = 0;
        private int m_stringPixelLength;
        private Win32.TEXTMETRIC m_tm;
        public Font TextFont = new Font("Arial", 10f);
        public int TextHeight = 0x10;
        public HorzPosition TextHorzPosition = HorzPosition.Left;
        public int TextHorzPositionValue = 0;
        public ScrollDirection TextScrollDirection = ScrollDirection.Left;
        public ScrollExtent TextScrollExtent = ScrollExtent.OnAndOff;
        public VertPosition TextVertPosition = VertPosition.Top;
        public int TextVertPositionValue = 0;

        private void AddAttribute(XmlNode parentNode, string attributeName, string attributeValue)
        {
            XmlAttribute node = parentNode.OwnerDocument.CreateAttribute(attributeName);
            node.Value = attributeValue;
            parentNode.Attributes.Append(node);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        ~Generator()
        {
            this.Dispose();
        }

        private unsafe Frame FrameFromBitmap(Bitmap bitmap, int length, int yOffset)
        {
            BitmapData bitmapdata = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, bitmap.PixelFormat);
            Frame frame = new Frame(length);
            int num2 = 0;
            while (num2 < bitmap.Height)
            {
                int* numPtr = (int*) (((int*)bitmapdata.Scan0) + ((num2 * bitmap.Width) * 4));
                for (int i = 0; i < bitmap.Width; i++)
                {
                    if (numPtr[i] != -1)
                    {
                        frame.Cells.Add((uint) ((i << 0x10) | yOffset));
                    }
                }
                num2++;
                yOffset++;
            }
            bitmap.UnlockBits(bitmapdata);
            return frame;
        }

        public List<Frame> GenerateTextFrames(string text)
        {
            List<Frame> list = new List<Frame>();
            foreach (List<Frame> list2 in this.GenerateTextFrames(text, 0))
            {
                list.AddRange(list2);
            }
            return list;
        }

        //TODO: Implement
        public IEnumerable<List<Frame>> GenerateTextFrames(string text, int maxYieldCount)
        {
            return (IEnumerable<List<Frame>>) new List<Frame>();
        }

        //public IEnumerable<List<Frame>> GenerateTextFrames(string text, int maxYieldCount)
        //{
        //    <GenerateTextFrames>d__0 d__ = new <GenerateTextFrames>d__0(-2);
        //    d__.<>4__this = this;
        //    d__.<>3__text = text;
        //    d__.<>3__maxYieldCount = maxYieldCount;
        //    return d__;
        //}

        public List<Frame> GenerateTextFrames(string text, TimeSpan time)
        {
            List<Frame> list = new List<Frame>();
            foreach (List<Frame> list2 in this.GenerateTextFrames(text, 0, time))
            {
                list.AddRange(list2);
            }
            return list;
        }

        //TODO: Implement
        public IEnumerable<List<Frame>> GenerateTextFrames(string text, int maxYieldCount, TimeSpan staticTextTimeLength)
        {
            return (IEnumerable<List<Frame>>)new List<Frame>();
        }

        //public IEnumerable<List<Frame>> GenerateTextFrames(string text, int maxYieldCount, TimeSpan staticTextTimeLength)
        //{
        //    <GenerateTextFrames>d__5 d__ = new <GenerateTextFrames>d__5(-2);
        //    d__.<>4__this = this;
        //    d__.<>3__text = text;
        //    d__.<>3__maxYieldCount = maxYieldCount;
        //    d__.<>3__staticTextTimeLength = staticTextTimeLength;
        //    return d__;
        //}

        private void InitFromText(string text)
        {
            this.m_fontHeight = this.TextHeight;
            this.TextFont = new Font(this.TextFont.FontFamily.Name, (float) this.m_fontHeight, this.TextFont.Style, GraphicsUnit.Pixel);
            Bitmap image = new Bitmap(1, 1);
            Graphics graphics = Graphics.FromImage(image);
            this.m_tm = Win32.GetTextMetrics(graphics, this.TextFont);
            int num = (this.IgnoreFontDescent ? this.m_tm.tmAscent : (this.m_tm.tmAscent + this.m_tm.tmDescent)) - this.m_tm.tmInternalLeading;
            if (num != this.m_fontHeight)
            {
                float num2 = 1f + (((float) (this.m_fontHeight - num)) / ((float) num));
                this.m_fontHeight = (int) (num2 * this.m_fontHeight);
                this.TextFont = new Font(this.TextFont.FontFamily.Name, (float) this.m_fontHeight, GraphicsUnit.Pixel);
            }
            this.m_stringPixelLength = ((int) graphics.MeasureString(text, this.TextFont).Width) + 1;
            graphics.Dispose();
            image.Dispose();
        }

        public void LoadFromXml(XmlNode parentNode)
        {
            this.FrameLength = int.Parse(parentNode["Frame"].Attributes["length"].Value);
            XmlNode node = parentNode["Font"];
            this.TextFont = new Font(node.Attributes["name"].Value, 10f);
            this.TextHeight = int.Parse(node.Attributes["height"].Value);
            this.IgnoreFontDescent = bool.Parse(node.Attributes["ignoreDescent"].Value);
            node = parentNode["Position"]["Horz"];
            this.TextHorzPosition = (HorzPosition) Enum.Parse(typeof(HorzPosition), node.Attributes["position"].Value);
            this.TextHorzPositionValue = int.Parse(node.Attributes["value"].Value);
            node = parentNode["Position"]["Vert"];
            this.TextVertPosition = (VertPosition) Enum.Parse(typeof(VertPosition), node.Attributes["position"].Value);
            this.TextVertPositionValue = int.Parse(node.Attributes["value"].Value);
            node = parentNode["Scroll"];
            this.TextScrollDirection = (ScrollDirection) Enum.Parse(typeof(ScrollDirection), node.Attributes["direction"].Value);
            this.TextScrollExtent = (ScrollExtent) Enum.Parse(typeof(ScrollExtent), node.Attributes["extent"].Value);
        }

        public void SaveToXml(XmlNode parentNode)
        {
            XmlNode node = parentNode.OwnerDocument.CreateElement("Frame");
            this.AddAttribute(node, "length", this.FrameLength.ToString());
            parentNode.AppendChild(node);
            node = parentNode.OwnerDocument.CreateElement("Font");
            this.AddAttribute(node, "name", this.TextFont.Name);
            this.AddAttribute(node, "height", this.TextHeight.ToString());
            this.AddAttribute(node, "ignoreDescent", this.IgnoreFontDescent.ToString());
            parentNode.AppendChild(node);
            XmlNode newChild = parentNode.OwnerDocument.CreateElement("Position");
            parentNode.AppendChild(newChild);
            node = parentNode.OwnerDocument.CreateElement("Horz");
            this.AddAttribute(node, "position", this.TextHorzPosition.ToString());
            this.AddAttribute(node, "value", this.TextHorzPositionValue.ToString());
            newChild.AppendChild(node);
            node = parentNode.OwnerDocument.CreateElement("Vert");
            this.AddAttribute(node, "position", this.TextVertPosition.ToString());
            this.AddAttribute(node, "value", this.TextVertPositionValue.ToString());
            newChild.AppendChild(node);
            node = parentNode.OwnerDocument.CreateElement("Scroll");
            this.AddAttribute(node, "direction", this.TextScrollDirection.ToString());
            this.AddAttribute(node, "extent", this.TextScrollExtent.ToString());
            parentNode.AppendChild(node);
        }

    //    private IEnumerable<List<Frame>> ScrollDown(string text, Graphics g, Bitmap textRaster, int y, int maxYieldCount)
    //    {
    //        <ScrollDown>d__27 d__ = new <ScrollDown>d__27(-2);
    //        d__.<>4__this = this;
    //        d__.<>3__text = text;
    //        d__.<>3__g = g;
    //        d__.<>3__textRaster = textRaster;
    //        d__.<>3__y = y;
    //        d__.<>3__maxYieldCount = maxYieldCount;
    //        return d__;
    //    }

    //    private IEnumerable<List<Frame>> ScrollLeft(string text, Graphics g, Bitmap textRaster, int y, int maxYieldCount)
    //    {
    //        <ScrollLeft>d__1b d__b = new <ScrollLeft>d__1b(-2);
    //        d__b.<>4__this = this;
    //        d__b.<>3__text = text;
    //        d__b.<>3__g = g;
    //        d__b.<>3__textRaster = textRaster;
    //        d__b.<>3__y = y;
    //        d__b.<>3__maxYieldCount = maxYieldCount;
    //        return d__b;
    //    }

    //    private IEnumerable<List<Frame>> ScrollNone(string text, Graphics g, Bitmap textRaster, int x, int y, int maxYieldCount)
    //    {
    //        <ScrollNone>d__16 d__ = new <ScrollNone>d__16(-2);
    //        d__.<>4__this = this;
    //        d__.<>3__text = text;
    //        d__.<>3__g = g;
    //        d__.<>3__textRaster = textRaster;
    //        d__.<>3__x = x;
    //        d__.<>3__y = y;
    //        d__.<>3__maxYieldCount = maxYieldCount;
    //        return d__;
    //    }

    //    private IEnumerable<List<Frame>> ScrollRight(string text, Graphics g, Bitmap textRaster, int y, int maxYieldCount)
    //    {
    //        <ScrollRight>d__21 d__ = new <ScrollRight>d__21(-2);
    //        d__.<>4__this = this;
    //        d__.<>3__text = text;
    //        d__.<>3__g = g;
    //        d__.<>3__textRaster = textRaster;
    //        d__.<>3__y = y;
    //        d__.<>3__maxYieldCount = maxYieldCount;
    //        return d__;
    //    }

    //    private IEnumerable<List<Frame>> ScrollUp(string text, Graphics g, Bitmap textRaster, int y, int maxYieldCount)
    //    {
    //        <ScrollUp>d__2d d__d = new <ScrollUp>d__2d(-2);
    //        d__d.<>4__this = this;
    //        d__d.<>3__text = text;
    //        d__d.<>3__g = g;
    //        d__d.<>3__textRaster = textRaster;
    //        d__d.<>3__y = y;
    //        d__d.<>3__maxYieldCount = maxYieldCount;
    //        return d__d;
    //    }

    //    [CompilerGenerated]
    //    private sealed class <GenerateTextFrames>d__0 : IEnumerable<List<Frame>>, IEnumerable, IEnumerator<List<Frame>>, IEnumerator, IDisposable
    //    {
    //        private int <>1__state;
    //        private List<Frame> <>2__current;
    //        public int <>3__maxYieldCount;
    //        public string <>3__text;
    //        public Generator <>4__this;
    //        public IEnumerator<List<Frame>> <>7__wrap2;
    //        public List<Frame> <frameset>5__1;
    //        public int maxYieldCount;
    //        public string text;

    //        [DebuggerHidden]
    //        public <GenerateTextFrames>d__0(int <>1__state)
    //        {
    //            this.<>1__state = <>1__state;
    //        }

    //        private bool MoveNext()
    //        {
    //            try
    //            {
    //                switch (this.<>1__state)
    //                {
    //                    case 0:
    //                        this.<>1__state = -1;
    //                        this.<>7__wrap2 = this.<>4__this.GenerateTextFrames(this.text, this.maxYieldCount, new TimeSpan(0, 0, 0, 0, this.<>4__this.FrameLength)).GetEnumerator();
    //                        this.<>1__state = 1;
    //                        while (this.<>7__wrap2.MoveNext())
    //                        {
    //                            this.<frameset>5__1 = this.<>7__wrap2.Current;
    //                            this.<>2__current = this.<frameset>5__1;
    //                            this.<>1__state = 2;
    //                            return true;
    //                        Label_0095:
    //                            this.<>1__state = 1;
    //                        }
    //                        this.<>1__state = -1;
    //                        if (this.<>7__wrap2 != null)
    //                        {
    //                            this.<>7__wrap2.Dispose();
    //                        }
    //                        break;

    //                    case 2:
    //                        goto Label_0095;
    //                }
    //                return false;
    //            }
    //            fault
    //            {
    //                ((IDisposable) this).Dispose();
    //            }
    //        }

    //        [DebuggerHidden]
    //        IEnumerator<List<Frame>> IEnumerable<List<Frame>>.GetEnumerator()
    //        {
    //            Generator.<GenerateTextFrames>d__0 d__;
    //            if (Interlocked.CompareExchange(ref this.<>1__state, 0, -2) == -2)
    //            {
    //                d__ = this;
    //            }
    //            else
    //            {
    //                d__ = new Generator.<GenerateTextFrames>d__0(0);
    //                d__.<>4__this = this.<>4__this;
    //            }
    //            d__.text = this.<>3__text;
    //            d__.maxYieldCount = this.<>3__maxYieldCount;
    //            return d__;
    //        }

    //        [DebuggerHidden]
    //        IEnumerator IEnumerable.GetEnumerator()
    //        {
    //            return this.System.Collections.Generic.IEnumerable<System.Collections.Generic.List<LedTriksUtil.Frame>>.GetEnumerator();
    //        }

    //        [DebuggerHidden]
    //        void IEnumerator.Reset()
    //        {
    //            throw new NotSupportedException();
    //        }

    //        void IDisposable.Dispose()
    //        {
    //            switch (this.<>1__state)
    //            {
    //                case 1:
    //                case 2:
    //                    try
    //                    {
    //                    }
    //                    finally
    //                    {
    //                        this.<>1__state = -1;
    //                        if (this.<>7__wrap2 != null)
    //                        {
    //                            this.<>7__wrap2.Dispose();
    //                        }
    //                    }
    //                    break;
    //            }
    //        }

    //        List<Frame> IEnumerator<List<Frame>>.Current
    //        {
    //            [DebuggerHidden]
    //            get
    //            {
    //                return this.<>2__current;
    //            }
    //        }

    //        object IEnumerator.Current
    //        {
    //            [DebuggerHidden]
    //            get
    //            {
    //                return this.<>2__current;
    //            }
    //        }
    //    }

    //    [CompilerGenerated]
    //    private sealed class <GenerateTextFrames>d__5 : IEnumerable<List<Frame>>, IEnumerable, IEnumerator<List<Frame>>, IEnumerator, IDisposable
    //    {
    //        private int <>1__state;
    //        private List<Frame> <>2__current;
    //        public int <>3__maxYieldCount;
    //        public TimeSpan <>3__staticTextTimeLength;
    //        public string <>3__text;
    //        public Generator <>4__this;
    //        public IEnumerator<List<Frame>> <>7__wrap10;
    //        public IEnumerator<List<Frame>> <>7__wrap11;
    //        public IEnumerator<List<Frame>> <>7__wrap12;
    //        public IEnumerator<List<Frame>> <>7__wrap13;
    //        public IEnumerator<List<Frame>> <>7__wrapf;
    //        public List<Frame> <frameset>5__a;
    //        public List<Frame> <frameset>5__b;
    //        public List<Frame> <frameset>5__c;
    //        public List<Frame> <frameset>5__d;
    //        public List<Frame> <frameset>5__e;
    //        public Graphics <g>5__8;
    //        public Bitmap <textRaster>5__9;
    //        public int <x>5__6;
    //        public int <y>5__7;
    //        public int maxYieldCount;
    //        public TimeSpan staticTextTimeLength;
    //        public string text;

    //        [DebuggerHidden]
    //        public <GenerateTextFrames>d__5(int <>1__state)
    //        {
    //            this.<>1__state = <>1__state;
    //        }

    //        private bool MoveNext()
    //        {
    //            bool flag;
    //            try
    //            {
    //                switch (this.<>1__state)
    //                {
    //                    case 0:
    //                        break;

    //                    case 2:
    //                        goto Label_0282;

    //                    case 4:
    //                        goto Label_032C;

    //                    case 6:
    //                        goto Label_03D6;

    //                    case 8:
    //                        goto Label_0480;

    //                    case 10:
    //                        goto Label_0529;

    //                    default:
    //                        goto Label_0580;
    //                }
    //                this.<>1__state = -1;
    //                new List<Frame>();
    //                this.<>4__this.m_staticCount = (int) (this.staticTextTimeLength.TotalMilliseconds / ((double) this.<>4__this.FrameLength));
    //                this.<>4__this.InitFromText(this.text);
    //                this.<textRaster>5__9 = new Bitmap(this.<>4__this.BoardPixelWidth, this.<>4__this.BoardPixelHeight, PixelFormat.Format32bppRgb);
    //                this.<g>5__8 = Graphics.FromImage(this.<textRaster>5__9);
    //                this.<g>5__8.TextRenderingHint = TextRenderingHint.SingleBitPerPixelGridFit;
    //                switch (this.<>4__this.TextVertPosition)
    //                {
    //                    case VertPosition.Top:
    //                        this.<y>5__7 = 0;
    //                        break;

    //                    case VertPosition.Bottom:
    //                        this.<y>5__7 = this.<>4__this.BoardPixelHeight - this.<>4__this.m_fontHeight;
    //                        break;

    //                    case VertPosition.Percent:
    //                        this.<y>5__7 = (this.<>4__this.BoardPixelHeight * this.<>4__this.TextVertPositionValue) / 100;
    //                        break;

    //                    default:
    //                        this.<y>5__7 = this.<>4__this.TextVertPositionValue;
    //                        break;
    //                }
    //                switch (this.<>4__this.TextHorzPosition)
    //                {
    //                    case HorzPosition.Left:
    //                        this.<x>5__6 = 0;
    //                        break;

    //                    case HorzPosition.Right:
    //                        this.<x>5__6 = this.<>4__this.BoardPixelWidth - this.<>4__this.m_stringPixelLength;
    //                        break;

    //                    case HorzPosition.Percent:
    //                        this.<x>5__6 = (this.<>4__this.BoardPixelWidth * this.<>4__this.TextHorzPositionValue) / 100;
    //                        break;

    //                    default:
    //                        this.<x>5__6 = this.<>4__this.TextHorzPositionValue;
    //                        break;
    //                }
    //                switch (this.<>4__this.TextScrollDirection)
    //                {
    //                    case ScrollDirection.None:
    //                        this.<>7__wrapf = this.<>4__this.ScrollNone(this.text, this.<g>5__8, this.<textRaster>5__9, this.<x>5__6, this.<y>5__7, this.maxYieldCount).GetEnumerator();
    //                        this.<>1__state = 1;
    //                        goto Label_028A;

    //                    case ScrollDirection.Left:
    //                        this.<>7__wrap10 = this.<>4__this.ScrollLeft(this.text, this.<g>5__8, this.<textRaster>5__9, this.<y>5__7, this.maxYieldCount).GetEnumerator();
    //                        this.<>1__state = 3;
    //                        goto Label_0334;

    //                    case ScrollDirection.Right:
    //                        this.<>7__wrap11 = this.<>4__this.ScrollRight(this.text, this.<g>5__8, this.<textRaster>5__9, this.<y>5__7, this.maxYieldCount).GetEnumerator();
    //                        this.<>1__state = 5;
    //                        goto Label_03DE;

    //                    case ScrollDirection.Up:
    //                        this.<>7__wrap13 = this.<>4__this.ScrollUp(this.text, this.<g>5__8, this.<textRaster>5__9, this.<y>5__7, this.maxYieldCount).GetEnumerator();
    //                        this.<>1__state = 9;
    //                        goto Label_0532;

    //                    case ScrollDirection.Down:
    //                        this.<>7__wrap12 = this.<>4__this.ScrollDown(this.text, this.<g>5__8, this.<textRaster>5__9, this.<y>5__7, this.maxYieldCount).GetEnumerator();
    //                        this.<>1__state = 7;
    //                        goto Label_0488;

    //                    default:
    //                        goto Label_0567;
    //                }
    //            Label_0256:
    //                this.<frameset>5__a = this.<>7__wrapf.Current;
    //                this.<>2__current = this.<frameset>5__a;
    //                this.<>1__state = 2;
    //                return true;
    //            Label_0282:
    //                this.<>1__state = 1;
    //            Label_028A:
    //                if (this.<>7__wrapf.MoveNext())
    //                {
    //                    goto Label_0256;
    //                }
    //                this.<>1__state = -1;
    //                if (this.<>7__wrapf != null)
    //                {
    //                    this.<>7__wrapf.Dispose();
    //                }
    //                goto Label_0567;
    //            Label_0300:
    //                this.<frameset>5__b = this.<>7__wrap10.Current;
    //                this.<>2__current = this.<frameset>5__b;
    //                this.<>1__state = 4;
    //                return true;
    //            Label_032C:
    //                this.<>1__state = 3;
    //            Label_0334:
    //                if (this.<>7__wrap10.MoveNext())
    //                {
    //                    goto Label_0300;
    //                }
    //                this.<>1__state = -1;
    //                if (this.<>7__wrap10 != null)
    //                {
    //                    this.<>7__wrap10.Dispose();
    //                }
    //                goto Label_0567;
    //            Label_03AA:
    //                this.<frameset>5__c = this.<>7__wrap11.Current;
    //                this.<>2__current = this.<frameset>5__c;
    //                this.<>1__state = 6;
    //                return true;
    //            Label_03D6:
    //                this.<>1__state = 5;
    //            Label_03DE:
    //                if (this.<>7__wrap11.MoveNext())
    //                {
    //                    goto Label_03AA;
    //                }
    //                this.<>1__state = -1;
    //                if (this.<>7__wrap11 != null)
    //                {
    //                    this.<>7__wrap11.Dispose();
    //                }
    //                goto Label_0567;
    //            Label_0454:
    //                this.<frameset>5__d = this.<>7__wrap12.Current;
    //                this.<>2__current = this.<frameset>5__d;
    //                this.<>1__state = 8;
    //                return true;
    //            Label_0480:
    //                this.<>1__state = 7;
    //            Label_0488:
    //                if (this.<>7__wrap12.MoveNext())
    //                {
    //                    goto Label_0454;
    //                }
    //                this.<>1__state = -1;
    //                if (this.<>7__wrap12 != null)
    //                {
    //                    this.<>7__wrap12.Dispose();
    //                }
    //                goto Label_0567;
    //            Label_04FF:
    //                this.<frameset>5__e = this.<>7__wrap13.Current;
    //                this.<>2__current = this.<frameset>5__e;
    //                this.<>1__state = 10;
    //                return true;
    //            Label_0529:
    //                this.<>1__state = 9;
    //            Label_0532:
    //                if (this.<>7__wrap13.MoveNext())
    //                {
    //                    goto Label_04FF;
    //                }
    //                this.<>1__state = -1;
    //                if (this.<>7__wrap13 != null)
    //                {
    //                    this.<>7__wrap13.Dispose();
    //                }
    //            Label_0567:
    //                this.<g>5__8.Dispose();
    //                this.<textRaster>5__9.Dispose();
    //            Label_0580:
    //                flag = false;
    //            }
    //            fault
    //            {
    //                ((IDisposable) this).Dispose();
    //            }
    //            return flag;
    //        }

    //        [DebuggerHidden]
    //        IEnumerator<List<Frame>> IEnumerable<List<Frame>>.GetEnumerator()
    //        {
    //            Generator.<GenerateTextFrames>d__5 d__;
    //            if (Interlocked.CompareExchange(ref this.<>1__state, 0, -2) == -2)
    //            {
    //                d__ = this;
    //            }
    //            else
    //            {
    //                d__ = new Generator.<GenerateTextFrames>d__5(0);
    //                d__.<>4__this = this.<>4__this;
    //            }
    //            d__.text = this.<>3__text;
    //            d__.maxYieldCount = this.<>3__maxYieldCount;
    //            d__.staticTextTimeLength = this.<>3__staticTextTimeLength;
    //            return d__;
    //        }

    //        [DebuggerHidden]
    //        IEnumerator IEnumerable.GetEnumerator()
    //        {
    //            return this.System.Collections.Generic.IEnumerable<System.Collections.Generic.List<LedTriksUtil.Frame>>.GetEnumerator();
    //        }

    //        [DebuggerHidden]
    //        void IEnumerator.Reset()
    //        {
    //            throw new NotSupportedException();
    //        }

    //        void IDisposable.Dispose()
    //        {
    //            switch (this.<>1__state)
    //            {
    //                case 1:
    //                case 2:
    //                    try
    //                    {
    //                    }
    //                    finally
    //                    {
    //                        this.<>1__state = -1;
    //                        if (this.<>7__wrapf != null)
    //                        {
    //                            this.<>7__wrapf.Dispose();
    //                        }
    //                    }
    //                    break;

    //                case 3:
    //                case 4:
    //                    try
    //                    {
    //                    }
    //                    finally
    //                    {
    //                        this.<>1__state = -1;
    //                        if (this.<>7__wrap10 != null)
    //                        {
    //                            this.<>7__wrap10.Dispose();
    //                        }
    //                    }
    //                    break;

    //                case 5:
    //                case 6:
    //                    try
    //                    {
    //                    }
    //                    finally
    //                    {
    //                        this.<>1__state = -1;
    //                        if (this.<>7__wrap11 != null)
    //                        {
    //                            this.<>7__wrap11.Dispose();
    //                        }
    //                    }
    //                    break;

    //                case 7:
    //                case 8:
    //                    try
    //                    {
    //                    }
    //                    finally
    //                    {
    //                        this.<>1__state = -1;
    //                        if (this.<>7__wrap12 != null)
    //                        {
    //                            this.<>7__wrap12.Dispose();
    //                        }
    //                    }
    //                    break;

    //                case 9:
    //                case 10:
    //                    try
    //                    {
    //                    }
    //                    finally
    //                    {
    //                        this.<>1__state = -1;
    //                        if (this.<>7__wrap13 != null)
    //                        {
    //                            this.<>7__wrap13.Dispose();
    //                        }
    //                    }
    //                    break;
    //            }
    //        }

    //        List<Frame> IEnumerator<List<Frame>>.Current
    //        {
    //            [DebuggerHidden]
    //            get
    //            {
    //                return this.<>2__current;
    //            }
    //        }

    //        object IEnumerator.Current
    //        {
    //            [DebuggerHidden]
    //            get
    //            {
    //                return this.<>2__current;
    //            }
    //        }
    //    }

    //    [CompilerGenerated]
    //    private sealed class <ScrollDown>d__27 : IEnumerable<List<Frame>>, IEnumerable, IEnumerator<List<Frame>>, IEnumerator, IDisposable
    //    {
    //        private int <>1__state;
    //        private List<Frame> <>2__current;
    //        public Graphics <>3__g;
    //        public int <>3__maxYieldCount;
    //        public string <>3__text;
    //        public Bitmap <>3__textRaster;
    //        public int <>3__y;
    //        public Generator <>4__this;
    //        public int <frameCount>5__29;
    //        public int <frameIndex>5__2a;
    //        public List<Frame> <frames>5__28;
    //        public Graphics g;
    //        public int maxYieldCount;
    //        public string text;
    //        public Bitmap textRaster;
    //        public int y;

    //        [DebuggerHidden]
    //        public <ScrollDown>d__27(int <>1__state)
    //        {
    //            this.<>1__state = <>1__state;
    //        }

    //        private bool MoveNext()
    //        {
    //            switch (this.<>1__state)
    //            {
    //                case 0:
    //                    this.<>1__state = -1;
    //                    this.<frames>5__28 = new List<Frame>();
    //                    this.<frameCount>5__29 = (this.<>4__this.TextScrollExtent == ScrollExtent.On) ? (this.<>4__this.m_fontHeight - this.<>4__this.m_tm.tmInternalLeading) : (this.<>4__this.m_fontHeight + this.textRaster.Height);
    //                    this.<frameIndex>5__2a = 0;
    //                    while (this.<frameIndex>5__2a < this.<frameCount>5__29)
    //                    {
    //                        this.g.FillRectangle(Brushes.White, 0, 0, this.textRaster.Width, this.textRaster.Height);
    //                        this.g.DrawString(this.text, this.<>4__this.TextFont, Brushes.Black, 0f, (float) (-this.<>4__this.m_fontHeight + (this.<frameIndex>5__2a + 1)));
    //                        this.<frames>5__28.Add(this.<>4__this.FrameFromBitmap(this.textRaster, this.<>4__this.FrameLength, this.y));
    //                        if (this.<frames>5__28.Count != this.maxYieldCount)
    //                        {
    //                            goto Label_016E;
    //                        }
    //                        this.<>2__current = this.<frames>5__28;
    //                        this.<>1__state = 1;
    //                        return true;
    //                    Label_015A:
    //                        this.<>1__state = -1;
    //                        this.<frames>5__28.Clear();
    //                    Label_016E:
    //                        this.<frameIndex>5__2a++;
    //                    }
    //                    if (this.maxYieldCount != 0)
    //                    {
    //                        break;
    //                    }
    //                    this.<>2__current = this.<frames>5__28;
    //                    this.<>1__state = 2;
    //                    return true;

    //                case 1:
    //                    goto Label_015A;

    //                case 2:
    //                    this.<>1__state = -1;
    //                    break;
    //            }
    //            return false;
    //        }

    //        [DebuggerHidden]
    //        IEnumerator<List<Frame>> IEnumerable<List<Frame>>.GetEnumerator()
    //        {
    //            Generator.<ScrollDown>d__27 d__;
    //            if (Interlocked.CompareExchange(ref this.<>1__state, 0, -2) == -2)
    //            {
    //                d__ = this;
    //            }
    //            else
    //            {
    //                d__ = new Generator.<ScrollDown>d__27(0);
    //                d__.<>4__this = this.<>4__this;
    //            }
    //            d__.text = this.<>3__text;
    //            d__.g = this.<>3__g;
    //            d__.textRaster = this.<>3__textRaster;
    //            d__.y = this.<>3__y;
    //            d__.maxYieldCount = this.<>3__maxYieldCount;
    //            return d__;
    //        }

    //        [DebuggerHidden]
    //        IEnumerator IEnumerable.GetEnumerator()
    //        {
    //            return this.System.Collections.Generic.IEnumerable<System.Collections.Generic.List<LedTriksUtil.Frame>>.GetEnumerator();
    //        }

    //        [DebuggerHidden]
    //        void IEnumerator.Reset()
    //        {
    //            throw new NotSupportedException();
    //        }

    //        void IDisposable.Dispose()
    //        {
    //        }

    //        List<Frame> IEnumerator<List<Frame>>.Current
    //        {
    //            [DebuggerHidden]
    //            get
    //            {
    //                return this.<>2__current;
    //            }
    //        }

    //        object IEnumerator.Current
    //        {
    //            [DebuggerHidden]
    //            get
    //            {
    //                return this.<>2__current;
    //            }
    //        }
    //    }

    //    [CompilerGenerated]
    //    private sealed class <ScrollLeft>d__1b : IEnumerable<List<Frame>>, IEnumerable, IEnumerator<List<Frame>>, IEnumerator, IDisposable
    //    {
    //        private int <>1__state;
    //        private List<Frame> <>2__current;
    //        public Graphics <>3__g;
    //        public int <>3__maxYieldCount;
    //        public string <>3__text;
    //        public Bitmap <>3__textRaster;
    //        public int <>3__y;
    //        public Generator <>4__this;
    //        public int <frameCount>5__1d;
    //        public int <frameIndex>5__1e;
    //        public List<Frame> <frames>5__1c;
    //        public Graphics g;
    //        public int maxYieldCount;
    //        public string text;
    //        public Bitmap textRaster;
    //        public int y;

    //        [DebuggerHidden]
    //        public <ScrollLeft>d__1b(int <>1__state)
    //        {
    //            this.<>1__state = <>1__state;
    //        }

    //        private bool MoveNext()
    //        {
    //            switch (this.<>1__state)
    //            {
    //                case 0:
    //                    this.<>1__state = -1;
    //                    this.<frames>5__1c = new List<Frame>();
    //                    this.<frameCount>5__1d = (this.<>4__this.TextScrollExtent == ScrollExtent.On) ? this.textRaster.Width : (this.textRaster.Width + this.<>4__this.m_stringPixelLength);
    //                    this.<frameIndex>5__1e = 0;
    //                    while (this.<frameIndex>5__1e < this.<frameCount>5__1d)
    //                    {
    //                        this.g.FillRectangle(Brushes.White, 0, 0, this.textRaster.Width, this.textRaster.Height);
    //                        this.g.DrawString(this.text, this.<>4__this.TextFont, Brushes.Black, (float) (this.textRaster.Width - (this.<frameIndex>5__1e + 1)), (float) -this.<>4__this.m_tm.tmInternalLeading);
    //                        this.<frames>5__1c.Add(this.<>4__this.FrameFromBitmap(this.textRaster, this.<>4__this.FrameLength, this.y));
    //                        if (this.<frames>5__1c.Count != this.maxYieldCount)
    //                        {
    //                            goto Label_0169;
    //                        }
    //                        this.<>2__current = this.<frames>5__1c;
    //                        this.<>1__state = 1;
    //                        return true;
    //                    Label_0155:
    //                        this.<>1__state = -1;
    //                        this.<frames>5__1c.Clear();
    //                    Label_0169:
    //                        this.<frameIndex>5__1e++;
    //                    }
    //                    if (this.maxYieldCount != 0)
    //                    {
    //                        break;
    //                    }
    //                    this.<>2__current = this.<frames>5__1c;
    //                    this.<>1__state = 2;
    //                    return true;

    //                case 1:
    //                    goto Label_0155;

    //                case 2:
    //                    this.<>1__state = -1;
    //                    break;
    //            }
    //            return false;
    //        }

    //        [DebuggerHidden]
    //        IEnumerator<List<Frame>> IEnumerable<List<Frame>>.GetEnumerator()
    //        {
    //            Generator.<ScrollLeft>d__1b d__b;
    //            if (Interlocked.CompareExchange(ref this.<>1__state, 0, -2) == -2)
    //            {
    //                d__b = this;
    //            }
    //            else
    //            {
    //                d__b = new Generator.<ScrollLeft>d__1b(0);
    //                d__b.<>4__this = this.<>4__this;
    //            }
    //            d__b.text = this.<>3__text;
    //            d__b.g = this.<>3__g;
    //            d__b.textRaster = this.<>3__textRaster;
    //            d__b.y = this.<>3__y;
    //            d__b.maxYieldCount = this.<>3__maxYieldCount;
    //            return d__b;
    //        }

    //        [DebuggerHidden]
    //        IEnumerator IEnumerable.GetEnumerator()
    //        {
    //            return this.System.Collections.Generic.IEnumerable<System.Collections.Generic.List<LedTriksUtil.Frame>>.GetEnumerator();
    //        }

    //        [DebuggerHidden]
    //        void IEnumerator.Reset()
    //        {
    //            throw new NotSupportedException();
    //        }

    //        void IDisposable.Dispose()
    //        {
    //        }

    //        List<Frame> IEnumerator<List<Frame>>.Current
    //        {
    //            [DebuggerHidden]
    //            get
    //            {
    //                return this.<>2__current;
    //            }
    //        }

    //        object IEnumerator.Current
    //        {
    //            [DebuggerHidden]
    //            get
    //            {
    //                return this.<>2__current;
    //            }
    //        }
    //    }

    //    [CompilerGenerated]
    //    private sealed class <ScrollNone>d__16 : IEnumerable<List<Frame>>, IEnumerable, IEnumerator<List<Frame>>, IEnumerator, IDisposable
    //    {
    //        private int <>1__state;
    //        private List<Frame> <>2__current;
    //        public Graphics <>3__g;
    //        public int <>3__maxYieldCount;
    //        public string <>3__text;
    //        public Bitmap <>3__textRaster;
    //        public int <>3__x;
    //        public int <>3__y;
    //        public Generator <>4__this;
    //        public int <frameIndex>5__18;
    //        public List<Frame> <frames>5__17;
    //        public Graphics g;
    //        public int maxYieldCount;
    //        public string text;
    //        public Bitmap textRaster;
    //        public int x;
    //        public int y;

    //        [DebuggerHidden]
    //        public <ScrollNone>d__16(int <>1__state)
    //        {
    //            this.<>1__state = <>1__state;
    //        }

    //        private bool MoveNext()
    //        {
    //            switch (this.<>1__state)
    //            {
    //                case 0:
    //                    this.<>1__state = -1;
    //                    this.<frames>5__17 = new List<Frame>();
    //                    this.<frameIndex>5__18 = 0;
    //                    while (this.<frameIndex>5__18 < this.<>4__this.m_staticCount)
    //                    {
    //                        this.g.FillRectangle(Brushes.White, 0, 0, this.textRaster.Width, this.textRaster.Height);
    //                        this.g.DrawString(this.text, this.<>4__this.TextFont, Brushes.Black, (float) this.x, (float) -this.<>4__this.m_tm.tmInternalLeading);
    //                        this.<frames>5__17.Add(this.<>4__this.FrameFromBitmap(this.textRaster, this.<>4__this.FrameLength, this.y));
    //                        if (this.<frames>5__17.Count != this.maxYieldCount)
    //                        {
    //                            goto Label_0124;
    //                        }
    //                        this.<>2__current = this.<frames>5__17;
    //                        this.<>1__state = 1;
    //                        return true;
    //                    Label_0110:
    //                        this.<>1__state = -1;
    //                        this.<frames>5__17.Clear();
    //                    Label_0124:
    //                        this.<frameIndex>5__18++;
    //                    }
    //                    if (this.maxYieldCount != 0)
    //                    {
    //                        break;
    //                    }
    //                    this.<>2__current = this.<frames>5__17;
    //                    this.<>1__state = 2;
    //                    return true;

    //                case 1:
    //                    goto Label_0110;

    //                case 2:
    //                    this.<>1__state = -1;
    //                    break;
    //            }
    //            return false;
    //        }

    //        [DebuggerHidden]
    //        IEnumerator<List<Frame>> IEnumerable<List<Frame>>.GetEnumerator()
    //        {
    //            Generator.<ScrollNone>d__16 d__;
    //            if (Interlocked.CompareExchange(ref this.<>1__state, 0, -2) == -2)
    //            {
    //                d__ = this;
    //            }
    //            else
    //            {
    //                d__ = new Generator.<ScrollNone>d__16(0);
    //                d__.<>4__this = this.<>4__this;
    //            }
    //            d__.text = this.<>3__text;
    //            d__.g = this.<>3__g;
    //            d__.textRaster = this.<>3__textRaster;
    //            d__.x = this.<>3__x;
    //            d__.y = this.<>3__y;
    //            d__.maxYieldCount = this.<>3__maxYieldCount;
    //            return d__;
    //        }

    //        [DebuggerHidden]
    //        IEnumerator IEnumerable.GetEnumerator()
    //        {
    //            return this.System.Collections.Generic.IEnumerable<System.Collections.Generic.List<LedTriksUtil.Frame>>.GetEnumerator();
    //        }

    //        [DebuggerHidden]
    //        void IEnumerator.Reset()
    //        {
    //            throw new NotSupportedException();
    //        }

    //        void IDisposable.Dispose()
    //        {
    //        }

    //        List<Frame> IEnumerator<List<Frame>>.Current
    //        {
    //            [DebuggerHidden]
    //            get
    //            {
    //                return this.<>2__current;
    //            }
    //        }

    //        object IEnumerator.Current
    //        {
    //            [DebuggerHidden]
    //            get
    //            {
    //                return this.<>2__current;
    //            }
    //        }
    //    }

    //    [CompilerGenerated]
    //    private sealed class <ScrollRight>d__21 : IEnumerable<List<Frame>>, IEnumerable, IEnumerator<List<Frame>>, IEnumerator, IDisposable
    //    {
    //        private int <>1__state;
    //        private List<Frame> <>2__current;
    //        public Graphics <>3__g;
    //        public int <>3__maxYieldCount;
    //        public string <>3__text;
    //        public Bitmap <>3__textRaster;
    //        public int <>3__y;
    //        public Generator <>4__this;
    //        public int <frameCount>5__23;
    //        public int <frameIndex>5__24;
    //        public List<Frame> <frames>5__22;
    //        public Graphics g;
    //        public int maxYieldCount;
    //        public string text;
    //        public Bitmap textRaster;
    //        public int y;

    //        [DebuggerHidden]
    //        public <ScrollRight>d__21(int <>1__state)
    //        {
    //            this.<>1__state = <>1__state;
    //        }

    //        private bool MoveNext()
    //        {
    //            switch (this.<>1__state)
    //            {
    //                case 0:
    //                    this.<>1__state = -1;
    //                    this.<frames>5__22 = new List<Frame>();
    //                    this.<frameCount>5__23 = (this.<>4__this.TextScrollExtent == ScrollExtent.On) ? this.<>4__this.m_stringPixelLength : (this.textRaster.Width + this.<>4__this.m_stringPixelLength);
    //                    this.<frameIndex>5__24 = 0;
    //                    while (this.<frameIndex>5__24 < this.<frameCount>5__23)
    //                    {
    //                        this.g.FillRectangle(Brushes.White, 0, 0, this.textRaster.Width, this.textRaster.Height);
    //                        this.g.DrawString(this.text, this.<>4__this.TextFont, Brushes.Black, (float) (-this.<>4__this.m_stringPixelLength + (this.<frameIndex>5__24 + 1)), (float) -this.<>4__this.m_tm.tmInternalLeading);
    //                        this.<frames>5__22.Add(this.<>4__this.FrameFromBitmap(this.textRaster, this.<>4__this.FrameLength, this.y));
    //                        if (this.<frames>5__22.Count != this.maxYieldCount)
    //                        {
    //                            goto Label_016A;
    //                        }
    //                        this.<>2__current = this.<frames>5__22;
    //                        this.<>1__state = 1;
    //                        return true;
    //                    Label_0156:
    //                        this.<>1__state = -1;
    //                        this.<frames>5__22.Clear();
    //                    Label_016A:
    //                        this.<frameIndex>5__24++;
    //                    }
    //                    if (this.maxYieldCount != 0)
    //                    {
    //                        break;
    //                    }
    //                    this.<>2__current = this.<frames>5__22;
    //                    this.<>1__state = 2;
    //                    return true;

    //                case 1:
    //                    goto Label_0156;

    //                case 2:
    //                    this.<>1__state = -1;
    //                    break;
    //            }
    //            return false;
    //        }

    //        [DebuggerHidden]
    //        IEnumerator<List<Frame>> IEnumerable<List<Frame>>.GetEnumerator()
    //        {
    //            Generator.<ScrollRight>d__21 d__;
    //            if (Interlocked.CompareExchange(ref this.<>1__state, 0, -2) == -2)
    //            {
    //                d__ = this;
    //            }
    //            else
    //            {
    //                d__ = new Generator.<ScrollRight>d__21(0);
    //                d__.<>4__this = this.<>4__this;
    //            }
    //            d__.text = this.<>3__text;
    //            d__.g = this.<>3__g;
    //            d__.textRaster = this.<>3__textRaster;
    //            d__.y = this.<>3__y;
    //            d__.maxYieldCount = this.<>3__maxYieldCount;
    //            return d__;
    //        }

    //        [DebuggerHidden]
    //        IEnumerator IEnumerable.GetEnumerator()
    //        {
    //            return this.System.Collections.Generic.IEnumerable<System.Collections.Generic.List<LedTriksUtil.Frame>>.GetEnumerator();
    //        }

    //        [DebuggerHidden]
    //        void IEnumerator.Reset()
    //        {
    //            throw new NotSupportedException();
    //        }

    //        void IDisposable.Dispose()
    //        {
    //        }

    //        List<Frame> IEnumerator<List<Frame>>.Current
    //        {
    //            [DebuggerHidden]
    //            get
    //            {
    //                return this.<>2__current;
    //            }
    //        }

    //        object IEnumerator.Current
    //        {
    //            [DebuggerHidden]
    //            get
    //            {
    //                return this.<>2__current;
    //            }
    //        }
    //    }

    //    [CompilerGenerated]
    //    private sealed class <ScrollUp>d__2d : IEnumerable<List<Frame>>, IEnumerable, IEnumerator<List<Frame>>, IEnumerator, IDisposable
    //    {
    //        private int <>1__state;
    //        private List<Frame> <>2__current;
    //        public Graphics <>3__g;
    //        public int <>3__maxYieldCount;
    //        public string <>3__text;
    //        public Bitmap <>3__textRaster;
    //        public int <>3__y;
    //        public Generator <>4__this;
    //        public int <frameCount>5__2f;
    //        public int <frameIndex>5__30;
    //        public List<Frame> <frames>5__2e;
    //        public Graphics g;
    //        public int maxYieldCount;
    //        public string text;
    //        public Bitmap textRaster;
    //        public int y;

    //        [DebuggerHidden]
    //        public <ScrollUp>d__2d(int <>1__state)
    //        {
    //            this.<>1__state = <>1__state;
    //        }

    //        private bool MoveNext()
    //        {
    //            switch (this.<>1__state)
    //            {
    //                case 0:
    //                    this.<>1__state = -1;
    //                    this.<frames>5__2e = new List<Frame>();
    //                    this.<frameCount>5__2f = (this.<>4__this.TextScrollExtent == ScrollExtent.On) ? (this.textRaster.Height + this.<>4__this.m_tm.tmInternalLeading) : (this.<>4__this.m_fontHeight + this.textRaster.Height);
    //                    this.<frameIndex>5__30 = 0;
    //                    while (this.<frameIndex>5__30 < this.<frameCount>5__2f)
    //                    {
    //                        this.g.FillRectangle(Brushes.White, 0, 0, this.textRaster.Width, this.textRaster.Height);
    //                        this.g.DrawString(this.text, this.<>4__this.TextFont, Brushes.Black, 0f, (float) (this.textRaster.Height - (this.<frameIndex>5__30 + 1)));
    //                        this.<frames>5__2e.Add(this.<>4__this.FrameFromBitmap(this.textRaster, this.<>4__this.FrameLength, this.y));
    //                        if (this.<frames>5__2e.Count != this.maxYieldCount)
    //                        {
    //                            goto Label_016D;
    //                        }
    //                        this.<>2__current = this.<frames>5__2e;
    //                        this.<>1__state = 1;
    //                        return true;
    //                    Label_0159:
    //                        this.<>1__state = -1;
    //                        this.<frames>5__2e.Clear();
    //                    Label_016D:
    //                        this.<frameIndex>5__30++;
    //                    }
    //                    if (this.maxYieldCount != 0)
    //                    {
    //                        break;
    //                    }
    //                    this.<>2__current = this.<frames>5__2e;
    //                    this.<>1__state = 2;
    //                    return true;

    //                case 1:
    //                    goto Label_0159;

    //                case 2:
    //                    this.<>1__state = -1;
    //                    break;
    //            }
    //            return false;
    //        }

    //        [DebuggerHidden]
    //        IEnumerator<List<Frame>> IEnumerable<List<Frame>>.GetEnumerator()
    //        {
    //            Generator.<ScrollUp>d__2d d__d;
    //            if (Interlocked.CompareExchange(ref this.<>1__state, 0, -2) == -2)
    //            {
    //                d__d = this;
    //            }
    //            else
    //            {
    //                d__d = new Generator.<ScrollUp>d__2d(0);
    //                d__d.<>4__this = this.<>4__this;
    //            }
    //            d__d.text = this.<>3__text;
    //            d__d.g = this.<>3__g;
    //            d__d.textRaster = this.<>3__textRaster;
    //            d__d.y = this.<>3__y;
    //            d__d.maxYieldCount = this.<>3__maxYieldCount;
    //            return d__d;
    //        }

    //        [DebuggerHidden]
    //        IEnumerator IEnumerable.GetEnumerator()
    //        {
    //            return this.System.Collections.Generic.IEnumerable<System.Collections.Generic.List<LedTriksUtil.Frame>>.GetEnumerator();
    //        }

    //        [DebuggerHidden]
    //        void IEnumerator.Reset()
    //        {
    //            throw new NotSupportedException();
    //        }

    //        void IDisposable.Dispose()
    //        {
    //        }

    //        List<Frame> IEnumerator<List<Frame>>.Current
    //        {
    //            [DebuggerHidden]
    //            get
    //            {
    //                return this.<>2__current;
    //            }
    //        }

    //        object IEnumerator.Current
    //        {
    //            [DebuggerHidden]
    //            get
    //            {
    //                return this.<>2__current;
    //            }
    //        }
    //    }
    }
}

