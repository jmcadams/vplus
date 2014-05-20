using System;
using System.Runtime.Serialization;

namespace Nutcracker {
    internal class NutcrackerException : Exception {
        // Constructors
        public NutcrackerException(string message) : base(message) {}
        public NutcrackerException(string message, Exception ex) : base(message, ex) {}

        // Ensure Exception is Serializable
        protected NutcrackerException(SerializationInfo info, StreamingContext ctxt) : base(info, ctxt) {}
    }
}