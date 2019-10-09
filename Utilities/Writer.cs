using System.IO;

namespace MinishRandomizer.Utilities {
  public class Writer {
    private readonly Stream stream_;

    private readonly BinaryWriter writer_;

    public Writer( Stream stream ) {
      stream_ = stream;
      writer_ = new BinaryWriter( stream );
    }

    ~Writer() {
      Flush();
    }

    public void Flush() {
      writer_.Flush();
    }

    public long Position => stream_.Position;

    public void SetPosition( long pos ) {
      stream_.Position = pos;
    }

    public void WriteByte( byte byteToWrite ) {
      writer_.Write( byteToWrite );
    }

    public void WriteByte( byte byteToWrite, long pos ) {
      stream_.Position = pos;
      WriteByte( byteToWrite );
    }

    public void WriteBytes( byte[] bytesToWrite ) {
      writer_.Write( bytesToWrite );
    }

    public void WriteBytes( byte[] bytesToWrite, long pos ) {
      stream_.Position = pos;
      WriteBytes( bytesToWrite );
    }

    public void WriteUInt16( ushort uint16 ) {
      writer_.Write( uint16 );
    }

    public void WriteUInt16( ushort uint16, long pos ) {
      stream_.Position = pos;
      WriteUInt16( uint16 );
    }

    public void WriteInt16( short int16 ) {
      writer_.Write( int16 );
    }

    public void WriteInt16( short int16, long pos ) {
      stream_.Position = pos;
      WriteInt16( int16 );
    }

    public void WriteUInt32( uint uint32 ) {
      writer_.Write( uint32 );
    }

    public void WriteUInt32( uint uint32, long pos ) {
      stream_.Position = pos;
      WriteUInt32( uint32 );
    }

    public void WriteInt( int int32 ) {
      writer_.Write( int32 );
    }

    public void WriteInt( int int32, long pos ) {
      stream_.Position = pos;
      WriteInt( int32 );
    }

    public void WriteAddr( int addr ) {
      writer_.Write( addr | 0x08000000 );
    }

    public void WriteAddr( int addr, long pos ) {
      stream_.Position = pos;
      WriteAddr( addr );
    }
  }
}
