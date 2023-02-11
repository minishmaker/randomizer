using System.Collections;
using ColorzCore.Parser.AST;

namespace ColorzCore.Raws
{
    internal class ListParam : IRawParam
    {
        private readonly int _numCoords;

        public ListParam(string name, int position, int length, int numCoords)
        {
            Name = name;
            Position = position;
            Length = length;
            this._numCoords = numCoords;
        }

        public string Name { get; }
        public int Position { get; }
        public int Length { get; }

        public void Set(BitArray data, IParamNode input)
        {
            var count = 0;
            var interior = ((ListNode)input).Interior;
            var bitsPerAtom = Length / _numCoords;
            foreach (var a in interior)
            {
                var res = a.ToInt();
                for (var i = 0; i < bitsPerAtom; i++, res >>= 1) data[i + Position + count] = (res & 1) == 1;
                count += bitsPerAtom;
            }

            for (; count < Length; count++)
                data[Position + count] = false;
        }

        public bool Fits(IParamNode input)
        {
            if (input.Type == ParamType.LIST)
            {
                var n = (ListNode)input;
                return n.NumCoords <= _numCoords;
            }

            return false;
        }
    }
}
