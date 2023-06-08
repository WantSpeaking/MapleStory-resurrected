using System;
using Helper;
using MapleLib.WzLib;
using ms.Helper;

//////////////////////////////////////////////////////////////////////////////////
//	This file is part of the continued Journey MMORPG client					//
//	Copyright (C) 2015-2019  Daniel Allendorf, Ryan Payton						//
//																				//
//	This program is free software: you can redistribute it and/or modify		//
//	it under the terms of the GNU Affero General Public License as published by	//
//	the Free Software Foundation, either version 3 of the License, or			//
//	(at your option) any later version.											//
//																				//
//	This program is distributed in the hope that it will be useful,				//
//	but WITHOUT ANY WARRANTY; without even the implied warranty of				//
//	MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the				//
//	GNU Affero General Public License for more details.							//
//																				//
//	You should have received a copy of the GNU Affero General Public License	//
//	along with this program.  If not, see <https://www.gnu.org/licenses/>.		//
//////////////////////////////////////////////////////////////////////////////////


namespace ms
{
	public class Rectangle_short
	{
		public static Rectangle_short zero = new Rectangle_short();

        private Point_short left_top = new Point_short ();

		private Point_short right_bottom = new Point_short ();

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sourceLeftTop">["range"]["lt"]</param>
		/// <param name="sourceRightBottom">["range"]["rb"]</param>
		public Rectangle_short (WzObject sourceLeftTop, WzObject sourceRightBottom)
		{
			left_top = sourceLeftTop?.GetPoint ().ToMSPoint () ?? Point_short.zero;
			right_bottom = sourceRightBottom?.GetPoint ().ToMSPoint () ?? Point_short.zero;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="source">["range"]</param>
		public Rectangle_short (WzObject source)
			: this (source["lt"], source["rb"])
		{
		}

		public Rectangle_short (Rectangle_short src)
			: this (src.left_top, src.right_bottom)
		{
		}

		public Rectangle_short (Point_short leftTop, Point_short rightBottom)
		{
			left_top = new Point_short (leftTop.x (), leftTop.y ());
			right_bottom = new Point_short (rightBottom.x (), rightBottom.y ());
		}

		public Rectangle_short (short left, short right, short top, short bottom)
		{
			left_top = new Point_short (left, top);
			right_bottom = new Point_short (right, bottom);
		}

		public Rectangle_short ()
		{
		}

		public void Set (short left, short right, short top, short bottom)
		{
			left_top.Set (left, top);
			right_bottom.Set (right, bottom);
		}

		public short width ()
		{
			return (short)Math.Abs (left () - right ());
		}

		public short height ()
		{
			return (short)Math.Abs (top () - bottom ());
		}

		public short left ()
		{
			return left_top.x ();
		}

		public short top ()
		{
			return left_top.y ();
		}

		public short right ()
		{
			return right_bottom.x ();
		}

		public short bottom ()
		{
			return right_bottom.y ();
		}

		public Point_short center ()
		{
			return new Point_short ((short)((left () + right ()) / 2), (short)((top () + bottom ()) / 2));
		}

		public bool contains (Point_short v)
		{
			return !straight () && v.x () >= left () && v.x () <= right () && v.y () >= top () && v.y () <= bottom ();
		}

		public bool contains (Rectangle_short ar)
		{
			return get_horizontal ().overlaps (new Range_short (ar.left (), ar.right ())) && get_vertical ().overlaps (new Range_short (ar.top (), ar.bottom ()));
		}

		public bool overlaps (Rectangle_short ar)
		{
			return get_horizontal ().overlaps (new Range_short (ar.left (), ar.right ())) && get_vertical ().overlaps (new Range_short (ar.top (), ar.bottom ()));
		}

		public bool straight ()
		{
			return left_top == right_bottom;
		}

		public bool empty ()
		{
			return left_top.straight () && right_bottom.straight () && straight ();
		}

		public Point_short get_left_top ()
		{
			return left_top;
		}

		public Point_short get_right_bottom ()
		{
			return right_bottom;
		}

		public Range_short get_horizontal ()
		{
			return new Range_short (left (), right ());
		}

		public Range_short get_vertical ()
		{
			return new Range_short (top (), bottom ());
		}

		public Rectangle_short shift (Point_short v)
		{
			left_top += v;
			right_bottom += v;
			return this;
		}

        public Rectangle_short shift_x(short value)
        {
			left_top.shift_x(value);
            right_bottom.shift_x(value);
            return this;
        }
        public Rectangle_short shift_y(short value)
        {
            left_top.shift_y(value);
            right_bottom.shift_y(value);
            return this;
        }
        public Rectangle_short expand (short value)
		{
			Point_short expand = new Point_short (value, value);
			left_top -= expand;
			right_bottom += expand;
			return this;
		}

		public override string ToString ()
		{
			return $"[left_top:{left_top} , right_bottom{right_bottom}]";
		}
	}

