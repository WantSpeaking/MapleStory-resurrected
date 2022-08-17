#define USE_NX

using System;




namespace ms
{
	public class Icon : IDisposable
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

		public virtual void Dispose ()
		{
		}

		public abstract class Type : System.IDisposable
		{
			public virtual void Dispose ()
			{
			}

			public abstract void drop_on_stage ();

			public abstract void drop_on_equips (EquipSlot.Id eqslot);

			public abstract bool drop_on_items (InventoryType.Id tab, EquipSlot.Id eqslot, short slot, bool equip);

			public abstract void drop_on_bindings (Point_short cursorposition, bool remove);
			public abstract void set_count (short NamelessParameter);
			public abstract IconType get_type ();
		}

		public class NullType : Type
		{
			public override void drop_on_stage ()
			{
			}

			public override void drop_on_equips (EquipSlot.Id UnnamedParameter1)
			{
			}

			public override bool drop_on_items (InventoryType.Id UnnamedParameter1, EquipSlot.Id UnnamedParameter2, short UnnamedParameter3, bool UnnamedParameter4)
			{
				return true;
			}

			public override void drop_on_bindings (Point_short UnnamedParameter1, bool UnnamedParameter2)
			{
			}

			public override void set_count (short UnnamedParameter1)
			{
			}

			public override IconType get_type ()
			{
				return IconType.NONE;
			}
		}

		public Icon (Type t, Texture tx, short c)
		{
			this.type = t;
			this.texture = new ms.Texture (tx);
			nTexture = new FairyGUI.NTexture (texture.texture2D);
			this.count = c;
			texture.shift (new Point_short (0, 32));
			showcount = c > -1;
			dragged = false;
		}

		public Icon () : this (new NullType (), new Texture (), -1)
		{
		}

		public Icon (Icon src) : this (src.type, src.texture, src.count)//todo 2 type is ptr which doesn't need new 
		{
		}

		public void drop_on_stage ()
		{
			type.drop_on_stage ();
		}

		public void drop_on_equips (EquipSlot.Id eqslot)
		{
			type.drop_on_equips (eqslot);
		}

		public bool drop_on_items (InventoryType.Id tab, EquipSlot.Id eqslot, short slot, bool equip)
		{
			bool remove_icon = type.drop_on_items (tab, eqslot, slot, equip);

			if (remove_icon)
			{
				new Sound (Sound.Name.DRAGEND).play ();
			}

			return remove_icon;
		}

		public void drop_on_bindings (Point_short cursorposition, bool remove)
		{
			type.drop_on_bindings (cursorposition, remove);
		}

		public void set_count (short c)
		{
			count = c;
			type.set_count (c);
		}

		public Icon.IconType get_type ()
		{
			return type.get_type ();
		}

		public void draw(DrawArgument args)
		{
			get_texture().draw(args);

			if (showcount)
			{
				countset.draw(Convert.ToString(count), args + new Point_short(0, 20));
			}
		}

		public void draw (Point_short position)
		{
			float opacity = dragged ? 0.5f : 1.0f;
			get_texture ().draw (new DrawArgument (new Point_short (position), opacity));

			if (showcount)
			{
				countset.draw (Convert.ToString (count), position + new Point_short (0, 20));
			}
		}

		public void dragdraw (Point_short cursorpos)
		{
			if (dragged)
			{
				get_texture ().draw (new DrawArgument (cursorpos - cursoroffset, 0.5f));
				//GraphicsGL.Instance.Batch.DrawString(Text.base_Font, $"{cursorpos}", new Microsoft.Xna.Framework.Vector2(cursorpos.x()- cursoroffset.x(), cursorpos.y()- cursoroffset.y()), Microsoft.Xna.Framework.Color.Black);
			}
		}

		public void start_drag (Point_short offset)
		{
			cursoroffset = new Point_short (offset);
			dragged = true;

			new Sound (Sound.Name.DRAGSTART).play ();
		}

		public void reset ()
		{
			dragged = false;
		}

		// Allows for Icon extensibility
		// Use this instead of referencing texture directly
		public virtual Texture get_texture ()
		{
			return texture;
		}

		public short get_count ()
		{
			return count;
		}

		public bool get_drag ()
		{
			return dragged;
		}

		public Type type;
		public bool showcount;
		public short count;

		public Texture texture;
		private bool dragged;
		private Point_short cursoroffset = new Point_short ();
		
		static Charset countset = new Charset (ms.wz.wzFile_ui["Basic.img"]["ItemNo"], Charset.Alignment.LEFT);

		public FairyGUI.NTexture nTexture;

	}
}


#if USE_NX
#endif