using System;
using System.Collections.Generic;
using System.Text;

namespace CoreCV
{
	// since no inheriting in struct, we use a field of Point<UInt32>
	public struct Size 
	{
		private Point<UInt32> inner_;
		public UInt32 w { get => inner_.x; set => inner_.x = value; }
		public UInt32 h { get => inner_.y; set => inner_.y = value; }

		public Size(UInt32 w, UInt32 h)
		{
			inner_.x = w;
			inner_.y = h;
		}

		public static implicit operator Size(Point<UInt32> p)
		{
			return new Size(p.x, p.y);
		}

		public static implicit operator Point<UInt32>(Size S)
		{
			return S.inner_;
		}

		public static Size operator +(Size a, Size b)
		{
			return a.inner_ + b.inner_;
		}

		public static Size operator -(Size a, Size b)
		{
			Size ret = a.inner_ - b.inner_;
			if (b.w > a.w) ret.w = 0;
			if (b.h > a.h) ret.h = 0;
			return ret;
		}

		public static Size operator *(Size a, UInt32 s)
		{
			return a.inner_ * s;
		}

		public static Size operator /(Size a, UInt32 s)
		{
			return a.inner_ / s;
		}

		public static bool operator ==(Size a, Size b)
		{
			return a.inner_ == b.inner_;
		}

		public static bool operator !=(Size a, Size b)
		{
			return !(a == b);
		}

		public override bool Equals(object b)
		{
			return this == (Size)b;
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		public override string ToString()
		{
			return new StringBuilder("(").Append(w.ToString())
				.Append(", ")
				.Append(h.ToString()).Append(")").ToString();
		}
	}
}
