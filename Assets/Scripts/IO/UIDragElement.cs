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
//ORIGINAL LINE: template <typename T>
	// Base class for UI Windows which can be moved with the mouse cursor.
	public abstract class UIDragElement <T>: UIElement where T : Configuration.PointEntry, new ()
	{
		public override void remove_cursor()
		{
			base.remove_cursor();

			if (dragged)
			{
				dragged = false;

				Setting<T>.get().save(position);
			}
		}

		public override Cursor.State send_cursor(bool clicked, Point<short> cursorpos)
		{
			if (clicked)
			{
				if (dragged)
				{
					position = cursorpos - cursoroffset;

					return Cursor.State.CLICKING;
				}
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: The following line was determined to be a copy constructor call - this should be verified and a copy constructor should be created if it does not yet exist:
//ORIGINAL LINE: else if (indragrange(cursorpos))
				else if (indragrange(cursorpos))
				{
					cursoroffset = cursorpos - position;
					dragged = true;

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: The following line was determined to be a copy constructor call - this should be verified and a copy constructor should be created if it does not yet exist:
//ORIGINAL LINE: return UIElement::send_cursor(clicked, cursorpos);
					return base.send_cursor(clicked, cursorpos);
				}
			}
			else
			{
				if (dragged)
				{
					dragged = false;

					Setting<T>.get().save(position);
				}
			}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: The following line was determined to be a copy constructor call - this should be verified and a copy constructor should be created if it does not yet exist:
//ORIGINAL LINE: return UIElement::send_cursor(clicked, cursorpos);
			return base.send_cursor(clicked, cursorpos);
		}

		protected UIDragElement() : this(new Point<short>(0, 0))
		{
		}

		protected UIDragElement(Point<short> d)
		{
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: The following line was determined to be a copy constructor call - this should be verified and a copy constructor should be created if it does not yet exist:
//ORIGINAL LINE: this.dragarea = new ms.Point<short>(d);
			this.dragarea = d;
			position = Setting<T>.get().load();
		}

		protected bool dragged = false;
		protected Point<short> dragarea = new Point<short>();
		protected Point<short> cursoroffset = new Point<short>();

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: virtual bool indragrange(Point<short> cursorpos) const
		public virtual bool indragrange(Point<short> cursorpos)
		{
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: The following line was determined to be a copy constructor call - this should be verified and a copy constructor should be created if it does not yet exist:
//ORIGINAL LINE: var bounds = Rectangle<short>(position, position + dragarea);
			var bounds = new Rectangle<short>(position, position + dragarea);

			return bounds.contains(cursorpos);
		}
	}
}