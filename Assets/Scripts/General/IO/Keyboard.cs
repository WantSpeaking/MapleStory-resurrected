using System.Collections.Generic;
using System.Linq;
using Helper;




namespace ms
{
	public class Keyboard
	{
		public class Mapping
		{
			public KeyType.Id type;
			public int action;

			public Mapping ()
			{
				this.type = KeyType.Id.NONE;
				this.action = 0;
			}

			public Mapping (Mapping src) : this (src.type, src.action)
			{
			}

			public Mapping (KeyType.Id in_type, int in_action)
			{
				this.type = in_type;
				this.action = in_action;
			}

			public static bool operator == (Mapping ImpliedObject, Mapping other)
			{
				if (object.Equals (ImpliedObject,null) ||　object.Equals (other,null) )
				{
					return false;
				}
				return ImpliedObject.type == other.type && ImpliedObject.action == other.action;
			}

			public static bool operator != (Mapping ImpliedObject, Mapping other)
			{
				if (object.Equals (ImpliedObject,null) ||　object.Equals (other,null) )
				{
					return true;
				}
				return ImpliedObject.type != other.type || ImpliedObject.action != other.action;
			}
		}

		int[] Keytable = new int[89]
		{
			0, 0, // 1
			GLFW_Util.GLFW_KEY_0, GLFW_Util.GLFW_KEY_1, GLFW_Util.GLFW_KEY_2, GLFW_Util.GLFW_KEY_3, GLFW_Util.GLFW_KEY_4, GLFW_Util.GLFW_KEY_5, GLFW_Util.GLFW_KEY_6, GLFW_Util.GLFW_KEY_7, GLFW_Util.GLFW_KEY_8, GLFW_Util.GLFW_KEY_9, GLFW_Util.GLFW_KEY_MINUS, GLFW_Util.GLFW_KEY_EQUAL,
			0, 0, // 15
			GLFW_Util.GLFW_KEY_Q, GLFW_Util.GLFW_KEY_W, GLFW_Util.GLFW_KEY_E, GLFW_Util.GLFW_KEY_R, GLFW_Util.GLFW_KEY_T, GLFW_Util.GLFW_KEY_Y, GLFW_Util.GLFW_KEY_U, GLFW_Util.GLFW_KEY_I, GLFW_Util.GLFW_KEY_O, GLFW_Util.GLFW_KEY_P, GLFW_Util.GLFW_KEY_LEFT_BRACKET, GLFW_Util.GLFW_KEY_RIGHT_BRACKET,
			0, // 28
			GLFW_Util.GLFW_KEY_LEFT_CONTROL, GLFW_Util.GLFW_KEY_A, GLFW_Util.GLFW_KEY_S, GLFW_Util.GLFW_KEY_D, GLFW_Util.GLFW_KEY_F, GLFW_Util.GLFW_KEY_G, GLFW_Util.GLFW_KEY_H, GLFW_Util.GLFW_KEY_J, GLFW_Util.GLFW_KEY_K, GLFW_Util.GLFW_KEY_L, GLFW_Util.GLFW_KEY_SEMICOLON, GLFW_Util.GLFW_KEY_APOSTROPHE, GLFW_Util.GLFW_KEY_GRAVE_ACCENT, GLFW_Util.GLFW_KEY_LEFT_SHIFT, GLFW_Util.GLFW_KEY_BACKSLASH, GLFW_Util.GLFW_KEY_Z, GLFW_Util.GLFW_KEY_X, GLFW_Util.GLFW_KEY_C, GLFW_Util.GLFW_KEY_V,
			GLFW_Util.GLFW_KEY_B,
			GLFW_Util.GLFW_KEY_N, GLFW_Util.GLFW_KEY_M, GLFW_Util.GLFW_KEY_COMMA, GLFW_Util.GLFW_KEY_PERIOD,
			0, 0, 0, // 55
			GLFW_Util.GLFW_KEY_LEFT_ALT, GLFW_Util.GLFW_KEY_SPACE,
			0, // 58
			GLFW_Util.GLFW_KEY_F1, GLFW_Util.GLFW_KEY_F2, GLFW_Util.GLFW_KEY_F3, GLFW_Util.GLFW_KEY_F4, GLFW_Util.GLFW_KEY_F5, GLFW_Util.GLFW_KEY_F6, GLFW_Util.GLFW_KEY_F7, GLFW_Util.GLFW_KEY_F8, GLFW_Util.GLFW_KEY_F9, GLFW_Util.GLFW_KEY_F10, GLFW_Util.GLFW_KEY_F11, GLFW_Util.GLFW_KEY_F12, GLFW_Util.GLFW_KEY_HOME,
			0, // 72
			GLFW_Util.GLFW_KEY_PAGE_UP,
			0, 0, 0, 0, 0, // 78
			GLFW_Util.GLFW_KEY_END,
			0, // 80
			GLFW_Util.GLFW_KEY_PAGE_DOWN, GLFW_Util.GLFW_KEY_INSERT, GLFW_Util.GLFW_KEY_DELETE, GLFW_Util.GLFW_KEY_ESCAPE, GLFW_Util.GLFW_KEY_RIGHT_CONTROL, GLFW_Util.GLFW_KEY_RIGHT_SHIFT, GLFW_Util.GLFW_KEY_RIGHT_ALT, GLFW_Util.GLFW_KEY_SCROLL_LOCK
		};

