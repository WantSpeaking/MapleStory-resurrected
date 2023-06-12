#define USE_NX

using System;
using System.Collections.Generic;
using System.Linq;
using MapleLib.WzLib;




namespace ms
{
	public class Reactor : MapObject
	{
		public Reactor (int o, int r, sbyte s, Point_short p) : base (o, p)
		{
			this.rid = r;
			this.state = s;
			string strid = string_format.extend_id (rid, 7);
			src = ms.wz.wzFile_reactor[strid + ".img"];//9102001.img
            if (src == null)
			{
				AppDebug.Log($"Reactor rid:{rid} is null");
				return;
			}
			var src_0 = src["0"];

            if (src_0 == null && src["info"]?["link"]!= null)
            {
                var linkNodeName = src["info"]?["link"]?.ToString();//9102000

                if (!string.IsNullOrEmpty(linkNodeName))
				{
					src_0 = ms.wz.wzFile_reactor[string_format.extend_id(int.Parse(linkNodeName), 7) + ".img"]?["0"];
                }

            }

			normal = src_0;
			animation_ended = true;
			dead = false;
			hittable = false;

			foreach (var sub in src_0)
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

		public override void draw (double viewx, double viewy, float alpha)
		{
			Point_short absp = phobj.get_absolute (viewx, viewy, alpha);
			Point_short shift = new Point_short (0, normal.get_origin ().y ());
			var drawArgum = new DrawArgument (absp - shift).SetParent (MapGameObject);
			if (animation_ended)
			{
				// TODO: Handle 'default' animations (horntail reactor floating)
				normal.draw (drawArgum, alpha);
			}
			else
			{
				//animations[(sbyte)(state - 1)].draw (drawArgum, 1.0F);
                animations[(sbyte)(state)].draw(drawArgum, 1.0F);
            }
		}

		public override sbyte update (Physics physics)
		{
			physics.move_object (phobj);

			if (!animation_ended)
			{
				//animation_ended = animations[(sbyte)(state - 1)].update ();
                animation_ended = animations[(sbyte)(state)].update();
            }

			if (animation_ended && dead)
			{
				deactivate ();
			}

			return phobj.fhlayer;
		}

		public void set_state (sbyte state)
		{
			AppDebug.Log($"set_state:{state}");
			//state = Math.Clamp(state, animations.Keys.Min(), animations.Keys.Max());
            // TODO: hit/break sounds
            if (hittable)
			{
                //animations[this.state] = src[this.state.ToString()]["hit"];
                animations[state] = src[state.ToString ()]["hit"];
				animation_ended = false;
			}

			this.state = state;
		}

		public void destroy (sbyte state, Point_short position)
		{
			//animations[this.state] = src[this.state.ToString ()]["hit"];
            animations[state] = src[state.ToString()]["hit"];
            this.state = state;
            //state++;
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
		//ORIGINAL LINE: bool is_in_range(const Rectangle_short& range) const
		public bool is_in_range (Rectangle_short range)
		{
			if (!active)
			{
				return false;
			}

			Rectangle_short bounds = new Rectangle_short (new Point_short (-30, (short)-normal.get_dimensions ().y ()), new Point_short ((short)(normal.get_dimensions ().x () - 10), 0)); //normal.get_bounds(); //animations.at(stance).get_bounds();
			bounds.shift (get_position ());

			return range.overlaps (bounds);
		}

		public override void Dispose ()
		{
			base.Dispose ();
			foreach (var pair in animations)
			{
				pair.Value.Dispose ();
			}
			animations.Clear ();
			animations = null;
			
			normal?.Dispose ();
			normal = null;
		}

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

		//private bool active;
		private bool hittable;
		private bool dead;

		private Animation normal = new Animation ();
	}
}


#if USE_NX
#endif