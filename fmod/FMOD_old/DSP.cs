namespace FMOD
{
    using System;
    using System.Runtime.InteropServices;
    using System.Text;

    public class DSP
    {
        private IntPtr dspraw;

        public RESULT addInput(DSP target, ref DSPConnection dspconnection)
        {
            RESULT oK = RESULT.OK;
            IntPtr ptr = new IntPtr();
            DSPConnection connection = null;
            try
            {
                oK = FMOD_DSP_AddInput(this.dspraw, target.getRaw(), ref ptr);
            }
            catch
            {
                oK = RESULT.ERR_INVALID_PARAM;
            }
            if (oK == RESULT.OK)
            {
                if (dspconnection == null)
                {
                    connection = new DSPConnection();
                    connection.setRaw(ptr);
                    dspconnection = connection;
                }
                else
                {
                    dspconnection.setRaw(ptr);
                }
            }
            return oK;
        }

        public RESULT disconnectAll(bool inputs, bool outputs)
        {
            return FMOD_DSP_DisconnectAll(this.dspraw, inputs ? 1 : 0, outputs ? 1 : 0);
        }

        public RESULT disconnectFrom(DSP target)
        {
            return FMOD_DSP_DisconnectFrom(this.dspraw, target.getRaw());
        }

        [DllImport("fmodex")]
        private static extern RESULT FMOD_DSP_AddInput(IntPtr dsp, IntPtr target, ref IntPtr dspconnection);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_DSP_DisconnectAll(IntPtr dsp, int inputs, int outputs);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_DSP_DisconnectFrom(IntPtr dsp, IntPtr target);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_DSP_GetActive(IntPtr dsp, ref int active);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_DSP_GetBypass(IntPtr dsp, ref int bypass);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_DSP_GetDefaults(IntPtr dsp, ref float frequency, ref float volume, ref float pan, ref int priority);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_DSP_GetInfo(IntPtr dsp, StringBuilder name, ref uint version, ref int channels, ref int configwidth, ref int configheight);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_DSP_GetInput(IntPtr dsp, int index, ref IntPtr input, ref IntPtr inputconnection);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_DSP_GetNumInputs(IntPtr dsp, ref int numinputs);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_DSP_GetNumOutputs(IntPtr dsp, ref int numoutputs);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_DSP_GetNumParameters(IntPtr dsp, ref int numparams);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_DSP_GetOutput(IntPtr dsp, int index, ref IntPtr output, ref IntPtr outputconnection);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_DSP_GetParameter(IntPtr dsp, int index, ref float val, StringBuilder valuestr, int valuestrlen);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_DSP_GetParameterInfo(IntPtr dsp, int index, StringBuilder name, StringBuilder label, StringBuilder description, int descriptionlen, ref float min, ref float max);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_DSP_GetSystemObject(IntPtr dsp, ref IntPtr system);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_DSP_GetType(IntPtr dsp, ref DSP_TYPE type);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_DSP_GetUserData(IntPtr dsp, ref IntPtr userdata);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_DSP_Release(IntPtr dsp);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_DSP_Remove(IntPtr dsp);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_DSP_Reset(IntPtr dsp);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_DSP_SetActive(IntPtr dsp, int active);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_DSP_SetBypass(IntPtr dsp, int bypass);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_DSP_SetDefaults(IntPtr dsp, float frequency, float volume, float pan, int priority);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_DSP_SetParameter(IntPtr dsp, int index, float val);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_DSP_SetUserData(IntPtr dsp, IntPtr userdata);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_DSP_ShowConfigDialog(IntPtr dsp, IntPtr hwnd, bool show);
        public RESULT getActive(ref bool active)
        {
            int num = 0;
            RESULT result = FMOD_DSP_GetActive(this.dspraw, ref num);
            active = num != 0;
            return result;
        }

        public RESULT getBypass(ref bool bypass)
        {
            int num = 0;
            RESULT result = FMOD_DSP_GetBypass(this.dspraw, ref num);
            bypass = num != 0;
            return result;
        }

        public RESULT getDefaults(ref float frequency, ref float volume, ref float pan, ref int priority)
        {
            return FMOD_DSP_GetDefaults(this.dspraw, ref frequency, ref volume, ref pan, ref priority);
        }

        public RESULT getInfo(StringBuilder name, ref uint version, ref int channels, ref int configwidth, ref int configheight)
        {
            return FMOD_DSP_GetInfo(this.dspraw, name, ref version, ref channels, ref configwidth, ref configheight);
        }

        public RESULT getInput(int index, ref DSP input, ref DSPConnection inputconnection)
        {
            RESULT oK = RESULT.OK;
            IntPtr ptr = new IntPtr();
            DSP dsp = null;
            IntPtr ptr2 = new IntPtr();
            DSPConnection connection = null;
            try
            {
                oK = FMOD_DSP_GetInput(this.dspraw, index, ref ptr, ref ptr2);
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
                if (inputconnection == null)
                {
                    connection = new DSPConnection();
                    connection.setRaw(ptr2);
                    inputconnection = connection;
                }
                else
                {
                    inputconnection.setRaw(ptr2);
                }
            }
            return oK;
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

        public RESULT getOutput(int index, ref DSP output, ref DSPConnection outputconnection)
        {
            RESULT oK = RESULT.OK;
            IntPtr ptr = new IntPtr();
            DSP dsp = null;
            IntPtr ptr2 = new IntPtr();
            DSPConnection connection = null;
            try
            {
                oK = FMOD_DSP_GetOutput(this.dspraw, index, ref ptr, ref ptr2);
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
                if (outputconnection == null)
                {
                    connection = new DSPConnection();
                    connection.setRaw(ptr2);
                    outputconnection = connection;
                }
                else
                {
                    outputconnection.setRaw(ptr2);
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

        public RESULT getSystemObject(ref _System system)
        {
            RESULT oK = RESULT.OK;
            IntPtr ptr = new IntPtr();
            _System system2 = null;
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
                    system2 = new _System();
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

        public RESULT getType(ref DSP_TYPE type)
        {
            return FMOD_DSP_GetType(this.dspraw, ref type);
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
            return FMOD_DSP_SetActive(this.dspraw, active ? 1 : 0);
        }

        public RESULT setBypass(bool bypass)
        {
            return FMOD_DSP_SetBypass(this.dspraw, bypass ? 1 : 0);
        }

        public RESULT setDefaults(float frequency, float volume, float pan, int priority)
        {
            return FMOD_DSP_SetDefaults(this.dspraw, frequency, volume, pan, priority);
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

