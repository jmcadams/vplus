using System.Globalization;

namespace Spectrum
{
    using System;
    using System.Drawing;

    internal class FrequencyBand
    {
	    private const int Spectrumsize = 512;
	    private const float BandWidth = (22050/Spectrumsize);
	    private static readonly double FrequencyBandwidthMultiplier = Math.Pow(2.0, 0.16666666666666666);
        private readonly string _mCenterFrequency;
	    private readonly int _mFmodHighFrequencyIndex;
        private readonly int _mFmodLowFrequencyIndex;
        private readonly int _mHighFrequency;
        private readonly int _mLowFrequency;
        private Rectangle _mMaxSliderRegion;
        private Rectangle _mMinSliderRegion;
	    private Rectangle _mRegion;
        private float _mResponseLevelMax = 1f;
        private float _mResponseLevelMin;


        public FrequencyBand(int centerFrequency)
        {
            _mCenterFrequency = (centerFrequency < 1000) ? centerFrequency.ToString(CultureInfo.InvariantCulture) : string.Format("{0:F1}k", centerFrequency / 1000f);
            _mLowFrequency = (int) (centerFrequency / FrequencyBandwidthMultiplier);
            _mHighFrequency = (int) (centerFrequency * FrequencyBandwidthMultiplier);
            _mFmodLowFrequencyIndex = (int) Math.Floor(_mLowFrequency / BandWidth);
            _mFmodHighFrequencyIndex = Math.Min((int) Math.Ceiling(_mHighFrequency / BandWidth), Spectrumsize);
        }

        public override string ToString()
        {
            return _mCenterFrequency;
        }

        public string CenterFrequency
        {
            get
            {
                return _mCenterFrequency;
            }
        }

	    public float CurrentScaledLevel { get; set; }

	    public int FmodHighFrequency
        {
            get
            {
                return _mFmodHighFrequencyIndex;
            }
        }

        public int FmodLowFrequency
        {
            get
            {
                return _mFmodLowFrequencyIndex;
            }
        }

        public Rectangle MaxSliderRegion
        {
            get
            {
                return _mMaxSliderRegion;
            }
            set
            {
                _mMaxSliderRegion = value;
                ResponseLevelMax = _mResponseLevelMax;
            }
        }

        public int MaxSliderTop
        {
            get
            {
                return _mMaxSliderRegion.Y;
            }
            set
            {
                if ((value >= _mRegion.Top) && (value <= _mRegion.Bottom - _mMaxSliderRegion.Height))
                {
                    ResponseLevelMax = (_mRegion.Bottom - value) / ((float) _mRegion.Height);
                }
            }
        }

        public Rectangle MinSliderRegion
        {
            get
            {
                return _mMinSliderRegion;
            }
            set
            {
                _mMinSliderRegion = value;
                ResponseLevelMin = _mResponseLevelMin;
            }
        }

        public int MinSliderTop
        {
            get
            {
                return _mMinSliderRegion.Y;
            }
            set
            {
                if ((value >= _mRegion.Top) && (value <= (_mRegion.Bottom - _mMinSliderRegion.Height)))
                {
                    ResponseLevelMin = (_mRegion.Bottom - (value + _mMinSliderRegion.Height)) / ((float) _mRegion.Height);
                }
            }
        }

	    public int MouseOffset { get; set; }

	    public Rectangle Region
        {
            get
            {
                return _mRegion;
            }
            set
            {
                _mRegion = value;
                ResponseLevelMin = _mResponseLevelMin;
                ResponseLevelMax = _mResponseLevelMax;
            }
        }

        public float ResponseLevelMax
        {
            get
            {
                return _mResponseLevelMax;
            }
            set
            {
                int num = (_mRegion.Bottom - ((int) (_mRegion.Height * value))) + _mMaxSliderRegion.Height;
                if (num >= _mMinSliderRegion.Top)
                {
                    float num2 = 1f / _mRegion.Height;
                    value = ((_mRegion.Bottom - _mMinSliderRegion.Top) + _mMaxSliderRegion.Height) * num2;
                }
                _mResponseLevelMax = value;
                _mMaxSliderRegion.Y = _mRegion.Bottom - ((int) (_mRegion.Height * value));
            }
        }

        public float ResponseLevelMin
        {
            get
            {
                return _mResponseLevelMin;
            }
            set
            {
                int num = (_mRegion.Bottom - ((int) (_mRegion.Height * value))) - _mMinSliderRegion.Height;
                if (num <= _mMaxSliderRegion.Bottom)
                {
                    float num2 = 1f / _mRegion.Height;
                    value = ((_mRegion.Bottom - _mMaxSliderRegion.Bottom) - _mMinSliderRegion.Height) * num2;
                }
                _mResponseLevelMin = value;
                _mMinSliderRegion.Y = (_mRegion.Bottom - ((int) (_mRegion.Height * value))) - _mMinSliderRegion.Height;
            }
        }
    }
}