	/*public class Rectangle_short where T : unmanaged, IComparable, IComparable<T>, IEquatable<T>, IConvertible
	{
		public Rectangle (WzObject sourceLeftTop, WzObject sourceRightBottom)
		{
			if (sourceLeftTop?.GetPoint () == null || sourceRightBottom?.GetPoint () == null)
			{
				left_top = new Point<T> ();
				right_bottom = new Point<T> ();
				return;
			}

			var tempPoint1 = sourceLeftTop.GetPoint ();
			left_top = new Point<T> (tempPoint1.X.ToT<T> (), tempPoint1.Y.ToT<T> ());

			var tempPoint2 = sourceRightBottom.GetPoint ();
			right_bottom = new Point<T> (tempPoint2.X.ToT<T> (), tempPoint2.Y.ToT<T> ());
		}

		public Rectangle (WzObject source) : this (source["lt"], source["rb"])
		{
		}

		public Rectangle (Rectangle_short src) : this (src.left_top, src.right_bottom)
		{
		}

		public Rectangle (Point<T> leftTop, Point<T> rightBottom)
		{
			left_top = new Point<T> (leftTop.x ().ToT<T> (), leftTop.y ().ToT<T> ());
			right_bottom = new Point<T> (rightBottom.x ().ToT<T> (), rightBottom.y ().ToT<T> ());
		}

		public Rectangle (T left, T right, T top, T bottom)
		{
			left_top = new Point<T> (left, top);
			right_bottom = new Point<T> (right, bottom);
		}

		public Rectangle ()
		{
		}

		public void Set (T left, T right, T top, T bottom)
		{
			left_top.Set (left, top);
			right_bottom.Set (right, bottom);
		}

		public T Width ()
		{
			return GenericArithmetic.Abs (GenericArithmetic.Subtract (left (), right ()));
		}

		public T height ()
		{
			return GenericArithmetic.Abs (GenericArithmetic.Subtract (top (), bottom ()));
		}

		public T left ()
		{
			return left_top.x ();
		}

		public T top ()
		{
			return left_top.y ();
		}

		public T right ()
		{
			return right_bottom.x ();
		}

		public T bottom ()
		{
			return right_bottom.y ();
		}

		public Point<T> center ()
		{
			return new Point<T> (GenericArithmetic.Average (left (), right ()), GenericArithmetic.Average (top (), bottom ()));
		}

		public bool contains (Point<T> v)
		{
			return !straight () && GenericArithmetic.GreaterThanOrEqual (v.x (), left ()) && GenericArithmetic.GreaterThanOrEqual (right (), v.x ()) && GenericArithmetic.GreaterThanOrEqual (v.y (), top ()) && GenericArithmetic.GreaterThanOrEqual (bottom (), v.y ());
		}

		public bool contains (Rectangle_short ar)
		{
			return get_horizontal ().contains (new Range<T> (ar.left (), ar.right ())) && get_vertical ().contains (new Range<T> (ar.top (), ar.bottom ()));
		}

		public bool overlaps (Rectangle_short ar)
		{
			return get_horizontal ().overlaps (new Range<T> (ar.left (), ar.right ())) && get_vertical ().overlaps (new Range<T> (ar.top (), ar.bottom ()));
		}

		public bool straight ()
		{
			return left_top == right_bottom;
		}

		public bool empty ()
		{
			return left_top.straight () && right_bottom.straight () && straight ();
		}

		public Point<T> get_left_top ()
		{
			return left_top;
		}

		public Point<T> get_right_bottom ()
		{
			return right_bottom;
		}

		public Range<T> get_horizontal ()
		{
			return new Range<T> (left (), right ());
		}

		public Range<T> get_vertical ()
		{
			return new Range<T> (top (), bottom ());
		}

		public void shift (Point<T> v)
		{
			left_top = left_top + v;
			right_bottom = right_bottom + v;
		}

		public Rectangle_short expand(T value)
        {
			var expand = new Point<T>(value, value);
			left_top = left_top - expand;
			right_bottom = right_bottom + expand;
			return this;
        }
		public override string ToString ()
		{
			return $"[left_top:{left_top} , right_bottom{right_bottom}]";
		}

		private Point<T> left_top = new Point<T> ();
		private Point<T> right_bottom = new Point<T> ();
	}*/

