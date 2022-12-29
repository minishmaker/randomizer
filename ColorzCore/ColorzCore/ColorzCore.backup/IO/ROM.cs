using System;
using System.IO;

namespace ColorzCore.IO
{
	internal class ROM
	{
		private readonly byte[] myData;
		private readonly BufferedStream myStream;
		private int size;

		public ROM(Stream myROM)
		{
			myStream = new BufferedStream(myROM);
			myData = new byte[0x2000000];
			size = myStream.Read(myData, 0, 0x2000000);
			myStream.Position = 0;
		}

		public void WriteROM()
		{
			myStream.Write(myData, 0, size);
			myStream.Flush();
		}

		public void WriteTo(int position, byte[] data)
		{
			Array.Copy(data, 0, myData, position, data.Length);
			if (data.Length + position > size)
				size = data.Length + position;
		}
	}
}
