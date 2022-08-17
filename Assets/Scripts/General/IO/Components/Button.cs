


using System;

namespace ms
{
	// Base class for different button types.
	public abstract class Button : System.IDisposable
	{
		public enum State
		{
			NORMAL,
			DISABLED,
			MOUSEOVER,
			PRESSED = 3,
			DOWN = 3,
			IDENTITY,
			//DOWN,
			//UP,
			NUM_STATES
		}

		public virtual void Dispose ()
		{
		}

		public abstract void draw (Point_short parentpos);
		public abstract void update ();
		public abstract Rectangle_short bounds (Point_short parentpos);
		public abstract short width ();
		public abstract Point_short origin ();
		public abstract Cursor.State send_cursor (bool clicked, Point_short cursorpos);

		public virtual bool in_combobox (Point_short cursorpos)
		{
			return false;
		}

		public virtual ushort get_selected ()
		{
			return 0;
		}

		public void set_position (Point_short pos)
		{
			position = new Point_short (pos);
		}

		public void set_state (State s)
		{
			if (s == Button.State.IDENTITY)
			{
				return;
			}

			state = s;
		}

		public void set_active (bool a)
		{
			active = a;
		}

		public void toggle_pressed ()
		{
			pressed = !pressed;
		}

		public bool is_active ()
		{
			return active && state != Button.State.DISABLED;
		}

		public Button.State get_state ()
		{
			return state;
		}

		public bool is_pressed ()
		{
			return pressed;
		}

		protected State state;
		protected Point_short position = new Point_short ();
		protected bool active;
		protected bool pressed;

		public bool is_PlaySound = true;

		public enum EventState
		{
			UP,
			DOWN,
			MOUSEOVER,
		}
		public EventState eventState { get; set; }

		public event EventHandler<object> OnDown;
		public event EventHandler<object> OnUp;
		public event EventHandler<object> OnClick;
		public event EventHandler<object> OnPressing;
		public event EventHandler<object> OnRollOver;
		public event EventHandler<object> OnRollIn;
		public event EventHandler<object> OnRollOut;

		public void Down(object sender,object data)
        {
			OnDown?.Invoke(sender, data);
		}
		public void Up(object sender, object data)
		{
			OnUp?.Invoke(sender, data);
		}
		public void Click(object sender, object data)
		{
			OnClick?.Invoke(sender, data);
		}
		public void Pressing(object sender, object data)
		{
			OnPressing?.Invoke(sender, data);
		}
		public void RollOver(object sender, object data)
		{
			OnRollOver?.Invoke(sender, data);
		}
		public void RollIn(object sender, object data)
		{
			OnRollIn?.Invoke(sender, data);
		}
		public void RollOut(object sender, object data)
		{
			OnRollOut?.Invoke(sender, data);
		}

		public void playSound(Sound.Name soundName)
        {
            if (is_PlaySound)
            {
				new Sound(soundName).play();
			}
        }
	}
}