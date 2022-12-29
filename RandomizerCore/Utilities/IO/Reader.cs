namespace RandomizerCore.Utilities.IO;

public class Reader
{
    private readonly BinaryReader _reader;
    private readonly Stream _stream;

    public Reader(Stream stream)
    {
        _stream = stream;
        _reader = new BinaryReader(stream);
    }

    public long Position => _stream.Position;

    public void SetPosition(long pos)
    {
        _stream.Position = pos;
    }

    public byte PeekByte()
    {
        var tempPos = _stream.Position;
        var b = ReadByte();
        _stream.Position = tempPos;
        return b;
    }

    public byte PeekByte(long pos)
    {
        _stream.Position = pos;
        return PeekByte();
    }

    public byte[] PeekBytes(int num)
    {
        var tempPos = _stream.Position;
        var b = ReadBytes(num);
        _stream.Position = tempPos;
        return b;
    }

    public byte[] PeekBytes(int num, long pos)
    {
        _stream.Position = pos;
        return PeekBytes(num);
    }

    public byte ReadByte()
    {
        return _reader.ReadByte();
    }

    public byte ReadByte(long pos)
    {
        _stream.Position = pos;
        return ReadByte();
    }

    public byte[] ReadBytes(int num)
    {
        return _reader.ReadBytes(num);
    }

    public byte[] ReadBytes(int num, long pos)
    {
        _stream.Position = pos;
        return ReadBytes(num);
    }

    public ushort ReadUInt16()
    {
        return _reader.ReadUInt16();
    }

    public ushort ReadUInt16(long pos)
    {
        _stream.Position = pos;
        return _reader.ReadUInt16();
    }

    public short ReadInt16()
    {
        return _reader.ReadInt16();
    }

    public short ReadInt16(long pos)
    {
        _stream.Position = pos;
        return _reader.ReadInt16();
    }

    public uint ReadUInt32()
    {
        return _reader.ReadUInt32();
    }

    public uint ReadUInt32(long pos)
    {
        _stream.Position = pos;
        return _reader.ReadUInt32();
    }

    public int ReadInt()
    {
        return _reader.ReadInt32();
    }

    public int ReadInt(long pos)
    {
        _stream.Position = pos;
        return _reader.ReadInt32();
    }

    public int ReadAddr()
    {
        return ReadInt() & 0xFFFFFF;
    }

    public int ReadAddr(long pos)
    {
        _stream.Position = pos;
        return ReadAddr();
    }
}