		int[] Shifttable = new int[126]
		{
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0, //  10
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0, //  20
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0, //  30
			0, 0, 49, 39, 51, 52, 53, 55, 0, 57, //  40
			48, 56, 61, 0, 0, 0, 0, 0, 0, 0, //  50
			0, 0, 0, 0, 0, 0, 0, 59, 0, 44, //  60
			0, 46, 47, 50, 97, 98, 99, 100, 101, 102, //  70
			103, 104, 105, 106, 107, 108, 109, 110, 111, 112, //  80
			113, 114, 115, 116, 117, 118, 119, 120, 121, 122, //  90
			0, 0, 0, 54, 45, 0, 0, 0, 0, 0, // 100
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0, // 110
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0, // 120
			0, 0, 91, 92, 93, 96 // 126
		};

		int[] Specialtable = new int[96]
		{
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0, // 10
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0, // 20
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0, // 30
			0, 0, 0, 0, 0, 0, 0, 0, 34, 0, // 40
			0, 0, 0, 60, 95, 62, 63, 41, 33, 64, // 50
			35, 36, 37, 94, 38, 42, 40, 0, 58, 0, // 60
			43, 0, 0, 0, 0, 0, 0, 0, 0, 0, // 70
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0, // 80
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0, // 90
			123, 124, 125, 0, 0, 126 // 96
		};

		public Keyboard ()
		{
			keymap[GLFW_Util.GLFW_KEY_LEFT] = new Mapping (KeyType.Id.ACTION, (int)KeyAction.Id.LEFT);
			keymap[GLFW_Util.GLFW_KEY_RIGHT] = new Mapping (KeyType.Id.ACTION, (int)KeyAction.Id.RIGHT);
			keymap[GLFW_Util.GLFW_KEY_UP] = new Mapping (KeyType.Id.ACTION, (int)KeyAction.Id.UP);
			keymap[GLFW_Util.GLFW_KEY_DOWN] = new Mapping (KeyType.Id.ACTION, (int)KeyAction.Id.DOWN);
			keymap[GLFW_Util.GLFW_KEY_ENTER] = new Mapping (KeyType.Id.ACTION, (int)KeyAction.Id.RETURN);
			keymap[GLFW_Util.GLFW_KEY_KP_ENTER] = new Mapping (KeyType.Id.ACTION, (int)KeyAction.Id.RETURN);
			keymap[GLFW_Util.GLFW_KEY_TAB] = new Mapping (KeyType.Id.ACTION, (int)KeyAction.Id.TAB);
			textactions[GLFW_Util.GLFW_KEY_BACKSPACE] = KeyAction.Id.BACK;
			textactions[GLFW_Util.GLFW_KEY_ENTER] = KeyAction.Id.RETURN;
			textactions[GLFW_Util.GLFW_KEY_KP_ENTER] = KeyAction.Id.RETURN;
			textactions[GLFW_Util.GLFW_KEY_SPACE] = KeyAction.Id.SPACE;
			textactions[GLFW_Util.GLFW_KEY_TAB] = KeyAction.Id.TAB;
			textactions[GLFW_Util.GLFW_KEY_ESCAPE] = KeyAction.Id.ESCAPE;
			textactions[GLFW_Util.GLFW_KEY_HOME] = KeyAction.Id.HOME;
			textactions[GLFW_Util.GLFW_KEY_END] = KeyAction.Id.END;
			textactions[GLFW_Util.GLFW_KEY_DELETE] = KeyAction.Id.DELETE;
		}

