namespace K8055
{
    using System;

    public static class K8055
    {
        private static bool m_busy = false;
        private static int[] m_refCounts = new int[4];

        public static void Close(int device)
        {
            if ((device < 0) || (device > 3))
            {
                throw new Exception("Invalid device");
            }
            if (m_refCounts[device] == 1)
            {
                K8055D.SetCurrentDevice((long) device);
                K8055D.CloseDevice();
                m_refCounts[device] = 0;
            }
            else if (m_refCounts[device] > 1)
            {
                m_refCounts[device]--;
            }
        }

        public static bool Open(int device)
        {
            if ((device < 0) || (device > 3))
            {
                throw new Exception("Invalid device");
            }
            if (m_refCounts[device]++ == 0)
            {
                return (K8055D.OpenDevice((long) device) == device);
            }
            return true;
        }

        public static long Read(int device)
        {
            if (m_busy)
            {
                return 0L;
            }
            if ((device < 0) || (device > 3))
            {
                throw new Exception("Invalid device");
            }
            if (m_refCounts[device] == 0)
            {
                throw new Exception("Device is not open");
            }
            K8055D.SetCurrentDevice((long) device);
            return K8055D.ReadAllDigital();
        }

        public static long SearchDevices()
        {
            int num;
            m_busy = true;
            for (num = 0; num < m_refCounts.Length; num++)
            {
                K8055D.SetCurrentDevice((long) num);
                K8055D.CloseDevice();
            }
            long num2 = K8055D.SearchDevices();
            for (num = 0; num < m_refCounts.Length; num++)
            {
                if (m_refCounts[num] > 0)
                {
                    K8055D.OpenDevice((long) num);
                }
            }
            m_busy = false;
            return num2;
        }

        public static void Version()
        {
            K8055D.Version();
        }

        public static void Write(int device, long data)
        {
            if (!m_busy)
            {
                if ((device < 0) || (device > 3))
                {
                    throw new Exception("Invalid device");
                }
                if (m_refCounts[device] == 0)
                {
                    throw new Exception("Device is not open");
                }
                K8055D.SetCurrentDevice((long) device);
                K8055D.WriteAllDigital(data);
            }
        }
    }
}

