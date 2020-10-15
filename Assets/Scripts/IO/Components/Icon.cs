/*
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
	public class Icon
	{
		public enum IconType : byte
		{
			NONE,
			SKILL,
			EQUIP,
			ITEM,
			KEY,
			NUM_TYPES
		}

		public abstract class Type : System.IDisposable
		{
			public virtual void Dispose()
			{
			}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: virtual void drop_on_stage() const = 0;
			public abstract void drop_on_stage();
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: virtual void drop_on_equips(EquipSlot::Id eqslot) const = 0;
			public abstract void drop_on_equips(EquipSlot.Id eqslot);
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: virtual bool drop_on_items(InventoryType::Id tab, EquipSlot::Id eqslot, short slot, bool equip) const = 0;
			public abstract bool drop_on_items(InventoryType.Id tab, EquipSlot.Id eqslot, short slot, bool equip);
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: virtual void drop_on_bindings(Point<short> cursorposition, bool remove) const = 0;
			public abstract void drop_on_bindings(Point<short> cursorposition, bool remove);
			public abstract void set_count(short NamelessParameter);
			public abstract IconType get_type();
		}

		public class NullType : Type
		{
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void drop_on_stage() const override
			private override void drop_on_stage()
			{
			}
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void drop_on_equips(EquipSlot::Id) const override
			private override void drop_on_equips(EquipSlot.Id UnnamedParameter1)
			{
			}
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: bool drop_on_items(InventoryType::Id, EquipSlot::Id, short, bool) const override
			private override bool drop_on_items(InventoryType.Id UnnamedParameter1, EquipSlot.Id UnnamedParameter2, short UnnamedParameter3, bool UnnamedParameter4)
			{
				return true;
			}
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void drop_on_bindings(Point<short>, bool) const override
			private override void drop_on_bindings(Point<short> UnnamedParameter1, bool UnnamedParameter2)
			{
			}
			private override void set_count(short UnnamedParameter1)
			{
			}
			private override IconType get_type()
			{
				return IconType.NONE;
			}
		}

		public Icon(std::unique_ptr<Type> t, Texture tx, short c)
		{
			this.type = std::move(t);
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: The following line was determined to be a copy constructor call - this should be verified and a copy constructor should be created if it does not yet exist:
//ORIGINAL LINE: this.texture = new ms.Texture(tx);
			this.texture = new ms.Texture(new ms.Texture(tx));
			this.count = c;
			texture.shift(new Point<short>(0, 32));
			showcount = c > -1;
			dragged = false;
		}
		public Icon() : this(std::make_unique<NullType>(), {}, -1)
		{
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void drop_on_stage() const
		public void drop_on_stage()
		{
			type.drop_on_stage();
		}
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void drop_on_equips(EquipSlot::Id eqslot) const
		public void drop_on_equips(EquipSlot.Id eqslot)
		{
			type.drop_on_equips(eqslot);
		}
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: bool drop_on_items(InventoryType::Id tab, EquipSlot::Id eqslot, short slot, bool equip) const
		public bool drop_on_items(InventoryType.Id tab, EquipSlot.Id eqslot, short slot, bool equip)
		{
			bool remove_icon = type.drop_on_items(tab, eqslot, slot, equip);

			if (remove_icon)
			{
				Sound(Sound.Name.DRAGEND).play();
			}

			return remove_icon;
		}
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void drop_on_bindings(Point<short> cursorposition, bool remove) const
		public void drop_on_bindings(Point<short> cursorposition, bool remove)
		{
			type.drop_on_bindings(cursorposition, remove);
		}
		public void set_count(short c)
		{
			count = c;
			type.set_count(c);
		}
		public Icon.IconType get_type()
		{
			return type.get_type();
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void draw(Point<short> position) const
		public void draw(Point<short> position)
		{
			float opacity = dragged ? 0.5f : 1.0f;
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: The following line was determined to be a copy constructor call - this should be verified and a copy constructor should be created if it does not yet exist:
//ORIGINAL LINE: get_texture().draw(DrawArgument(position, opacity));
			get_texture().draw(new DrawArgument(new ms.Point(new ms.Point(position)), opacity));

			if (showcount)
			{
				Charset countset = new Charset(nl.nx.ui["Basic.img"]["ItemNo"], Charset.Alignment.LEFT);
				countset.draw(Convert.ToString(count), position + new Point<short>(0, 20));
			}
		}
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void dragdraw(Point<short> cursorpos) const
		public void dragdraw(Point<short> cursorpos)
		{
			if (dragged)
			{
				get_texture().draw(new DrawArgument(cursorpos - cursoroffset, 0.5f));
			}
		}
		public void start_drag(Point<short> offset)
		{
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created if it does not yet exist:
//ORIGINAL LINE: cursoroffset = offset;
			cursoroffset.CopyFrom(offset);
			dragged = true;

			Sound(Sound.Name.DRAGSTART).play();
		}
		public void reset()
		{
			dragged = false;
		}

		// Allows for Icon extensibility
		// Use this instead of referencing texture directly
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: virtual Texture get_texture() const
		public virtual Texture get_texture()
		{
			return texture;
		}
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: short get_count() const
		public short get_count()
		{
			return count;
		}
		public bool get_drag()
		{
			return dragged;
		}

		private std::unique_ptr<Type> type = new std::unique_ptr<Type>();
		private bool showcount;
		private short count;

		private Texture texture = new Texture();
		private bool dragged;
		private Point<short> cursoroffset = new Point<short>();
	}
}



#if USE_NX
#endif
*/
