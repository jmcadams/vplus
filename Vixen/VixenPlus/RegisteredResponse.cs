namespace Vixen
{
    using System;

    internal class RegisteredResponse
    {
        public int EcHandle;
        public string InterfaceTypeName;
        public int Line;

        public RegisteredResponse(string interfaceTypeName, int lineIndex, int ecHandle)
        {
            this.InterfaceTypeName = interfaceTypeName;
            this.Line = lineIndex;
            this.EcHandle = ecHandle;
        }
    }
}

