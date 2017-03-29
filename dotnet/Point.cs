using System;
using System.Collections.Generic;
using System.Text;

namespace CoreCV
{
    public struct Point<T> where T : struct, IConvertible, IComparable<T>, IComparable, IEquatable<T>, IFormattable
	{
		public T x, y;
		public Point(T x, T y)
		{
			this.x = x;
			this.y = y;
		}

		public T dot(Point<T> other)
		{
			return GenericArithmetic<T>.add(
				GenericArithmetic<T>.multiply(x, other.x),
				GenericArithmetic<T>.multiply(y, other.y)
				);
		}

		public T cross(Point<T> other)
		{
			return GenericArithmetic<T>.substract(
				GenericArithmetic<T>.multiply(x, other.y),
				GenericArithmetic<T>.multiply(y, other.x)
				);
		}

		public bool inside(Rect<T> r)
		{
			return r.contains(this);
		}

		public static Point<T> operator +(Point<T> a, Point<T> b)
		{
			return new Point<T>(
					GenericArithmetic<T>.add(a.x, b.x),
					GenericArithmetic<T>.add(a.y, b.y)
				);
		}

		public static Point<T> operator -(Point<T> a, Point<T> b)
		{
			return new Point<T>(
					GenericArithmetic<T>.substract(a.x, b.x),
					GenericArithmetic<T>.substract(a.y, b.y)
				);
		}

		public static Point<T> operator *(Point<T> a, T s)
		{
			return new Point<T>(
					GenericArithmetic<T>.multiply(a.x, s),
					GenericArithmetic<T>.multiply(a.y, s)
				);
		}

		public static Point<T> operator /(Point<T> a, T s)
		{
			return new Point<T>(
					GenericArithmetic<T>.divide(a.x, s),
					GenericArithmetic<T>.divide(a.y, s)
				);
		}

		public static bool operator ==(Point<T> a, Point<T> b)
		{
			return GenericArithmetic<T>.eq(a.x, b.x)
				&& GenericArithmetic<T>.eq(a.y, b.y);
		}

		public static bool operator !=(Point<T> a, Point<T> b)
		{
			return !(a == b);
		}

		public override bool Equals(object b)
		{
			return this == (Point<T>)b;
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		public override string ToString()
		{
			return new StringBuilder("(").Append(x.ToString())
				.Append(", ")
				.Append(y.ToString()).Append(")").ToString();
		}
	}
}
