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
        private Rectangle m_maxSliderRegion;
        private Rectangle m_minSliderRegion;
        private int m_mouseOffset = 0;
        private Rectangle m_region;
        private float m_responseLevelMax = 1f;
        private float m_responseLevelMin = 0f;
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

        public Rectangle MaxSliderRegion
        {
            get
            {
                return this.m_maxSliderRegion;
            }
            set
            {
                this.m_maxSliderRegion = value;
                this.ResponseLevelMax = this.m_responseLevelMax;
            }
        }

        public int MaxSliderTop
        {
            get
            {
                return this.m_maxSliderRegion.Y;
            }
            set
            {
                if ((value >= this.m_region.Top) && (value <= (this.m_region.Bottom - this.m_maxSliderRegion.Height)))
                {
                    this.ResponseLevelMax = ((float) (this.m_region.Bottom - value)) / ((float) this.m_region.Height);
                }
            }
        }

        public Rectangle MinSliderRegion
        {
            get
            {
                return this.m_minSliderRegion;
            }
            set
            {
                this.m_minSliderRegion = value;
                this.ResponseLevelMin = this.m_responseLevelMin;
            }
        }

        public int MinSliderTop
        {
            get
            {
                return this.m_minSliderRegion.Y;
            }
            set
            {
                if ((value >= this.m_region.Top) && (value <= (this.m_region.Bottom - this.m_minSliderRegion.Height)))
                {
                    this.ResponseLevelMin = ((float) (this.m_region.Bottom - (value + this.m_minSliderRegion.Height))) / ((float) this.m_region.Height);
                }
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
                this.ResponseLevelMin = this.m_responseLevelMin;
                this.ResponseLevelMax = this.m_responseLevelMax;
            }
        }

        public float ResponseLevelMax
        {
            get
            {
                return this.m_responseLevelMax;
            }
            set
            {
                int num = (this.m_region.Bottom - ((int) (this.m_region.Height * value))) + this.m_maxSliderRegion.Height;
                if (num >= this.m_minSliderRegion.Top)
                {
                    float num2 = 1f / ((float) this.m_region.Height);
                    value = ((this.m_region.Bottom - this.m_minSliderRegion.Top) + this.m_maxSliderRegion.Height) * num2;
                }
                this.m_responseLevelMax = value;
                this.m_maxSliderRegion.Y = this.m_region.Bottom - ((int) (this.m_region.Height * value));
            }
        }

        public float ResponseLevelMin
        {
            get
            {
                return this.m_responseLevelMin;
            }
            set
            {
                int num = (this.m_region.Bottom - ((int) (this.m_region.Height * value))) - this.m_minSliderRegion.Height;
                if (num <= this.m_maxSliderRegion.Bottom)
                {
                    float num2 = 1f / ((float) this.m_region.Height);
                    value = ((this.m_region.Bottom - this.m_maxSliderRegion.Bottom) - this.m_minSliderRegion.Height) * num2;
                }
                this.m_responseLevelMin = value;
                this.m_minSliderRegion.Y = (this.m_region.Bottom - ((int) (this.m_region.Height * value))) - this.m_minSliderRegion.Height;
            }
        }
    }
}

