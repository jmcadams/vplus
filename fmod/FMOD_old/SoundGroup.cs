namespace FMOD
{
    using System;
    using System.Runtime.InteropServices;
    using System.Text;

    public class SoundGroup
    {
        private IntPtr soundgroupraw;

        [DllImport("fmodex")]
        private static extern RESULT FMOD_SoundGroup_GetMaxAudible(IntPtr soundgroupraw, ref int maxaudible);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_SoundGroup_GetMaxAudibleBehavior(IntPtr soundgroupraw, ref SOUNDGROUP_BEHAVIOR behavior);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_SoundGroup_GetMuteFadeSpeed(IntPtr soundgroupraw, ref float speed);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_SoundGroup_GetName(IntPtr soundgroupraw, StringBuilder name, int namelen);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_SoundGroup_GetNumPlaying(IntPtr soundgroupraw, ref int numplaying);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_SoundGroup_GetNumSounds(IntPtr soundgroupraw, ref int numsounds);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_SoundGroup_GetSound(IntPtr soundgroupraw, int index, ref IntPtr sound);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_SoundGroup_GetSystemObject(IntPtr soundgroupraw, ref IntPtr system);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_SoundGroup_GetUserData(IntPtr soundgroupraw, ref IntPtr userdata);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_SoundGroup_Release(IntPtr soundgroupraw);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_SoundGroup_SetMaxAudible(IntPtr soundgroupraw, int maxaudible);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_SoundGroup_SetMaxAudibleBehavior(IntPtr soundgroupraw, SOUNDGROUP_BEHAVIOR behavior);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_SoundGroup_SetMuteFadeSpeed(IntPtr soundgroupraw, float speed);
        [DllImport("fmodex")]
        private static extern RESULT FMOD_SoundGroup_SetUserData(IntPtr soundgroupraw, IntPtr userdata);
        public RESULT getMaxAudible(ref int maxaudible)
        {
            return FMOD_SoundGroup_GetMaxAudible(this.soundgroupraw, ref maxaudible);
        }

        public RESULT getMaxAudibleBehavior(ref SOUNDGROUP_BEHAVIOR behavior)
        {
            return FMOD_SoundGroup_GetMaxAudibleBehavior(this.soundgroupraw, ref behavior);
        }

        public RESULT getMuteFadeSpeed(ref float speed)
        {
            return FMOD_SoundGroup_GetMuteFadeSpeed(this.soundgroupraw, ref speed);
        }

        public RESULT getName(StringBuilder name, int namelen)
        {
            return FMOD_SoundGroup_GetName(this.soundgroupraw, name, namelen);
        }

        public RESULT getNumPlaying(ref int numplaying)
        {
            return FMOD_SoundGroup_GetNumPlaying(this.soundgroupraw, ref numplaying);
        }

        public RESULT getNumSounds(ref int numsounds)
        {
            return FMOD_SoundGroup_GetNumSounds(this.soundgroupraw, ref numsounds);
        }

        public IntPtr getRaw()
        {
            return this.soundgroupraw;
        }

        public RESULT getSound(int index, ref Sound sound)
        {
            RESULT oK = RESULT.OK;
            IntPtr ptr = new IntPtr();
            Sound sound2 = null;
            try
            {
                oK = FMOD_SoundGroup_GetSound(this.soundgroupraw, index, ref ptr);
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

        public RESULT getSystemObject(ref _System system)
        {
            RESULT oK = RESULT.OK;
            IntPtr ptr = new IntPtr();
            _System system2 = null;
            try
            {
                oK = FMOD_SoundGroup_GetSystemObject(this.soundgroupraw, ref ptr);
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
                    system2.setRaw(ptr);
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
            return FMOD_SoundGroup_GetUserData(this.soundgroupraw, ref userdata);
        }

        public RESULT release()
        {
            return FMOD_SoundGroup_Release(this.soundgroupraw);
        }

        public RESULT setMaxAudible(int maxaudible)
        {
            return FMOD_SoundGroup_SetMaxAudible(this.soundgroupraw, maxaudible);
        }

        public RESULT setMaxAudibleBehavior(SOUNDGROUP_BEHAVIOR behavior)
        {
            return FMOD_SoundGroup_SetMaxAudibleBehavior(this.soundgroupraw, behavior);
        }

        public RESULT setMuteFadeSpeed(float speed)
        {
            return FMOD_SoundGroup_SetMuteFadeSpeed(this.soundgroupraw, speed);
        }

        public void setRaw(IntPtr soundgroup)
        {
            this.soundgroupraw = new IntPtr();
            this.soundgroupraw = soundgroup;
        }

        public RESULT setUserData(IntPtr userdata)
        {
            return FMOD_SoundGroup_SetUserData(this.soundgroupraw, userdata);
        }
    }
}

