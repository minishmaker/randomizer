using System;
using System.Collections.Generic;
using ColorzCore.DataTypes;

namespace ColorzCore.Raws
{
    internal class Flag
    {
        public Flag()
        {
            Values = new Left<IList<string>, Tuple<int, int>>(new List<string>());
        }

        public Flag(IList<string> values)
        {
            Values = new Left<IList<string>, Tuple<int, int>>(values);
        }

        public Flag(int valBeg, int valEnd)
        {
            Values = new Right<IList<string>, Tuple<int, int>>(new Tuple<int, int>(valBeg, valEnd));
        }

        public IEither<IList<string>, Tuple<int, int>> Values { get; }
    }
}
