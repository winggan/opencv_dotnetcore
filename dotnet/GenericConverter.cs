using System;
using System.Collections.Generic;
using System.Text;

using System.Linq.Expressions;

namespace CoreCV
{
    internal class GenericConverter<from, to> 
		where from : struct, IConvertible, IComparable<from>, IComparable, IEquatable<from>, IFormattable
		where to : struct, IConvertible, IComparable<to>, IComparable, IEquatable<to>, IFormattable
	{
		public static readonly Type[] supportTypes = {
			typeof(sbyte),
			typeof(byte),
			typeof(char),
			typeof(Int16),
			typeof(UInt16),
			typeof(Int32),
			typeof(UInt32),
			typeof(Int64),
			typeof(UInt64),
			typeof(float),
			typeof(double),
			typeof(decimal),
			typeof(System.Numerics.BigInteger),
		};

		public static readonly Func<from, to> convert = initConverter();

		private static Func<from, to> initConverter()
		{
			checkType(typeof(from));
			checkType(typeof(to));
			ParameterExpression src = Expression.Parameter(typeof(from));
			UnaryExpression body = Expression.Convert(src, typeof(to));
			var ret = Expression.Lambda<Func<from, to>>(body, src).Compile();
			if (ret == null)
				throw new NullReferenceException("fail to get converter from " + typeof(from).FullName + " to " + typeof(to).FullName);
			return ret;
		}

		private static void checkType(Type t)
		{
			foreach (var sp in supportTypes)
				if (t == sp)
					return;
			throw new NotSupportedException("Convert is not support for " + t.FullName);
		}
	}
}