	/*public class Rectangle
	{
		public Rectangle (WzObject sourceLeftTop, WzObject sourceRightBottom)
		{
			if (sourceLeftTop?.GetPoint () == null || sourceRightBottom?.GetPoint () == null)
			{
				left_top = new Point ();
				right_bottom = new Point ();
				return;
			}

			var tempPoint1 = sourceLeftTop.GetPoint ();
			left_top = new Point (tempPoint1.X, tempPoint1.Y);

			var tempPoint2 = sourceRightBottom.GetPoint ();
			right_bottom = new Point (tempPoint2.X, tempPoint2.Y);
		}

		public Rectangle (WzObject source) : this (source["lt"], source["rb"])
		{
		}

		public Rectangle (Rectangle src) : this (src.left_top, src.right_bottom)
		{
		}

		public Rectangle (Point leftTop, Point rightBottom)
		{
			left_top = new Point (leftTop.x (), leftTop.y ());
			right_bottom = new Point (rightBottom.x (), rightBottom.y ());
		}

		public Rectangle (int left, int right, int top, int bottom)
		{
			left_top = new Point (left, top);
			right_bottom = new Point (right, bottom);
		}

		public Rectangle ()
		{
		}

		public void Set (int left, int right, int top, int bottom)
		{
			left_top.Set (left, top);
			right_bottom.Set (right, bottom);
		}

		public Point width ()
		{
			return (Point)(left () - right ());
		}

		public Point height ()
		{
			return (Point)(top () - bottom ());
		}

		public Point left ()
		{
			return left_top.x ();
		}

		public Point top ()
		{
			return left_top.y ();
		}

		public Point right ()
		{
			return right_bottom.x ();
		}

		public Point bottom ()
		{
			return right_bottom.y ();
		}

		public Point center ()
		{
			return new Point ((int)((left () + right ()) / 2), (int)((top () + bottom ()) / 2));
		}

		public bool contains (Point v)
		{
			return
				!straight () &&
				v.x () >= left () && v.x () <= right () &&
				v.y () >= top () && v.y () <= bottom ();
		}

		public bool contains (Rectangle ar)
		{
			return
				get_horizontal ().overlaps (new Range_Point (ar.left (), ar.right ())) &&
				get_vertical ().overlaps (new Range_Point (ar.top (), ar.bottom ()));
		}

		public bool overlaps (Rectangle ar)
		{
			return get_horizontal ().overlaps (new Range_Point (ar.left (), ar.right ())) && get_vertical ().overlaps (new Range_Point (ar.top (), ar.bottom ()));
		}

		public bool straight ()
		{
			return left_top == right_bottom;
		}

		public bool empty ()
		{
			return left_top.straight () && right_bottom.straight () && straight ();
		}

		public Point get_left_top ()
		{
			return left_top;
		}

		public Point get_right_bottom ()
		{
			return right_bottom;
		}

		public Range_Point get_horizontal ()
		{
			return new Range_Point (left (), right ());
		}

		public Range_Point get_vertical ()
		{
			return new Range_Point (top (), bottom ());
		}

		public void shift (Point v)
		{
			left_top = left_top + v;
			right_bottom = right_bottom + v;
		}

		public Rectangle expand (Point value)
		{
			var expand = new Point (value, value);
			left_top = left_top - expand;
			right_bottom = right_bottom + expand;
			return this;
		}
		public override string ToString ()
		{
			return $"[left_top:{left_top} , right_bottom{right_bottom}]";
		}

		private Point left_top = new Point ();
		private Point right_bottom = new Point ();
	}*/
}