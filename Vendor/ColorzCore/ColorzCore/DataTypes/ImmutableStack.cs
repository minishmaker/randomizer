using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ColorzCore.DataTypes
{
    public class ImmutableStack<T> : IEnumerable<T>
    {
        private static ImmutableStack<T> _emptyList = new ImmutableStack<T>();
        int? _count;
        private IMaybe<Tuple<T, ImmutableStack<T>>> _member;

        public ImmutableStack(T elem, ImmutableStack<T> tail)
        {
            _member = new Just<Tuple<T, ImmutableStack<T>>>(new Tuple<T, ImmutableStack<T>>(elem, tail));
        }

        private ImmutableStack()
        {
            _member = new Nothing<Tuple<T, ImmutableStack<T>>>();
            _count = null;
        }

        public static ImmutableStack<T> Nil
        {
            get { return _emptyList; }
        }

        public bool IsEmpty
        {
            get { return _member.IsNothing; }
        }

        public T Head
        {
            get { return _member.FromJust.Item1; }
        }

        public ImmutableStack<T> Tail
        {
            get { return _member.FromJust.Item2; }
        }

        public int Count
        {
            get
            {
                if (_count.HasValue)
                    return _count.Value;
                else
                    return (_count = Tail.Count + 1).Value;
            }
        }

        /*
        public bool Contains(T toLookFor)
        {
            bool acc = false;
            for(ImmutableStack<T> temp = this; !acc && !temp.IsEmpty; temp = temp.Tail) acc |= temp.Head.Equals(toLookFor);
            return acc;
        }
        */
        public IEnumerator<T> GetEnumerator()
        {
            var temp = this;
            while (!temp.IsEmpty)
            {
                yield return temp.Head;
                temp = temp.Tail;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            var temp = this;
            while (!temp.IsEmpty)
            {
                yield return temp.Head;
                temp = temp.Tail;
            }
        }

        public static ImmutableStack<T> FromEnumerable(IEnumerable<T> content)
        {
            return content.Reverse()
                .Aggregate(Nil, (ImmutableStack<T> acc, T elem) => new ImmutableStack<T>(elem, acc));
        }
    }
}
