using System;
using System.Collections;
using System.Collections.Generic;

namespace ColorzCore.DataTypes
{
	internal static class Extension
	{
		public static IEnumerable<T> PadTo<T>(this IEnumerable<T> self, int totalLength, T zero)
		{
			var count = 0;
			foreach (var t in self)
			{
				yield return t;
				count++;
			}

			for (; count < totalLength; count++)
				yield return zero;
		}

		public static BitArray Append(this BitArray x, BitArray y)
		{
			var startLen = x.Length;
			x.Length += y.Length;
			for (var i = startLen; i < x.Length; i++) x[i] = y[i - startLen];
			return x;
		}

		public static int ToInt(this string numString)
		{
			if (numString.StartsWith("$"))
				return Convert.ToInt32(numString.Substring(1), 16);
			if (numString.StartsWith("0x"))
				return Convert.ToInt32(numString.Substring(2), 16);
			if (numString.EndsWith("b"))
				return Convert.ToInt32(numString.Substring(0, numString.Length - 1), 2);
			return Convert.ToInt32(numString);
		}

		public static Dictionary<K, IList<V>> AddTo<K, V>(this Dictionary<K, IList<V>> self, K key, V val)
		{
			if (self.ContainsKey(key))
				self[key].Add(val);
			else
				self[key] = new List<V> { val };
			return self;
		}
	}
}
