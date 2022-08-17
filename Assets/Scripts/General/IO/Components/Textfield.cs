using System;
using System.Collections.Generic;
using System.Linq;
using FairyGUI;
using ms;




namespace ms
{
	public class Textfield
	{
		public enum State
		{
			NORMAL,
			DISABLED,
			FOCUSED
		}

		public bool isForbid;

		private GTextInput gTextInput;

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

		private Action<string> onreturn;

		private SortedDictionary<int, Action> callbacks = new SortedDictionary<int, Action> ();

		private Action ontext;

		public Textfield ()
		{
			text = "";
		}

		public Textfield (Text.Font font, Text.Alignment alignment, Color.Name text_color, Rectangle_short bounds, uint limit)
			: this (font, alignment, text_color, text_color, 1f, new Rectangle_short (bounds), limit)
		{
		}

		public Textfield (Text.Font font, Text.Alignment alignment, Color.Name text_color, Color.Name marker_color, float marker_opacity, Rectangle_short bounds, uint limit)
		{
			this.bounds = new Rectangle_short (bounds);
			this.limit = limit;
			marker = new ColorLine (12, marker_color, marker_opacity, vertical: true);
			text = "";
			markerpos = 0u;
			crypt = 0;
			state = State.NORMAL;
			gTextInput = new GTextInput ();
			gTextInput.border = 5;
			TextFormat textFormat = new TextFormat
			{
				size = 30
			};
			gTextInput.textFormat = textFormat;
			gTextInput.SetSize ((float)bounds.width () * Singleton<ms.Window>.Instance.ratio, (float)bounds.height () * Singleton<ms.Window>.Instance.ratio);
			//AppDebug.Log ($"bounds:{bounds} ratio:{Singleton<ms.Window>.Instance.ratio} size x:{(float)bounds.width () * Singleton<ms.Window>.Instance.ratio}");
			GRoot.inst.AddChild (gTextInput);
		}

		public void draw (Point_short position)
		{
			draw (new Point_short (position), new Point_short (0, 0));
		}

		public void draw (Point_short position, Point_short marker_adjust)
		{
			if (state != State.DISABLED && !isForbid)
			{
				Point_short absp = bounds.get_left_top () + position;
				short drawPosX = absp.x ();
				int drawPosY = -absp.y ();
				//AppDebug.Log ($"size:{gTextInput.size }");
				gTextInput.displayObject.SetPosition ((float)drawPosX * Singleton<ms.Window>.Instance.ratio, (float)(-drawPosY) * Singleton<ms.Window>.Instance.ratio, 0f);
			}
		}

		public void update (Point_short parent)
		{
			if (state != State.DISABLED && !isForbid)
			{
				parentpos = new Point_short (parent);
				elapsed += 8;
				if (elapsed > 256)
				{
					showmarker = !showmarker;
					elapsed = 0;
				}
			}
		}

		public void send_key (KeyType.Id type, int key, bool pressed)
		{
			if (!pressed)
			{
				return;
			}
			switch (type)
			{
				case KeyType.Id.ACTION:
					switch (key)
					{
						case 107:
							if (markerpos != 0)
							{
								markerpos--;
							}
							break;
						case 108:
							if (markerpos < text.Length)
							{
								markerpos++;
							}
							break;
						case 111:
							if (text.Length > 0 && markerpos != 0)
							{
								text = text.Remove ((int)(markerpos - 1), 1);
								markerpos--;
								modifytext (text);
							}
							break;
						case 113:
							if (onreturn != null)
							{
								onreturn (gTextInput.text);
							}
							break;
						case 115:
							add_string (" ");
							break;
						case 117:
							markerpos = 0u;
							break;
						case 118:
							markerpos = (uint)text.Length;
							break;
						case 116:
							if (text.Length > 0 && markerpos < text.Length)
							{
								text = text.Remove ((int)markerpos, 1);
								modifytext (text);
							}
							break;
						default:
							if (callbacks.Any ((KeyValuePair<int, Action> pair) => pair.Key == key))
							{
								callbacks[key] ();
							}
							break;
					}
					break;
				case KeyType.Id.TEXT:
					{
						if (ontext != null && (char.IsDigit ((char)key) || char.IsLetter ((char)key)))
						{
							ontext ();
							break;
						}
						int ss = 1;
						add_string (((char)key).ToString ());
						break;
					}
			}
		}

		public void add_string (string str)
		{
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
					Singleton<UI>.get ().remove_textfield ();
				}
				if (state == State.FOCUSED)
				{
					Singleton<UI>.get ().focus_textfield (this);
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
			gTextInput.displayAsPassword = true;
		}

		public void set_enter_callback (Action<string> onr)
		{
			onreturn = onr;
		}

		public void set_key_callback (KeyAction.Id key, Action action)
		{
			callbacks[(int)key] = action;
		}

		public void set_text_callback (Action action)
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
				return Cursor.State.CANCLICK;
			}
			if (clicked && state == State.FOCUSED)
			{
				set_state (State.NORMAL);
			}
			return Cursor.State.IDLE;
		}

		public bool empty ()
		{
			return string.IsNullOrEmpty (text);
		}

		public State get_state ()
		{
			return state;
		}

		public Rectangle_short get_bounds ()
		{
			return new Rectangle_short (bounds.get_left_top () + parentpos, bounds.get_right_bottom () + parentpos);
		}

		public string get_text ()
		{
			return gTextInput.text;
		}

		public bool can_copy_paste ()
		{
			if (ontext != null)
			{
				ontext ();
				return false;
			}
			return true;
		}

		private void modifytext (string t)
		{
			gTextInput.text = t;
		}

		private bool belowlimit ()
		{
			return false;
		}

		public void OnActivityChange (bool isActive)
		{
			if (gTextInput != null)
			{
				gTextInput.visible = isActive;
			}
		}
	}

}