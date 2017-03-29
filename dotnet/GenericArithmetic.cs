using System;
using System.Collections.Generic;
using System.Text;

using System.Linq.Expressions;

namespace CoreCV
{
	// borrow idea from http://www.yoda.arachsys.com/csharp/genericoperators.html
	internal class GenericArithmetic<T> 
		where T: struct, IConvertible, IComparable<T>, IComparable, IEquatable<T>, IFormattable
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

		public static readonly Func<T, T, T> add       = initAdder();
		public static readonly Func<T, T, T> substract = initSubstractor();
		public static readonly Func<T, T, T> multiply  = initMultiplier();
		public static readonly Func<T, T, T> divide    = intiDivider();
		public static readonly Func<T, T, bool> gt     = initGreaterThan();
		public static readonly Func<T, T, bool> lt     = initLessThan();
		public static readonly Func<T, T, bool> eq     = initEqual();

		public static readonly T zero = initZero();

		private static T initZero()
		{
			return substract(default(T), default(T));
		}

		private static Func<T, T, T> initAdder()
		{
			checkType();
			ParameterExpression paramA = Expression.Parameter(typeof(T));
			ParameterExpression paramB = Expression.Parameter(typeof(T));
			BinaryExpression body = Expression.Add(paramA, paramB);
			return Expression.Lambda<Func<T, T, T>>(body, paramA, paramB).Compile();
		}
		private static Func<T, T, T> initSubstractor()
		{
			ParameterExpression paramA = Expression.Parameter(typeof(T));
			ParameterExpression paramB = Expression.Parameter(typeof(T));
			BinaryExpression body = Expression.Subtract(paramA, paramB);
			return Expression.Lambda<Func<T, T, T>>(body, paramA, paramB).Compile();
		}
		private static Func<T, T, T> initMultiplier()
		{
			ParameterExpression paramA = Expression.Parameter(typeof(T));
			ParameterExpression paramB = Expression.Parameter(typeof(T));
			BinaryExpression body = Expression.Multiply(paramA, paramB);
			return Expression.Lambda<Func<T, T, T>>(body, paramA, paramB).Compile();
		}
		private static Func<T, T, T> intiDivider()
		{
			ParameterExpression paramA = Expression.Parameter(typeof(T));
			ParameterExpression paramB = Expression.Parameter(typeof(T));
			BinaryExpression body = Expression.Divide(paramA, paramB);
			return Expression.Lambda<Func<T, T, T>>(body, paramA, paramB).Compile();
		}

		private static Func<T, T, bool> initGreaterThan()
		{
			ParameterExpression paramA = Expression.Parameter(typeof(T));
			ParameterExpression paramB = Expression.Parameter(typeof(T));
			BinaryExpression body = Expression.GreaterThan(paramA, paramB);
			return Expression.Lambda<Func<T, T, bool>>(body, paramA, paramB).Compile();
		}

		private static Func<T, T, bool> initLessThan()
		{
			ParameterExpression paramA = Expression.Parameter(typeof(T));
			ParameterExpression paramB = Expression.Parameter(typeof(T));
			BinaryExpression body = Expression.LessThan(paramA, paramB);
			return Expression.Lambda<Func<T, T, bool>>(body, paramA, paramB).Compile();
		}

		private static Func<T, T, bool> initEqual()
		{
			ParameterExpression paramA = Expression.Parameter(typeof(T));
			ParameterExpression paramB = Expression.Parameter(typeof(T));
			BinaryExpression body = Expression.Equal(paramA, paramB);
			return Expression.Lambda<Func<T, T, bool>>(body, paramA, paramB).Compile();
		}

		private static void checkType()
		{
			var t = typeof(T);
			foreach (var sp in supportTypes)
				if (t == sp)
					return;
			throw new NotSupportedException("Arithmetic is not support for " + t.FullName);
			
		}
	}
}
