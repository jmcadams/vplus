namespace Prop2SeqGen
{
    using System;

    internal class AudioSelection
    {
        public int MaxRecords;
        public string Text;

        public AudioSelection(string[] values)
        {
            if (values.Length < 2)
            {
                throw new Exception("Malformed audio selection.\nInsufficient number of values.");
            }
            if (!int.TryParse(values[0], out this.MaxRecords))
            {
                throw new Exception("Malformed audio selection.\nInvalid numeric value.");
            }
            this.Text = values[1];
        }

        public override string ToString()
        {
            return this.Text;
        }
    }
}

