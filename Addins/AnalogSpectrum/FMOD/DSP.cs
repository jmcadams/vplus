namespace FMOD
{
    using System;
    using System.Runtime.InteropServices;
    using System.Text;

    public class DSP
    {
        private IntPtr dspraw;

        public RESULT addInput(DSP target)
        {
            return FMOD_DSP_AddInput(this.dspraw, target.getRaw());
        }

        public RESULT disconnectFrom(DSP target)
        {
            return FMOD_DSP_DisconnectFrom(this.dspraw, target.getRaw());
        }

        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_DSP_AddInput(IntPtr dsp, IntPtr target);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_DSP_DisconnectFrom(IntPtr dsp, IntPtr target);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_DSP_GetActive(IntPtr dsp, ref bool active);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_DSP_GetBypass(IntPtr dsp, ref bool bypass);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_DSP_GetDefaults(IntPtr dsp, ref float frequency, ref float volume, ref float pan, ref int priority);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_DSP_GetInfo(IntPtr dsp, StringBuilder name, ref uint version, ref int channels, ref int configwidth, ref int configheight);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_DSP_GetInput(IntPtr dsp, int index, ref IntPtr input);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_DSP_GetInputLevels(IntPtr dsp, int index, SPEAKER speaker, float[] levels, int numlevels);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_DSP_GetInputMix(IntPtr dsp, int index, ref float volume);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_DSP_GetNumInputs(IntPtr dsp, ref int numinputs);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_DSP_GetNumOutputs(IntPtr dsp, ref int numoutputs);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_DSP_GetNumParameters(IntPtr dsp, ref int numparams);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_DSP_GetOutput(IntPtr dsp, int index, ref IntPtr output);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_DSP_GetParameter(IntPtr dsp, int index, ref float val, StringBuilder valuestr, int valuestrlen);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_DSP_GetParameterInfo(IntPtr dsp, int index, StringBuilder name, StringBuilder label, StringBuilder description, int descriptionlen, ref float min, ref float max);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_DSP_GetSystemObject(IntPtr dsp, ref IntPtr system);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_DSP_GetUserData(IntPtr dsp, ref IntPtr userdata);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_DSP_Release(IntPtr dsp);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_DSP_Remove(IntPtr dsp);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_DSP_Reset(IntPtr dsp);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_DSP_SetActive(IntPtr dsp, bool active);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_DSP_SetBypass(IntPtr dsp, bool bypass);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_DSP_SetDefaults(IntPtr dsp, float frequency, float volume, float pan, int priority);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_DSP_SetInputLevels(IntPtr dsp, int index, SPEAKER speaker, float[] levels, int numlevels);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_DSP_SetInputMix(IntPtr dsp, int index, float volume);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_DSP_SetParameter(IntPtr dsp, int index, float val);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_DSP_SetUserData(IntPtr dsp, IntPtr userdata);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_DSP_ShowConfigDialog(IntPtr dsp, IntPtr hwnd, bool show);
        public RESULT getActive(ref bool active)
        {
            return FMOD_DSP_GetActive(this.dspraw, ref active);
        }

        public RESULT getBypass(ref bool bypass)
        {
            return FMOD_DSP_GetBypass(this.dspraw, ref bypass);
        }

        public RESULT getDefaults(ref float frequency, ref float volume, ref float pan, ref int priority)
        {
            return FMOD_DSP_GetDefaults(this.dspraw, ref frequency, ref volume, ref pan, ref priority);
        }

        public RESULT getInfo(StringBuilder name, ref uint version, ref int channels, ref int configwidth, ref int configheight)
        {
            return FMOD_DSP_GetInfo(this.dspraw, name, ref version, ref channels, ref configwidth, ref configheight);
        }

        public RESULT getInput(int index, ref DSP input)
        {
            RESULT oK = RESULT.OK;
            IntPtr ptr = new IntPtr();
            DSP dsp = null;
            try
            {
                oK = FMOD_DSP_GetInput(this.dspraw, index, ref ptr);
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

        public RESULT getInputLevels(int index, SPEAKER speaker, float[] levels, int numlevels)
        {
            return FMOD_DSP_GetInputLevels(this.dspraw, index, speaker, levels, numlevels);
        }

        public RESULT getInputMix(int index, ref float volume)
        {
            return FMOD_DSP_GetInputMix(this.dspraw, index, ref volume);
        }

        public RESULT getNumInputs(ref int numinputs)
        {
            return FMOD_DSP_GetNumInputs(this.dspraw, ref numinputs);
        }

        public RESULT getNumOutputs(ref int numoutputs)
        {
            return FMOD_DSP_GetNumOutputs(this.dspraw, ref numoutputs);
        }

        public RESULT getNumParameters(ref int numparams)
        {
            return FMOD_DSP_GetNumParameters(this.dspraw, ref numparams);
        }

        public RESULT getOutput(int index, ref DSP output)
        {
            RESULT oK = RESULT.OK;
            IntPtr ptr = new IntPtr();
            DSP dsp = null;
            try
            {
                oK = FMOD_DSP_GetOutput(this.dspraw, index, ref ptr);
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

        public RESULT getParameter(int index, ref float val, StringBuilder valuestr, int valuestrlen)
        {
            return FMOD_DSP_GetParameter(this.dspraw, index, ref val, valuestr, valuestrlen);
        }

        public RESULT getParameterInfo(int index, StringBuilder name, StringBuilder label, StringBuilder description, int descriptionlen, ref float min, ref float max)
        {
            return FMOD_DSP_GetParameterInfo(this.dspraw, index, name, label, description, descriptionlen, ref min, ref max);
        }

        public IntPtr getRaw()
        {
            return this.dspraw;
        }

        public RESULT getSystemObject(ref System system)
        {
            RESULT oK = RESULT.OK;
            IntPtr ptr = new IntPtr();
            System system2 = null;
            try
            {
                oK = FMOD_DSP_GetSystemObject(this.dspraw, ref ptr);
            }
            catch
            {
                oK = RESULT.ERR_INVALID_PARAM;
            }
            if (oK == RESULT.OK)
            {
                if (system == null)
                {
                    system2 = new System();
                    system2.setRaw(this.dspraw);
                    system = system2;
                }
                else
                {
                    system.setRaw(ptr);
                }
            }
            return oK;
        }

        public RESULT getUserData(ref IntPtr userdata)
        {
            return FMOD_DSP_GetUserData(this.dspraw, ref userdata);
        }

        public RESULT release()
        {
            return FMOD_DSP_Release(this.dspraw);
        }

        public RESULT remove()
        {
            return FMOD_DSP_Remove(this.dspraw);
        }

        public RESULT reset()
        {
            return FMOD_DSP_Reset(this.dspraw);
        }

        public RESULT setActive(bool active)
        {
            return FMOD_DSP_SetActive(this.dspraw, active);
        }

        public RESULT setBypass(bool bypass)
        {
            return FMOD_DSP_SetBypass(this.dspraw, bypass);
        }

        public RESULT setDefaults(float frequency, float volume, float pan, int priority)
        {
            return FMOD_DSP_SetDefaults(this.dspraw, frequency, volume, pan, priority);
        }

        public RESULT setInputLevels(int index, SPEAKER speaker, float[] levels, int numlevels)
        {
            return FMOD_DSP_SetInputLevels(this.dspraw, index, speaker, levels, numlevels);
        }

        public RESULT setInputMix(int index, float volume)
        {
            return FMOD_DSP_SetInputMix(this.dspraw, index, volume);
        }

        public RESULT setParameter(int index, float val)
        {
            return FMOD_DSP_SetParameter(this.dspraw, index, val);
        }

        public void setRaw(IntPtr dsp)
        {
            this.dspraw = new IntPtr();
            this.dspraw = dsp;
        }

        public RESULT setUserData(IntPtr userdata)
        {
            return FMOD_DSP_SetUserData(this.dspraw, userdata);
        }

        public RESULT showConfigDialog(IntPtr hwnd, bool show)
        {
            return FMOD_DSP_ShowConfigDialog(this.dspraw, hwnd, show);
        }
    }
}

