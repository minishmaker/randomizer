namespace RandomizerCore.Utilities.IO;

public class Writer
{
    private readonly Stream Stream;

    // ReSharper disable once InconsistentNaming
    private readonly BinaryWriter _writer;

    public Writer(Stream stream)
    {
        Stream = stream;
        _writer = new BinaryWriter(stream);
    }

    public long Position => Stream.Position;

    ~Writer()
    {
        Flush();
    }

    public void Flush()
    {
        _writer.Flush();
    }

    public void SetPosition(long pos)
    {
        Stream.Position = pos;
    }

    public void WriteByte(byte byteToWrite)
    {
        _writer.Write(byteToWrite);
    }

    public void WriteByte(byte byteToWrite, long pos)
    {
        Stream.Position = pos;
        WriteByte(byteToWrite);
    }

    public void WriteBytes(byte[] bytesToWrite)
    {
        _writer.Write(bytesToWrite);
    }

    public void WriteBytes(byte[] bytesToWrite, long pos)
    {
        Stream.Position = pos;
        WriteBytes(bytesToWrite);
    }

    public void WriteUInt16(ushort uint16)
    {
        _writer.Write(uint16);
    }

    public void WriteUInt16(ushort uint16, long pos)
    {
        Stream.Position = pos;
        WriteUInt16(uint16);
    }

    public void WriteInt16(short int16)
    {
        _writer.Write(int16);
    }

    public void WriteInt16(short int16, long pos)
    {
        Stream.Position = pos;
        WriteInt16(int16);
    }

    public void WriteUInt32(uint uint32)
    {
        _writer.Write(uint32);
    }

    public void WriteUInt32(uint uint32, long pos)
    {
        Stream.Position = pos;
        WriteUInt32(uint32);
    }

    public void WriteInt(int int32)
    {
        _writer.Write(int32);
    }

    public void WriteInt(int int32, long pos)
    {
        Stream.Position = pos;
        WriteInt(int32);
    }

    public void WriteAddr(int addr)
    {
        _writer.Write(addr | 0x08000000);
    }

    public void WriteAddr(int addr, long pos)
    {
        Stream.Position = pos;
        WriteAddr(addr);
    }
}
