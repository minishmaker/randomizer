using System;
using System.IO;

namespace ColorzCore.IO
{
    internal class Rom
    {
        private readonly byte[] _myData;
        private readonly BufferedStream _myStream;
        private int _size;

        public Rom(Stream myRom)
        {
            _myStream = new BufferedStream(myRom);
            _myData = new byte[0x2000000];
            _size = _myStream.Read(_myData, 0, 0x2000000);
            _myStream.Position = 0;
        }

        public void WriteRom()
        {
            _myStream.Write(_myData, 0, _size);
            _myStream.Flush();
        }

        public void WriteTo(int position, byte[] data)
        {
            Array.Copy(data, 0, _myData, position, data.Length);
            if (data.Length + position > _size)
                _size = data.Length + position;
        }
    }
}
