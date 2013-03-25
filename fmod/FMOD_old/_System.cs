namespace FMOD
{
    using System;
    using System.Runtime.InteropServices;
    using System.Text;

    public class _System
    {
        private IntPtr systemraw;

        public RESULT addDSP(DSP dsp, ref DSPConnection dspconnection)
        {
            RESULT oK = RESULT.OK;
            IntPtr ptr = new IntPtr();
            DSPConnection connection = null;
            try
            {
                oK = FMOD_System_AddDSP(this.systemraw, dsp.getRaw(), ref ptr);
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

        public RESULT attachFileSystem(FILE_OPENCALLBACK useropen, FILE_CLOSECALLBACK userclose, FILE_READCALLBACK userread, FILE_SEEKCALLBACK userseek)
        {
            return FMOD_System_AttachFileSystem(this.systemraw, useropen, userclose, userread, userseek);
        }

        public RESULT close()
        {
            if (VERSION.platform == Platform.X64)
            {
                return FMOD_System_Close_64(this.systemraw);
            }
            return FMOD_System_Close_32(this.systemraw);
        }

        public RESULT createChannelGroup(string name, ref ChannelGroup channelgroup)
        {
            RESULT oK = RESULT.OK;
            IntPtr ptr = new IntPtr();
            ChannelGroup group = null;
            try
            {
                oK = FMOD_System_CreateChannelGroup(this.systemraw, name, ref ptr);
            }
            catch
            {
                oK = RESULT.ERR_INVALID_PARAM;
            }
            if (oK == RESULT.OK)
            {
                if (channelgroup == null)
                {
                    group = new ChannelGroup();
                    group.setRaw(ptr);
                    channelgroup = group;
                }
                else
                {
                    channelgroup.setRaw(ptr);
                }
            }
            return oK;
        }

        public RESULT createCodec(IntPtr codecdescription, uint priority)
        {
            return FMOD_System_CreateCodec(this.systemraw, codecdescription, priority);
        }

        public RESULT createDSP(ref DSP_DESCRIPTION description, ref DSP dsp)
        {
            RESULT oK = RESULT.OK;
            IntPtr ptr = new IntPtr();
            DSP dsp2 = null;
            try
            {
                oK = FMOD_System_CreateDSP(this.systemraw, ref description, ref ptr);
            }
            catch
            {
                oK = RESULT.ERR_INVALID_PARAM;
            }
            if (oK == RESULT.OK)
            {
                if (dsp == null)
                {
                    dsp2 = new DSP();
                    dsp2.setRaw(ptr);
                    dsp = dsp2;
                }
                else
                {
                    dsp.setRaw(ptr);
                }
            }
            return oK;
        }

        public RESULT createDSPByIndex(int index, ref DSP dsp)
        {
            RESULT oK = RESULT.OK;
            IntPtr ptr = new IntPtr();
            DSP dsp2 = null;
            try
            {
                oK = FMOD_System_CreateDSPByIndex(this.systemraw, index, ref ptr);
            }
            catch
            {
                oK = RESULT.ERR_INVALID_PARAM;
            }
            if (oK == RESULT.OK)
            {
                if (dsp == null)
                {
                    dsp2 = new DSP();
                    dsp2.setRaw(ptr);
                    dsp = dsp2;
                }
                else
                {
                    dsp.setRaw(ptr);
                }
            }
            return oK;
        }

        public RESULT createDSPByPlugin(uint handle, ref IntPtr dsp)
        {
            return FMOD_System_CreateDSPByPlugin(this.systemraw, handle, ref dsp);
        }

        public RESULT createDSPByType(DSP_TYPE type, ref DSP dsp)
        {
            RESULT oK = RESULT.OK;
            IntPtr ptr = new IntPtr();
            DSP dsp2 = null;
            try
            {
                oK = FMOD_System_CreateDSPByType(this.systemraw, type, ref ptr);
            }
            catch
            {
                oK = RESULT.ERR_INVALID_PARAM;
            }
            if (oK == RESULT.OK)
            {
                if (dsp == null)
                {
                    dsp2 = new DSP();
                    dsp2.setRaw(ptr);
                    dsp = dsp2;
                }
                else
                {
                    dsp.setRaw(ptr);
                }
            }
            return oK;
        }

        public RESULT createGeometry(int maxpolygons, int maxvertices, ref Geometry geometryf)
        {
            RESULT oK = RESULT.OK;
            IntPtr ptr = new IntPtr();
            Geometry geometry = null;
            try
            {
                oK = FMOD_System_CreateGeometry(this.systemraw, maxpolygons, maxvertices, ref ptr);
            }
            catch
            {
                oK = RESULT.ERR_INVALID_PARAM;
            }
            if (oK == RESULT.OK)
            {
                if (geometryf == null)
                {
                    geometry = new Geometry();
                    geometry.setRaw(ptr);
                    geometryf = geometry;
                }
                else
                {
                    geometryf.setRaw(ptr);
                }
            }
            return oK;
        }

        public RESULT createSound(string name_or_data, MODE mode, ref Sound sound)
        {
            RESULT oK = RESULT.OK;
            IntPtr ptr = new IntPtr();
            Sound sound2 = null;
            try
            {
                if (VERSION.platform == Platform.X64)
                {
                    oK = FMOD_System_CreateSound_64(this.systemraw, name_or_data, mode, 0, ref ptr);
                }
                else
                {
                    oK = FMOD_System_CreateSound_32(this.systemraw, name_or_data, mode, 0, ref ptr);
                }
            }
            catch
            {
                oK = RESULT.ERR_INVALID_PARAM;
            }
            if (oK == RESULT.OK)
            {
                if (sound == null)
                {
                    sound2 = new Sound();
                    sound2.setRaw(ptr);
                    sound = sound2;
                }
                else
                {
                    sound.setRaw(ptr);
                }
            }
            return oK;
        }

        public RESULT createSound(byte[] data, MODE mode, ref CREATESOUNDEXINFO exinfo, ref Sound sound)
        {
            RESULT oK = RESULT.OK;
            IntPtr ptr = new IntPtr();
            Sound sound2 = null;
            try
            {
                if (VERSION.platform == Platform.X64)
                {
                    oK = FMOD_System_CreateSound_64(this.systemraw, data, mode, ref exinfo, ref ptr);
                }
                else
                {
                    oK = FMOD_System_CreateSound_32(this.systemraw, data, mode, ref exinfo, ref ptr);
                }
            }
            catch
            {
                oK = RESULT.ERR_INVALID_PARAM;
            }
            if (oK == RESULT.OK)
            {
                if (sound == null)
                {
                    sound2 = new Sound();
                    sound2.setRaw(ptr);
                    sound = sound2;
                }
                else
                {
                    sound.setRaw(ptr);
                }
            }
            return oK;
        }

        public RESULT createSound(string name_or_data, MODE mode, ref CREATESOUNDEXINFO exinfo, ref Sound sound)
        {
            RESULT oK = RESULT.OK;
            IntPtr ptr = new IntPtr();
            Sound sound2 = null;
            try
            {
                if (VERSION.platform == Platform.X64)
                {
                    oK = FMOD_System_CreateSound_64(this.systemraw, name_or_data, mode, ref exinfo, ref ptr);
                }
                else
                {
                    oK = FMOD_System_CreateSound_32(this.systemraw, name_or_data, mode, ref exinfo, ref ptr);
                }
            }
            catch
            {
                oK = RESULT.ERR_INVALID_PARAM;
            }
            if (oK == RESULT.OK)
            {
                if (sound == null)
                {
                    sound2 = new Sound();
                    sound2.setRaw(ptr);
                    sound = sound2;
                }
                else
                {
                    sound.setRaw(ptr);
                }
            }
            return oK;
        }

        public RESULT createStream(string name_or_data, MODE mode, ref Sound sound)
        {
            RESULT oK = RESULT.OK;
            IntPtr ptr = new IntPtr();
            Sound sound2 = null;
            try
            {
                oK = FMOD_System_CreateStream(this.systemraw, name_or_data, mode, 0, ref ptr);
            }
            catch
            {
                oK = RESULT.ERR_INVALID_PARAM;
            }
            if (oK == RESULT.OK)
            {
                if (sound == null)
                {
                    sound2 = new Sound();
                    sound2.setRaw(ptr);
                    sound = sound2;
                }
                else
                {
                    sound.setRaw(ptr);
                }
            }
            return oK;
        }

        public RESULT createStream(byte[] data, MODE mode, ref CREATESOUNDEXINFO exinfo, ref Sound sound)
        {
            RESULT oK = RESULT.OK;
            IntPtr ptr = new IntPtr();
            Sound sound2 = null;
            try
            {
                oK = FMOD_System_CreateStream(this.systemraw, data, mode, ref exinfo, ref ptr);
            }
            catch
            {
                oK = RESULT.ERR_INVALID_PARAM;
            }
            if (oK == RESULT.OK)
            {
                if (sound == null)
                {
                    sound2 = new Sound();
                    sound2.setRaw(ptr);
                    sound = sound2;
                }
                else
                {
                    sound.setRaw(ptr);
                }
            }
            return oK;
        }

        public RESULT createStream(string name_or_data, MODE mode, ref CREATESOUNDEXINFO exinfo, ref Sound sound)
        {
            RESULT oK = RESULT.OK;
            IntPtr ptr = new IntPtr();
            Sound sound2 = null;
            try
            {
                oK = FMOD_System_CreateStream(this.systemraw, name_or_data, mode, ref exinfo, ref ptr);
            }
            catch
            {
                oK = RESULT.ERR_INVALID_PARAM;
            }
            if (oK == RESULT.OK)
            {
                if (sound == null)
                {
                    sound2 = new Sound();
                    sound2.setRaw(ptr);
                    sound = sound2;
                }
                else
                {
                    sound.setRaw(ptr);
                }
            }
            return oK;
        }

        [DllImport("fmodex")]
        private static extern RESULT FMOD_System_AddDSP(IntPtr system, IntPtr dsp, ref IntPtr dspconnection);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_System_AttachFileSystem(IntPtr system, FILE_OPENCALLBACK useropen, FILE_CLOSECALLBACK userclose, FILE_READCALLBACK userread, FILE_SEEKCALLBACK userseek);
        [DllImport("fmodex", EntryPoint="FMOD_System_Close")]
        private static extern RESULT FMOD_System_Close_32(IntPtr system);
        [DllImport("fmodex64", EntryPoint="FMOD_System_Close")]
        private static extern RESULT FMOD_System_Close_64(IntPtr system);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_System_CreateChannelGroup(IntPtr system, string name, ref IntPtr channelgroup);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_System_CreateCodec(IntPtr system, IntPtr codecdescription, uint priority);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_System_CreateDSP(IntPtr system, ref DSP_DESCRIPTION description, ref IntPtr dsp);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_System_CreateDSPByIndex(IntPtr system, int index, ref IntPtr dsp);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_System_CreateDSPByPlugin(IntPtr system, uint handle, ref IntPtr dsp);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_System_CreateDSPByType(IntPtr system, DSP_TYPE type, ref IntPtr dsp);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_System_CreateGeometry(IntPtr system, int maxPolygons, int maxVertices, ref IntPtr geometryf);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_System_CreateReverb(IntPtr system, ref IntPtr reverb);
        [DllImport("fmodex", EntryPoint="FMOD_System_CreateSound")]
        private static extern RESULT FMOD_System_CreateSound_32(IntPtr system, byte[] name_or_data, MODE mode, ref CREATESOUNDEXINFO exinfo, ref IntPtr sound);
        [DllImport("fmodex", EntryPoint="FMOD_System_CreateSound")]
        private static extern RESULT FMOD_System_CreateSound_32(IntPtr system, byte[] name_or_data, MODE mode, int exinfo, ref IntPtr sound);
        [DllImport("fmodex", EntryPoint="FMOD_System_CreateSound")]
        private static extern RESULT FMOD_System_CreateSound_32(IntPtr system, string name_or_data, MODE mode, int exinfo, ref IntPtr sound);
        [DllImport("fmodex", EntryPoint="FMOD_System_CreateSound", CharSet=CharSet.Unicode)]
        private static extern RESULT FMOD_System_CreateSound_32(IntPtr system, string name_or_data, MODE mode, ref CREATESOUNDEXINFO exinfo, ref IntPtr sound);
        [DllImport("fmodex64", EntryPoint="FMOD_System_CreateSound")]
        private static extern RESULT FMOD_System_CreateSound_64(IntPtr system, string name_or_data, MODE mode, int exinfo, ref IntPtr sound);
        [DllImport("fmodex64", EntryPoint="FMOD_System_CreateSound")]
        private static extern RESULT FMOD_System_CreateSound_64(IntPtr system, byte[] name_or_data, MODE mode, ref CREATESOUNDEXINFO exinfo, ref IntPtr sound);
        [DllImport("fmodex64", EntryPoint="FMOD_System_CreateSound", CharSet=CharSet.Unicode)]
        private static extern RESULT FMOD_System_CreateSound_64(IntPtr system, string name_or_data, MODE mode, ref CREATESOUNDEXINFO exinfo, ref IntPtr sound);
        [DllImport("fmodex64", EntryPoint="FMOD_System_CreateSound")]
        private static extern RESULT FMOD_System_CreateSound_64(IntPtr system, byte[] name_or_data, MODE mode, int exinfo, ref IntPtr sound);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_System_CreateSoundGroup(IntPtr system, StringBuilder name, ref SoundGroup soundgroup);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_System_CreateStream(IntPtr system, byte[] name_or_data, MODE mode, ref CREATESOUNDEXINFO exinfo, ref IntPtr sound);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_System_CreateStream(IntPtr system, byte[] name_or_data, MODE mode, int exinfo, ref IntPtr sound);
        [DllImport("fmodex", CharSet=CharSet.Unicode)]
        private static extern RESULT FMOD_System_CreateStream(IntPtr system, string name_or_data, MODE mode, ref CREATESOUNDEXINFO exinfo, ref IntPtr sound);
        [DllImport("fmodex64")]
        private static extern RESULT FMOD_System_CreateStream(IntPtr system, string name_or_data, MODE mode, int exinfo, ref IntPtr sound);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_System_Get3DListenerAttributes(IntPtr system, int listener, ref VECTOR pos, ref VECTOR vel, ref VECTOR forward, ref VECTOR up);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_System_Get3DNumListeners(IntPtr system, ref int numlisteners);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_System_Get3DSettings(IntPtr system, ref float dopplerscale, ref float distancefactor, ref float rolloffscale);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_System_Get3DSpeakerPosition(IntPtr system, SPEAKER speaker, ref float x, ref float y, ref int active);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_System_GetAdvancedSettings(IntPtr system, ref ADVANCEDSETTINGS settings);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_System_GetCDROMDriveName(IntPtr system, int drive, StringBuilder drivename, int drivenamelen, StringBuilder scsiname, int scsinamelen, StringBuilder devicename, int devicenamelen);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_System_GetChannel(IntPtr system, int channelid, ref IntPtr channel);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_System_GetChannelsPlaying(IntPtr system, ref int channels);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_System_GetCPUUsage(IntPtr system, ref float dsp, ref float stream, ref float update, ref float total);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_System_GetDriver(IntPtr system, ref int driver);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_System_GetDriverCaps(IntPtr system, int id, ref CAPS caps, ref int minfrequency, ref int maxfrequency, ref SPEAKERMODE controlpanelspeakermode);
        [DllImport("fmodex", EntryPoint="FMOD_System_GetDriverInfo")]
        private static extern RESULT FMOD_System_GetDriverInfo_32(IntPtr system, int id, StringBuilder name, int namelen, ref GUID guid);
        [DllImport("fmodex64", EntryPoint="FMOD_System_GetDriverInfo")]
        private static extern RESULT FMOD_System_GetDriverInfo_64(IntPtr system, int id, StringBuilder name, int namelen, ref GUID guid);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_System_GetDSPBufferSize(IntPtr system, ref uint bufferlength, ref int numbuffers);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_System_GetDSPHead(IntPtr system, ref IntPtr dsp);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_System_GetFileBufferSize(IntPtr system, ref int sizebytes);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_System_GetGeometrySettings(IntPtr system, ref float maxWorldSize);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_System_GetHardwareChannels(IntPtr system, ref int num2d, ref int num3d, ref int total);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_System_GetMasterChannelGroup(IntPtr system, ref IntPtr channelgroup);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_System_GetMasterSoundGroup(IntPtr system, ref IntPtr soundgroup);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_System_GetNetworkProxy(IntPtr system, StringBuilder proxy, int proxylen);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_System_GetNetworkTimeout(IntPtr system, ref int timeout);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_System_GetNumCDROMDrives(IntPtr system, ref int numdrives);
        [DllImport("fmodex", EntryPoint="FMOD_System_GetNumDrivers")]
        private static extern RESULT FMOD_System_GetNumDrivers_32(IntPtr system, ref int numdrivers);
        [DllImport("fmodex64", EntryPoint="FMOD_System_GetNumDrivers")]
        private static extern RESULT FMOD_System_GetNumDrivers_64(IntPtr system, ref int numdrivers);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_System_GetNumPlugins(IntPtr system, PLUGINTYPE plugintype, ref int numplugins);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_System_GetOutput(IntPtr system, ref OUTPUTTYPE output);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_System_GetOutputByPlugin(IntPtr system, ref uint handle);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_System_GetOutputHandle(IntPtr system, ref IntPtr handle);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_System_GetPluginHandle(IntPtr system, PLUGINTYPE plugintype, int index, ref uint handle);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_System_GetPluginInfo(IntPtr system, uint handle, ref PLUGINTYPE plugintype, StringBuilder name, int namelen, ref uint version);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_System_GetRecordDriver(IntPtr system, ref int driver);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_System_GetRecordDriverInfo(IntPtr system, int id, StringBuilder name, int namelen, ref GUID guid);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_System_GetRecordNumDrivers(IntPtr system, ref int numdrivers);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_System_GetRecordPosition(IntPtr system, ref uint position);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_System_GetReverbAmbientProperties(IntPtr system, ref REVERB_PROPERTIES prop);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_System_GetReverbProperties(IntPtr system, ref REVERB_PROPERTIES prop);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_System_GetSoftwareChannels(IntPtr system, ref int numsoftwarechannels);
        [DllImport("fmodex", EntryPoint="FMOD_System_GetSoftwareFormat")]
        private static extern RESULT FMOD_System_GetSoftwareFormat_32(IntPtr system, ref int samplerate, ref SOUND_FORMAT format, ref int numoutputchannels, ref int maxinputchannels, ref DSP_RESAMPLER resamplemethod, ref int bits);
        [DllImport("fmodex64", EntryPoint="FMOD_System_GetSoftwareFormat")]
        private static extern RESULT FMOD_System_GetSoftwareFormat_64(IntPtr system, ref int samplerate, ref SOUND_FORMAT format, ref int numoutputchannels, ref int maxinputchannels, ref DSP_RESAMPLER resamplemethod, ref int bits);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_System_GetSoundRAM(IntPtr system, ref int currentalloced, ref int maxalloced, ref int total);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_System_GetSpeakerMode(IntPtr system, ref SPEAKERMODE speakermode);
        [DllImport("fmodex", EntryPoint="FMOD_System_GetSpectrum")]
        private static extern RESULT FMOD_System_GetSpectrum_32(IntPtr system, [MarshalAs(UnmanagedType.LPArray)] float[] spectrumarray, int numvalues, int channeloffset, DSP_FFT_WINDOW windowtype);
        [DllImport("fmodex64", EntryPoint="FMOD_System_GetSpectrum")]
        private static extern RESULT FMOD_System_GetSpectrum_64(IntPtr system, [MarshalAs(UnmanagedType.LPArray)] float[] spectrumarray, int numvalues, int channeloffset, DSP_FFT_WINDOW windowtype);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_System_GetStreamBufferSize(IntPtr system, ref uint filebuffersize, ref TIMEUNIT filebuffersizetype);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_System_GetUserData(IntPtr system, ref IntPtr userdata);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_System_GetVersion(IntPtr system, ref uint version);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_System_GetWaveData(IntPtr system, [MarshalAs(UnmanagedType.LPArray)] float[] wavearray, int numvalues, int channeloffset);
        [DllImport("fmodex", EntryPoint="FMOD_System_Init")]
        private static extern RESULT FMOD_System_Init_32(IntPtr system, int maxchannels, INITFLAG flags, IntPtr extradata);
        [DllImport("fmodex64", EntryPoint="FMOD_System_Init")]
        private static extern RESULT FMOD_System_Init_64(IntPtr system, int maxchannels, INITFLAG flags, IntPtr extradata);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_System_IsRecording(IntPtr system, ref bool recording);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_System_LoadGeometry(IntPtr system, IntPtr data, int dataSize, ref IntPtr geometry);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_System_LoadPlugin(IntPtr system, string filename, ref uint handle, uint priority);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_System_LockDSP(IntPtr system);
        [DllImport("fmodex")]
        public static extern RESULT FMOD_System_PlayDSP(IntPtr system, CHANNELINDEX channelid, IntPtr dsp, int paused, ref IntPtr channel);
        [DllImport("fmodex", EntryPoint="FMOD_System_PlaySound")]
        private static extern RESULT FMOD_System_PlaySound_32(IntPtr system, CHANNELINDEX channelid, IntPtr sound, int paused, ref IntPtr channel);
        [DllImport("fmodex64", EntryPoint="FMOD_System_PlaySound")]
        private static extern RESULT FMOD_System_PlaySound_64(IntPtr system, CHANNELINDEX channelid, IntPtr sound, int paused, ref IntPtr channel);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_System_RecordStart(IntPtr system, IntPtr sound, bool loop);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_System_RecordStop(IntPtr system);
        [DllImport("fmodex", EntryPoint="FMOD_System_Release")]
        private static extern RESULT FMOD_System_Release_32(IntPtr system);
        [DllImport("fmodex64", EntryPoint="FMOD_System_Release")]
        private static extern RESULT FMOD_System_Release_64(IntPtr system);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_System_Set3DListenerAttributes(IntPtr system, int listener, ref VECTOR pos, ref VECTOR vel, ref VECTOR forward, ref VECTOR up);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_System_Set3DNumListeners(IntPtr system, int numlisteners);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_System_Set3DRolloffCallback(IntPtr system, CB_3D_ROLLOFFCALLBACK callback);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_System_Set3DSettings(IntPtr system, float dopplerscale, float distancefactor, float rolloffscale);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_System_Set3DSpeakerPosition(IntPtr system, SPEAKER speaker, float x, float y, int active);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_System_SetAdvancedSettings(IntPtr system, ref ADVANCEDSETTINGS settings);
        [DllImport("fmodex", EntryPoint="FMOD_System_SetDriver")]
        private static extern RESULT FMOD_System_SetDriver_32(IntPtr system, int driver);
        [DllImport("fmodex64", EntryPoint="FMOD_System_SetDriver")]
        private static extern RESULT FMOD_System_SetDriver_64(IntPtr system, int driver);
        [DllImport("fmodex", EntryPoint="FMOD_System_SetDSPBufferSize")]
        private static extern RESULT FMOD_System_SetDSPBufferSize_32(IntPtr system, uint bufferlength, int numbuffers);
        [DllImport("fmodex64", EntryPoint="FMOD_System_SetDSPBufferSize")]
        private static extern RESULT FMOD_System_SetDSPBufferSize_64(IntPtr system, uint bufferlength, int numbuffers);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_System_SetFileBufferSize(IntPtr system, int sizebytes);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_System_SetFileSystem(IntPtr system, FILE_OPENCALLBACK useropen, FILE_CLOSECALLBACK userclose, FILE_READCALLBACK userread, FILE_SEEKCALLBACK userseek, int buffersize);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_System_SetGeometrySettings(IntPtr system, float maxWorldSize);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_System_SetHardwareChannels(IntPtr system, int min2d, int max2d, int min3d, int max3d);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_System_SetNetworkProxy(IntPtr system, string proxy);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_System_SetNetworkTimeout(IntPtr system, int timeout);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_System_SetOutput(IntPtr system, OUTPUTTYPE output);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_System_SetOutputByPlugin(IntPtr system, uint handle);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_System_SetPluginPath(IntPtr system, string path);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_System_SetRecordDriver(IntPtr system, int driver);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_System_SetReverbAmbientProperties(IntPtr system, ref REVERB_PROPERTIES prop);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_System_SetReverbProperties(IntPtr system, ref REVERB_PROPERTIES prop);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_System_SetSoftwareChannels(IntPtr system, int numsoftwarechannels);
        [DllImport("fmodex", EntryPoint="FMOD_System_SetSoftwareFormat")]
        private static extern RESULT FMOD_System_SetSoftwareFormat_32(IntPtr system, int samplerate, SOUND_FORMAT format, int numoutputchannels, int maxinputchannels, DSP_RESAMPLER resamplemethod);
        [DllImport("fmodex64", EntryPoint="FMOD_System_SetSoftwareFormat")]
        private static extern RESULT FMOD_System_SetSoftwareFormat_64(IntPtr system, int samplerate, SOUND_FORMAT format, int numoutputchannels, int maxinputchannels, DSP_RESAMPLER resamplemethod);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_System_SetSpeakerMode(IntPtr system, SPEAKERMODE speakermode);
        [DllImport("fmodex", EntryPoint="FMOD_System_SetStreamBufferSize")]
        private static extern RESULT FMOD_System_SetStreamBufferSize_32(IntPtr system, uint filebuffersize, TIMEUNIT filebuffersizetype);
        [DllImport("fmodex64", EntryPoint="FMOD_System_SetStreamBufferSize")]
        private static extern RESULT FMOD_System_SetStreamBufferSize_64(IntPtr system, uint filebuffersize, TIMEUNIT filebuffersizetype);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_System_SetUserData(IntPtr system, IntPtr userdata);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_System_UnloadPlugin(IntPtr system, uint handle);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_System_UnlockDSP(IntPtr system);
        [DllImport("fmodex", EntryPoint="FMOD_System_Update")]
        private static extern RESULT FMOD_System_Update_32(IntPtr system);
        [DllImport("fmodex64", EntryPoint="FMOD_System_Update")]
        private static extern RESULT FMOD_System_Update_64(IntPtr system);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_System_UpdateFinished(IntPtr system);
        public RESULT get3DListenerAttributes(int listener, ref VECTOR pos, ref VECTOR vel, ref VECTOR forward, ref VECTOR up)
        {
            return FMOD_System_Get3DListenerAttributes(this.systemraw, listener, ref pos, ref vel, ref forward, ref up);
        }

        public RESULT get3DNumListeners(ref int numlisteners)
        {
            return FMOD_System_Get3DNumListeners(this.systemraw, ref numlisteners);
        }

        public RESULT get3DSettings(ref float dopplerscale, ref float distancefactor, ref float rolloffscale)
        {
            return FMOD_System_Get3DSettings(this.systemraw, ref dopplerscale, ref distancefactor, ref rolloffscale);
        }

        public RESULT get3DSpeakerPosition(SPEAKER speaker, ref float x, ref float y, ref bool active)
        {
            int num = 0;
            RESULT result = FMOD_System_Get3DSpeakerPosition(this.systemraw, speaker, ref x, ref y, ref num);
            active = num != 0;
            return result;
        }

        public RESULT getAdvancedSettings(ref ADVANCEDSETTINGS settings)
        {
            return FMOD_System_GetAdvancedSettings(this.systemraw, ref settings);
        }

        public RESULT getCDROMDriveName(int drive, StringBuilder drivename, int drivenamelen, StringBuilder scsiname, int scsinamelen, StringBuilder devicename, int devicenamelen)
        {
            return FMOD_System_GetCDROMDriveName(this.systemraw, drive, drivename, drivenamelen, scsiname, scsinamelen, devicename, devicenamelen);
        }

        public RESULT getChannel(int channelid, ref Channel channel)
        {
            RESULT oK = RESULT.OK;
            IntPtr ptr = new IntPtr();
            Channel channel2 = null;
            try
            {
                oK = FMOD_System_GetChannel(this.systemraw, channelid, ref ptr);
            }
            catch
            {
                oK = RESULT.ERR_INVALID_PARAM;
            }
            if (oK == RESULT.OK)
            {
                if (channel == null)
                {
                    channel2 = new Channel();
                    channel2.setRaw(ptr);
                    channel = channel2;
                }
                else
                {
                    channel.setRaw(ptr);
                }
            }
            return oK;
        }

        public RESULT getChannelsPlaying(ref int channels)
        {
            return FMOD_System_GetChannelsPlaying(this.systemraw, ref channels);
        }

        public RESULT getCPUUsage(ref float dsp, ref float stream, ref float update, ref float total)
        {
            return FMOD_System_GetCPUUsage(this.systemraw, ref dsp, ref stream, ref update, ref total);
        }

        public RESULT getDriver(ref int driver)
        {
            return FMOD_System_GetDriver(this.systemraw, ref driver);
        }

        public RESULT getDriverCaps(int id, ref CAPS caps, ref int minfrequency, ref int maxfrequency, ref SPEAKERMODE controlpanelspeakermode)
        {
            return FMOD_System_GetDriverCaps(this.systemraw, id, ref caps, ref minfrequency, ref maxfrequency, ref controlpanelspeakermode);
        }

        public RESULT getDriverInfo(int id, StringBuilder name, int namelen, ref GUID guid)
        {
            if (VERSION.platform == Platform.X64)
            {
                return FMOD_System_GetDriverInfo_64(this.systemraw, id, name, namelen, ref guid);
            }
            return FMOD_System_GetDriverInfo_32(this.systemraw, id, name, namelen, ref guid);
        }

        public RESULT getDSPBufferSize(ref uint bufferlength, ref int numbuffers)
        {
            return FMOD_System_GetDSPBufferSize(this.systemraw, ref bufferlength, ref numbuffers);
        }

        public RESULT getDSPHead(ref DSP dsp)
        {
            RESULT oK = RESULT.OK;
            IntPtr ptr = new IntPtr();
            DSP dsp2 = null;
            try
            {
                oK = FMOD_System_GetDSPHead(this.systemraw, ref ptr);
            }
            catch
            {
                oK = RESULT.ERR_INVALID_PARAM;
            }
            if (oK == RESULT.OK)
            {
                if (dsp == null)
                {
                    dsp2 = new DSP();
                    dsp2.setRaw(ptr);
                    dsp = dsp2;
                }
                else
                {
                    dsp.setRaw(ptr);
                }
            }
            return oK;
        }

        public RESULT getGeometrySettings(ref float maxworldsize)
        {
            return FMOD_System_GetGeometrySettings(this.systemraw, ref maxworldsize);
        }

        public RESULT getHardwareChannels(ref int num2d, ref int num3d, ref int total)
        {
            return FMOD_System_GetHardwareChannels(this.systemraw, ref num2d, ref num3d, ref total);
        }

        public RESULT getMasterChannelGroup(ref ChannelGroup channelgroup)
        {
            RESULT oK = RESULT.OK;
            IntPtr ptr = new IntPtr();
            ChannelGroup group = null;
            try
            {
                oK = FMOD_System_GetMasterChannelGroup(this.systemraw, ref ptr);
            }
            catch
            {
                oK = RESULT.ERR_INVALID_PARAM;
            }
            if (oK == RESULT.OK)
            {
                if (channelgroup == null)
                {
                    group = new ChannelGroup();
                    group.setRaw(ptr);
                    channelgroup = group;
                }
                else
                {
                    channelgroup.setRaw(ptr);
                }
            }
            return oK;
        }

        public RESULT getMasterSoundGroup(ref SoundGroup soundgroup)
        {
            RESULT oK = RESULT.OK;
            IntPtr ptr = new IntPtr();
            SoundGroup group = null;
            try
            {
                oK = FMOD_System_GetMasterSoundGroup(this.systemraw, ref ptr);
            }
            catch
            {
                oK = RESULT.ERR_INVALID_PARAM;
            }
            if (oK == RESULT.OK)
            {
                if (soundgroup == null)
                {
                    group = new SoundGroup();
                    group.setRaw(ptr);
                    soundgroup = group;
                }
                else
                {
                    soundgroup.setRaw(ptr);
                }
            }
            return oK;
        }

        public RESULT getNetworkTimeout(ref int timeout)
        {
            return FMOD_System_GetNetworkTimeout(this.systemraw, ref timeout);
        }

        public RESULT getNumCDROMDrives(ref int numdrives)
        {
            return FMOD_System_GetNumCDROMDrives(this.systemraw, ref numdrives);
        }

        public RESULT getNumDrivers(ref int numdrivers)
        {
            if (VERSION.platform == Platform.X64)
            {
                return FMOD_System_GetNumDrivers_64(this.systemraw, ref numdrivers);
            }
            return FMOD_System_GetNumDrivers_32(this.systemraw, ref numdrivers);
        }

        public RESULT getNumPlugins(PLUGINTYPE plugintype, ref int numplugins)
        {
            return FMOD_System_GetNumPlugins(this.systemraw, plugintype, ref numplugins);
        }

        public RESULT getOutput(ref OUTPUTTYPE output)
        {
            return FMOD_System_GetOutput(this.systemraw, ref output);
        }

        public RESULT getOutputByPlugin(ref uint handle)
        {
            return FMOD_System_GetOutputByPlugin(this.systemraw, ref handle);
        }

        public RESULT getOutputHandle(ref IntPtr handle)
        {
            return FMOD_System_GetOutputHandle(this.systemraw, ref handle);
        }

        public RESULT getPluginHandle(PLUGINTYPE plugintype, int index, ref uint handle)
        {
            return FMOD_System_GetPluginHandle(this.systemraw, plugintype, index, ref handle);
        }

        public RESULT getPluginInfo(uint handle, ref PLUGINTYPE plugintype, StringBuilder name, int namelen, ref uint version)
        {
            return FMOD_System_GetPluginInfo(this.systemraw, handle, ref plugintype, name, namelen, ref version);
        }

        public RESULT getProxy(StringBuilder proxy, int proxylen)
        {
            return FMOD_System_GetNetworkProxy(this.systemraw, proxy, proxylen);
        }

        public IntPtr getRaw()
        {
            return this.systemraw;
        }

        public RESULT getRecordDriver(ref int driver)
        {
            return FMOD_System_GetRecordDriver(this.systemraw, ref driver);
        }

        public RESULT getRecordDriverInfo(int id, StringBuilder name, int namelen, ref GUID guid)
        {
            return FMOD_System_GetRecordDriverInfo(this.systemraw, id, name, namelen, ref guid);
        }

        public RESULT getRecordNumDrivers(ref int numdrivers)
        {
            return FMOD_System_GetRecordNumDrivers(this.systemraw, ref numdrivers);
        }

        public RESULT getRecordPosition(ref uint position)
        {
            return FMOD_System_GetRecordPosition(this.systemraw, ref position);
        }

        public RESULT getReverbAmbientProperties(ref REVERB_PROPERTIES prop)
        {
            return FMOD_System_GetReverbAmbientProperties(this.systemraw, ref prop);
        }

        public RESULT getReverbProperties(ref REVERB_PROPERTIES prop)
        {
            return FMOD_System_GetReverbProperties(this.systemraw, ref prop);
        }

        public RESULT getSoftwareChannels(ref int numsoftwarechannels)
        {
            return FMOD_System_GetSoftwareChannels(this.systemraw, ref numsoftwarechannels);
        }

        public RESULT getSoftwareFormat(ref int samplerate, ref SOUND_FORMAT format, ref int numoutputchannels, ref int maxinputchannels, ref DSP_RESAMPLER resamplemethod, ref int bits)
        {
            if (VERSION.platform == Platform.X64)
            {
                return FMOD_System_GetSoftwareFormat_64(this.systemraw, ref samplerate, ref format, ref numoutputchannels, ref maxinputchannels, ref resamplemethod, ref bits);
            }
            return FMOD_System_GetSoftwareFormat_32(this.systemraw, ref samplerate, ref format, ref numoutputchannels, ref maxinputchannels, ref resamplemethod, ref bits);
        }

        public RESULT getSoundRam(ref int currentalloced, ref int maxalloced, ref int total)
        {
            return FMOD_System_GetSoundRAM(this.systemraw, ref currentalloced, ref maxalloced, ref total);
        }

        public RESULT getSpeakerMode(ref SPEAKERMODE speakermode)
        {
            return FMOD_System_GetSpeakerMode(this.systemraw, ref speakermode);
        }

        public RESULT getSpectrum(float[] spectrumarray, int numvalues, int channeloffset, DSP_FFT_WINDOW windowtype)
        {
            if (VERSION.platform == Platform.X64)
            {
                return FMOD_System_GetSpectrum_64(this.systemraw, spectrumarray, numvalues, channeloffset, windowtype);
            }
            return FMOD_System_GetSpectrum_32(this.systemraw, spectrumarray, numvalues, channeloffset, windowtype);
        }

        public RESULT getStreamBufferSize(ref uint filebuffersize, ref TIMEUNIT filebuffersizetype)
        {
            return FMOD_System_GetStreamBufferSize(this.systemraw, ref filebuffersize, ref filebuffersizetype);
        }

        public RESULT getUserData(ref IntPtr userdata)
        {
            return FMOD_System_GetUserData(this.systemraw, ref userdata);
        }

        public RESULT getVersion(ref uint version)
        {
            return FMOD_System_GetVersion(this.systemraw, ref version);
        }

        public RESULT getWaveData(float[] wavearray, int numvalues, int channeloffset)
        {
            return FMOD_System_GetWaveData(this.systemraw, wavearray, numvalues, channeloffset);
        }

        public RESULT init(int maxchannels, INITFLAG flags, IntPtr extradata)
        {
            if (VERSION.platform == Platform.X64)
            {
                return FMOD_System_Init_64(this.systemraw, maxchannels, flags, extradata);
            }
            return FMOD_System_Init_32(this.systemraw, maxchannels, flags, extradata);
        }

        public RESULT isRecording(ref bool recording)
        {
            return FMOD_System_IsRecording(this.systemraw, ref recording);
        }

        public RESULT loadGeometry(IntPtr data, int datasize, ref Geometry geometry)
        {
            RESULT oK = RESULT.OK;
            IntPtr ptr = new IntPtr();
            Geometry geometry2 = null;
            try
            {
                oK = FMOD_System_LoadGeometry(this.systemraw, data, datasize, ref ptr);
            }
            catch
            {
                oK = RESULT.ERR_INVALID_PARAM;
            }
            if (oK == RESULT.OK)
            {
                if (geometry == null)
                {
                    geometry2 = new Geometry();
                    geometry2.setRaw(ptr);
                    geometry = geometry2;
                }
                else
                {
                    geometry.setRaw(ptr);
                }
            }
            return oK;
        }

        public RESULT loadPlugin(string filename, ref uint handle, uint priority)
        {
            return FMOD_System_LoadPlugin(this.systemraw, filename, ref handle, priority);
        }

        public RESULT lockDSP()
        {
            return FMOD_System_LockDSP(this.systemraw);
        }

        public RESULT playDSP(CHANNELINDEX channelid, DSP dsp, bool paused, ref Channel channel)
        {
            IntPtr ptr;
            RESULT oK = RESULT.OK;
            Channel channel2 = null;
            if (channel != null)
            {
                ptr = channel.getRaw();
            }
            else
            {
                ptr = new IntPtr();
            }
            try
            {
                oK = FMOD_System_PlayDSP(this.systemraw, channelid, dsp.getRaw(), paused ? 1 : 0, ref ptr);
            }
            catch
            {
                oK = RESULT.ERR_INVALID_PARAM;
            }
            if (oK == RESULT.OK)
            {
                if (channel == null)
                {
                    channel2 = new Channel();
                    channel2.setRaw(ptr);
                    channel = channel2;
                }
                else
                {
                    channel.setRaw(ptr);
                }
            }
            return oK;
        }

        public RESULT playSound(CHANNELINDEX channelid, Sound sound, bool paused, ref Channel channel)
        {
            IntPtr ptr;
            RESULT oK = RESULT.OK;
            Channel channel2 = null;
            if (channel != null)
            {
                ptr = channel.getRaw();
            }
            else
            {
                ptr = new IntPtr();
            }
            try
            {
                if (VERSION.platform == Platform.X64)
                {
                    oK = FMOD_System_PlaySound_64(this.systemraw, channelid, sound.getRaw(), paused ? 1 : 0, ref ptr);
                }
                else
                {
                    oK = FMOD_System_PlaySound_32(this.systemraw, channelid, sound.getRaw(), paused ? 1 : 0, ref ptr);
                }
            }
            catch
            {
                oK = RESULT.ERR_INVALID_PARAM;
            }
            if (oK == RESULT.OK)
            {
                if (channel == null)
                {
                    channel2 = new Channel();
                    channel2.setRaw(ptr);
                    channel = channel2;
                }
                else
                {
                    channel.setRaw(ptr);
                }
            }
            return oK;
        }

        public RESULT recordStart(Sound sound, bool loop)
        {
            return FMOD_System_RecordStart(this.systemraw, sound.getRaw(), loop);
        }

        public RESULT recordStop()
        {
            return FMOD_System_RecordStop(this.systemraw);
        }

        public RESULT release()
        {
            if (VERSION.platform == Platform.X64)
            {
                return FMOD_System_Release_64(this.systemraw);
            }
            return FMOD_System_Release_32(this.systemraw);
        }

        public RESULT set3DListenerAttributes(int listener, ref VECTOR pos, ref VECTOR vel, ref VECTOR forward, ref VECTOR up)
        {
            return FMOD_System_Set3DListenerAttributes(this.systemraw, listener, ref pos, ref vel, ref forward, ref up);
        }

        public RESULT set3DNumListeners(int numlisteners)
        {
            return FMOD_System_Set3DNumListeners(this.systemraw, numlisteners);
        }

        public RESULT set3DRolloffCallback(CB_3D_ROLLOFFCALLBACK callback)
        {
            return FMOD_System_Set3DRolloffCallback(this.systemraw, callback);
        }

        public RESULT set3DSettings(float dopplerscale, float distancefactor, float rolloffscale)
        {
            return FMOD_System_Set3DSettings(this.systemraw, dopplerscale, distancefactor, rolloffscale);
        }

        public RESULT set3DSpeakerPosition(SPEAKER speaker, float x, float y, bool active)
        {
            return FMOD_System_Set3DSpeakerPosition(this.systemraw, speaker, x, y, active ? 1 : 0);
        }

        public RESULT setAdvancedSettings(ref ADVANCEDSETTINGS settings)
        {
            return FMOD_System_SetAdvancedSettings(this.systemraw, ref settings);
        }

        public RESULT setDriver(int driver)
        {
            if (VERSION.platform == Platform.X64)
            {
                return FMOD_System_SetDriver_64(this.systemraw, driver);
            }
            return FMOD_System_SetDriver_32(this.systemraw, driver);
        }

        public RESULT setDSPBufferSize(uint bufferlength, int numbuffers)
        {
            if (VERSION.platform == Platform.X64)
            {
                return FMOD_System_SetDSPBufferSize_64(this.systemraw, bufferlength, numbuffers);
            }
            return FMOD_System_SetDSPBufferSize_32(this.systemraw, bufferlength, numbuffers);
        }

        public RESULT setFileSystem(FILE_OPENCALLBACK useropen, FILE_CLOSECALLBACK userclose, FILE_READCALLBACK userread, FILE_SEEKCALLBACK userseek, int buffersize)
        {
            return FMOD_System_SetFileSystem(this.systemraw, useropen, userclose, userread, userseek, buffersize);
        }

        public RESULT setGeometrySettings(float maxworldsize)
        {
            return FMOD_System_SetGeometrySettings(this.systemraw, maxworldsize);
        }

        public RESULT setHardwareChannels(int min2d, int max2d, int min3d, int max3d)
        {
            return FMOD_System_SetHardwareChannels(this.systemraw, min2d, max2d, min3d, max3d);
        }

        public RESULT setNetworkProxy(string proxy)
        {
            return FMOD_System_SetNetworkProxy(this.systemraw, proxy);
        }

        public RESULT setNetworkTimeout(int timeout)
        {
            return FMOD_System_SetNetworkTimeout(this.systemraw, timeout);
        }

        public RESULT setOutput(OUTPUTTYPE output)
        {
            return FMOD_System_SetOutput(this.systemraw, output);
        }

        public RESULT setOutputByPlugin(uint handle)
        {
            return FMOD_System_SetOutputByPlugin(this.systemraw, handle);
        }

        public RESULT setPluginPath(string path)
        {
            return FMOD_System_SetPluginPath(this.systemraw, path);
        }

        public void setRaw(IntPtr system)
        {
            this.systemraw = new IntPtr();
            this.systemraw = system;
        }

        public RESULT setRecordDriver(int driver)
        {
            return FMOD_System_SetRecordDriver(this.systemraw, driver);
        }

        public RESULT setReverbAmbientProperties(ref REVERB_PROPERTIES prop)
        {
            return FMOD_System_SetReverbAmbientProperties(this.systemraw, ref prop);
        }

        public RESULT setReverbProperties(ref REVERB_PROPERTIES prop)
        {
            return FMOD_System_SetReverbProperties(this.systemraw, ref prop);
        }

        public RESULT setSoftwareChannels(int numsoftwarechannels)
        {
            return FMOD_System_SetSoftwareChannels(this.systemraw, numsoftwarechannels);
        }

        public RESULT setSoftwareFormat(int samplerate, SOUND_FORMAT format, int numoutputchannels, int maxinputchannels, DSP_RESAMPLER resamplemethod)
        {
            if (VERSION.platform == Platform.X64)
            {
                return FMOD_System_SetSoftwareFormat_64(this.systemraw, samplerate, format, numoutputchannels, maxinputchannels, resamplemethod);
            }
            return FMOD_System_SetSoftwareFormat_32(this.systemraw, samplerate, format, numoutputchannels, maxinputchannels, resamplemethod);
        }

        public RESULT setSpeakerMode(SPEAKERMODE speakermode)
        {
            return FMOD_System_SetSpeakerMode(this.systemraw, speakermode);
        }

        public RESULT setStreamBufferSize(uint filebuffersize, TIMEUNIT filebuffersizetype)
        {
            if (VERSION.platform == Platform.X64)
            {
                return FMOD_System_SetStreamBufferSize_64(this.systemraw, filebuffersize, filebuffersizetype);
            }
            return FMOD_System_SetStreamBufferSize_32(this.systemraw, filebuffersize, filebuffersizetype);
        }

        public RESULT setUserData(IntPtr userdata)
        {
            return FMOD_System_SetUserData(this.systemraw, userdata);
        }

        public RESULT unloadPlugin(uint handle)
        {
            return FMOD_System_UnloadPlugin(this.systemraw, handle);
        }

        public RESULT unlockDSP()
        {
            return FMOD_System_UnlockDSP(this.systemraw);
        }

        public RESULT update()
        {
            if (VERSION.platform == Platform.X64)
            {
                return FMOD_System_Update_64(this.systemraw);
            }
            return FMOD_System_Update_32(this.systemraw);
        }
    }
}

