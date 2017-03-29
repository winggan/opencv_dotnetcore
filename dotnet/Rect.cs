using System;
using System.Collections.Generic;
using System.Text;

namespace CoreCV
{
	public struct Rect<T> where T : struct, IConvertible, IComparable<T>, IComparable, IEquatable<T>, IFormattable
	{
		public T x, y, width, height;

		public Rect(T x, T y, T width, T height)
		{
			this.x = x;
			this.y = y;
			this.width = width;
			this.height = height;
		}

		public Rect(Point<T> pt1, Point<T> pt2)
		{
			bool xCondition = GenericArithmetic<T>.gt(pt1.x, pt2.x);
			bool yCondition = GenericArithmetic<T>.gt(pt1.x, pt2.x);
			if (xCondition) // pt1.x > pt2.x
			{
				x = pt2.x;
				width = GenericArithmetic<T>.substract(pt1.x, pt2.x);
			}
			else
			{
				x = pt1.x;
				width = GenericArithmetic<T>.substract(pt2.x, pt1.x);
			}

			if (yCondition) // pt1.y > pt2.y
			{
				y = pt2.y;
				height = GenericArithmetic<T>.substract(pt1.y, pt2.y);
			}
			else
			{
				y = pt1.y;
				height = GenericArithmetic<T>.substract(pt2.y, pt1.y);
			}
		}

		public Rect(Point<T> origin, T width, T height)
		{
			this.x = origin.x;
			this.y = origin.y;
			this.width = width;
			this.height = height;
		}

		public Point<T> topLeft => new Point<T>(x, y);
		public Point<T> bottomRight => new Point<T>(
				GenericArithmetic<T>.add(x, width),
				GenericArithmetic<T>.add(y, height)
			);

		public T area => GenericArithmetic<T>.multiply(width, height);
		public bool contains(Point<T> pt)
		{
			var br = bottomRight;
			return !GenericArithmetic<T>.lt(pt.x, x)
				&& !GenericArithmetic<T>.lt(pt.y, y)
				&& !GenericArithmetic<T>.gt(pt.x, br.x)
				&& !GenericArithmetic<T>.gt(pt.y, br.y);
		}

		public bool contains(Rect<T> rect)
		{
			return contains(rect.topLeft) && contains(rect.bottomRight);
		}

		public static bool operator ==(Rect<T> a, Rect<T> b)
		{
			return GenericArithmetic<T>.eq(a.x, b.x)
				&& GenericArithmetic<T>.eq(a.y, b.y)
				&& GenericArithmetic<T>.eq(a.width, b.width)
				&& GenericArithmetic<T>.eq(a.height, b.height);
		}

		public static bool operator !=(Rect<T> a, Rect<T> b)
		{
			return !(a == b);
		}

		public override bool Equals(object b)
		{
			return this == (Rect<T>)b;
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		public static Rect<T> operator &(Rect<T> a, Rect<T> b)
		{
			var abr = a.bottomRight;
			var bbr = b.bottomRight;
			T x = GenericArithmetic<T>.gt(a.x, b.x) ? a.x : b.x;
			T y = GenericArithmetic<T>.gt(a.y, b.y) ? a.y : b.y;
			T w = GenericArithmetic<T>.substract( 
				GenericArithmetic<T>.lt(abr.x, bbr.x) ? abr.x : bbr.x,
				x);
			T h = GenericArithmetic<T>.substract(
				GenericArithmetic<T>.lt(abr.y, bbr.y) ? abr.y : bbr.y,
				y);

			if (!GenericArithmetic<T>.gt(w, GenericArithmetic<T>.zero)
			 || !GenericArithmetic<T>.gt(h, GenericArithmetic<T>.zero))
				return new Rect<T>();

			return new Rect<T>(x, y, w, h);
		}

		public static Rect<T> operator |(Rect<T> a, Rect<T> b)
		{
			var abr = a.bottomRight;
			var bbr = b.bottomRight;
			T x = GenericArithmetic<T>.lt(a.x, b.x) ? a.x : b.x;
			T y = GenericArithmetic<T>.lt(a.y, b.y) ? a.y : b.y;
			T w = GenericArithmetic<T>.substract(
				GenericArithmetic<T>.gt(abr.x, bbr.x) ? abr.x : bbr.x,
				x);
			T h = GenericArithmetic<T>.substract(
				GenericArithmetic<T>.gt(abr.y, bbr.y) ? abr.y : bbr.y,
				y);

			return new Rect<T>(x, y, w, h);
		}

		public override string ToString()
		{
			return new StringBuilder("[").Append(x.ToString())
				.Append(", ").Append(y.ToString())
				.Append(", ").Append(width.ToString())
				.Append(", ").Append(height.ToString())
				.Append("]").ToString();
		}

		// implicit conversion operator itself can not has any generic parameter
		// so this explicit conversion method is create 
		// further more that is a "safe" way to force user to use explicit conversion
		public Rect<V> toRect<V>() where V : struct, IConvertible, IComparable<V>, IComparable, IEquatable<V>, IFormattable
		{
			return new Rect<V>(
					GenericConverter<T, V>.convert(x),
					GenericConverter<T, V>.convert(y),
					GenericConverter<T, V>.convert(width),
					GenericConverter<T, V>.convert(height)
				);
		}
	}
}
