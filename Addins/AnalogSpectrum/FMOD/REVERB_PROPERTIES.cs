namespace FMOD
{
    using System;
    using System.Runtime.InteropServices;

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
            this.Instance = instance;
            this.Environment = environment;
            this.EnvSize = envSize;
            this.EnvDiffusion = envDiffusion;
            this.Room = room;
            this.RoomHF = roomHF;
            this.RoomLF = roomLF;
            this.DecayTime = decayTime;
            this.DecayHFRatio = decayHFRatio;
            this.DecayLFRatio = decayLFRatio;
            this.Reflections = reflections;
            this.ReflectionsDelay = reflectionsDelay;
            this.ReflectionsPan[0] = reflectionsPanx;
            this.ReflectionsPan[1] = reflectionsPany;
            this.ReflectionsPan[2] = reflectionsPanz;
            this.Reverb = reverb;
            this.ReverbDelay = reverbDelay;
            this.ReverbPan[0] = reverbPanx;
            this.ReverbPan[1] = reverbPany;
            this.ReverbPan[2] = reverbPanz;
            this.EchoTime = echoTime;
            this.EchoDepth = echoDepth;
            this.ModulationTime = modulationTime;
            this.ModulationDepth = modulationDepth;
            this.AirAbsorptionHF = airAbsorptionHF;
            this.HFReference = hfReference;
            this.LFReference = lfReference;
            this.RoomRolloffFactor = roomRolloffFactor;
            this.Diffusion = diffusion;
            this.Density = density;
            this.Flags = flags;
        }
    }
}

