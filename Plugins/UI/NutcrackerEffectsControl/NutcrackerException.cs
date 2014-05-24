using System;
using System.Runtime.Serialization;

using VixenPlusCommon.Annotations;

namespace Nutcracker {
    public class NutcrackerException : Exception {
        // Constructors
        public NutcrackerException(string message, Exception ex) : base(message, ex) {}

        // Ensure Exception is Serializable
        [UsedImplicitly]
        protected NutcrackerException(SerializationInfo info, StreamingContext ctxt) : base(info, ctxt) {}
    }
}