


namespace ms
{
	public class StatefulIcon : Icon
	{
		public enum State : byte
		{
			NORMAL,
			DISABLED,
			MOUSEOVER,
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
			public override void drop_on_stage()
			{
			}
			public override void drop_on_equips(EquipSlot.Id UnnamedParameter1)
			{
			}
			public override bool drop_on_items(InventoryType.Id UnnamedParameter1, EquipSlot.Id UnnamedParameter2, short UnnamedParameter3, bool UnnamedParameter4)
			{
				return true;
			}
			public override void drop_on_bindings(Point_short UnnamedParameter1, bool UnnamedParameter2)
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
		public StatefulIcon(Type type, Texture ntx, Texture dtx, Texture motx) : base(type, new Texture (ntx) , -1)
		{
			ntx.shift(new Point_short(0, 32));
			dtx.shift(new Point_short(0, 32));
			motx.shift(new Point_short(0, 32));

			textures[State.NORMAL] = ntx;
			textures[State.DISABLED] = dtx;
			textures[State.MOUSEOVER] = motx;

			state = State.NORMAL;
		}

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
