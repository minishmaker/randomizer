using System;
using System.Diagnostics;
using MinishRandomizer.Utilities;
using System.IO;

namespace MinishRandomizer.Core
{
    public class ROM
    {
        public static ROM Instance { get; private set; }
        public readonly string path;

        public readonly byte[] romData;
        public readonly Reader reader;

        public RegionVersion version { get; private set;  } = RegionVersion.None;
        public HeaderData headers { get; private set; }


        public ROM(string filePath)
        {
            Instance = this;
            path = filePath;
            byte[] smallData = File.ReadAllBytes(filePath);
            if (smallData.Length >= 0x02000000)
            {
                romData = smallData;
            }
            else
            {
                romData = new byte[0x2000000];
                smallData.CopyTo(romData, 0);
            }

            Stream stream = Stream.Synchronized(new MemoryStream(romData));
            reader = new Reader(stream);
            Debug.WriteLine("Read " + stream.Length + " bytes.");

            SetupRom();
        }

        private void SetupRom()
        {
            // Determine game region and if valid ROM
            byte[] regionBytes = reader.ReadBytes(4, 0xAC);
            string region = System.Text.Encoding.UTF8.GetString(regionBytes);
            Debug.WriteLine("Region detected: "+region);

            if (region == "BZMP")
            {
                version = RegionVersion.EU;
            }

            if (region == "BZMJ")
            {
                version = RegionVersion.JP;
            }

            if (region == "BZME")
            {
                version = RegionVersion.US;
            }

            if (version != RegionVersion.None)
            {
                headers = new Header().GetHeaderAddresses(version);
            }
        }
    }
}
