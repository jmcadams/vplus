namespace FMOD
{
    using System;

    public class DELAYTYPE_UTILITY
    {
        private void FMOD_64BIT_ADD(ref uint hi1, ref uint lo1, uint hi2, uint lo2)
        {
            hi1 += hi2 + (((lo1 + lo2) < lo1) ? 1U : 0);
            lo1 += lo2;
        }

        private void FMOD_64BIT_SUB(ref uint hi1, ref uint lo1, uint hi2, uint lo2)
        {
            hi1 -= hi2 + (((lo1 - lo2) > lo1) ? 1U : 0);
            lo1 -= lo2;
        }
    }
}

