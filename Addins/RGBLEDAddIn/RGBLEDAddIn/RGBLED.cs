namespace RGBLEDAddIn
{
    using System;
    using System.Windows.Forms;
    using System.Xml;
    using VixenPlus;

    public class RGBLED : IAddIn, ILoadable, IPlugIn
    {
        public bool Execute(EventSequence sequence)
        {
            if (sequence == null)
            {
                throw new Exception("RGBLED plug-in requires a sequence to be open.");
            }
            MainDialog dialog = new MainDialog(sequence);
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                int num = Math.Min(dialog.StartEventIndex + dialog.DurationEventCount, sequence.TotalEventPeriods);
                float num2 = ((float) (dialog.EndColor.R - dialog.StartColor.R)) / ((float) dialog.DurationEventCount);
                float num3 = ((float) (dialog.EndColor.G - dialog.StartColor.G)) / ((float) dialog.DurationEventCount);
                float num4 = ((float) (dialog.EndColor.B - dialog.StartColor.B)) / ((float) dialog.DurationEventCount);
                float r = dialog.StartColor.R;
                float g = dialog.StartColor.G;
                float b = dialog.StartColor.B;
                int startChannel = dialog.StartChannel;
                for (int i = dialog.StartEventIndex; i < num; i++)
                {
                    sequence.EventValues[startChannel, i] = (byte) r;
                    sequence.EventValues[startChannel + 1, i] = (byte) g;
                    sequence.EventValues[startChannel + 2, i] = (byte) b;
                    r += num2;
                    g += num3;
                    b += num4;
                }
                dialog.Dispose();
                return true;
            }
            dialog.Dispose();
            return false;
        }

        public void Loading(XmlNode dataNode)
        {
        }

        public void Unloading()
        {
        }

        public string Author
        {
            get
            {
                return "Vixen and VixenPlus Developers";
            }
        }

        public LoadableDataLocation DataLocationPreference
        {
            get
            {
                return LoadableDataLocation.Sequence;
            }
        }

        public string Description
        {
            get
            {
                return "UI helper for RGBLED sequences";
            }
        }

        public string Name
        {
            get
            {
                return "RGBLED";
            }
        }
    }
}

