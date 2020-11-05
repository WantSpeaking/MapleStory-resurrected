using System.Collections.Generic;
using UnityEngine;

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
	public class CharLook
	{
		public CharLook (LookEntry entry)
		{
			reset ();

			set_body (entry.skin);
			set_hair (entry.hairid);
			set_face (entry.faceid);
			Debug.Log ($"CharLook entry.equips:{entry.equips.ToDebugLog()}");

			foreach (var equip in entry.equips)
			{
				add_equip (equip.Value);
			}
		}

		public CharLook ()
		{
			reset ();

			body = null;
			hair = null;
			face = null;
		}

		public void reset ()
		{
			flip = true;

			action = null;
			actionstr = "";
			actframe = 0;

			set_stance (Stance.Id.STAND1);
			stframe.set (0);
			stelapsed = 0;

			set_expression (Expression.Id.DEFAULT);
			expframe.set (0);
			expelapsed = 0;
		}

		public void draw (DrawArgument args, float alpha)
		{
			if (body == null || hair == null || face == null)
			{
				return;
			}

			Point<short> acmove = new Point<short> ();

			if (action != null)
			{
				acmove = (action.get_move ());
			}

			DrawArgument relargs = new DrawArgument (acmove, flip);

			Stance.Id interstance = stance.get (alpha);
			Expression.Id interexpression = expression.get (alpha);
			byte interframe = stframe.get (alpha);
			byte interexpframe = expframe.get (alpha);
			//Debug.Log ($"alpha:{alpha} \t interframe:{interframe}");
			switch (interstance)
			{
				case Stance.Id.STAND1:
				case Stance.Id.STAND2:
				{
					if ((bool)alerted)
					{
						interstance = Stance.Id.ALERT;
					}

					break;
				}
			}

			draw (relargs + args, interstance, interexpression, interframe, interexpframe);
		}

		public void draw (Point<short> position, bool flipped, Stance.Id interstance, Expression.Id interexpression)
		{
			interstance = equips.adjust_stance (interstance);
			draw (new DrawArgument (position, flipped), interstance, interexpression, 0, 0);
		}

		public bool update (ushort timestep)
		{
			//Debug.Log ($"now:{stance.get()}\t last:{stance.last ()}");

			if (timestep == 0)
			{
				stance.normalize ();
				stframe.normalize ();
				expression.normalize ();
				expframe.normalize ();
				return false;
			}

			alerted.update ();
			expcooldown.update ();

			bool aniend = false;

			if (action == null)
			{
				ushort delay = get_delay (stance.get (), stframe.get ());
				ushort delta = (ushort)(delay - stelapsed);

				if (timestep >= delta)
				{
					stelapsed = (ushort)(timestep - delta);

					byte nextframe = getnextframe (stance.get (), stframe.get ());
					float threshold = (float)delta / timestep;
					stframe.next (nextframe, threshold);

					if (stframe == 0)
					{
						aniend = true;
					}
				}
				else
				{
					stance.normalize ();
					stframe.normalize ();

					stelapsed += timestep;
				}
			}
			else
			{
				ushort delay = action.get_delay ();
				ushort delta = (ushort)(delay - stelapsed);

				if (timestep >= delta)
				{
					stelapsed = (ushort)(timestep - delta);
					actframe = drawinfo.next_actionframe (actionstr, actframe);

					if (actframe > 0)
					{
						action = drawinfo.get_action (actionstr, actframe);

						float threshold = (float)delta / timestep;
						stance.next (action.get_stance (), threshold);
						stframe.next (action.get_frame (), threshold);
					}
					else
					{
						aniend = true;
						action = null;
						actionstr = "";
						set_stance (Stance.Id.STAND1);
					}
				}
				else
				{
					stance.normalize ();
					stframe.normalize ();

					stelapsed += timestep;
				}
				
			}

			ushort expdelay = (ushort)face.get_delay (expression.get (), expframe.get ());
			ushort expdelta = (ushort)(expdelay - expelapsed);

			if (timestep >= expdelta)
			{
				expelapsed = (ushort)(timestep - expdelta);

				byte nextexpframe = face.nextframe (expression.get (), expframe.get ());
				float fcthreshold = (float)expdelta / timestep;
				expframe.next (nextexpframe, fcthreshold);

				if (expframe == 0)
				{
					if (expression == Expression.Id.DEFAULT)
					{
						expression.next (Expression.Id.BLINK, fcthreshold);
					}
					else
					{
						expression.next (Expression.Id.DEFAULT, fcthreshold);
					}
				}
			}
			else
			{
				expression.normalize ();
				expframe.normalize ();

				expelapsed += timestep;
			}

			return aniend;
		}

		public void set_hair (int hair_id)
		{
			/*if (!hairstyles.ContainsKey (hair_id))
			{
				var hair = new Hair (hair_id,drawinfo);p
				hairstyles.Add (hair_id,hair);
			}*/
			if (hairstyles.TryGetValue (hair_id, out hair)) return;
			hair = new Hair (hair_id, drawinfo);
			hairstyles.Add (hair_id, hair);
			/*var iter = hairstyles.find(hair_id);

			if (iter == hairstyles.end())
			{
				iter = hairstyles.emplace(piecewise_construct, forward_as_tuple(hair_id), forward_as_tuple(hair_id, drawinfo)).first;
			}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: Iterators are only converted within the context of 'while' and 'for' loops:
			hair = iter.second;*/
		}

		public void set_body (int skin_id)
		{
			/*if (!bodytypes.ContainsKey (skin_id))
			{
				var body = new Body (skin_id,drawinfo);
				bodytypes.Add (skin_id,body);
			}*/
			if (bodytypes.TryGetValue (skin_id, out body)) return;
			body = new Body (skin_id, drawinfo);
			bodytypes.Add (skin_id, body);

			//body = tempBody;
			/*var iter = bodytypes.find(skin_id);

			if (iter == bodytypes.end())
			{
				iter = bodytypes.emplace(piecewise_construct, forward_as_tuple(skin_id), forward_as_tuple(skin_id, drawinfo)).first;
			}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: Iterators are only converted within the context of 'while' and 'for' loops:
			body = iter.second;*/
		}

		public void set_face (int face_id)
		{
			/*if (!facetypes.ContainsKey (face_id))
			{
				var face = new Face (face_id);
				facetypes.Add (face_id,face);
			}*/
			if (facetypes.TryGetValue (face_id, out face)) return;
			face = new Face (face_id);
			facetypes.Add (face_id, face);

			/*var iter = facetypes.find(face_id);

			if (iter == facetypes.end())
			{
				iter = facetypes.Add(face_id, face_id).first;
			}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: Iterators are only converted within the context of 'while' and 'for' loops:
			face = iter.second;*/
		}

		public void add_equip (int itemid)
		{
			equips.add_equip (itemid, drawinfo);
			updatetwohanded ();
		}

		public void remove_equip (EquipSlot.Id slot)
		{
			equips.remove_equip (slot);

			if (slot == EquipSlot.Id.WEAPON)
			{
				updatetwohanded ();
			}
		}

		public void attack (bool degenerate)
		{
			int weapon_id = equips.get_weapon ();

			if (weapon_id <= 0)
			{
				return;
			}

			WeaponData weapon = WeaponData.get (weapon_id);

			byte attacktype = weapon.get_attack ();

			if (attacktype == 9 && !degenerate)
			{
				stance.set (Stance.Id.SHOT);
				set_action ("handgun");
			}
			else
			{
				stance.set (getattackstance (attacktype, degenerate));
				stframe.set (0);
				stelapsed = 0;
			}

			//weapon.get_usesound(degenerate).play();//todo get_usesound
		}

		public void attack (Stance.Id newstance)
		{
			if (action != null || newstance == Stance.Id.NONE)
			{
				return;
			}

			switch (newstance)
			{
				case Stance.Id.SHOT:
					set_action ("handgun");
					break;
				default:
					set_stance (newstance);
					break;
			}
		}

		public void set_stance (Stance.Id newstance)
		{
			if (action != null || newstance == Stance.Id.NONE)
			{
				return;
			}

			Stance.Id adjstance = equips.adjust_stance (newstance);

			if (stance != adjstance)
			{
				stance.set (adjstance);
				stframe.set (0);
				stelapsed = 0;
			}
		}

		public void set_expression (Expression.Id newexpression)
		{
			if (expression != newexpression && expcooldown == null)
			{
				expression.set (newexpression);
				expframe.set (0);

				expelapsed = 0;
				expcooldown.set_for (5000);
			}
		}

		public void set_action (string acstr)
		{
			if (acstr == actionstr || acstr == "")
			{
				return;
			}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: Variables cannot be declared in if/while/switch conditions in C#:
			var ac_stance = Stance.by_string (acstr);
			if (ac_stance != 0)
			{
				set_stance (ac_stance);
			}
			else
			{
				action = drawinfo.get_action (acstr, 0);

				if (action != null)
				{
					actframe = 0;
					stelapsed = 0;
					actionstr = acstr;

					stance.set (action.get_stance ());
					stframe.set (action.get_frame ());
				}
			}
		}

		public void set_direction (bool f)
		{
			flip = f;
		}

		public void set_alerted (long millis)
		{
			alerted.set_for (millis);
		}

		public bool get_alerted ()
		{
			return (bool)alerted;
		}

		public bool is_twohanded (Stance.Id st)
		{
			switch (st)
			{
				case Stance.Id.STAND1:
				case Stance.Id.WALK1:
					return false;
				case Stance.Id.STAND2:
				case Stance.Id.WALK2:
					return true;
				default:
					return equips.is_twohanded ();
			}
		}

		public ushort get_attackdelay (uint no, byte first_frame)
		{
			if (action != null)
			{
				return drawinfo.get_attackdelay (actionstr, no);
			}

			ushort delay = 0;

			for (byte frame = 0; frame < first_frame; frame++)
			{
				delay += get_delay (stance.get (), frame);
			}

			return delay;
		}

		public byte get_frame ()
		{
			return stframe.get ();
		}

		public Stance.Id get_stance ()
		{
			return stance.get ();
		}

		public Body get_body ()
		{
			return body;
		}

		public Hair get_hair ()
		{
			return hair;
		}

		public Face get_face ()
		{
			return face;
		}

		public CharEquips get_equips ()
		{
			return equips;
		}

		// Initialize drawinfo
		public static void init ()
		{
			drawinfo.init ();
		}

		private void updatetwohanded ()
		{
			Stance.Id basestance = Stance.baseof (stance.get ());
			set_stance (basestance);
		}

		private DrawArgument lastDraw_args;
		private Stance.Id lastDraw_interstance;
		private Expression.Id lastDraw_interexpression;
		private sbyte lastDraw_interframe = -1;
		private byte lastDraw_interexpframe;

		private void erase (DrawArgument args, Stance.Id interstance, Expression.Id interexpression, byte interframe, byte interexpframe)
		{
			if (args == null) return;
			//Point<short> faceshift = drawinfo.getfacepos (interstance, interframe);
			Point<short> faceshift = drawinfo.getfacepos (interstance, interframe) ?? Point<short>.zero;
			DrawArgument faceargs = args + new DrawArgument (faceshift, false, new Point<short> ());

			if (Stance.is_climbing (interstance))
			{
				body.draw (interstance, Body.Layer.BODY, interframe, args.SetOrderInLayer (256), false);
				equips.draw (EquipSlot.Id.GLOVES, interstance, Clothing.Layer.GLOVE, interframe, args.IncreaseOrderInLayer (1), false);
				equips.draw (EquipSlot.Id.SHOES, interstance, Clothing.Layer.SHOES, interframe, args.IncreaseOrderInLayer (1), false);
				equips.draw (EquipSlot.Id.BOTTOM, interstance, Clothing.Layer.PANTS, interframe, args.IncreaseOrderInLayer (1), false);
				equips.draw (EquipSlot.Id.TOP, interstance, Clothing.Layer.TOP, interframe, args.IncreaseOrderInLayer (1), false);
				equips.draw (EquipSlot.Id.TOP, interstance, Clothing.Layer.MAIL, interframe, args.IncreaseOrderInLayer (1), false);
				equips.draw (EquipSlot.Id.CAPE, interstance, Clothing.Layer.CAPE, interframe, args.IncreaseOrderInLayer (1), false);
				body.draw (interstance, Body.Layer.HEAD, interframe, args.IncreaseOrderInLayer (1), false);
				equips.draw (EquipSlot.Id.EARACC, interstance, Clothing.Layer.EARRINGS, interframe, args.IncreaseOrderInLayer (1), false);

				switch (equips.getcaptype ())
				{
					case CharEquips.CapType.NONE:
						hair.draw (interstance, Hair.Layer.BACK, interframe, args.IncreaseOrderInLayer (1), false);
						break;
					case CharEquips.CapType.HEADBAND:
						equips.draw (EquipSlot.Id.HAT, interstance, Clothing.Layer.CAP, interframe, args.IncreaseOrderInLayer (1), false);
						hair.draw (interstance, Hair.Layer.BACK, interframe, args.IncreaseOrderInLayer (1), false);
						break;
					case CharEquips.CapType.HALFCOVER:
						hair.draw (interstance, Hair.Layer.BELOWCAP, interframe, args.IncreaseOrderInLayer (1), false);
						equips.draw (EquipSlot.Id.HAT, interstance, Clothing.Layer.CAP, interframe, args.IncreaseOrderInLayer (1), false);
						break;
					case CharEquips.CapType.FULLCOVER:
						equips.draw (EquipSlot.Id.HAT, interstance, Clothing.Layer.CAP, interframe, args.IncreaseOrderInLayer (1), false);
						break;
				}

				equips.draw (EquipSlot.Id.SHIELD, interstance, Clothing.Layer.BACKSHIELD, interframe, args.IncreaseOrderInLayer (1), false);
				equips.draw (EquipSlot.Id.WEAPON, interstance, Clothing.Layer.BACKWEAPON, interframe, args.IncreaseOrderInLayer (1), false);
			}
			else
			{
				hair.draw (interstance, Hair.Layer.BELOWBODY, interframe, args, false);
				equips.draw (EquipSlot.Id.CAPE, interstance, Clothing.Layer.CAPE, interframe, args.IncreaseOrderInLayer (1), false);
				equips.draw (EquipSlot.Id.SHIELD, interstance, Clothing.Layer.SHIELD_BELOW_BODY, interframe, args.IncreaseOrderInLayer (1), false);
				equips.draw (EquipSlot.Id.WEAPON, interstance, Clothing.Layer.WEAPON_BELOW_BODY, interframe, args.IncreaseOrderInLayer (1), false);
				equips.draw (EquipSlot.Id.HAT, interstance, Clothing.Layer.CAP_BELOW_BODY, interframe, args.IncreaseOrderInLayer (1), false);
				body.draw (interstance, Body.Layer.BODY, interframe, args.IncreaseOrderInLayer (1), false);
				equips.draw (EquipSlot.Id.GLOVES, interstance, Clothing.Layer.WRIST_OVER_BODY, interframe, args.IncreaseOrderInLayer (1), false);
				equips.draw (EquipSlot.Id.GLOVES, interstance, Clothing.Layer.GLOVE_OVER_BODY, interframe, args.IncreaseOrderInLayer (1), false);
				equips.draw (EquipSlot.Id.SHOES, interstance, Clothing.Layer.SHOES, interframe, args.IncreaseOrderInLayer (1), false);
				body.draw (interstance, Body.Layer.ARM_BELOW_HEAD, interframe, args.IncreaseOrderInLayer (1), false);

				if (equips.has_overall ())
				{
					equips.draw (EquipSlot.Id.TOP, interstance, Clothing.Layer.MAIL, interframe, args.IncreaseOrderInLayer (1), false);
				}
				else
				{
					equips.draw (EquipSlot.Id.BOTTOM, interstance, Clothing.Layer.PANTS, interframe, args.IncreaseOrderInLayer (1), false);
					equips.draw (EquipSlot.Id.TOP, interstance, Clothing.Layer.TOP, interframe, args.IncreaseOrderInLayer (1), false);
				}

				body.draw (interstance, Body.Layer.ARM_BELOW_HEAD_OVER_MAIL, interframe, args.IncreaseOrderInLayer (1), false);
				equips.draw (EquipSlot.Id.SHIELD, interstance, Clothing.Layer.SHIELD_OVER_HAIR, interframe, args.IncreaseOrderInLayer (1), false);
				equips.draw (EquipSlot.Id.EARACC, interstance, Clothing.Layer.EARRINGS, interframe, args.IncreaseOrderInLayer (1), false);
				body.draw (interstance, Body.Layer.HEAD, interframe, args.IncreaseOrderInLayer (1), false);
				hair.draw (interstance, Hair.Layer.SHADE, interframe, args.IncreaseOrderInLayer (1), false);
				hair.draw (interstance, Hair.Layer.DEFAULT, interframe, args.IncreaseOrderInLayer (1), false);
				face.draw (interexpression, interexpframe, faceargs.SetOrderInLayer (args.orderInLayer).IncreaseOrderInLayer (1), false);
				equips.draw (EquipSlot.Id.FACE, interstance, Clothing.Layer.FACEACC, 0, faceargs.SetOrderInLayer (args.orderInLayer).IncreaseOrderInLayer (1), false);
				equips.draw (EquipSlot.Id.EYEACC, interstance, Clothing.Layer.EYEACC, interframe, args.IncreaseOrderInLayer (1), false);
				equips.draw (EquipSlot.Id.SHIELD, interstance, Clothing.Layer.SHIELD, interframe, args.IncreaseOrderInLayer (1), false);

				switch (equips.getcaptype ())
				{
					case CharEquips.CapType.NONE:
						hair.draw (interstance, Hair.Layer.OVERHEAD, interframe, args.IncreaseOrderInLayer (1), false);
						break;
					case CharEquips.CapType.HEADBAND:
						equips.draw (EquipSlot.Id.HAT, interstance, Clothing.Layer.CAP, interframe, args.IncreaseOrderInLayer (1), false);
						hair.draw (interstance, Hair.Layer.DEFAULT, interframe, args.IncreaseOrderInLayer (1), false);
						hair.draw (interstance, Hair.Layer.OVERHEAD, interframe, args.IncreaseOrderInLayer (1), false);
						equips.draw (EquipSlot.Id.HAT, interstance, Clothing.Layer.CAP_OVER_HAIR, interframe, args.IncreaseOrderInLayer (1), false);
						break;
					case CharEquips.CapType.HALFCOVER:
						hair.draw (interstance, Hair.Layer.DEFAULT, interframe, args.IncreaseOrderInLayer (1), false);
						equips.draw (EquipSlot.Id.HAT, interstance, Clothing.Layer.CAP, interframe, args.IncreaseOrderInLayer (1), false);
						break;
					case CharEquips.CapType.FULLCOVER:
						equips.draw (EquipSlot.Id.HAT, interstance, Clothing.Layer.CAP, interframe, args.IncreaseOrderInLayer (1), false);
						break;
				}

				equips.draw (EquipSlot.Id.WEAPON, interstance, Clothing.Layer.WEAPON_BELOW_ARM, interframe, args.IncreaseOrderInLayer (1), false);
				bool twohanded = is_twohanded (interstance);

				if (twohanded)
				{
					equips.draw (EquipSlot.Id.TOP, interstance, Clothing.Layer.MAILARM, interframe, args.IncreaseOrderInLayer (1), false);
					body.draw (interstance, Body.Layer.ARM, interframe, args.IncreaseOrderInLayer (1), false);
					equips.draw (EquipSlot.Id.WEAPON, interstance, Clothing.Layer.WEAPON, interframe, args.IncreaseOrderInLayer (1), false);
				}
				else
				{
					equips.draw (EquipSlot.Id.WEAPON, interstance, Clothing.Layer.WEAPON, interframe, args.IncreaseOrderInLayer (1), false);
					body.draw (interstance, Body.Layer.ARM, interframe, args.IncreaseOrderInLayer (1), false);
					equips.draw (EquipSlot.Id.TOP, interstance, Clothing.Layer.MAILARM, interframe, args.IncreaseOrderInLayer (1), false);
				}

				equips.draw (EquipSlot.Id.GLOVES, interstance, Clothing.Layer.WRIST, interframe, args.IncreaseOrderInLayer (1), false);
				equips.draw (EquipSlot.Id.GLOVES, interstance, Clothing.Layer.GLOVE, interframe, args.IncreaseOrderInLayer (1), false);
				equips.draw (EquipSlot.Id.WEAPON, interstance, Clothing.Layer.WEAPON_OVER_GLOVE, interframe, args.IncreaseOrderInLayer (1), false);

				body.draw (interstance, Body.Layer.HAND_BELOW_WEAPON, interframe, args.IncreaseOrderInLayer (1), false);

				body.draw (interstance, Body.Layer.ARM_OVER_HAIR, interframe, args.IncreaseOrderInLayer (1), false);
				body.draw (interstance, Body.Layer.ARM_OVER_HAIR_BELOW_WEAPON, interframe, args.IncreaseOrderInLayer (1), false);
				equips.draw (EquipSlot.Id.WEAPON, interstance, Clothing.Layer.WEAPON_OVER_HAND, interframe, args.IncreaseOrderInLayer (1), false);
				equips.draw (EquipSlot.Id.WEAPON, interstance, Clothing.Layer.WEAPON_OVER_BODY, interframe, args.IncreaseOrderInLayer (1), false);
				body.draw (interstance, Body.Layer.HAND_OVER_HAIR, interframe, args.IncreaseOrderInLayer (1), false);
				body.draw (interstance, Body.Layer.HAND_OVER_WEAPON, interframe, args.IncreaseOrderInLayer (1), false);

				equips.draw (EquipSlot.Id.GLOVES, interstance, Clothing.Layer.WRIST_OVER_HAIR, interframe, args.IncreaseOrderInLayer (1), false);
				equips.draw (EquipSlot.Id.GLOVES, interstance, Clothing.Layer.GLOVE_OVER_HAIR, interframe, args.IncreaseOrderInLayer (1), false);
			}
		}

		private void draw (DrawArgument args, Stance.Id interstance, Expression.Id interexpression, byte interframe, byte interexpframe)
		{
			if (lastDraw_interframe != -1)
			{
				erase (lastDraw_args, lastDraw_interstance, lastDraw_interexpression, (byte)lastDraw_interframe, lastDraw_interexpframe);
			}

			//Point<short> faceshift = drawinfo.getfacepos (interstance, interframe);
			Point<short> faceshift = drawinfo.getfacepos (interstance, interframe, flip) ?? Point<short>.zero;
			DrawArgument faceargs = args + new DrawArgument (faceshift, false, new Point<short> ());

			if (Stance.is_climbing (interstance))
			{
				body.draw (interstance, Body.Layer.BODY, interframe, args.SetOrderInLayer (256));
				equips.draw (EquipSlot.Id.GLOVES, interstance, Clothing.Layer.GLOVE, interframe, args.IncreaseOrderInLayer (1));
				equips.draw (EquipSlot.Id.SHOES, interstance, Clothing.Layer.SHOES, interframe, args.IncreaseOrderInLayer (1));
				equips.draw (EquipSlot.Id.BOTTOM, interstance, Clothing.Layer.PANTS, interframe, args.IncreaseOrderInLayer (1));
				equips.draw (EquipSlot.Id.TOP, interstance, Clothing.Layer.TOP, interframe, args.IncreaseOrderInLayer (1));
				equips.draw (EquipSlot.Id.TOP, interstance, Clothing.Layer.MAIL, interframe, args.IncreaseOrderInLayer (1));
				equips.draw (EquipSlot.Id.CAPE, interstance, Clothing.Layer.CAPE, interframe, args.IncreaseOrderInLayer (1));
				body.draw (interstance, Body.Layer.HEAD, interframe, args.IncreaseOrderInLayer (1));
				equips.draw (EquipSlot.Id.EARACC, interstance, Clothing.Layer.EARRINGS, interframe, args.IncreaseOrderInLayer (1));

				switch (equips.getcaptype ())
				{
					case CharEquips.CapType.NONE:
						hair.draw (interstance, Hair.Layer.BACK, interframe, args.IncreaseOrderInLayer (1));
						break;
					case CharEquips.CapType.HEADBAND:
						equips.draw (EquipSlot.Id.HAT, interstance, Clothing.Layer.CAP, interframe, args.IncreaseOrderInLayer (1));
						hair.draw (interstance, Hair.Layer.BACK, interframe, args.IncreaseOrderInLayer (1));
						break;
					case CharEquips.CapType.HALFCOVER:
						hair.draw (interstance, Hair.Layer.BELOWCAP, interframe, args.IncreaseOrderInLayer (1));
						equips.draw (EquipSlot.Id.HAT, interstance, Clothing.Layer.CAP, interframe, args.IncreaseOrderInLayer (1));
						break;
					case CharEquips.CapType.FULLCOVER:
						equips.draw (EquipSlot.Id.HAT, interstance, Clothing.Layer.CAP, interframe, args.IncreaseOrderInLayer (1));
						break;
				}

				equips.draw (EquipSlot.Id.SHIELD, interstance, Clothing.Layer.BACKSHIELD, interframe, args.IncreaseOrderInLayer (1));
				equips.draw (EquipSlot.Id.WEAPON, interstance, Clothing.Layer.BACKWEAPON, interframe, args.IncreaseOrderInLayer (1));
			}
			else
			{
				hair.draw (interstance, Hair.Layer.BELOWBODY, interframe, args.SetOrderInLayer (256));
				equips.draw (EquipSlot.Id.CAPE, interstance, Clothing.Layer.CAPE, interframe, args.IncreaseOrderInLayer (1));
				equips.draw (EquipSlot.Id.SHIELD, interstance, Clothing.Layer.SHIELD_BELOW_BODY, interframe, args.IncreaseOrderInLayer (1));
				equips.draw (EquipSlot.Id.WEAPON, interstance, Clothing.Layer.WEAPON_BELOW_BODY, interframe, args.IncreaseOrderInLayer (1));
				equips.draw (EquipSlot.Id.HAT, interstance, Clothing.Layer.CAP_BELOW_BODY, interframe, args.IncreaseOrderInLayer (1));
				body.draw (interstance, Body.Layer.BODY, interframe, args.IncreaseOrderInLayer (1));
				equips.draw (EquipSlot.Id.GLOVES, interstance, Clothing.Layer.WRIST_OVER_BODY, interframe, args.IncreaseOrderInLayer (1));
				equips.draw (EquipSlot.Id.GLOVES, interstance, Clothing.Layer.GLOVE_OVER_BODY, interframe, args.IncreaseOrderInLayer (1));
				equips.draw (EquipSlot.Id.SHOES, interstance, Clothing.Layer.SHOES, interframe, args.IncreaseOrderInLayer (1));
				body.draw (interstance, Body.Layer.ARM_BELOW_HEAD, interframe, args.IncreaseOrderInLayer (1));

				if (equips.has_overall ())
				{
					equips.draw (EquipSlot.Id.TOP, interstance, Clothing.Layer.MAIL, interframe, args.IncreaseOrderInLayer (1));
				}
				else
				{
					equips.draw (EquipSlot.Id.BOTTOM, interstance, Clothing.Layer.PANTS, interframe, args.IncreaseOrderInLayer (1));
					equips.draw (EquipSlot.Id.TOP, interstance, Clothing.Layer.TOP, interframe, args.IncreaseOrderInLayer (1));
				}

				body.draw (interstance, Body.Layer.ARM_BELOW_HEAD_OVER_MAIL, interframe, args.IncreaseOrderInLayer (1));
				equips.draw (EquipSlot.Id.SHIELD, interstance, Clothing.Layer.SHIELD_OVER_HAIR, interframe, args.IncreaseOrderInLayer (1));
				equips.draw (EquipSlot.Id.EARACC, interstance, Clothing.Layer.EARRINGS, interframe, args.IncreaseOrderInLayer (1));
				body.draw (interstance, Body.Layer.HEAD, interframe, args.IncreaseOrderInLayer (1));
				hair.draw (interstance, Hair.Layer.SHADE, interframe, args.IncreaseOrderInLayer (1));
				hair.draw (interstance, Hair.Layer.DEFAULT, interframe, args.IncreaseOrderInLayer (1));
				face.draw (interexpression, interexpframe, faceargs.SetOrderInLayer (args.orderInLayer).IncreaseOrderInLayer (1));
				equips.draw (EquipSlot.Id.FACE, interstance, Clothing.Layer.FACEACC, 0, faceargs.SetOrderInLayer (args.orderInLayer).IncreaseOrderInLayer (1));
				equips.draw (EquipSlot.Id.EYEACC, interstance, Clothing.Layer.EYEACC, interframe, args.IncreaseOrderInLayer (1));
				equips.draw (EquipSlot.Id.SHIELD, interstance, Clothing.Layer.SHIELD, interframe, args.IncreaseOrderInLayer (1));

				switch (equips.getcaptype ())
				{
					case CharEquips.CapType.NONE:
						hair.draw (interstance, Hair.Layer.OVERHEAD, interframe, args.IncreaseOrderInLayer (1));
						break;
					case CharEquips.CapType.HEADBAND:
						equips.draw (EquipSlot.Id.HAT, interstance, Clothing.Layer.CAP, interframe, args.IncreaseOrderInLayer (1));
						hair.draw (interstance, Hair.Layer.DEFAULT, interframe, args.IncreaseOrderInLayer (1));
						hair.draw (interstance, Hair.Layer.OVERHEAD, interframe, args.IncreaseOrderInLayer (1));
						equips.draw (EquipSlot.Id.HAT, interstance, Clothing.Layer.CAP_OVER_HAIR, interframe, args.IncreaseOrderInLayer (1));
						break;
					case CharEquips.CapType.HALFCOVER:
						hair.draw (interstance, Hair.Layer.DEFAULT, interframe, args.IncreaseOrderInLayer (1));
						equips.draw (EquipSlot.Id.HAT, interstance, Clothing.Layer.CAP, interframe, args.IncreaseOrderInLayer (1));
						break;
					case CharEquips.CapType.FULLCOVER:
						equips.draw (EquipSlot.Id.HAT, interstance, Clothing.Layer.CAP, interframe, args.IncreaseOrderInLayer (1));
						break;
				}

				equips.draw (EquipSlot.Id.WEAPON, interstance, Clothing.Layer.WEAPON_BELOW_ARM, interframe, args.IncreaseOrderInLayer (1));
				bool twohanded = is_twohanded (interstance);

				if (twohanded)
				{
					equips.draw (EquipSlot.Id.TOP, interstance, Clothing.Layer.MAILARM, interframe, args.IncreaseOrderInLayer (1));
					body.draw (interstance, Body.Layer.ARM, interframe, args.IncreaseOrderInLayer (1));
					equips.draw (EquipSlot.Id.WEAPON, interstance, Clothing.Layer.WEAPON, interframe, args.IncreaseOrderInLayer (1));
				}
				else
				{
					equips.draw (EquipSlot.Id.WEAPON, interstance, Clothing.Layer.WEAPON, interframe, args.IncreaseOrderInLayer (1));
					body.draw (interstance, Body.Layer.ARM, interframe, args.IncreaseOrderInLayer (1));
					equips.draw (EquipSlot.Id.TOP, interstance, Clothing.Layer.MAILARM, interframe, args.IncreaseOrderInLayer (1));
				}

				equips.draw (EquipSlot.Id.GLOVES, interstance, Clothing.Layer.WRIST, interframe, args.IncreaseOrderInLayer (1));
				equips.draw (EquipSlot.Id.GLOVES, interstance, Clothing.Layer.GLOVE, interframe, args.IncreaseOrderInLayer (1));
				equips.draw (EquipSlot.Id.WEAPON, interstance, Clothing.Layer.WEAPON_OVER_GLOVE, interframe, args.IncreaseOrderInLayer (1));

				body.draw (interstance, Body.Layer.HAND_BELOW_WEAPON, interframe, args.IncreaseOrderInLayer (1));

				body.draw (interstance, Body.Layer.ARM_OVER_HAIR, interframe, args.IncreaseOrderInLayer (1));
				body.draw (interstance, Body.Layer.ARM_OVER_HAIR_BELOW_WEAPON, interframe, args.IncreaseOrderInLayer (1));
				equips.draw (EquipSlot.Id.WEAPON, interstance, Clothing.Layer.WEAPON_OVER_HAND, interframe, args.IncreaseOrderInLayer (1));
				equips.draw (EquipSlot.Id.WEAPON, interstance, Clothing.Layer.WEAPON_OVER_BODY, interframe, args.IncreaseOrderInLayer (1));
				body.draw (interstance, Body.Layer.HAND_OVER_HAIR, interframe, args.IncreaseOrderInLayer (1));
				body.draw (interstance, Body.Layer.HAND_OVER_WEAPON, interframe, args.IncreaseOrderInLayer (1));

				equips.draw (EquipSlot.Id.GLOVES, interstance, Clothing.Layer.WRIST_OVER_HAIR, interframe, args.IncreaseOrderInLayer (1));
				equips.draw (EquipSlot.Id.GLOVES, interstance, Clothing.Layer.GLOVE_OVER_HAIR, interframe, args.IncreaseOrderInLayer (1));
			}

			lastDraw_args = args;
			lastDraw_interstance = interstance;
			lastDraw_interexpression = interexpression;
			lastDraw_interframe = (sbyte)interframe;
			lastDraw_interexpframe = interexpframe;
		}

		private ushort get_delay (Stance.Id st, byte fr)
		{
			return drawinfo.get_delay (st, fr);
		}

		private byte getnextframe (Stance.Id st, byte fr)
		{
			return drawinfo.nextframe (st, fr);
		}
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: Stance::Id getattackstance(byte attack, bool degenerate) const

		private List<Stance.Id>[] degen_stances =
		{
			new List<Stance.Id> {Stance.Id.NONE},
			new List<Stance.Id> {Stance.Id.NONE},
			new List<Stance.Id> {Stance.Id.NONE},
			new List<Stance.Id> {Stance.Id.SWINGT1, Stance.Id.SWINGT3},
			new List<Stance.Id> {Stance.Id.SWINGT1, Stance.Id.STABT1},
			new List<Stance.Id> {Stance.Id.NONE},
			new List<Stance.Id> {Stance.Id.NONE},
			new List<Stance.Id> {Stance.Id.SWINGT1, Stance.Id.STABT1},
			new List<Stance.Id> {Stance.Id.NONE},
			new List<Stance.Id> {Stance.Id.SWINGP1, Stance.Id.STABT2}
		};

		private List<Stance.Id>[] attack_stances =
		{
			new List<Stance.Id> {Stance.Id.NONE},
			new List<Stance.Id> {Stance.Id.STABO1, Stance.Id.STABO2, Stance.Id.SWINGO1, Stance.Id.SWINGO2, Stance.Id.SWINGO3},
			new List<Stance.Id> {Stance.Id.STABT1, Stance.Id.SWINGP1},
			new List<Stance.Id> {Stance.Id.SHOOT1},
			new List<Stance.Id> {Stance.Id.SHOOT2},
			new List<Stance.Id> {Stance.Id.STABO1, Stance.Id.STABO2, Stance.Id.SWINGT1, Stance.Id.SWINGT2, Stance.Id.SWINGT3},
			new List<Stance.Id> {Stance.Id.SWINGO1, Stance.Id.SWINGO2},
			new List<Stance.Id> {Stance.Id.SWINGO1, Stance.Id.SWINGO2},
			new List<Stance.Id> {Stance.Id.NONE},
			new List<Stance.Id> {Stance.Id.SHOT}
		};

		private Stance.Id getattackstance (byte attack, bool degenerate)
		{
			if (stance == Stance.Id.PRONE)
			{
				return Stance.Id.PRONESTAB;
			}

			if (attack <= (int)Attack.NONE || attack >= (int)Attack.NUM_ATTACKS)
			{
				return Stance.Id.STAND1;
			}

			var stances = degenerate ? degen_stances[attack] : attack_stances[attack];

			if (stances.Count == 0)
			{
				return Stance.Id.STAND1;
			}

			int index = randomizer.next_int (stances.Count);

			return stances[(int)index];
		}

		private enum Attack
		{
			NONE = 0,
			S1A1M1D = 1,
			SPEAR = 2,
			BOW = 3,
			CROSSBOW = 4,
			S2A2M2 = 5,
			WAND = 6,
			CLAW = 7,
			GUN = 9,
			NUM_ATTACKS
		}

		private Nominal<Stance.Id> stance = new Nominal<Stance.Id> ();
		private Nominal<byte> stframe = new Nominal<byte> ();
		private ushort stelapsed;

		private Nominal<Expression.Id> expression = new Nominal<Expression.Id> ();
		private Nominal<byte> expframe = new Nominal<byte> ();
		private ushort expelapsed;
		private TimedBool expcooldown = new TimedBool ();

		private bool flip;

		private BodyAction action;
		private string actionstr;
		private byte actframe;

		private Body body;
		private Hair hair;
		private Face face;
		private CharEquips equips = new CharEquips ();

		private Randomizer randomizer = new Randomizer ();
		private TimedBool alerted = new TimedBool ();

		private static BodyDrawInfo drawinfo = new BodyDrawInfo ();
		private static Dictionary<int, Hair> hairstyles = new Dictionary<int, Hair> ();
		private static Dictionary<int, Face> facetypes = new Dictionary<int, Face> ();
		private static Dictionary<int, Body> bodytypes = new Dictionary<int, Body> ();
	}
}