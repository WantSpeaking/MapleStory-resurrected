#define USE_NX

using System;
using FairyGUI;
using MapleLib.WzLib;
using ms_Unity;
using UnityEngine;

namespace ms
{
	public class ChatBalloon:IDisposable
	{
		public ChatBalloon (sbyte type)
		{
			string typestr = String.Empty;

			if (type < 0)
			{
				switch (type)
				{
					case -1:
						typestr = "dead";
						break;
				}
			}
			else
			{
				typestr = Convert.ToString (type);
			}

			var src = ms.wz.wzProvider_ui["ChatBalloon.img"][typestr];

			arrow = src["arrow"];
			frame = new MapleFrame (src);

			textlabel = new Text (Text.Font.A11M, Text.Alignment.CENTER, Color.Name.BLACK, "", 80);

			duration = 0;

			fGUI_ChatBalloon = FGUI_ChatBalloon.CreateInstance ();
			GRoot.inst.AddChild (fGUI_ChatBalloon);
		}

		public ChatBalloon () : this (0)
		{
		}

		public void draw (Point_short position)
		{
			if (duration == 0)
			{
				fGUI_ChatBalloon.visible = false;
				fGUI_ChatBalloon.text = "";
				return;
			}

			/*short Width = textlabel.Width ();
			short height = textlabel.height ();*/
			short width = 20;
			short height = 100;

			/*frame.draw (new Point_short (position), width, height);
			arrow.draw (position);
			textlabel.draw (position + new Point_short ((short)(-width / 2 - 8), (short)(-height + 6)));*/

			Vector2 screenPos = UnityEngine.Camera.main.WorldToScreenPoint (new Vector3 (position.x (), position.y (), 1));
			screenPos.y = screenPos.y - Screen.height;
			fGUI_ChatBalloon.position = GRoot.inst.GlobalToLocal (screenPos);



		}

		public void update ()
		{
			duration -= (short)Constants.TIMESTEP;

			if (duration < 0)
			{
				duration = 0;
			}
		}

		public void change_text (string text)
		{
			textlabel.change_text (text);
			fGUI_ChatBalloon.visible = true;
			fGUI_ChatBalloon.text = text;
			duration = DURATION;
		}

		public void expire ()
		{
			duration = 0;
		}

		// How long a line stays on screen
		private const short DURATION = 4000; // 4 seconds

		private MapleFrame frame;
		private Text textlabel;
		private Texture arrow;
		private short duration;
		private FGUI_ChatBalloon fGUI_ChatBalloon;
		public void Dispose ()
		{
			GRoot.inst.RemoveChild (fGUI_ChatBalloon);
            //fGUI_ChatBalloon.Dispose ();//TODO Dispose fGUI_ChatBalloon
        }
    }

	public class ChatBalloonHorizontal
	{
		public ChatBalloonHorizontal ()
		{
			var Balloon = ms.wz.wzProvider_ui["Login.img"]["WorldNotice"]["Balloon"];

			arrow = Balloon["arrow"];
			center = Balloon["c"];
			east = Balloon["e"];
			northeast = Balloon["ne"];
			north = Balloon["n"];
			northwest = Balloon["nw"];
			west = Balloon["w"];
			southwest = Balloon["sw"];
			south = Balloon["s"];
			southeast = Balloon["se"];

			xtile = Math.Max (north.width (), (short)1);
			ytile = Math.Max (west.height (), (short)1);

			textlabel = new Text (Text.Font.A13B, Text.Alignment.LEFT, Color.Name.BLACK);
		}

		//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
		//ORIGINAL LINE: void draw(Point_short position) const
		public void draw (Point_short position)
		{
			short width = (short)(textlabel.width () + 9);
			short height = (short)(textlabel.height () - 2);

			short left = (short)(position.x () - width / 2);
			short top = (short)(position.y () - height);
			short right = (short)(left + width);
			short bottom = (short)(top + height);

			northwest.draw (new DrawArgument (left, top));
			southwest.draw (new DrawArgument (left, bottom));

			for (short y = top; y < bottom; y += ytile)
			{
				west.draw (new DrawArgument (left, y));
				east.draw (new DrawArgument (right, y));
			}

			center.draw (new DrawArgument (new Point_short ((short)(left - 8), top), new Point_short ((short)(width + 8), height)));

			for (short x = left; x < right; x += xtile)
			{
				north.draw (new DrawArgument (x, top));
				south.draw (new DrawArgument (x, bottom));
			}

			northeast.draw (new DrawArgument (right, top));
			southeast.draw (new DrawArgument (right, bottom));

			arrow.draw (new DrawArgument ((short)(right + 1), top));
			textlabel.draw (new DrawArgument ((short)(left + 6), (short)(top - 5)));
		}

		public void change_text (string text)
		{
			textlabel.change_text (text);
		}

		private Text textlabel;
		private Texture arrow = new Texture ();
		private Texture center = new Texture ();
		private Texture east = new Texture ();
		private Texture northeast = new Texture ();
		private Texture north = new Texture ();
		private Texture northwest = new Texture ();
		private Texture west = new Texture ();
		private Texture southwest = new Texture ();
		private Texture south = new Texture ();
		private Texture southeast = new Texture ();
		private short xtile;
		private short ytile;
	}
}