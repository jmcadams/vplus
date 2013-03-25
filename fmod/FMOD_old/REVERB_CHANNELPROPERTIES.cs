namespace FMOD
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct REVERB_CHANNELPROPERTIES
    {
        public int Direct;
        public int DirectHF;
        public int Room;
        public int RoomHF;
        public int Obstruction;
        public float ObstructionLFRatio;
        public int Occlusion;
        public float OcclusionLFRatio;
        public float OcclusionRoomRatio;
        public float OcclusionDirectRatio;
        public int Exclusion;
        public float ExclusionLFRatio;
        public int OutsideVolumeHF;
        public float DopplerFactor;
        public float RolloffFactor;
        public float RoomRolloffFactor;
        public float AirAbsorptionFactor;
        public uint Flags;
    }
}

