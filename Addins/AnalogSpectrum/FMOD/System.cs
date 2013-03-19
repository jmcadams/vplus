using System;
using System.Runtime.InteropServices;
using System.Text;

namespace FMOD
{

    public class System
    {
        private IntPtr systemraw;

        public RESULT addDSP(DSP dsp)
        {
            return FMOD_System_AddDSP(this.systemraw, dsp.getRaw());
        }

        public RESULT attachFileSystem(FILE_OPENCALLBACK useropen, FILE_CLOSECALLBACK userclose, FILE_READCALLBACK userread, FILE_SEEKCALLBACK userseek)
        {
            return FMOD_System_AttachFileSystem(this.systemraw, useropen, userclose, userread, userseek);
        }

        public RESULT close()
        {
            return FMOD_System_Close(this.systemraw);
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

        public RESULT createCodec(IntPtr codecdescription)
        {
            return FMOD_System_CreateCodec(this.systemraw, codecdescription);
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
                oK = FMOD_System_CreateSound(this.systemraw, name_or_data, mode, 0, ref ptr);
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
                oK = FMOD_System_CreateSound(this.systemraw, data, mode, ref exinfo, ref ptr);
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
                oK = FMOD_System_CreateSound(this.systemraw, name_or_data, mode, ref exinfo, ref ptr);
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

        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_System_AddDSP(IntPtr system, IntPtr dsp);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_System_AttachFileSystem(IntPtr system, FILE_OPENCALLBACK useropen, FILE_CLOSECALLBACK userclose, FILE_READCALLBACK userread, FILE_SEEKCALLBACK userseek);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_System_Close(IntPtr system);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_System_CreateChannelGroup(IntPtr system, string name, ref IntPtr channelgroup);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_System_CreateCodec(IntPtr system, IntPtr codecdescription);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_System_CreateDSP(IntPtr system, ref DSP_DESCRIPTION description, ref IntPtr dsp);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_System_CreateDSPByIndex(IntPtr system, int index, ref IntPtr dsp);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_System_CreateDSPByType(IntPtr system, DSP_TYPE type, ref IntPtr dsp);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_System_CreateGeometry(IntPtr system, int maxPolygons, int maxVertices, ref IntPtr geometryf);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_System_CreateSound(IntPtr system, string name_or_data, MODE mode, ref CREATESOUNDEXINFO exinfo, ref IntPtr sound);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_System_CreateSound(IntPtr system, byte[] name_or_data, MODE mode, int exinfo, ref IntPtr sound);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_System_CreateSound(IntPtr system, string name_or_data, MODE mode, int exinfo, ref IntPtr sound);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_System_CreateSound(IntPtr system, byte[] name_or_data, MODE mode, ref CREATESOUNDEXINFO exinfo, ref IntPtr sound);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_System_CreateStream(IntPtr system, byte[] name_or_data, MODE mode, ref CREATESOUNDEXINFO exinfo, ref IntPtr sound);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_System_CreateStream(IntPtr system, byte[] name_or_data, MODE mode, int exinfo, ref IntPtr sound);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_System_CreateStream(IntPtr system, string name_or_data, MODE mode, int exinfo, ref IntPtr sound);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_System_CreateStream(IntPtr system, string name_or_data, MODE mode, ref CREATESOUNDEXINFO exinfo, ref IntPtr sound);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_System_Get3DListenerAttributes(IntPtr system, int listener, ref VECTOR pos, ref VECTOR vel, ref VECTOR forward, ref VECTOR up);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_System_Get3DNumListeners(IntPtr system, ref int numlisteners);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_System_Get3DSettings(IntPtr system, ref float dopplerscale, ref float distancefactor, ref float rolloffscale);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_System_GetAdvancedSettings(IntPtr system, ref ADVANCEDSETTINGS settings);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_System_GetCDROMDriveName(IntPtr system, int drive, StringBuilder drivename, int drivenamelen, StringBuilder scsiname, int scsinamelen, StringBuilder devicename, int devicenamelen);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_System_GetChannel(IntPtr system, int channelid, ref IntPtr channel);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_System_GetChannelsPlaying(IntPtr system, ref int channels);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_System_GetCPUUsage(IntPtr system, ref float dsp, ref float stream, ref float update, ref float total);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_System_GetDriver(IntPtr system, ref int driver);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_System_GetDriverCaps(IntPtr system, int id, ref CAPS caps, ref int minfrequency, ref int maxfrequency, ref SPEAKERMODE controlpanelspeakermode);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_System_GetDriverName(IntPtr system, int id, StringBuilder name, int namelen);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_System_GetDSPBufferSize(IntPtr system, ref uint bufferlength, ref int numbuffers);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_System_GetDSPHead(IntPtr system, ref IntPtr dsp);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_System_GetFileBufferSize(IntPtr system, ref int sizebytes);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_System_GetGeometrySettings(IntPtr system, ref float maxWorldSize);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_System_GetHardwareChannels(IntPtr system, ref int num2d, ref int num3d, ref int total);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_System_GetMasterChannelGroup(IntPtr system, ref IntPtr channelgroup);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_System_GetNetworkProxy(IntPtr system, StringBuilder proxy, int proxylen);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_System_GetNetworkTimeout(IntPtr system, ref int timeout);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_System_GetNumCDROMDrives(IntPtr system, ref int numdrives);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_System_GetNumDrivers(IntPtr system, ref int numdrivers);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_System_GetNumPlugins(IntPtr system, PLUGINTYPE plugintype, ref int numplugins);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_System_GetOutput(IntPtr system, ref OUTPUTTYPE output);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_System_GetOutputByPlugin(IntPtr system, ref int index);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_System_GetOutputHandle(IntPtr system, ref IntPtr handle);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_System_GetPluginInfo(IntPtr system, PLUGINTYPE plugintype, int index, StringBuilder name, int namelen, ref uint version);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_System_GetRecordDriver(IntPtr system, ref int driver);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_System_GetRecordDriverName(IntPtr system, int id, StringBuilder name, int namelen);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_System_GetRecordNumDrivers(IntPtr system, ref int numdrivers);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_System_GetRecordPosition(IntPtr system, ref uint position);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_System_GetReverbProperties(IntPtr system, ref REVERB_PROPERTIES prop);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_System_GetSoftwareChannels(IntPtr system, ref int numsoftwarechannels);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_System_GetSoftwareFormat(IntPtr system, ref int samplerate, ref SOUND_FORMAT format, ref int numoutputchannels, ref int maxinputchannels, ref DSP_RESAMPLER resamplemethod, ref int bits);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_System_GetSoundRAM(IntPtr system, ref int currentalloced, ref int maxalloced, ref int total);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_System_GetSpeakerMode(IntPtr system, ref SPEAKERMODE speakermode);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_System_GetSpeakerPosition(IntPtr system, SPEAKER speaker, ref float x, ref float y);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_System_GetSpectrum(IntPtr system, [MarshalAs(UnmanagedType.LPArray)] float[] spectrumarray, int numvalues, int channeloffset, DSP_FFT_WINDOW windowtype);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_System_GetStreamBufferSize(IntPtr system, ref uint filebuffersize, ref TIMEUNIT filebuffersizetype);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_System_GetUserData(IntPtr system, ref IntPtr userdata);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_System_GetVersion(IntPtr system, ref uint version);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_System_GetWaveData(IntPtr system, [MarshalAs(UnmanagedType.LPArray)] float[] wavearray, int numvalues, int channeloffset);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_System_Init(IntPtr system, int maxchannels, INITFLAG flags, IntPtr extradata);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_System_IsRecording(IntPtr system, ref bool recording);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_System_LoadGeometry(IntPtr system, IntPtr data, int dataSize, ref IntPtr geometry);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_System_LoadPlugin(IntPtr system, string filename, ref PLUGINTYPE plugintype, ref int index);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_System_LockDSP(IntPtr system);
        [DllImport("fmodex.dll")]
        public static extern RESULT FMOD_System_PlayDSP(IntPtr system, CHANNELINDEX channelid, IntPtr dsp, bool paused, ref IntPtr channel);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_System_PlaySound(IntPtr system, CHANNELINDEX channelid, IntPtr sound, bool paused, ref IntPtr channel);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_System_RecordStart(IntPtr system, IntPtr sound, bool loop);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_System_RecordStop(IntPtr system);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_System_Release(IntPtr system);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_System_Set3DListenerAttributes(IntPtr system, int listener, ref VECTOR pos, ref VECTOR vel, ref VECTOR forward, ref VECTOR up);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_System_Set3DNumListeners(IntPtr system, int numlisteners);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_System_Set3DSettings(IntPtr system, float dopplerscale, float distancefactor, float rolloffscale);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_System_SetAdvancedSettings(IntPtr system, ref ADVANCEDSETTINGS settings);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_System_SetDriver(IntPtr system, int driver);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_System_SetDSPBufferSize(IntPtr system, uint bufferlength, int numbuffers);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_System_SetFileBufferSize(IntPtr system, int sizebytes);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_System_SetFileSystem(IntPtr system, FILE_OPENCALLBACK useropen, FILE_CLOSECALLBACK userclose, FILE_READCALLBACK userread, FILE_SEEKCALLBACK userseek, int buffersize);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_System_SetGeometrySettings(IntPtr system, float maxWorldSize);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_System_SetHardwareChannels(IntPtr system, int min2d, int max2d, int min3d, int max3d);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_System_SetNetworkProxy(IntPtr system, string proxy);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_System_SetNetworkTimeout(IntPtr system, int timeout);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_System_SetOutput(IntPtr system, OUTPUTTYPE output);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_System_SetOutputByPlugin(IntPtr system, int index);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_System_SetPluginPath(IntPtr system, string path);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_System_SetRecordDriver(IntPtr system, int driver);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_System_SetReverbProperties(IntPtr system, ref REVERB_PROPERTIES prop);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_System_SetSoftwareChannels(IntPtr system, int numsoftwarechannels);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_System_SetSoftwareFormat(IntPtr system, int samplerate, SOUND_FORMAT format, int numoutputchannels, int maxinputchannels, DSP_RESAMPLER resamplemethod);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_System_SetSpeakerMode(IntPtr system, SPEAKERMODE speakermode);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_System_SetSpeakerPosition(IntPtr system, SPEAKER speaker, float x, float y);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_System_SetStreamBufferSize(IntPtr system, uint filebuffersize, TIMEUNIT filebuffersizetype);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_System_SetUserData(IntPtr system, IntPtr userdata);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_System_UnloadPlugin(IntPtr system, PLUGINTYPE plugintype, int index);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_System_UnlockDSP(IntPtr system);
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_System_Update(IntPtr system);
        [DllImport("fmodex.dll")]
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

        public RESULT getDriverName(int id, StringBuilder name, int namelen)
        {
            return FMOD_System_GetDriverName(this.systemraw, id, name, namelen);
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
            return FMOD_System_GetNumDrivers(this.systemraw, ref numdrivers);
        }

        public RESULT getNumPlugins(PLUGINTYPE plugintype, ref int numplugins)
        {
            return FMOD_System_GetNumPlugins(this.systemraw, plugintype, ref numplugins);
        }

        public RESULT getOutput(ref OUTPUTTYPE output)
        {
            return FMOD_System_GetOutput(this.systemraw, ref output);
        }

        public RESULT getOutputByPlugin(ref int index)
        {
            return FMOD_System_GetOutputByPlugin(this.systemraw, ref index);
        }

        public RESULT getOutputHandle(ref IntPtr handle)
        {
            return FMOD_System_GetOutputHandle(this.systemraw, ref handle);
        }

        public RESULT getPluginInfo(PLUGINTYPE plugintype, int index, StringBuilder name, int namelen, ref uint version)
        {
            return FMOD_System_GetPluginInfo(this.systemraw, plugintype, index, name, namelen, ref version);
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

        public RESULT getRecordDriverName(int id, StringBuilder name, int namelen)
        {
            return FMOD_System_GetRecordDriverName(this.systemraw, id, name, namelen);
        }

        public RESULT getRecordNumDrivers(ref int numdrivers)
        {
            return FMOD_System_GetRecordNumDrivers(this.systemraw, ref numdrivers);
        }

        public RESULT getRecordPosition(ref uint position)
        {
            return FMOD_System_GetRecordPosition(this.systemraw, ref position);
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
            return FMOD_System_GetSoftwareFormat(this.systemraw, ref samplerate, ref format, ref numoutputchannels, ref maxinputchannels, ref resamplemethod, ref bits);
        }

        public RESULT getSoundRam(ref int currentalloced, ref int maxalloced, ref int total)
        {
            return FMOD_System_GetSoundRAM(this.systemraw, ref currentalloced, ref maxalloced, ref total);
        }

        public RESULT getSpeakerMode(ref SPEAKERMODE speakermode)
        {
            return FMOD_System_GetSpeakerMode(this.systemraw, ref speakermode);
        }

        public RESULT getSpeakerPosition(SPEAKER speaker, ref float x, ref float y)
        {
            return FMOD_System_GetSpeakerPosition(this.systemraw, speaker, ref x, ref y);
        }

        public RESULT getSpectrum(float[] spectrumarray, int numvalues, int channeloffset, DSP_FFT_WINDOW windowtype)
        {
            return FMOD_System_GetSpectrum(this.systemraw, spectrumarray, numvalues, channeloffset, windowtype);
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
            return FMOD_System_Init(this.systemraw, maxchannels, flags, extradata);
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

        public RESULT loadPlugin(string filename, ref PLUGINTYPE plugintype, ref int index)
        {
            return FMOD_System_LoadPlugin(this.systemraw, filename, ref plugintype, ref index);
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
                oK = FMOD_System_PlayDSP(this.systemraw, channelid, dsp.getRaw(), paused, ref ptr);
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
                oK = FMOD_System_PlaySound(this.systemraw, channelid, sound.getRaw(), paused, ref ptr);
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
            return FMOD_System_Release(this.systemraw);
        }

        public RESULT set3DListenerAttributes(int listener, ref VECTOR pos, ref VECTOR vel, ref VECTOR forward, ref VECTOR up)
        {
            return FMOD_System_Set3DListenerAttributes(this.systemraw, listener, ref pos, ref vel, ref forward, ref up);
        }

        public RESULT set3DNumListeners(int numlisteners)
        {
            return FMOD_System_Set3DNumListeners(this.systemraw, numlisteners);
        }

        public RESULT set3DSettings(float dopplerscale, float distancefactor, float rolloffscale)
        {
            return FMOD_System_Set3DSettings(this.systemraw, dopplerscale, distancefactor, rolloffscale);
        }

        public RESULT setAdvancedSettings(ref ADVANCEDSETTINGS settings)
        {
            return FMOD_System_SetAdvancedSettings(this.systemraw, ref settings);
        }

        public RESULT setDriver(int driver)
        {
            return FMOD_System_SetDriver(this.systemraw, driver);
        }

        public RESULT setDSPBufferSize(uint bufferlength, int numbuffers)
        {
            return FMOD_System_SetDSPBufferSize(this.systemraw, bufferlength, numbuffers);
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

        public RESULT setOutputByPlugin(int index)
        {
            return FMOD_System_SetOutputByPlugin(this.systemraw, index);
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
            return FMOD_System_SetSoftwareFormat(this.systemraw, samplerate, format, numoutputchannels, maxinputchannels, resamplemethod);
        }

        public RESULT setSpeakerMode(SPEAKERMODE speakermode)
        {
            return FMOD_System_SetSpeakerMode(this.systemraw, speakermode);
        }

        public RESULT setSpeakerPosition(SPEAKER speaker, float x, float y)
        {
            return FMOD_System_SetSpeakerPosition(this.systemraw, speaker, x, y);
        }

        public RESULT setStreamBufferSize(uint filebuffersize, TIMEUNIT filebuffersizetype)
        {
            return FMOD_System_SetStreamBufferSize(this.systemraw, filebuffersize, filebuffersizetype);
        }

        public RESULT setUserData(IntPtr userdata)
        {
            return FMOD_System_SetUserData(this.systemraw, userdata);
        }

        public RESULT unloadPlugin(PLUGINTYPE plugintype, int index)
        {
            return FMOD_System_UnloadPlugin(this.systemraw, plugintype, index);
        }

        public RESULT unlockDSP()
        {
            return FMOD_System_UnlockDSP(this.systemraw);
        }

        public RESULT update()
        {
            return FMOD_System_Update(this.systemraw);
        }

        public RESULT updateFinished()
        {
            return FMOD_System_UpdateFinished(this.systemraw);
        }
    }
}

