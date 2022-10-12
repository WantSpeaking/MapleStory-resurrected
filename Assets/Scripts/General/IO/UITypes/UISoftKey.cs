#define USE_NX

using System;
using System.Collections.Generic;
using Beebyte.Obfuscator;
using MapleLib.WzLib;




namespace ms
{
	[Skip]
	// Keyboard which is used via the mouse
	// The game uses this for PIC/PIN input
	public class UISoftKey : UIElement
	{
		public delegate void OkCallback (string entered);

		public delegate void CancelCallback ();

		public const Type TYPE = UIElement.Type.SOFTKEYBOARD;
		public const bool FOCUSED = true;
		public const bool TOGGLED = false;

		public UISoftKey (OkCallback ok_callback, CancelCallback cancel_callback, string tooltip_text, Point_short tooltip_pos) : base (new Point_short (104, 140), new Point_short (0, 0))
		{
			this.ok_callback = ok_callback;
			this.cancel_callback = cancel_callback;
			this.tooltip_pos = new Point_short (tooltip_pos);
			this.highCase = false;
			WzObject SoftKey = ms.wz.wzFile_ui["Login.img"]["Common"]["SoftKey"];
			WzObject backgrnd = SoftKey["backgrnd"];

			sprites.Add (backgrnd);

			Point_short button_adj = new Point_short (1, 0);

			buttons[(int)Buttons.BtCancel] = new MapleButton (SoftKey["BtCancel"], button_adj);
			buttons[(int)Buttons.BtDel] = new MapleButton (SoftKey["BtDel"], button_adj);
			buttons[(int)Buttons.BtOK] = new MapleButton (SoftKey["BtOK"], button_adj);
			buttons[(int)Buttons.BtShift] = new MapleButton (SoftKey["BtShift"], button_adj);

			#region BtNum

			#region Row 1

			string[] row1KeysMap = new string[ROW1_KEYS];

			row1KeysMap[0] = "1";
			row1KeysMap[1] = "2";
			row1KeysMap[2] = "3";
			row1KeysMap[3] = "4";
			row1KeysMap[4] = "5";
			row1KeysMap[5] = "6";
			row1KeysMap[6] = "7";
			row1KeysMap[7] = "8";
			row1KeysMap[8] = "9";
			row1KeysMap[9] = "0";

			var row1r1 = random.next_int (ROW_MAX);
			var row1r2 = random.next_int (ROW_MAX);

			while (row1r1 == row1r2)
			{
				row1r2 = random.next_int (ROW_MAX);
			}

			row1keys[row1r1] = "Blank";
			row1keys[row1r2] = "Blank";

			ushort keyIndex = 0;
			for (int i = 0; i < row1keys.Length; i++)
			{
				string key = row1keys[i];
				if (key != "Blank" && keyIndex < ROW1_KEYS)
				{
					row1keys[i] = row1KeysMap[keyIndex];
					keyIndex++;
				}
			}

			for (ushort i = 0; i < ROW_MAX; i++)
			{
				string key = row1keys[i];

				if (key == "Blank")
				{
					buttons[(uint)((int)Buttons.BtNum0 + i)] = new MapleButton (SoftKey["BtblankNum"], keypos (i, 0));
				}
				else
				{
					buttons[(uint)((int)Buttons.BtNum0 + i)] = new MapleButton (SoftKey["BtNum"][key], keypos (i, 0));
				}
			}

			#endregion

			#endregion

			#region BtLowCase / BtHighCase

			#region Row 2

			string[] row2KeysMap = new string[ROW2_KEYS];

			row2KeysMap[0] = get_key_map_index ("Q");
			row2KeysMap[1] = get_key_map_index ("W");
			row2KeysMap[2] = get_key_map_index ("E");
			row2KeysMap[3] = get_key_map_index ("R");
			row2KeysMap[4] = get_key_map_index ("T");
			row2KeysMap[5] = get_key_map_index ("Y");
			row2KeysMap[6] = get_key_map_index ("U");
			row2KeysMap[7] = get_key_map_index ("I");
			row2KeysMap[8] = get_key_map_index ("O");
			row2KeysMap[9] = get_key_map_index ("P");

			var row2r1 = random.next_int (ROW_MAX);
			var row2r2 = random.next_int (ROW_MAX);

			while (row2r1 == row2r2)
			{
				row2r2 = random.next_int (ROW_MAX);
			}

			row2keys[row2r1] = "Blank";
			row2keys[row2r2] = "Blank";

			keyIndex = 0;
			for (int i = 0; i < row2keys.Length; i++)
			{
				string key = row2keys[i];
				if (key != "Blank" && keyIndex < ROW2_KEYS)
				{
					row2keys[i] = row2KeysMap[keyIndex];
					keyIndex++;
				}
			}

			ushort caseKeyIndex = 0;

			for (ushort i = 0; i < ROW_MAX; i++, caseKeyIndex++)
			{
				string key = row2keys[i];

				if (key == "Blank")
				{
					BtCaseKeys[caseKeyIndex][true] = new MapleButton (SoftKey["BtblankCase"], keypos (i, 1));
					BtCaseKeys[caseKeyIndex][false] = new MapleButton (SoftKey["BtblankCase"], keypos (i, 1));
				}
				else
				{
					BtCaseKeys[caseKeyIndex][true] = new MapleButton (SoftKey["BtHighCase"][key], keypos (i, 1));
					BtCaseKeys[caseKeyIndex][false] = new MapleButton (SoftKey["BtLowCase"][key], keypos (i, 1));
				}
			}

			#endregion

			#region Row 3

			string[] row3KeysMap = new string[ROW3_KEYS];

			row3KeysMap[0] = get_key_map_index ("A");
			row3KeysMap[1] = get_key_map_index ("S");
			row3KeysMap[2] = get_key_map_index ("D");
			row3KeysMap[3] = get_key_map_index ("F");
			row3KeysMap[4] = get_key_map_index ("G");
			row3KeysMap[5] = get_key_map_index ("H");
			row3KeysMap[6] = get_key_map_index ("J");
			row3KeysMap[7] = get_key_map_index ("K");
			row3KeysMap[8] = get_key_map_index ("L");

			var row3r1 = random.next_int (ROW_MAX);
			var row3r2 = random.next_int (ROW_MAX);

			while (row3r1 == row3r2)
			{
				row3r2 = random.next_int (ROW_MAX);
			}

			var row3r3 = random.next_int (ROW_MAX);

			while (row3r1 == row3r3 || row3r2 == row3r3)
			{
				row3r3 = random.next_int (ROW_MAX);
			}

			row3keys[row3r1] = "Blank";
			row3keys[row3r2] = "Blank";
			row3keys[row3r3] = "Blank";

			keyIndex = 0;
			for (int i = 0; i < row3keys.Length; i++)
			{
				string key = row3keys[i];
				if (key != "Blank" && keyIndex < ROW3_KEYS)
				{
					row3keys[i] = row3KeysMap[keyIndex];
					keyIndex++;
				}
			}

			for (ushort i = 0; i < ROW_MAX; i++, caseKeyIndex++)
			{
				string key = row3keys[i];

				if (key == "Blank")
				{
					BtCaseKeys[caseKeyIndex][true] = new MapleButton (SoftKey["BtblankCase"], keypos (i, 2));
					BtCaseKeys[caseKeyIndex][false] = new MapleButton (SoftKey["BtblankCase"], keypos (i, 2));
				}
				else
				{
					BtCaseKeys[caseKeyIndex][true] = new MapleButton (SoftKey["BtHighCase"][key], keypos (i, 2));
					BtCaseKeys[caseKeyIndex][false] = new MapleButton (SoftKey["BtLowCase"][key], keypos (i, 2));
				}
			}

			#endregion

			#region Row 4

			string[] row4KeysMap = new string[ROW4_KEYS];

			row4KeysMap[0] = get_key_map_index ("Z");
			row4KeysMap[1] = get_key_map_index ("X");
			row4KeysMap[2] = get_key_map_index ("C");
			row4KeysMap[3] = get_key_map_index ("V");
			row4KeysMap[4] = get_key_map_index ("B");
			row4KeysMap[5] = get_key_map_index ("N");
			row4KeysMap[6] = get_key_map_index ("M");

			var row4r1 = random.next_int (ROW4_MAX);

			row4keys[row4r1] = "Blank";

			keyIndex = 0;
			for (int i = 0; i < row4keys.Length; i++)
			{
				string key = row4keys[i];
				if (key != "Blank" && keyIndex < ROW4_KEYS)
				{
					row4keys[i] = row4KeysMap[keyIndex];
					keyIndex++;
				}
			}

			for (ushort i = 0; i < ROW4_MAX; i++, caseKeyIndex++)
			{
				string key = row4keys[i];

				if (key == "Blank")
				{
					BtCaseKeys[caseKeyIndex][true] = new MapleButton (SoftKey["BtblankCase"], keypos (i, 3));
					BtCaseKeys[caseKeyIndex][false] = new MapleButton (SoftKey["BtblankCase"], keypos (i, 3));
				}
				else
				{
					BtCaseKeys[caseKeyIndex][true] = new MapleButton (SoftKey["BtHighCase"][key], keypos (i, 3));
					BtCaseKeys[caseKeyIndex][false] = new MapleButton (SoftKey["BtLowCase"][key], keypos (i, 3));
				}
			}

			#endregion

			#endregion

			Point_short textfield_tl = new Point_short (350, 205);

			textfield = new Textfield (Text.Font.A11M, Text.Alignment.LEFT, Color.Name.EMPEROR, new Rectangle_short (new Point_short (textfield_tl), textfield_tl + new Point_short (117, 20)), MAX_TEXT_LEN);
			textfield.set_cryptchar ((sbyte)'*');

			textfield.set_enter_callback ((string UnnamedParameter1) => { button_pressed ((ushort)Buttons.BtOK); });

			textfield.set_key_callback (KeyAction.Id.ESCAPE, () => { button_pressed ((ushort)Buttons.BtCancel); });

			textfield.set_text_callback (() =>
			{
				clear_tooltip ();
				show_text ("and the numbers and letters can only be entered using a mouse.", 175, true, 1);
			});

			textfield_pos = new Point_short (0, -4);

			show_text (tooltip_text);

			dimension = new Texture (backgrnd).get_dimensions ();
		}

