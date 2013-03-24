namespace LedTriks
{
    using LedTriksUtil;
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using System.Threading;
    using System.Xml;
    using VixenPlus;

    public static class Actions
    {
        public static Frame[] GenerateText(Context context, string text)
        {
            context.Generator.TextFont = new Font(context.FontName, (float) context.Generator.TextHeight, context.FontStyle, GraphicsUnit.Pixel);
            return context.Generator.GenerateTextFrames(text).ToArray();
        }

        public static Frame[] GenerateText(Context context, string text, TimeSpan staticTextTimeLength)
        {
            context.Generator.TextFont = new Font(context.FontName, (float) context.Generator.TextHeight, context.FontStyle, GraphicsUnit.Pixel);
            return context.Generator.GenerateTextFrames(text, staticTextTimeLength).ToArray();
        }

        public static void GenerateTextAndOutput(Context context, string text)
        {
            context.Generator.TextFont = new Font(context.FontName, (float) context.Generator.TextHeight, context.FontStyle, GraphicsUnit.Pixel);
            foreach (List<Frame> list in context.Generator.GenerateTextFrames(text, 10))
            {
                context.FrameBuffer.AddRange(list);
            }
        }

        public static void GenerateTextAndOutput(Context context, string text, TimeSpan staticTextTimeLength)
        {
            context.Generator.TextFont = new Font(context.FontName, (float) context.Generator.TextHeight, context.FontStyle, GraphicsUnit.Pixel);
            foreach (List<Frame> list in context.Generator.GenerateTextFrames(text, 10, staticTextTimeLength))
            {
                context.FrameBuffer.AddRange(list);
            }
        }

        public static Frame[] LoadFramesFromSequence(string sequenceFileName)
        {
            List<Frame> list = new List<Frame>();
            EventSequence sequence = new EventSequence(Path.Combine(Paths.SequencePath, sequenceFileName));
            foreach (XmlNode node in sequence.Extensions[".led"].SelectNodes("Frames/Frame"))
            {
                list.Add(new Frame(node));
            }
            return list.ToArray();
        }

        public static Frame[] MergeFrames(Frame[] sourceFrames, Frame[] destFrames)
        {
            return MergeFrames(sourceFrames, 0, destFrames, 0, Math.Min(sourceFrames.Length, destFrames.Length));
        }

        public static Frame[] MergeFrames(Frame[] sourceFrames, Frame[] destFrames, int count)
        {
            return MergeFrames(sourceFrames, 0, destFrames, 0, count);
        }

        public static Frame[] MergeFrames(Frame[] sourceFrames, int sourceIndex, Frame[] destFrames, int destIndex, int count)
        {
            int num = Math.Min(sourceFrames.Length, sourceIndex + count);
            int num2 = Math.Min(destFrames.Length, destIndex + count);
            int num3 = Math.Min(num, num2);
            for (int i = 0; i < num3; i++)
            {
                destFrames[destIndex + i] = sourceFrames[sourceIndex + i].MergeWith(destFrames[destIndex + i]);
            }
            return destFrames;
        }

        public static void OutputFrames(Context context, Frame[] frames)
        {
            context.FrameBuffer.AddRange(frames);
        }

        public static void WaitForEnd(Context context)
        {
            while ((context.State == Context.RunState.Running) && (context.FrameBuffer.Count > 0))
            {
                Thread.Sleep(context.FrameLength);
            }
        }
    }
}

