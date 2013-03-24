namespace VixenPlus
{
	internal class ExecutionClientStub
	{
		private readonly System.Net.IPAddress _ipAddress;
		private readonly string _name;

		public ExecutionClientStub(string name, System.Net.IPAddress ipAddress)
		{
			_name = name;
			_ipAddress = ipAddress;
		}

		public System.Net.IPAddress IPAddress
		{
			get
			{
				return _ipAddress;
			}
		}

		public string Name
		{
			get
			{
				return _name;
			}
		}

		public int Ping { get; set; }
	}
}

