namespace RandomizerCore.Random;

/// <summary>
/// A random number generator that uses the 64-bit squares RNG algorithm found here: https://squaresrng.wixsite.com/rand
/// </summary>
public class SquaresRandomNumberGenerator
{
    internal const ulong DefaultKey = 0xb639dc821e6b5a27UL;
    private readonly ulong _key;
    private ulong _previousValue;

    public SquaresRandomNumberGenerator() : this(DefaultKey)
    { }

    public SquaresRandomNumberGenerator(ulong key)
    {
        _key = key;
        //File time is accurate to 100ns
        _previousValue = (ulong)DateTime.Now.ToFileTime() * 100;
    }

    public SquaresRandomNumberGenerator(ulong key, ulong defaultValue)
    {
        _key = key;
        _previousValue = defaultValue;
    }

    public ulong Next()
    {
        var x = _previousValue * _key;
        var z = x + _key;

        _previousValue = ComputeRounds(x, z);
        return _previousValue;
    }

    public int Next(int upperBound)
    {
        
        var x = _previousValue * _key;
        var z = x + _key;

        _previousValue = ComputeRounds(x, z);
        return (int)((_previousValue % (uint)upperBound) & int.MaxValue);
    }

    public ulong Next(ulong counter, ulong upperBound)
    {
        var x = counter * _key;
        var z = x + _key;

        _previousValue = ComputeRounds(x, z);
        return _previousValue % upperBound;
    }

    private static ulong ComputeRounds(ulong x, ulong z)
    {
        var y = x;
        
        //Round 1
        x = x * x + y;
        x = (x >> 32) | (x << 32);
        
        //Round 2
        x = x * x + z;
        x = (x >> 32) | (x << 32);
        
        //Round 3
        x = x * x + y;
        x = (x >> 32) | (x << 32);
        
        //Round 4
        var t = x = x * x + z;
        x = (x >> 32) | (x << 32);
        
        //Round 5
        return t ^ ((x * x + y) >> 32);
    }
}
