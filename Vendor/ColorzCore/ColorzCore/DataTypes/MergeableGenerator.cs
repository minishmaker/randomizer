using System.Collections;
using System.Collections.Generic;

namespace ColorzCore.DataTypes
{
    public class MergeableGenerator<T> : IEnumerable<T>
    {
        private readonly Stack<IEnumerator<T>> _myEnums;

        public MergeableGenerator(IEnumerable<T> baseEnum)
        {
            _myEnums = new Stack<IEnumerator<T>>();
            _myEnums.Push(baseEnum.GetEnumerator());
        }

        public bool Eos { get; private set; }

        public T Current => _myEnums.Peek().Current;

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            while (!Eos)
            {
                yield return Current;
                MoveNext();
            }
        }

        public IEnumerator GetEnumerator()
        {
            while (!Eos)
            {
                yield return Current;
                MoveNext();
            }
        }

        public bool MoveNext()
        {
            if (!_myEnums.Peek().MoveNext())
            {
                if (_myEnums.Count > 1)
                {
                    _myEnums.Pop();
                    return true;
                }

                Eos = true;
                return false;
            }

            return true;
        }

        public void PrependEnumerator(IEnumerator<T> nextEnum)
        {
            if (Eos)
                _myEnums.Pop();
            _myEnums.Push(nextEnum);
            Prime();
        }

        public void PutBack(T elem)
        {
            PrependEnumerator(new List<T> { elem }.GetEnumerator());
        }

        private bool Prime()
        {
            if (!_myEnums.Peek().MoveNext())
            {
                _myEnums.Pop();
                Eos = _myEnums.Count == 1;
            }
            else
            {
                Eos = false;
            }

            return !Eos;
        }
    }
}
