using System.Collections.Generic;
using ColorzCore.DataTypes;
using ColorzCore.Lexer;

namespace ColorzCore.Parser
{
    internal class Pool
    {
        public static readonly string PooledLabelPrefix = "__POOLED$";

        private long _poolLabelCounter;

        public Pool()
        {
            Lines = new List<PooledLine>();
            _poolLabelCounter = 0;
        }

        public List<PooledLine> Lines { get; }

        public string MakePoolLabelName()
        {
            // The presence of $ in the label name guarantees that it can't be a user label
            return string.Format("{0}{1}", PooledLabelPrefix, _poolLabelCounter++);
        }

        public struct PooledLine
        {
            public ImmutableStack<Closure> Scope { get; }
            public List<Token> Tokens { get; }

            public PooledLine(ImmutableStack<Closure> scope, List<Token> tokens)
            {
                Scope = scope;
                Tokens = tokens;
            }
        }
    }
}
