using System;
using System.IO;

using VixenPlus;

using VixenPlusCommon.Annotations;

// ReSharper disable once CheckNamespace
namespace SeqIOHelpers {
    [UsedImplicitly]
    public class FPPfseq : IFileIOHandler {

        public string DialogFilterList() {
            return string.Format("Falcon Player Sequence (*{0})|*{0}", FileExtension());
        }

        public string Name() {
            return "Falcon Player Sequence";
        }

        public string FileExtension() {
            return ".fseq";
        }

        public int PreferredOrder() {
            return 3;
        }

        public bool IsNativeToVixenPlus() {
            return false;
        }

        public bool CanSave() {
            return true;
        }


        public void SaveSequence(EventSequence eventSequence) {
            const byte colorEncoding = 2, gamma = 1, versionMajor = 1, versionMinor = 0;
            const string fileType = "PSEQ";
            const ushort fixedHeaderLength = 28, mediaHeaderSize = 5, padding = 0, universeCount = 0, universeSize = 0;

            var mediaFileName = eventSequence.Audio.FileName;
            var mediaHeaderTotalLength = mediaFileName.Length == 0 ? 0 : mediaFileName.Length + mediaHeaderSize;

            var offsetToSequenceData = RoundUshortTo4((ushort) (fixedHeaderLength + mediaHeaderTotalLength));
            var numberOfChannels = RoundUIntTo4((uint) eventSequence.FullChannelCount);
            var numberOfEvents = (uint)eventSequence.TotalEventPeriods;
            var frameSizeMs = (ushort)eventSequence.EventPeriod;

            using (var fileStream = new FileStream(eventSequence.FileName, FileMode.Create)) {
                using (var binaryWriter = new BinaryWriter(fileStream)) {
                    binaryWriter.Write(fileType.ToCharArray()); // 0:3
                    binaryWriter.Write(offsetToSequenceData); //  4:5
                    binaryWriter.Write(versionMinor); // 6
                    binaryWriter.Write(versionMajor); //  7
                    binaryWriter.Write(fixedHeaderLength); //  8:9
                    binaryWriter.Write(numberOfChannels); // (step size) 10:13
                    binaryWriter.Write(numberOfEvents); // (steps/frames) 14:17
                    binaryWriter.Write(frameSizeMs); // 18:19
                    binaryWriter.Write(universeCount); // 20:21
                    binaryWriter.Write(universeSize); //  22:23
                    binaryWriter.Write(gamma); // 24
                    binaryWriter.Write(colorEncoding); // 25 
                    binaryWriter.Write(padding); // 26:27
                    if (mediaHeaderTotalLength > 0) {
                        binaryWriter.Write((ushort) (mediaHeaderTotalLength)); // 28:29
                        binaryWriter.Write("mf".ToCharArray()); // 30:31
                        binaryWriter.Write(mediaFileName.ToCharArray()); // 32:32+mediaFileName.Length 
                    }
                    
                    // (pad to nearest 4)
                    var padSize = offsetToSequenceData - (fixedHeaderLength + mediaHeaderTotalLength);

                    for (var pad = 0; pad < padSize; pad++) {
                        binaryWriter.Write((byte) 0);
                    }

                    // Write the event data
                    for (var period = 0; period < numberOfEvents; period++) {
                        for (var channel = 0; channel < numberOfChannels; channel++) {
                            binaryWriter.Write(eventSequence.EventValues[channel, period]);
                        }
                    }

                    // Done and Done :)
                    binaryWriter.Close();
                }

            }
        }


        private static uint RoundUIntTo4(uint i) {
            return (i % 4 == 0) ? i : i + 4 - (i % 4);
        }


        private static ushort RoundUshortTo4(ushort i) {
            return (ushort) (i % 4 == 0 ? i : i + 4 - (i % 4));
        }


        public void SaveProfile(Profile profile) {
            throw new NotSupportedException("Format does not support profiles.");
        }

        public bool CanOpen() {
            return false;
        }

        public EventSequence OpenSequence(string fileName) {
            throw new NotSupportedException("Can not open fseq files as profile information is lost upon conversion.");
        }

        public Profile OpenProfile(string fileName) {
            throw new NotSupportedException("Format does not support profiles.");
        }

        public void LoadEmbeddedData(string fileName, EventSequence es) {
            throw new NotSupportedException("Format does not support embedded data.");
        }

        public bool SupportsProfiles {
            get { return false; }
        }
    }
}