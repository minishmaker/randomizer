using System.Collections;
using System.Collections.Generic;

namespace ColorzCore.DataTypes
{
	public class MergeableGenerator<T> : IEnumerable<T>
	{
		private readonly Stack<IEnumerator<T>> myEnums;

		public MergeableGenerator(IEnumerable<T> baseEnum)
		{
			myEnums = new Stack<IEnumerator<T>>();
			myEnums.Push(baseEnum.GetEnumerator());
		}

		public bool EOS { get; private set; }

		public T Current => myEnums.Peek().Current;

		IEnumerator<T> IEnumerable<T>.GetEnumerator()
		{
			while (!EOS)
			{
				yield return Current;
				MoveNext();
			}
		}

		public IEnumerator GetEnumerator()
		{
			while (!EOS)
			{
				yield return Current;
				MoveNext();
			}
		}

		public bool MoveNext()
		{
			if (!myEnums.Peek().MoveNext())
			{
				if (myEnums.Count > 1)
				{
					myEnums.Pop();
					return true;
				}

				EOS = true;
				return false;
			}

			return true;
		}

		public void PrependEnumerator(IEnumerator<T> nextEnum)
		{
			if (EOS)
				myEnums.Pop();
			myEnums.Push(nextEnum);
			Prime();
		}

		public void PutBack(T elem)
		{
			PrependEnumerator(new List<T> { elem }.GetEnumerator());
		}

		private bool Prime()
		{
			if (!myEnums.Peek().MoveNext())
			{
				myEnums.Pop();
				EOS = myEnums.Count == 1;
			}
			else
			{
				EOS = false;
			}

			return !EOS;
		}
	}
}
