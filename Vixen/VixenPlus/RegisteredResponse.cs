namespace Vixen
{
	internal class RegisteredResponse
	{
		public int EcHandle;
		public string InterfaceTypeName;
		public int Line;

		public RegisteredResponse(string interfaceTypeName, int lineIndex, int ecHandle)
		{
			InterfaceTypeName = interfaceTypeName;
			Line = lineIndex;
			EcHandle = ecHandle;
		}
	}
}