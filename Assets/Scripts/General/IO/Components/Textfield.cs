using System;
using System.Collections.Generic;
using System.Linq;




namespace ms
{
	public class Textfield
	{
		public bool isForbid;
		public enum State
		{
			NORMAL,
			DISABLED,
			FOCUSED,
		}

		public Textfield ()
		{
			text = "";
		}

		public Textfield (Text.Font font, Text.Alignment alignment, Color.Name text_color, Rectangle_short bounds, uint limit) : this (font, alignment, text_color, text_color, 1.0f, new Rectangle_short (bounds) , limit)
		{
		}

		public Textfield (Text.Font font, Text.Alignment alignment, Color.Name text_color, Color.Name marker_color, float marker_opacity, Rectangle_short bounds, uint limit)
		{
			this.bounds = new Rectangle_short (bounds);
			this.limit = limit;
			textlabel = new Text (font, alignment, text_color, "", 0, false);
			marker = new ColorLine (12, marker_color, marker_opacity, true);

			text = "";
			markerpos = 0;
			crypt = 0;
			state = State.NORMAL;
		}

		public void draw (Point_short position)
		{
			draw (new Point_short (position), new Point_short (0, 0));
		}

		public void draw (Point_short position, Point_short marker_adjust)
		{
			if (state == State.DISABLED || isForbid)
			{
				return;
			}

			Point_short absp = bounds.get_left_top () + position;

			if (text.Length > 0)
			{
				textlabel.draw (absp);
			}

			if (state == State.FOCUSED && showmarker)
			{
				Point_short mpos = absp + new Point_short ((short)(textlabel.advance (markerpos) - 1), 8) + marker_adjust;

				if (crypt > 0)
				{
					mpos.shift (1, -3);
				}

				marker.draw (mpos);
			}

		}

		public void update (Point_short parent)
		{
			if (state == State.DISABLED || isForbid)
			{
				return;
			}

			parentpos= new Point_short (parent);
			elapsed += Constants.TIMESTEP;

			if (elapsed > 256)
			{
				showmarker = !showmarker;
				elapsed = 0;
			}
		}

		public void send_key (KeyType.Id type, int key, bool pressed)
		{
			if (pressed)
			{
				if (type == KeyType.Id.ACTION)
				{
					switch ((KeyAction.Id)key)
					{
						case KeyAction.Id.LEFT:
						{
							if (markerpos > 0)
							{
								markerpos--;
							}

							break;
						}
						case KeyAction.Id.RIGHT:
						{
							if (markerpos < text.Length)
							{
								markerpos++;
							}

							break;
						}
						case KeyAction.Id.BACK:
						{
							if (text.Length > 0 && markerpos > 0)
							{
								text = text.Remove ((int)(markerpos - 1), 1);

								markerpos--;

								modifytext (text);
							}

							break;
						}
						case KeyAction.Id.RETURN:
						{
							if (onreturn != null)
							{
								onreturn (text);
							}

							break;
						}
						case KeyAction.Id.SPACE:
						{
							add_string (" ");
							break;
						}
						case KeyAction.Id.HOME:
						{
							markerpos = 0;
							break;
						}
						case KeyAction.Id.END:
						{
							markerpos = (uint)text.Length;
							break;
						}
						case KeyAction.Id.DELETE:
						{
							if (text.Length > 0 && markerpos < text.Length)
							{
								text = text.Remove ((int)markerpos, 1);

								modifytext (text);
							}

							break;
						}
						default:
						{
							if (callbacks.Any (pair => pair.Key == key))
							{
								callbacks[key] ();
							}

							break;
						}
					}
				}
				else if (type == KeyType.Id.TEXT)
				{
					if (ontext != null)
					{
						if (char.IsDigit ((char)key) || char.IsLetter ((char)key))
						{
							ontext ();
							return;
						}
					}

					int ss = 1;
					//stringstream ss = new stringstream ();
					var a = (char)key;

					//ss <<= a;

					add_string (a.ToString ());
				}
			}
		}

		public void add_string (string str)
		{
			foreach (var c in str)
			{
				if (belowlimit ())
				{
					text = text.Insert ((int)markerpos, c.ToString ());

					markerpos++;

					modifytext (text);
				}
			}
		}

		public void set_state (State st)
		{
			if (state != st)
			{
				state = st;

				if (state != State.DISABLED)
				{
					elapsed = 0;
					showmarker = true;
				}
				else
				{
					UI.get ().remove_textfield ();
				}

				if (state == State.FOCUSED)
				{
					UI.get ().focus_textfield (this);
				}
			}
		}

		public void change_text (string t)
		{
			modifytext (t);

			markerpos = (uint)text.Length;
		}

		public void set_cryptchar (sbyte character)
		{
			crypt = character;
		}

		public void set_enter_callback (System.Action<string> onr)
		{
			onreturn = onr;
		}

		public void set_key_callback (KeyAction.Id key, System.Action action)
		{
			callbacks[(int)key] = action;
		}

		public void set_text_callback (System.Action action)
		{
			ontext = action;
		}

		public Cursor.State send_cursor (Point_short cursorpos, bool clicked)
		{
			if (state == State.DISABLED)
			{
				return Cursor.State.IDLE;
			}

			if (get_bounds ().contains (cursorpos))
			{
				if (clicked)
				{
					if (state == State.NORMAL)
					{
						set_state (State.FOCUSED);
					}

					return Cursor.State.CLICKING;
				}
				else
				{
					return Cursor.State.CANCLICK;
				}
			}
			else
			{
				if (clicked && state == State.FOCUSED)
				{
					set_state (State.NORMAL);
				}

				return Cursor.State.IDLE;
			}
		}

		public bool empty ()
		{
			return string.IsNullOrEmpty (text);
		}

		public Textfield.State get_state ()
		{
			return state;
		}

		public Rectangle_short get_bounds ()
		{
			return new Rectangle_short (bounds.get_left_top () + parentpos, bounds.get_right_bottom () + parentpos);
		}

		public string get_text ()
		{
			return text;
		}

		public bool can_copy_paste ()
		{
			if (ontext != null)
			{
				ontext ();

				return false;
			}
			else
			{
				return true;
			}
		}

		private void modifytext (string t)
		{
			if (crypt > 0)
			{
				string crypted = String.Empty;
				crypted = crypted.insert (0, t.Length, (char)crypt);

				textlabel.change_text (crypted);
			}
			else
			{
				textlabel.change_text (t);
			}

			text = t;
		}

		private bool belowlimit ()
		{
			if (limit > 0)
			{
				return text.Length < limit;
			}
			else
			{
				ushort advance = textlabel.advance ((uint)text.Length);

				return (advance + 50) < bounds.get_horizontal ().length ();
			}
		}

		private Text textlabel = new Text ();
		private string text;
		private ColorLine marker = new ColorLine ();
		private bool showmarker;
		private ushort elapsed;
		private uint markerpos;
		private Rectangle_short bounds = new Rectangle_short ();
		private Point_short parentpos = new Point_short ();
		private uint limit;
		private sbyte crypt;
		private State state;

		private System.Action<string> onreturn;
		private SortedDictionary<int, System.Action> callbacks = new SortedDictionary<int, System.Action> ();
		private System.Action ontext;
	}
}