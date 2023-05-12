#define USE_NX

using System;
using System.Collections.Generic;
using Helper;
using ms.Helper;
using MapleLib.WzLib;





namespace ms
{
	public class Expression
	{
		public enum Id
		{
			DEFAULT,
			BLINK,
			HIT,
			SMILE,
			TROUBLED,
			CRY,
			ANGRY,
			BEWILDERED,
			STUNNED,
			BLAZE,
			BOWING,
			CHEERS,
			CHU,
			DAM,
			DESPAIR,
			GLITTER,
			HOT,
			HUM,
			LOVE,
			OOPS,
			PAIN,
			SHINE,
			VOMIT,
			WINK,
		}

		public static Expression.Id byaction (uint action)
		{
			action -= 98;

			if (action < EnumUtil.GetEnumLength<Id> ())
			{
				return (Id)action;
			}

			Console.Write ("Unknown Expression::Id action: [");
			Console.Write (action);
			Console.Write ("]");
			Console.Write ("\n");

			return Expression.Id.DEFAULT;
		}

		public static EnumMap<Id, string> names = new EnumMap<Id, string> ();
	}

	public class Face
	{
		public Face (int faceid)
		{
			init_Dict ();
			string strid = "000" + Convert.ToString (faceid);
			WzObject face_00020000img = ms.wz.wzFile_character["Face"][strid + ".img"];

			if (face_00020000img == null)
			{
				AppDebug.Log ($"face is null,id:{strid}");

				face_00020000img = ms.wz.wzFile_character["Face"]["00020000.img"];
				faceid = 20000;
			}
			foreach (var iter in Expression.names)
			{
				Expression.Id exp = iter.Key;

				if (exp == Expression.Id.DEFAULT)
				{
					expressions[(int)Expression.Id.DEFAULT].Add (0, new Frame (face_00020000img["default"], "Player"));
				}
				else
				{
					string expname = iter.Key.ToString ();
					WzObject wzObj_face_00020000img_angry = face_00020000img[expname];
					if (wzObj_face_00020000img_angry is WzImageProperty property_face_00020000img_angry)
					{
						foreach (var property_face_00020000img_angry_0 in property_face_00020000img_angry.WzProperties)
						{
							byte.TryParse (property_face_00020000img_angry_0.Name, out var tempKey);

							expressions[(int)exp][tempKey] = new Frame (property_face_00020000img_angry_0, "Player");
							//expressions[(int)exp].Add (tempKey, new Frame (property_face_00020000img_angry_0));
						}
					}

					/*for (byte frame = 0; WzObject framenode = face_00020000img_angry[frame]; ++frame)
					{
						expressions[(int)exp].Add(frame, framenode);
					}*/
				}
			}

			name = ms.wz.wzFile_string["Eqp.img"]["Eqp"]["Face"][Convert.ToString (faceid)]["name"].ToString ();
		}


		//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
		//ORIGINAL LINE: void draw(Expression::Id expression, byte frame, const DrawArgument& args) const
		public void draw (Expression.Id expression, byte frame, DrawArgument args, bool drawOrErase = true)
		{

			Frame frameit = null;
			expressions[(int)expression]?.TryGetValue (frame, out frameit);

			if (drawOrErase)
			{
				frameit?.texture.draw (args);
			}
			else
			{
				frameit?.texture.erase ();
			}

			/*//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: Iterators are only converted within the context of 'while' and 'for' loops:
						if (frameit != expressions[(int)expression].end())
						{
			//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: Iterators are only converted within the context of 'while' and 'for' loops:
							frameit.second.texture.draw(args);
						}*/
		}

		//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
		//ORIGINAL LINE: byte nextframe(Expression::Id exp, byte frame) const
		public byte nextframe (Expression.Id exp, byte frame)
		{
			var tempFrame = (byte)(frame + 1);
			return expressions[(int)exp].ContainsKey (tempFrame) ? tempFrame : (byte)0;
		}

		//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
		//ORIGINAL LINE: short get_delay(Expression::Id exp, byte frame) const
		public short get_delay (Expression.Id exp, byte frame)
		{
			var delayit = (ushort)100;
			if (expressions[(int)exp].TryGetValue (frame, out var frameit))
			{
				delayit = frameit.delay;
			}

			return (short)delayit;
			/*var delayit = expressions[(int)exp].find(frame);
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: Iterators are only converted within the context of 'while' and 'for' loops:
			return delayit != expressions[(int)exp].end() ? delayit.second.delay : 100;*/
		}

		//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
		//ORIGINAL LINE: const string& get_name() const
		public string get_name ()
		{
			return name;
		}

		private class Frame
		{
			public Texture texture = new Texture ();
			public ushort delay;
			public Frame (WzObject src, string layerMaskName)
			{
				Init (src, layerMaskName);
			}
			public Frame (WzObject src)
			{
				Init (src);
			}

			private void Init (WzObject src, string layerMaskName = "Default")
			{
				texture = new Texture (src["face"], layerMaskName);

				Point_short shift = src["face"]?["map"]?["brow"]?.GetPoint ().ToMSPoint () ?? Point_short.zero;
				if (src.FullPath.Contains ("default"))
				{
					//AppDebug.Log ($"initial origin :{texture.get_origin ()} \t Face.Frame shift: {shift}");
				}

				texture.shift (-shift);

				delay = src["delay"];

				if (delay == 0)
				{
					delay = 2500;
				}
			}
		}

		private Dictionary<byte, Frame>[] expressions = new Dictionary<byte, Frame>[EnumUtil.GetEnumLength<Expression.Id> ()];

		private void init_Dict ()
		{
			for (int i = 0; i < EnumUtil.GetEnumLength<Expression.Id> (); i++)
			{
				expressions[i] = new Dictionary<byte, Frame> ();
			}
		}

		private string name;
	}
}


#if USE_NX
#endif