		public UISoftKey (OkCallback ok_callback, CancelCallback cancel_callback, string tooltip_text) : this (ok_callback, cancel_callback, tooltip_text, new Point_short (0, 0))
		{
		}

		public UISoftKey (OkCallback ok_callback, CancelCallback cancel_callback) : this (ok_callback, cancel_callback, "")
		{
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: UISoftKey(OkCallback ok_callback) : UISoftKey(ok_callback, []()
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: The implementation of the following method could not be found:
//		override UISoftKey(OkCallback ok_callback) : UISoftKey(ok_callback, () => TangibleLambdaToken88void draw(float inter);
		public override void update ()
		{
			base.update ();

			textfield.update (new Point_short (textfield_pos));
			textfield.set_state (Textfield.State.FOCUSED);

			if (tooltip)
			{
				if (tooltip_timestep > 0)
				{
					tooltip_timestep -= (short)Constants.TIMESTEP;
				}
				else
				{
					clear_tooltip ();
				}
			}
		}

		public new void deactivate ()
		{
			UI.get ().remove (UIElement.Type.SOFTKEYBOARD);
		}

		public override Cursor.State send_cursor (bool clicked, Point_short cursorpos)
		{
			Cursor.State new_state = textfield.send_cursor (new Point_short (cursorpos), clicked);
			if (new_state != Cursor.State.IDLE)
			{
				return new_state;
			}

			for (ushort i = 0; i < CASE_KEYS; i++)
			{
				var btn = BtCaseKeys[i][highCase];

				if (btn.is_active () && btn.bounds (position).contains (cursorpos))
				{
					if (btn.get_state () == Button.State.NORMAL)
					{
						new Sound (Sound.Name.BUTTONOVER).play ();

						btn.set_state (Button.State.MOUSEOVER);
						return Cursor.State.CANCLICK;
					}
					else if (btn.get_state () == Button.State.MOUSEOVER)
					{
						if (clicked)
						{
							new Sound (Sound.Name.BUTTONCLICK).play ();

							btn.set_state (case_pressed (i));

							return Cursor.State.IDLE;
						}
						else
						{
							return Cursor.State.CANCLICK;
						}
					}
				}
				else if (btn.get_state () == Button.State.MOUSEOVER)
				{
					btn.set_state (Button.State.NORMAL);
				}
			}

			return base.send_cursor (clicked, new Point_short (cursorpos));
		}

		public override void send_key (int keycode, bool pressed, bool escape)
		{
			if (pressed)
			{
				if (escape)
				{
					button_pressed ((ushort)Buttons.BtCancel);
				}
				else if (keycode == (int)KeyAction.Id.RETURN)
				{
					button_pressed ((ushort)Buttons.BtOK);
				}
			}
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: UIElement::Type get_type() const override
		public override UIElement.Type get_type ()
		{
			return TYPE;
		}

		public override Button.State button_pressed (ushort buttonid)
		{
			string pic = textfield.get_text ();
			int size = pic.Length;

			if (buttonid == (int)Buttons.BtCancel)
			{
				deactivate ();

				if (cancel_callback != null)
				{
					cancel_callback ();
				}

				return Button.State.NORMAL;
			}
			else if (buttonid == (int)Buttons.BtDel)
			{
				if (size > 0)
				{
					pic.pop_back ();
					textfield.change_text (pic);
				}

				return Button.State.NORMAL;
			}
			else if (buttonid == (int)Buttons.BtOK)
			{
				if (size >= MIN_TEXT_LEN)
				{
					if (check_pic ())
					{
						deactivate ();

						if (ok_callback != null)
						{
							ok_callback (pic);
						}
					}
				}
				else
				{
					clear_tooltip ();
					show_text ("The PIC needs to be at least 6 characters long.");
				}

				return Button.State.NORMAL;
			}
			else if (buttonid == (int)Buttons.BtShift)
			{
				highCase = !highCase;

				return Button.State.NORMAL;
			}
			else if (buttonid >= (int)Buttons.BtNum0)
			{
				string key = row1keys[(int)(buttonid - Buttons.BtNum0)];

				if (key != "Blank")
				{
					append_key (key);
				}

				return Button.State.NORMAL;
			}
			else
			{
				return Button.State.DISABLED;
			}
		}

		private void show_text (string text, ushort maxwidth = 0, bool formatted = true, short line_adj = 0)
		{
			tetooltip.set_text (text, maxwidth, formatted, line_adj);

			if (!string.IsNullOrEmpty (text))
			{
				tooltip = tetooltip;
				tooltip_timestep = 7 * 1000;
			}
		}

		private void clear_tooltip ()
		{
			tooltip_pos = new Point_short (0, 0);
			tetooltip.set_text ("");
			tooltip = new Optional<Tooltip> ();
		}

		private void append_key (string key)
		{
			string pic = textfield.get_text ();
			int size = pic.Length;

			if (size < MAX_TEXT_LEN)
			{
				pic.append (key);
				textfield.change_text (pic);
			}
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: Point_short keypos(ushort index, ushort row) const
		private Point_short keypos (ushort index, ushort row)
		{
			short x = (short)(index * 45);
			short y = (short)(row * 43);

			// Third row starts at the third character position
			if (row == 3)
			{
				x += 45 * 2;
			}

			return new Point_short ((short)(27 + x), (short)(95 + y));
		}

		private Button.State case_pressed (ushort buttonid)
		{
			// Row 2
			if (buttonid >= 0 && buttonid < ROW_MAX)
			{
				string string_index = row2keys[buttonid];

				if (string_index != "Blank")
				{
					ushort index = Convert.ToUInt16 (string_index);
					string key = get_key_from_index (index);

					append_key (key);
				}

				return Button.State.NORMAL;
			}
			// Row 3
			else if (buttonid >= ROW_MAX && buttonid < ROW_MAX * 2)
			{
				string string_index = row3keys[buttonid - ROW_MAX];

				if (string_index != "Blank")
				{
					ushort index = Convert.ToUInt16 (string_index);
					string key = get_key_from_index (index);

					append_key (key);
				}

				return Button.State.NORMAL;
			}
			// Row 4
			else if (buttonid >= ROW_MAX * 2 && buttonid < ROW_MAX * 2 + ROW4_MAX)
			{
				string string_index = row4keys[buttonid - ROW_MAX * 2];

				if (string_index != "Blank")
				{
					ushort index = Convert.ToUInt16 (string_index);
					string key = get_key_from_index (index);

					append_key (key);
				}

				return Button.State.NORMAL;
			}

			return Button.State.DISABLED;
		}

		private string get_key_map_index (string key)
		{
			foreach (var map in KeyMap)
			{
				if (map.Value == key)
				{
					return Convert.ToString (map.Key);
				}
			}

			Console.Write ("Could not find index for key [");
			Console.Write (key);
			Console.Write ("] in KeyMap.");
			Console.Write ("\n");

			return "Blank";
		}

		private string get_key_from_index (ushort index)
		{
			if (KeyMap.TryGetValue (index, out var key))
			{
				return highCase ? key : string_format.tolower (key);
			}

			Console.Write ("Could not find key at index [");
			Console.Write (index);
			Console.Write ("] in KeyMap.");
			Console.Write ("\n");

			return "Blank";
		}

		private bool check_pic ()
		{
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: Pointer arithmetic is detected on this variable, so pointers on this variable are left unchanged:
			var text = textfield.get_text ();

			if (text == null)
			{
				return false;
			}

			int count = 0;
			sbyte m = (sbyte)' ';
			bool reptitive = false;

			foreach (var pStr in text)
			{
				if (pStr == m)
				{
					count++;
				}
				else
				{
					count = 0;
					m = (sbyte)pStr;
				}

				if (count > 2)
				{
					reptitive = true;
					break;
				}
			}

			if (reptitive)
			{
				clear_tooltip ();
				show_text ("Your 2nd password cannot contain three of the same character in a row.", 220, true, 1);

				return false;
			}

			bool requirements = false;

			// TODO: Add check for required amount of characters

			if (requirements)
			{
				clear_tooltip ();
				show_text ("Your 2nd password must have at least two of the following: uppercase letters, lowercase letters, numbers, and special characters.", 242, true, 1);

				return false;
			}

			return true;
		}

		private enum Buttons : ushort
		{
			BtCancel,
			BtDel,
			BtOK,
			BtShift,
			BtNum0
		}

		private const uint MIN_TEXT_LEN = 6;
		private const uint MAX_TEXT_LEN = 16;

		private const ushort ROW_MAX = 12;

		private const ushort ROW1_KEYS = 10;
		private const ushort ROW2_KEYS = 10;
		private const ushort ROW3_KEYS = 9;
		private const ushort ROW4_KEYS = 7;

		private const ushort ROW4_MAX = ROW4_KEYS + 1;
		private const ushort CASE_KEYS = ROW_MAX * 2 + ROW4_MAX;

		private bool highCase;
		private string[] row1keys = new string[ROW_MAX];
		private string[] row2keys = new string[ROW_MAX];
		private string[] row3keys = new string[ROW_MAX];
		private string[] row4keys = new string[ROW4_MAX];
		private BoolPair<Button>[] BtCaseKeys =new BoolPair<Button>[CASE_KEYS];

		private OkCallback ok_callback;
		private CancelCallback cancel_callback;

		private Textfield textfield = new Textfield ();
		private Point_short textfield_pos = new Point_short ();

		private short tooltip_timestep;
		private TextTooltip tetooltip = new TextTooltip ();
		private Optional<Tooltip> tooltip = new Optional<Tooltip> ();
		private Point_short tooltip_pos = new Point_short ();

		private Randomizer random = new Randomizer ();

		private SortedDictionary<ushort, string> KeyMap = new SortedDictionary<ushort, string> ()
		{
			{0, "A"},
			{1, "B"},
			{2, "C"},
			{3, "D"},
			{4, "E"},
			{5, "F"},
			{6, "G"},
			{7, "H"},
			{8, "I"},
			{9, "J"},
			{10, "K"},
			{11, "L"},
			{12, "M"},
			{13, "N"},
			{14, "O"},
			{15, "P"},
			{16, "Q"},
			{17, "R"},
			{18, "S"},
			{19, "T"},
			{20, "U"},
			{21, "V"},
			{22, "W"},
			{23, "X"},
			{24, "Y"},
			{25, "Z"}
		};
	}
}