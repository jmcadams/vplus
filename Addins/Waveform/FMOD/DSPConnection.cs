namespace FMOD
{
    using System;
    using System.Runtime.InteropServices;

    public class DSPConnection
    {
        private IntPtr dspconnectionraw;

        [DllImport("fmodex")]
        private static extern RESULT FMOD_DSPConnection_GetInput(IntPtr dspconnection, ref IntPtr input);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_DSPConnection_GetLevels(IntPtr dspconnection, SPEAKER speaker, float[] levels, int numlevels);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_DSPConnection_GetMix(IntPtr dspconnection, ref float volume);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_DSPConnection_GetOutput(IntPtr dspconnection, ref IntPtr output);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_DSPConnection_GetUserData(IntPtr dspconnection, ref IntPtr userdata);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_DSPConnection_SetLevels(IntPtr dspconnection, SPEAKER speaker, float[] levels, int numlevels);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_DSPConnection_SetMix(IntPtr dspconnection, float volume);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_DSPConnection_SetUserData(IntPtr dspconnection, IntPtr userdata);
        public RESULT getInput(ref DSP input)
        {
            RESULT oK = RESULT.OK;
            IntPtr ptr = new IntPtr();
            DSP dsp = null;
            try
            {
                oK = FMOD_DSPConnection_GetInput(this.dspconnectionraw, ref ptr);
            }
            catch
            {
                oK = RESULT.ERR_INVALID_PARAM;
            }
            if (oK == RESULT.OK)
            {
                if (input == null)
                {
                    dsp = new DSP();
                    dsp.setRaw(ptr);
                    input = dsp;
                }
                else
                {
                    input.setRaw(ptr);
                }
            }
            return oK;
        }

        public RESULT getLevels(SPEAKER speaker, float[] levels, int numlevels)
        {
            return FMOD_DSPConnection_GetLevels(this.dspconnectionraw, speaker, levels, numlevels);
        }

        public RESULT getMix(ref float volume)
        {
            return FMOD_DSPConnection_GetMix(this.dspconnectionraw, ref volume);
        }

        public RESULT getOutput(ref DSP output)
        {
            RESULT oK = RESULT.OK;
            IntPtr ptr = new IntPtr();
            DSP dsp = null;
            try
            {
                oK = FMOD_DSPConnection_GetOutput(this.dspconnectionraw, ref ptr);
            }
            catch
            {
                oK = RESULT.ERR_INVALID_PARAM;
            }
            if (oK == RESULT.OK)
            {
                if (output == null)
                {
                    dsp = new DSP();
                    dsp.setRaw(ptr);
                    output = dsp;
                }
                else
                {
                    output.setRaw(ptr);
                }
            }
            return oK;
        }

        public IntPtr getRaw()
        {
            return this.dspconnectionraw;
        }

        public RESULT getUserData(ref IntPtr userdata)
        {
            return FMOD_DSPConnection_GetUserData(this.dspconnectionraw, ref userdata);
        }

        public RESULT setLevels(SPEAKER speaker, float[] levels, int numlevels)
        {
            return FMOD_DSPConnection_SetLevels(this.dspconnectionraw, speaker, levels, numlevels);
        }

        public RESULT setMix(float volume)
        {
            return FMOD_DSPConnection_SetMix(this.dspconnectionraw, volume);
        }

        public void setRaw(IntPtr dspconnection)
        {
            this.dspconnectionraw = new IntPtr();
            this.dspconnectionraw = dspconnection;
        }

        public RESULT setUserData(IntPtr userdata)
        {
            return FMOD_DSPConnection_SetUserData(this.dspconnectionraw, userdata);
        }
    }
}

