namespace RandomizerCore.Utilities.IO;

public class Reader
{
    // ReSharper disable once InconsistentNaming
    private readonly BinaryReader _reader;
    private readonly Stream Stream;

    public Reader(Stream stream)
    {
        Stream = stream;
        _reader = new BinaryReader(stream);
    }

    public long Position => Stream.Position;

    public void SetPosition(long pos)
    {
        Stream.Position = pos;
    }

    public byte PeekByte()
    {
        var tempPos = Stream.Position;
        var b = ReadByte();
        Stream.Position = tempPos;
        return b;
    }

    public byte PeekByte(long pos)
    {
        Stream.Position = pos;
        return PeekByte();
    }

    public byte[] PeekBytes(int num)
    {
        var tempPos = Stream.Position;
        var b = ReadBytes(num);
        Stream.Position = tempPos;
        return b;
    }

    public byte[] PeekBytes(int num, long pos)
    {
        Stream.Position = pos;
        return PeekBytes(num);
    }

    public byte ReadByte()
    {
        return _reader.ReadByte();
    }

    public byte ReadByte(long pos)
    {
        Stream.Position = pos;
        return ReadByte();
    }

    public byte[] ReadBytes(int num)
    {
        return _reader.ReadBytes(num);
    }

    public byte[] ReadBytes(int num, long pos)
    {
        Stream.Position = pos;
        return ReadBytes(num);
    }

    public ushort ReadUInt16()
    {
        return _reader.ReadUInt16();
    }

    public ushort ReadUInt16(long pos)
    {
        Stream.Position = pos;
        return _reader.ReadUInt16();
    }

    public short ReadInt16()
    {
        return _reader.ReadInt16();
    }

    public short ReadInt16(long pos)
    {
        Stream.Position = pos;
        return _reader.ReadInt16();
    }

    public uint ReadUInt32()
    {
        return _reader.ReadUInt32();
    }

    public uint ReadUInt32(long pos)
    {
        Stream.Position = pos;
        return _reader.ReadUInt32();
    }

    public int ReadInt()
    {
        return _reader.ReadInt32();
    }

    public int ReadInt(long pos)
    {
        Stream.Position = pos;
        return _reader.ReadInt32();
    }

    public int ReadAddr()
    {
        return ReadInt() & 0xFFFFFF;
    }

    public int ReadAddr(long pos)
    {
        Stream.Position = pos;
        return ReadAddr();
    }
}
