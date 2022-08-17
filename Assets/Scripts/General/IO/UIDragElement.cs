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

		public override Cursor.State send_cursor(bool clicked, Point_short cursorpos)
		{
			if (clicked)
			{
				if (dragged)
				{
					position = cursorpos - cursoroffset;

					return Cursor.State.CLICKING;
				}
				else if (indragrange(new Point_short (cursorpos)))
				{
					cursoroffset = cursorpos - position;
					dragged = true;

					return base.send_cursor(clicked, new Point_short (cursorpos));
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

			return base.send_cursor(clicked,new Point_short (cursorpos));
		}

		protected UIDragElement() : this(new Point_short(0, 0))
		{
		}

		protected UIDragElement(Point_short d)
		{
			this.dragarea = new Point_short (d);
			position = Setting<T>.get().load();
		}

		protected bool dragged = false;
		protected Point_short dragarea = new Point_short();
		protected Point_short cursoroffset = new Point_short();

		public virtual bool indragrange(Point_short cursorpos)
		{
			var bounds = new Rectangle_short(new Point_short (position), position + dragarea);

			return bounds.contains(cursorpos);
		}
	}
}