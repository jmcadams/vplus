namespace Standard
{
    using System;
    using System.Collections.Generic;
    using Vixen;

    public static class Actions
    {
        private static byte[,] BresenhamArray(int from, int to, int span, byte level, int eventPeriod)
        {
            int num5;
            int num6;
            int num7;
            int num8;
            int num10;
            int num11;
            int num13;
            int num14;
            int num = Math.Abs((int) (from - to)) + 1;
            byte[,] buffer = new byte[num, span / eventPeriod];
            int num3 = buffer.GetLength(1) - 1;
            int num4 = buffer.GetLength(0) - 1;
            if (num3 >= num4)
            {
                num5 = num3 + 1;
                num6 = (num4 << 1) - num3;
                num7 = num4 << 1;
                num8 = (num4 - num3) << 1;
                num10 = 1;
                num11 = 1;
                num13 = 0;
                num14 = 1;
            }
            else
            {
                num5 = num4 + 1;
                num6 = (num3 << 1) - num4;
                num7 = num3 << 1;
                num8 = (num3 - num4) << 1;
                num10 = 0;
                num11 = 1;
                num13 = 1;
                num14 = 1;
            }
            if (from > to)
            {
                num13 = -num13;
                num14 = -num14;
            }
            int num9 = 0;
            int num12 = from - Math.Min(from, to);
            for (int i = 0; i < num5; i++)
            {
                buffer[num12, num9] = level;
                if (num6 < 0)
                {
                    num6 += num7;
                    num9 += num10;
                    num12 += num13;
                }
                else
                {
                    num6 += num8;
                    num9 += num11;
                    num12 += num14;
                }
            }
            return buffer;
        }

        private static void Cap(ref int value)
        {
            value = Math.Max(value, 0);
            value = Math.Min(value, 100);
        }

        public static void Chase(Context context, IChannelEnumerable channels, params uint[] modifiers)
        {
            byte level = 0xff;
            int num2 = 0;
            BlockingTimer timer = null;
            int num3 = 0;
            ParseModifiers(context, modifiers);
            if (context.m_modifiers.TryGetValue(ModifierType.At, out num3))
            {
                level = (byte) Math.Round((double) ((num3 * 255f) / 100f), MidpointRounding.AwayFromZero);
            }
            if (!context.m_modifiers.TryGetValue(ModifierType.For, out num2))
            {
                throw new Exception("The Chase action requires a time span modifier (Over, For).");
            }
            timer = GenerateTimeFrameTimer(context, BresenhamArray(channels[0].OutputChannel, channels[channels.Count - 1].OutputChannel, num2, level, context.m_eventPeriodLength), channels, context.m_eventPeriodLength);
            if (context.m_modifiers.ContainsKey(ModifierType.Wait) && (timer != null))
            {
                context.WaitOn(timer);
            }
        }

        private static BlockingTimer GenerateIntervalTimer(Context context, byte[,] eventValues, IChannelEnumerable channels, int interval, int count, byte intensityLevel)
        {
            return context.CreateTimer(interval, new IntervalState(eventValues, intensityLevel, channels, count));
        }

        private static BlockingTimer GenerateRandomIntervalTimer(Context context, byte[,] eventValues, IChannelEnumerable channels, int interval, int count, float saturationLevel, byte intensityLevel)
        {
            return context.CreateTimer(interval, new RandomIntervalState(eventValues, channels, saturationLevel, intensityLevel, count));
        }

        internal static void GenerateRandomValues(byte[,] valueArray, float saturationLevel, byte intensityLevel)
        {
            int tickCount = Environment.TickCount;
            while (tickCount == Environment.TickCount)
            {
            }
            System.Random random = new System.Random((int) (Environment.TickCount * 1.1f));
            int length = valueArray.GetLength(0);
            int num4 = valueArray.GetLength(1);
            List<int> list = new List<int>();
            for (int i = 0; i < num4; i++)
            {
                int num5;
                if (!(saturationLevel == 0f))
                {
                    num5 = (int) Math.Round((double) (length * saturationLevel));
                }
                else
                {
                    num5 = random.Next(length + 1);
                }
                list.Clear();
                for (int j = 0; j < length; j++)
                {
                    valueArray[j, i] = 0;
                    list.Add(j);
                }
                while (num5-- > 0)
                {
                    int index = random.Next(list.Count);
                    valueArray[list[index], i] = intensityLevel;
                    list.RemoveAt(index);
                }
            }
        }

        private static BlockingTimer GenerateTimeFrameTimer(Context context, byte[,] eventValues, IChannelEnumerable channels, int interval)
        {
            return context.CreateTimer(interval, new TimeFrameState(eventValues, channels));
        }

        public static void Off(Context context, IChannelEnumerable channels, params uint[] modifiers)
        {
            BlockingTimer timer = null;
            BlockingTimer timer2 = null;
            int count;
            byte[,] buffer;
            int num = 0;
            int num2 = 0;
            ParseModifiers(context, modifiers);
            context.m_modifiers.TryGetValue(ModifierType.For, out num);
            context.m_modifiers.TryGetValue(ModifierType.Every, out num2);
            if ((num != 0) && (num2 == 0))
            {
                count = channels.Count;
                buffer = new byte[count, 1];
                for (int i = 0; i < count; i++)
                {
                    for (int j = 0; j < buffer.GetLength(1); j++)
                    {
                        buffer[i, j] = 0;
                    }
                }
                timer = GenerateTimeFrameTimer(context, buffer, channels, num);
            }
            else if (num2 != 0)
            {
                buffer = new byte[channels.Count, 1];
                count = 0;
                if (num != 0)
                {
                    count = num / num2;
                }
                timer2 = GenerateIntervalTimer(context, buffer, channels, num2, count, 0);
            }
            else
            {
                context.BeginUpdate();
                foreach (Channel channel in channels)
                {
                    context.AffectChannel(channel.OutputChannel, 0);
                }
                context.EndUpdate();
            }
            if (context.m_modifiers.ContainsKey(ModifierType.Wait) && (timer != null))
            {
                context.WaitOn(timer);
            }
        }

        public static void On(Context context, IChannelEnumerable channels, params uint[] modifiers)
        {
            int num2;
            int count;
            byte[,] buffer;
            byte intensityLevel = 0xff;
            BlockingTimer timer = null;
            BlockingTimer timer2 = null;
            int num3 = 0;
            int num4 = 0;
            ParseModifiers(context, modifiers);
            if (context.m_modifiers.TryGetValue(ModifierType.At, out num2))
            {
                intensityLevel = (byte) Math.Round((double) ((num2 * 255f) / 100f), MidpointRounding.AwayFromZero);
            }
            context.m_modifiers.TryGetValue(ModifierType.For, out num3);
            context.m_modifiers.TryGetValue(ModifierType.Every, out num4);
            if ((num3 != 0) && (num4 == 0))
            {
                count = channels.Count;
                buffer = new byte[count, 1];
                for (int i = 0; i < count; i++)
                {
                    for (int j = 0; j < buffer.GetLength(1); j++)
                    {
                        buffer[i, j] = intensityLevel;
                    }
                }
                timer = GenerateTimeFrameTimer(context, buffer, channels, num3);
            }
            else if (num4 != 0)
            {
                buffer = new byte[channels.Count, 1];
                count = 0;
                if (num3 != 0)
                {
                    count = num3 / num4;
                }
                timer2 = GenerateIntervalTimer(context, buffer, channels, num4, count, intensityLevel);
            }
            else
            {
                context.BeginUpdate();
                foreach (Channel channel in channels)
                {
                    context.AffectChannel(channel.OutputChannel, intensityLevel);
                }
                context.EndUpdate();
            }
            if (context.m_modifiers.ContainsKey(ModifierType.Wait) && (timer != null))
            {
                context.WaitOn(timer);
            }
        }

        private static void ParseModifiers(Context context, params uint[] modifiers)
        {
            context.m_modifiers.Clear();
            foreach (uint num in modifiers)
            {
                context.m_modifiers[(ModifierType) (num & Modifier.MODIFIER_TYPE)] = ((int) num) & Modifier.MODIFIER_VALUE;
            }
        }

        public static void Ramp(Context context, IChannelEnumerable channels, int startIntensity, int endIntensity, params uint[] modifiers)
        {
            int num = 0;
            BlockingTimer timer = null;
            Cap(ref startIntensity);
            Cap(ref endIntensity);
            ParseModifiers(context, modifiers);
            if (!context.m_modifiers.TryGetValue(ModifierType.For, out num))
            {
                throw new Exception("The Ramp action requires a time span modifier (Over, For).");
            }
            byte[,] eventValues = new byte[channels.Count, num / context.m_eventPeriodLength];
            byte num2 = (byte) ((((float) startIntensity) / 100f) * 255f);
            byte num3 = (byte) ((((float) endIntensity) / 100f) * 255f);
            float num4 = ((float) (num3 - num2)) / ((float) (eventValues.GetLength(1) - 1));
            for (int i = 0; i < eventValues.GetLength(1); i++)
            {
                byte num6 = (byte) (num2 + (i * num4));
                for (int j = 0; j < channels.Count; j++)
                {
                    eventValues[j, i] = num6;
                }
            }
            timer = GenerateTimeFrameTimer(context, eventValues, channels, context.m_eventPeriodLength);
            if (context.m_modifiers.ContainsKey(ModifierType.Wait) && (timer != null))
            {
                context.WaitOn(timer);
            }
        }

        public static void Random(Context context, IChannelEnumerable channels, int saturationLevel, params uint[] modifiers)
        {
            byte[,] buffer;
            byte intensityLevel = 0xff;
            int num2 = 0;
            int num3 = 0;
            BlockingTimer timer = null;
            int num4 = 0;
            Cap(ref saturationLevel);
            ParseModifiers(context, modifiers);
            if (context.m_modifiers.TryGetValue(ModifierType.At, out num4))
            {
                intensityLevel = (byte) Math.Round((double) ((num4 * 255f) / 100f), MidpointRounding.AwayFromZero);
            }
            context.m_modifiers.TryGetValue(ModifierType.For, out num2);
            context.m_modifiers.TryGetValue(ModifierType.Every, out num3);
            if ((num2 != 0) && (num3 == 0))
            {
                buffer = new byte[channels.Count, num2 / context.m_eventPeriodLength];
                GenerateRandomValues(buffer, ((float) saturationLevel) / 100f, intensityLevel);
                timer = GenerateTimeFrameTimer(context, buffer, channels, context.m_eventPeriodLength);
            }
            else if (num3 != 0)
            {
                buffer = new byte[channels.Count, 1];
                int count = 0;
                if (num2 != 0)
                {
                    count = num2 / num3;
                }
                timer = GenerateRandomIntervalTimer(context, buffer, channels, num3, count, ((float) saturationLevel) / 100f, intensityLevel);
            }
            else
            {
                buffer = new byte[channels.Count, 1];
                context.OutputRandom(buffer, ((float) saturationLevel) / 100f, intensityLevel);
            }
            if (context.m_modifiers.ContainsKey(ModifierType.Wait) && (timer != null))
            {
                context.WaitOn(timer);
            }
        }

        public static void WaitOnTimers(Context context)
        {
            context.WaitOnAny();
        }
    }
}

