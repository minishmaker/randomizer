using System.Collections;
using ColorzCore.Parser.AST;

namespace ColorzCore.Raws
{
    internal class AtomicParam : IRawParam
    {
        private readonly bool _pointer;

        public AtomicParam(string name, int position, int length, bool isPointer)
        {
            Name = name;
            Position = position;
            Length = length;
            _pointer = isPointer;
        }

        public string Name { get; }

        public int Position { get; }

        public int Length { get; }

        public void Set(BitArray data, IParamNode input)
        {
            Set(data, ((IAtomNode)input).ToInt());
        }

        public bool Fits(IParamNode input)
        {
            return input.Type == ParamType.ATOM;
        }

        public void Set(BitArray data, int res)
        {
            if (_pointer && res != 0)
                res |= 0x08000000;
            byte[] resBytes = { (byte)res, (byte)(res >> 8), (byte)(res >> 16), (byte)(res >> 24) };
            var bits = new BitArray(resBytes);
            for (var i = Position; i < Position + Length; i++)
                data[i] = bits[i - Position];
        }
    }
}
