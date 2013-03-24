using System.Runtime.InteropServices;

namespace FMOD
{
    public class REVERB_PROPERTIES
    {
        public float AirAbsorptionHF;
        public float DecayHFRatio;
        public float DecayLFRatio;
        public float DecayTime;
        public float Density;
        public float Diffusion;
        public float EchoDepth;
        public float EchoTime;
        public float EnvDiffusion;
        public uint Environment;
        public float EnvSize;
        public uint Flags;
        public float HFReference;
        public int Instance;
        public float LFReference;
        public float ModulationDepth;
        public float ModulationTime;
        public int Reflections;
        public float ReflectionsDelay;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst=3)]
        public float[] ReflectionsPan;
        public int Reverb;
        public float ReverbDelay;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst=3)]
        public float[] ReverbPan;
        public int Room;
        public int RoomHF;
        public int RoomLF;
        public float RoomRolloffFactor;

        public REVERB_PROPERTIES(int instance, uint environment, float envSize, float envDiffusion, int room, int roomHF, int roomLF, float decayTime, float decayHFRatio, float decayLFRatio, int reflections, float reflectionsDelay, float reflectionsPanx, float reflectionsPany, float reflectionsPanz, int reverb, float reverbDelay, float reverbPanx, float reverbPany, float reverbPanz, float echoTime, float echoDepth, float modulationTime, float modulationDepth, float airAbsorptionHF, float hfReference, float lfReference, float roomRolloffFactor, float diffusion, float density, uint flags)
        {
            Instance = instance;
            Environment = environment;
            EnvSize = envSize;
            EnvDiffusion = envDiffusion;
            Room = room;
            RoomHF = roomHF;
            RoomLF = roomLF;
            DecayTime = decayTime;
            DecayHFRatio = decayHFRatio;
            DecayLFRatio = decayLFRatio;
            Reflections = reflections;
            ReflectionsDelay = reflectionsDelay;
            ReflectionsPan[0] = reflectionsPanx;
            ReflectionsPan[1] = reflectionsPany;
            ReflectionsPan[2] = reflectionsPanz;
            Reverb = reverb;
            ReverbDelay = reverbDelay;
            ReverbPan[0] = reverbPanx;
            ReverbPan[1] = reverbPany;
            ReverbPan[2] = reverbPanz;
            EchoTime = echoTime;
            EchoDepth = echoDepth;
            ModulationTime = modulationTime;
            ModulationDepth = modulationDepth;
            AirAbsorptionHF = airAbsorptionHF;
            HFReference = hfReference;
            LFReference = lfReference;
            RoomRolloffFactor = roomRolloffFactor;
            Diffusion = diffusion;
            Density = density;
            Flags = flags;
        }
    }
}

