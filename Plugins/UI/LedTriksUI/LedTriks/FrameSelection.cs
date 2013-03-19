namespace LedTriks
{
	using System;
	using System.Collections.Generic;
	using LedTriksUtil;

	internal class FrameSelection
	{
		public int Count;
		public List<Frame> Source;
		public int StartIndex;

		public FrameSelection(List<Frame> source, int startIndex, int count)
		{
			this.Source = source;
			this.StartIndex = startIndex;
			this.Count = count;
		}
	}
}

