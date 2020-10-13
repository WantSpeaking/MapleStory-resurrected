#define USE_NX

using System;

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
	public class PetLook
	{
		public enum Stance : byte
		{
			MOVE,
			STAND,
			JUMP,
			ALERT,
			PRONE,
			FLY,
			HANG,
			WARP,
			LENGTH
		}

		public static Stance stancebyvalue (byte value)
		{
			byte valueh = (byte)(value / 2);

			return valueh >= ((int)Stance.LENGTH) ? Stance.STAND : (Stance)valueh;
		}

		public PetLook (int iid, string nm, int uqid, Point<short> pos, byte st, int fhid)
		{
			itemid = iid;
			name = nm;
			uniqueid = uqid;

			set_position (pos.x (), pos.y ());
			set_stance (st);

			//namelabel = new Text(Text.Font.A13M, Text.Alignment.CENTER, Color.Name.WHITE, Text.Background.NAMETAG, name);

			string strid = Convert.ToString (iid);
			var src = nl.nx.wzFile_item["Pet"][strid + ".img"];

			animations[Stance.MOVE] = new Animation (src["move"]);
			animations[Stance.STAND] = new Animation (src["stand0"]);
			animations[Stance.JUMP] = new Animation (src["jump"]);
			animations[Stance.ALERT] = new Animation (src["alert"]);
			animations[Stance.PRONE] = new Animation (src["prone"]);
			animations[Stance.FLY] = new Animation (src["fly"]);
			animations[Stance.HANG] = new Animation (src["hang"]);

			var effsrc = nl.nx.wzFile_effect["PetEff.img"][strid];

			animations[Stance.WARP] = new Animation (effsrc["warp"]);
		}

		public PetLook ()
		{
			itemid = 0;
			name = "";
			uniqueid = 0;
			stance = Stance.STAND;
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void draw(double viewx, double viewy, float alpha) const
		public void draw (double viewx, double viewy, float alpha)
		{
			Point<short> absp = phobj.get_absolute (viewx, viewy, alpha);

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: The following line was determined to be a copy constructor call - this should be verified and a copy constructor should be created if it does not yet exist:
//ORIGINAL LINE: animations[stance].draw(DrawArgument(absp, flip), alpha);
			animations[stance].draw (new DrawArgument (absp, flip), alpha);
			//namelabel.draw (absp);
		}

		public void update (Physics physics, Point<short> charpos)
		{
			const double PETWALKFORCE = 0.35;
			const double PETFLYFORCE = 0.2;

			Point<short> curpos = phobj.get_position ();

			switch (stance)
			{
				case Stance.STAND:
				case Stance.MOVE:
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: The following line was determined to be a copy constructor call - this should be verified and a copy constructor should be created if it does not yet exist:
//ORIGINAL LINE: if (curpos.distance(charpos) > 150)
					if (curpos.distance (charpos) > 150)
					{
						set_position (charpos.x (), charpos.y ());
					}
					else
					{
						if (charpos.x () - curpos.x () > 50)
						{
							phobj.hforce = PETWALKFORCE;
							flip = true;

							set_stance (Stance.MOVE);
						}
						else if (charpos.x () - curpos.x () < -50)
						{
							phobj.hforce = -PETWALKFORCE;
							flip = false;

							set_stance (Stance.MOVE);
						}
						else
						{
							phobj.hforce = 0.0;

							set_stance (Stance.STAND);
						}
					}

					phobj.type = PhysicsObject.Type.NORMAL;
					phobj.clear_flag (PhysicsObject.Flag.NOGRAVITY);
					break;
				case Stance.HANG:
					set_position (charpos.x (), charpos.y ());
					phobj.set_flag (PhysicsObject.Flag.NOGRAVITY);
					break;
				case Stance.FLY:
					if ((charpos - curpos).length () > 250)
					{
						set_position (charpos.x (), charpos.y ());
					}
					else
					{
						if (charpos.x () - curpos.x () > 50)
						{
							phobj.hforce = PETFLYFORCE;
							flip = true;
						}
						else if (charpos.x () - curpos.x () < -50)
						{
							phobj.hforce = -PETFLYFORCE;
							flip = false;
						}
						else
						{
							phobj.hforce = 0.0f;
						}

						if (charpos.y () - curpos.y () > 50.0f)
						{
							phobj.vforce = PETFLYFORCE;
						}
						else if (charpos.y () - curpos.y () < -50.0f)
						{
							phobj.vforce = -PETFLYFORCE;
						}
						else
						{
							phobj.vforce = 0.0f;
						}
					}

					phobj.type = PhysicsObject.Type.FLYING;
					phobj.clear_flag (PhysicsObject.Flag.NOGRAVITY);
					break;
			}

			physics.move_object (phobj);

			animations[stance].update ();
		}

		public void set_position (short x, short y)
		{
			phobj.set_x (x);
			phobj.set_y (y);
		}

		public void set_stance (Stance st)
		{
			if (stance != st)
			{
				stance = st;
				animations[stance].reset ();
			}
		}

		public void set_stance (byte stancebyte)
		{
			flip = stancebyte % 2 == 1;
			stance = stancebyvalue (stancebyte);
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: int get_itemid() const
		public int get_itemid ()
		{
			return itemid;
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: PetLook::Stance get_stance() const
		public PetLook.Stance get_stance ()
		{
			return stance;
		}

		private int itemid;
		private string name;
		private int uniqueid;
		private Stance stance;
		private bool flip;

		private EnumMap<Stance, Animation> animations = new EnumMap<Stance, Animation> ();

		private PhysicsObject phobj = new PhysicsObject ();
		//private Text namelabel = new Text();
	}
}

#if USE_NX
#endif