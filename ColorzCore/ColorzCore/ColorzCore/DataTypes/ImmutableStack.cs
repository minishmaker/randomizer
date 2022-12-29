using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ColorzCore.DataTypes
{
	public class ImmutableStack<T> : IEnumerable<T>
	{
		private static ImmutableStack<T> emptyList = new ImmutableStack<T>();
		int? count;
		private Maybe<Tuple<T, ImmutableStack<T>>> member;

		public ImmutableStack(T elem, ImmutableStack<T> tail)
		{
			member = new Just<Tuple<T, ImmutableStack<T>>>(new Tuple<T, ImmutableStack<T>>(elem, tail));
		}

		private ImmutableStack()
		{
			member = new Nothing<Tuple<T, ImmutableStack<T>>>();
			count = null;
		}

		public static ImmutableStack<T> Nil
		{
			get { return emptyList; }
		}

		public bool IsEmpty
		{
			get { return member.IsNothing; }
		}

		public T Head
		{
			get { return member.FromJust.Item1; }
		}

		public ImmutableStack<T> Tail
		{
			get { return member.FromJust.Item2; }
		}

		public int Count
		{
			get
			{
				if (count.HasValue)
					return count.Value;
				else
					return (count = Tail.Count + 1).Value;
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
			ImmutableStack<T> temp = this;
			while (!temp.IsEmpty)
			{
				yield return temp.Head;
				temp = temp.Tail;
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			ImmutableStack<T> temp = this;
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
