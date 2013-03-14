namespace Spectrum
{
    using System;
    using System.Drawing;

    internal class FrequencyBand
    {
        private static float BAND_WIDTH = (0x5622 / SPECTRUMSIZE);
        private static double FREQUENCY_BANDWIDTH_MULTIPLIER = Math.Pow(2.0, 0.16666666666666666);
        private string m_centerFrequency;
        private float m_currentScaledLevel;
        private int m_fmodHighFrequencyIndex;
        private int m_fmodLowFrequencyIndex;
        private int m_highFrequency;
        private int m_lowFrequency;
        private int m_mouseOffset = 0;
        private Rectangle m_region;
        private float m_responseLevel = 0f;
        private Rectangle m_sliderRegion;
        private static int SPECTRUMSIZE = 0x200;

        public FrequencyBand(int centerFrequency)
        {
            this.m_centerFrequency = (centerFrequency < 0x3e8) ? centerFrequency.ToString() : string.Format("{0:F1}k", ((float) centerFrequency) / 1000f);
            this.m_lowFrequency = (int) (((double) centerFrequency) / FREQUENCY_BANDWIDTH_MULTIPLIER);
            this.m_highFrequency = (int) (centerFrequency * FREQUENCY_BANDWIDTH_MULTIPLIER);
            this.m_fmodLowFrequencyIndex = (int) Math.Floor((double) (((float) this.m_lowFrequency) / BAND_WIDTH));
            this.m_fmodHighFrequencyIndex = Math.Min((int) Math.Ceiling((double) (((float) this.m_highFrequency) / BAND_WIDTH)), SPECTRUMSIZE);
        }

        public override string ToString()
        {
            return this.m_centerFrequency;
        }

        public string CenterFrequency
        {
            get
            {
                return this.m_centerFrequency;
            }
        }

        public float CurrentScaledLevel
        {
            get
            {
                return this.m_currentScaledLevel;
            }
            set
            {
                this.m_currentScaledLevel = value;
            }
        }

        public int FmodHighFrequency
        {
            get
            {
                return this.m_fmodHighFrequencyIndex;
            }
        }

        public int FmodLowFrequency
        {
            get
            {
                return this.m_fmodLowFrequencyIndex;
            }
        }

        public int MouseOffset
        {
            get
            {
                return this.m_mouseOffset;
            }
            set
            {
                this.m_mouseOffset = value;
            }
        }

        public Rectangle Region
        {
            get
            {
                return this.m_region;
            }
            set
            {
                this.m_region = value;
                this.ResponseLevel = this.m_responseLevel;
            }
        }

        public float ResponseLevel
        {
            get
            {
                return this.m_responseLevel;
            }
            set
            {
                this.m_responseLevel = value;
                this.m_sliderRegion.Y = (this.m_region.Bottom - ((int) (this.m_region.Height * value))) - this.m_sliderRegion.Height;
            }
        }

        public Rectangle SliderRegion
        {
            get
            {
                return this.m_sliderRegion;
            }
            set
            {
                this.m_sliderRegion = value;
                this.ResponseLevel = this.m_responseLevel;
            }
        }

        public int SliderTop
        {
            get
            {
                return this.m_sliderRegion.Y;
            }
            set
            {
                if ((value >= this.m_region.Top) && (value <= (this.m_region.Bottom - this.m_sliderRegion.Height)))
                {
                    this.ResponseLevel = ((float) (this.m_region.Bottom - (value + this.m_sliderRegion.Height))) / ((float) this.m_region.Height);
                }
            }
        }
    }
}

