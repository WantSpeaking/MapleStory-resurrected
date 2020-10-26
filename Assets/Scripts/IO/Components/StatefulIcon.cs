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
	public class StatefulIcon : Icon
	{
		public enum State : byte
		{
			NORMAL,
			DISABLED,
			MOUSEOVER,
			LENGTH
		}

		public new abstract class Type : Icon.Type
		{
			public override void Dispose()
			{
				base.Dispose();
			}

			public abstract void set_state(State state);
		}

		public new class NullType : Type
		{
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void drop_on_stage() const override
			public override void drop_on_stage()
			{
			}
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void drop_on_equips(EquipSlot::Id) const override
			public override void drop_on_equips(EquipSlot.Id UnnamedParameter1)
			{
			}
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: bool drop_on_items(InventoryType::Id, EquipSlot::Id, short, bool) const override
			public override bool drop_on_items(InventoryType.Id UnnamedParameter1, EquipSlot.Id UnnamedParameter2, short UnnamedParameter3, bool UnnamedParameter4)
			{
				return true;
			}
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void drop_on_bindings(Point<short>, bool) const override
			public override void drop_on_bindings(Point<short> UnnamedParameter1, bool UnnamedParameter2)
			{
			}
			public override void set_count(short UnnamedParameter1)
			{
			}
			public override void set_state(State UnnamedParameter1)
			{
			}
			public override Icon.IconType get_type()
			{
				return IconType.NONE;
			}
		}

		public StatefulIcon() : this(new NullType(), new Texture(), new Texture(), new Texture())
		{
		}
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: The following line was determined to be a copy constructor call - this should be verified and a copy constructor should be created if it does not yet exist:
//ORIGINAL LINE: base(move(type), ntx, -1);
		public StatefulIcon(Type type, Texture ntx, Texture dtx, Texture motx) : base(type, ntx, -1)
		{
			ntx.shift(new Point<short>(0, 32));
			dtx.shift(new Point<short>(0, 32));
			motx.shift(new Point<short>(0, 32));

			textures[State.NORMAL] = ntx;
			textures[State.DISABLED] = dtx;
			textures[State.MOUSEOVER] = motx;

			state = State.NORMAL;
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: Texture get_texture() const override
		public override Texture get_texture()
		{
			return textures[state];
		}

		public void set_state(State s)
		{
			state = s;
		}

		private State state;
		private EnumMap<State, Texture> textures = new EnumMap<State, Texture>();
	}
}
