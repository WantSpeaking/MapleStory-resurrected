using System;
using Helper;
using MapleLib.WzLib;




namespace ms
{
	// Class that represents the mouse cursor
	public class Cursor : IDisposable
	{
		// Maple cursor states that are linked to the cursor's animation
		public enum State
		{
			IDLE,
			CANCLICK,
			GAME,
			HOUSE,
			CANCLICK2,
			CANGRAB,
			GIFT,
			VSCROLL,
			HSCROLL,
			VSCROLLIDLE,
			HSCROLLIDLE,
			GRABBING,
			CLICKING,
			RCLICK,
	/*		LEAF = 18,
			CHATBARVDRAG = 67,
			CHATBARHDRAG,
			CHATBARBLTRDRAG,
			CHATBARMOVE = 72,
			CHATBARBRTLDRAG,*/
		}

		public Cursor ()
		{
			state = Cursor.State.IDLE;
			hide_counter = 0;
		}

		public void init ()
		{
			WzObject src = ms.wz.wzFile_ui["Basic.img"]["Cursor"];
			var count = animations.Count;
			foreach (State stateKey in Enum.GetValues (typeof (State)))
			{
				animations[stateKey] = src[((int)stateKey).ToString ()];
			}

			/*foreach (var iter in animations)
			{
				animations[iter.Key] = src[iter.Key.ToString()];
				//iter.Value = src[iter.Key];
			}*/
		}

		public void draw (float alpha)
		{
			const long HIDE_AFTER = HIDE_TIME / Constants.TIMESTEP;

			if (hide_counter < HIDE_AFTER)
			{
				animations[state]?.draw (position, alpha);
			}
		}

		public void update ()
		{
			animations[state]?.update ();

			switch (state)
			{
				case Cursor.State.CANCLICK:
				case Cursor.State.CANCLICK2:
				case Cursor.State.CANGRAB:
				case Cursor.State.CLICKING:
				case Cursor.State.GRABBING:
					hide_counter = 0;
					break;
				default:
					hide_counter++;
					break;
			}
		}

		public void set_state (State s)
		{
			if (state != s)
			{
				state = s;

				animations[state]?.reset ();
				hide_counter = 0;
			}
		}

		public void set_position (Point_short pos)
		{
			position = new Point_short (pos);
			hide_counter = 0;
		}

		public Cursor.State get_state ()
		{
			return state;
		}

		public Point_short get_position ()
		{
			return position;
		}

        public void Dispose()
        {
			foreach (var pair in animations)
			{
				pair.Value?.Dispose();
            }
        }

        private EnumMap<State, Animation> animations = new EnumMap<State, Animation> ();

		private State state;
		private Point_short position = new Point_short ();
		private int hide_counter;

		private const long HIDE_TIME = 15_000;
	}
}
