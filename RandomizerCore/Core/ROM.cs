using System.Diagnostics;
using System.Text;
using RandomizerCore.Utilities.IO;

namespace RandomizerCore.Core;

public class Rom
{
    public readonly string path;
    public readonly Reader reader;

    public readonly byte[] romData;


    public Rom(string filePath)
    {
        Instance = this;
        path = filePath;
        var smallData = File.ReadAllBytes(filePath);
        if (smallData.Length >= 0x01000000)
        {
            romData = smallData;
        }
        else
        {
            romData = new byte[0x1000000];
            smallData.CopyTo(romData, 0);
        }

        var stream = Stream.Synchronized(new MemoryStream(romData));
        reader = new Reader(stream);
        Debug.WriteLine("Read " + stream.Length + " bytes.");

        SetupRom();
    }

    public static Rom Instance { get; private set; }

    public RegionVersion Version { get; private set; } = RegionVersion.None;
    public HeaderData Headers { get; private set; }

    private void SetupRom()
    {
        // Determine game region and if valid ROM
        var regionBytes = reader.ReadBytes(4, 0xAC);
        var region = Encoding.UTF8.GetString(regionBytes);
        Debug.WriteLine("Region detected: " + region);

        if (region == "BZMP") Version = RegionVersion.EU;

        if (region == "BZMJ") Version = RegionVersion.JP;

        if (region == "BZME") Version = RegionVersion.US;

        if (Version != RegionVersion.None) Headers = new Header().GetHeaderAddresses(Version);
    }
}