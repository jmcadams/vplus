namespace LedTriks
{
    using LedTriksUtil;
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using Vixen;

    public class Context : IDisposable
    {
        private const int BOARD_HEIGHT = 0x10;
        private const int BOARD_WIDTH = 0x30;
        private Size m_boardLayout = Size.Empty;
        private string m_fontName = "Arial";
        private System.Drawing.FontStyle m_fontStyle = System.Drawing.FontStyle.Regular;
        private List<Frame> m_frameBuffer;
        private LedTriksUtil.Generator m_generator = new LedTriksUtil.Generator();
        private RunState m_runState = RunState.Stopped;

        public Context()
        {
            this.State = RunState.Running;
        }

        public void Dispose()
        {
            this.Stop();
            GC.SuppressFinalize(this);
        }

        ~Context()
        {
            this.Dispose();
        }

        public void Stop()
        {
            if (this.State == RunState.Running)
            {
                this.State = RunState.Stopping;
            }
        }

        public Size BoardLayout
        {
            get
            {
                if (this.m_boardLayout == Size.Empty)
                {
                    this.m_boardLayout = new Size(this.m_generator.BoardPixelWidth / 0x30, this.m_generator.BoardPixelHeight / 0x10);
                }
                return this.m_boardLayout;
            }
            set
            {
                this.m_generator.BoardPixelHeight = value.Height * 0x10;
                this.m_generator.BoardPixelWidth = value.Width * 0x30;
                this.m_boardLayout = new Size(value.Width, value.Height);
            }
        }

        public string FontName
        {
            get
            {
                return this.m_fontName;
            }
            set
            {
                this.m_fontName = value;
            }
        }

        public System.Drawing.FontStyle FontStyle
        {
            get
            {
                return this.m_fontStyle;
            }
            set
            {
                this.m_fontStyle = value;
            }
        }

        public List<Frame> FrameBuffer
        {
            get
            {
                return this.m_frameBuffer;
            }
        }

        public int FrameLength
        {
            get
            {
                return this.m_generator.FrameLength;
            }
            set
            {
                this.m_generator.FrameLength = value;
            }
        }

        public LedTriksUtil.Generator Generator
        {
            get
            {
                return this.m_generator;
            }
        }

        public bool IgnoreFontDescent
        {
            get
            {
                return this.m_generator.IgnoreFontDescent;
            }
            set
            {
                this.m_generator.IgnoreFontDescent = value;
            }
        }

        public EventSequence Sequence
        {
            set
            {
                this.m_frameBuffer = (List<Frame>) value.UserData;
            }
        }

        public RunState State
        {
            get
            {
                return this.m_runState;
            }
            set
            {
                this.m_runState = value;
                if (value == RunState.Stopping)
                {
                    if (this.m_generator != null)
                    {
                        this.m_generator.Dispose();
                        this.m_generator = null;
                    }
                    this.m_runState = RunState.Stopped;
                }
            }
        }

        public int TextHeight
        {
            get
            {
                return this.m_generator.TextHeight;
            }
            set
            {
                this.m_generator.TextHeight = value;
            }
        }

        public HorzPosition TextHorzPosition
        {
            get
            {
                return this.m_generator.TextHorzPosition;
            }
            set
            {
                this.m_generator.TextHorzPosition = value;
            }
        }

        public int TextHorzPositionValue
        {
            get
            {
                return this.m_generator.TextHorzPositionValue;
            }
            set
            {
                this.m_generator.TextHorzPositionValue = value;
            }
        }

        public ScrollDirection TextScrollDirection
        {
            get
            {
                return this.m_generator.TextScrollDirection;
            }
            set
            {
                this.m_generator.TextScrollDirection = value;
            }
        }

        public ScrollExtent TextScrollExtent
        {
            get
            {
                return this.m_generator.TextScrollExtent;
            }
            set
            {
                this.m_generator.TextScrollExtent = value;
            }
        }

        public VertPosition TextVertPosition
        {
            get
            {
                return this.m_generator.TextVertPosition;
            }
            set
            {
                this.m_generator.TextVertPosition = value;
            }
        }

        public int TextVertPositionValue
        {
            get
            {
                return this.m_generator.TextVertPositionValue;
            }
            set
            {
                this.m_generator.TextVertPositionValue = value;
            }
        }

        public enum RunState
        {
            Running,
            Stopping,
            Stopped
        }
    }
}