		public void assign (byte key, byte tid, int action)
		{
			KeyType.Id type = KeyType.typebyid (tid);
			if (type != KeyType.Id.NONE)
			{
				Mapping mapping = new Mapping (type, action);

				keymap[Keytable[key]] = mapping;
				maplekeys[key] = mapping;
			}
		}

		public void remove (byte key)
		{
			Mapping mapping = new Mapping (KeyType.Id.NONE, 0);
			keymap[Keytable[key]] = mapping;
			maplekeys[key] = mapping;
		}

		public int leftshiftcode ()
		{
			return GLFW_Util.GLFW_KEY_LEFT_SHIFT;
		}

		public int rightshiftcode ()
		{
			return GLFW_Util.GLFW_KEY_RIGHT_SHIFT;
		}

		public int capslockcode ()
		{
			return (int)GLFW_Util.GLFW_KEY_CAPS_LOCK;
		}

		public int leftctrlcode ()
		{
			return (int)GLFW_Util.GLFW_KEY_LEFT_CONTROL;
		}

		public int rightctrlcode ()
		{
			return (int)GLFW_Util.GLFW_KEY_LEFT_CONTROL;
		}

		public SortedDictionary<int, Keyboard.Mapping> get_maplekeys ()
		{
			return maplekeys;
		}

		public KeyAction.Id get_ctrl_action (int keycode)
		{
			switch (keycode)
			{
				case (int)GLFW_Util.GLFW_KEY_C:
					return KeyAction.Id.COPY;
				case (int)GLFW_Util.GLFW_KEY_V:
					return KeyAction.Id.PASTE;
				/*case GLFW_KEY_A:
					return KeyAction::Id::SELECTALL;*/
				default:
					return KeyAction.Id.NONE;
			}
		}

		public Keyboard.Mapping get_mapping (int keycode)
		{
			if (keymap.TryGetValue (keycode, out var mapping))
			{
				return mapping;
			}
			else
			{
				return new Mapping (KeyType.Id.NONE, 0);
			}

			/*var iter = keymap.find (keycode);
			if (iter == keymap.end ())
			{
				return new Mapping (KeyType.Id.NONE, 0);
			}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: Iterators are only converted within the context of 'while' and 'for' loops:
			return iter.second;*/
		}

		public Keyboard.Mapping get_maple_mapping (int keycode)
		{
			if (maplekeys.TryGetValue (keycode, out var mapping))
			{
				return mapping;
			}
			else
			{
				return new Mapping (KeyType.Id.NONE, 0);
			}

			/*var iter = maplekeys.find (keycode);
			if (iter == maplekeys.end ())
			{
				return new Mapping (KeyType.Id.NONE, 0);
			}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: Iterators are only converted within the context of 'while' and 'for' loops:
			return iter.second;*/
		}

		public Keyboard.Mapping get_text_mapping (int keycode, bool shift)
		{
			if (textactions.Any (pair => pair.Key == keycode))
			{
				return new Mapping (KeyType.Id.ACTION, (int)textactions[keycode]);
			}
			else if (keycode == 39 || (keycode >= 44 && keycode <= 57) || keycode == 59 || keycode == 61 || (keycode >= 91 && keycode <= 93) || keycode == 96)
			{
				if (!shift)
				{
					return new Mapping (KeyType.Id.TEXT, keycode);
				}
				else
				{
					return new Mapping (KeyType.Id.TEXT, Specialtable[keycode - 1]);
				}
			}
			else if (keycode >= 33 && keycode <= 126)
			{
				if (shift)
				{
					return new Mapping (KeyType.Id.TEXT, keycode);
				}
				else
				{
					return new Mapping (KeyType.Id.TEXT, Shifttable[keycode - 1]);
				}
			}
			else
			{
				switch (keycode)
				{
					case GLFW_Util.GLFW_KEY_LEFT:
					case GLFW_Util.GLFW_KEY_RIGHT:
					case GLFW_Util.GLFW_KEY_UP:
					case GLFW_Util.GLFW_KEY_DOWN:
						return keymap[keycode];
					default:
						return new Mapping (KeyType.Id.NONE, 0);
				}
			}
		}

		private SortedDictionary<int, Mapping> keymap = new SortedDictionary<int, Mapping> ();
		private SortedDictionary<int, Mapping> maplekeys = new SortedDictionary<int, Mapping> ();
		private SortedDictionary<int, KeyAction.Id> textactions = new SortedDictionary<int, KeyAction.Id> ();
		private SortedDictionary<int, bool> keystate = new SortedDictionary<int, bool> ();
	}
}