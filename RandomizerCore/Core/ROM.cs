using System.Diagnostics;
using System.Text;
using RandomizerCore.Utilities.IO;
using RandomizerCore.Utilities.Logging;

namespace RandomizerCore.Core;

public class Rom
{
    public readonly string Path;
    public readonly Reader Reader;

    public readonly byte[] RomData;

    public Rom(string filePath)
    {
        Path = filePath;
        var smallData = File.ReadAllBytes(filePath);
        if (smallData.Length >= 0x01000000)
        {
            RomData = smallData;
        }
        else
        {
            RomData = new byte[0x1000000];
            smallData.CopyTo(RomData, 0);
        }

        var stream = Stream.Synchronized(new MemoryStream(RomData));
        Reader = new Reader(stream);
        Debug.WriteLine("Read " + stream.Length + " bytes.");

        SetupRom();
    }

    public static Rom? Instance { get; private set; }

    public RegionVersion Version { get; private set; } = RegionVersion.None;
    public HeaderData Headers { get; private set; }

    public static void Initialize(string filePath)
    {
        Logger.Instance.LogInfo("Loading ROM");
        Instance = new Rom(filePath);
        Logger.Instance.LogInfo("ROM Loaded Successfully");
    }

    private void SetupRom()
    {
        // Determine game region and if valid ROM
        var regionBytes = Reader.ReadBytes(4, 0xAC);
        var region = Encoding.UTF8.GetString(regionBytes);
        Debug.WriteLine("Region detected: " + region);

        if (region == "BZMP") Version = RegionVersion.Eu;

        if (region == "BZMJ") Version = RegionVersion.Jp;

        if (region == "BZME") Version = RegionVersion.Us;

        if (Version != RegionVersion.None) Headers = new Header().GetHeaderAddresses(Version);
    }
}
