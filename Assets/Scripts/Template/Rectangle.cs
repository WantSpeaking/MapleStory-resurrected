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
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: The original C++ template specifier was replaced with a C# generic specifier, which may not produce the same behavior:
//ORIGINAL LINE: template <class T>
	public class Rectangle<T> where T : unmanaged, IComparable, IComparable<T>, IEquatable<T>, IConvertible
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

		public Rectangle (Rectangle<T> src) : this (src.left_top, src.right_bottom)
		{
		}

		public Rectangle (Point<T> leftTop, Point<T> rightBottom)
		{
			left_top = leftTop;
			right_bottom = rightBottom;
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

		public T width ()
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
			return !straight () && (dynamic)v.x () >= left () && (dynamic)v.x () <= right () && (dynamic)v.y () >= top () && (dynamic)v.y () <= bottom ();
		}

		public bool contains (Rectangle<T> ar)
		{
			return get_horizontal ().contains (new Range<T> (ar.left (), ar.right ())) && get_vertical ().contains (new Range<T> (ar.top (), ar.bottom ()));
		}

		public bool overlaps (Rectangle<T> ar)
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
			//left_top= (left_top + v);
			right_bottom = right_bottom + v;
			//right_bottom= (right_bottom + v);
		}

		public override string ToString ()
		{
			return $"[left_top:{left_top} , right_bottom{right_bottom}]";
		}

		private Point<T> left_top = new Point<T> ();
		private Point<T> right_bottom = new Point<T> ();
	}
}