using System;
using System.Collections.Generic;
using System.Linq;

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
	public class Textfield
	{
		public enum State
		{
			NORMAL,
			DISABLED,
			FOCUSED
		}

		public Textfield ()
		{
			text = "";
		}

		public Textfield (Text.Font font, Text.Alignment alignment, Color.Name text_color, Rectangle<short> bounds, uint limit) : this (font, alignment, text_color, text_color, 1.0f, bounds, limit)
		{
		}

		public Textfield (Text.Font font, Text.Alignment alignment, Color.Name text_color, Color.Name marker_color, float marker_opacity, Rectangle<short> bounds, uint limit)
		{
			this.bounds = bounds;
			this.limit = limit;
			textlabel = new Text (font, alignment, text_color, "", 0, false);
			marker = new ColorLine (12, marker_color, marker_opacity, true);

			text = "";
			markerpos = 0;
			crypt = 0;
			state = State.NORMAL;
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void draw(Point<short> position) const
		public void draw (Point<short> position)
		{
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: The following line was determined to be a copy constructor call - this should be verified and a copy constructor should be created if it does not yet exist:
//ORIGINAL LINE: draw(position, Point<short>(0, 0));
			draw (position, new Point<short> (0, 0));
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void draw(Point<short> position, Point<short> marker_adjust) const
		public void draw (Point<short> position, Point<short> marker_adjust)
		{
			if (state == State.DISABLED)
			{
				return;
			}

			Point<short> absp = bounds.get_left_top () + position;

			if (text.Length > 0)
			{
				textlabel.draw (absp);
			}

			if (state == State.FOCUSED && showmarker)
			{
				Point<short> mpos = absp + new Point<short> ((short)(textlabel.advance (markerpos) - 1), 8) + marker_adjust;

				if (crypt > 0)
				{
					mpos.shift (1, -3);
				}

				marker.draw (mpos);
			}
		}

		public void update (Point<short> parent)
		{
			if (state == State.DISABLED)
			{
				return;
			}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created if it does not yet exist:
//ORIGINAL LINE: parentpos = parent;
			parentpos= (parent);
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
					sbyte a = (sbyte)key;

					ss <<= a;

					add_string (ss.ToString ());
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

		public Cursor.State send_cursor (Point<short> cursorpos, bool clicked)
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

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: bool empty() const
		public bool empty ()
		{
			return string.IsNullOrEmpty (text);
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: Textfield::State get_state() const
		public Textfield.State get_state ()
		{
			return state;
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: Rectangle<short> get_bounds() const
		public Rectangle<short> get_bounds ()
		{
			return new Rectangle<short> (bounds.get_left_top () + parentpos, bounds.get_right_bottom () + parentpos);
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: const string& get_text() const
		public string get_text ()
		{
			return text;
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: bool can_copy_paste() const
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

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: bool belowlimit() const
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
		private Rectangle<short> bounds = new Rectangle<short> ();
		private Point<short> parentpos = new Point<short> ();
		private uint limit;
		private sbyte crypt;
		private State state;

		private System.Action<string> onreturn;
		private SortedDictionary<int, System.Action> callbacks = new SortedDictionary<int, System.Action> ();
		private System.Action ontext;
	}
}