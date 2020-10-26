#define USE_NX

using System.Collections.Generic;
using MapleLib.WzLib;

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
	public class Reactor : MapObject
	{
		public Reactor (int o, int r, sbyte s, Point<short> p) : base (o, p)
		{
			this.rid = r;
			this.state = s;
			string strid = string_format.extend_id (rid, 7);
			src = nl.nx.wzFile_reactor[strid + ".img"];

			normal = src[0.ToString ()];
			animation_ended = true;
			dead = false;
			hittable = false;

			foreach (var sub in src[0.ToString ()])
			{
				if (sub.Name == "event")
				{
					if (sub["0"]["type"] == 0)
					{
						hittable = true;
					}
				}
			}
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void draw(double viewx, double viewy, float alpha) const override
		public override void draw (double viewx, double viewy, float alpha)
		{
			Point<short> absp = phobj.get_absolute (viewx, viewy, alpha);
			Point<short> shift = new Point<short> (0, normal.get_origin ().y ());

			if (animation_ended)
			{
				// TODO: Handle 'default' animations (horntail reactor floating)
				normal.draw (absp - shift, alpha);
			}
			else
			{
				animations[(sbyte)(state - 1)].draw (new DrawArgument (absp - shift), 1.0F);
			}
		}

		public new sbyte update (Physics physics)
		{
			physics.move_object (phobj);

			if (!animation_ended)
			{
				animation_ended = animations[(sbyte)(state - 1)].update ();
			}

			if (animation_ended && dead)
			{
				deactivate ();
			}

			return phobj.fhlayer;
		}

		public void set_state (sbyte state)
		{
			// TODO: hit/break sounds
			if (hittable)
			{
				animations[this.state] = src[this.state.ToString ()]["hit"];
				animation_ended = false;
			}

			this.state = state;
		}

		public void destroy (sbyte state, Point<short> position)
		{
			animations[this.state] = src[this.state.ToString ()]["hit"];
			state++;
			dead = true;
			animation_ended = false;
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: bool is_hittable() const
		public bool is_hittable ()
		{
			return hittable;
		}

		// Check if this mob collides with the specified rectangle
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: bool is_in_range(const Rectangle<short>& range) const
		public bool is_in_range (Rectangle<short> range)
		{
			if (!active)
			{
				return false;
			}

			Rectangle<short> bounds = new Rectangle<short> (new Point<short> (-30, (short)-normal.get_dimensions ().y ()), new Point<short> ((short)(normal.get_dimensions ().x () - 10), 0)); //normal.get_bounds(); //animations.at(stance).get_bounds();
			bounds.shift (get_position ());

			return range.overlaps (bounds);
		}

		private int oid;
		private int rid;

		private sbyte state;
		// TODO: Below
		//int8_t stance; // ??
		// TODO: These are in the GMS client
		//bool movable; // Snowball?
		//int32_t questid;
		//bool activates_by_touch;

		private WzObject src;
		private SortedDictionary<sbyte, Animation> animations = new SortedDictionary<sbyte, Animation> ();
		private bool animation_ended;

		private bool active;
		private bool hittable;
		private bool dead;

		private Animation normal = new Animation ();
	}
}


#if USE_NX
#endif