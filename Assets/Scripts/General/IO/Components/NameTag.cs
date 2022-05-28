using System.Collections.Generic;
using MapleLib.WzLib;




namespace ms
{
	public class NameTag
	{
		public NameTag (WzObject src, Text.Font f, string n)
		{
			name = new OutlinedText (f, Text.Alignment.CENTER, Color.Name.EAGLE, Color.Name.JAMBALAYA);
			name.change_text (n);

			textures[false].Add (src["0"]["0"]);
			textures[false].Add (src["0"]["1"]);
			textures[false].Add (src["0"]["2"]);

			textures[true].Add (src["1"]["0"]);
			textures[true].Add (src["1"]["1"]);
			textures[true].Add (src["1"]["2"]);

			selected = false;
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void draw(Point_short position) const
		public void draw (Point_short position)
		{
			position.shift (new Point_short (1, 2));

			var tag = textures[selected];

			short width = name.width ();

			// If ever changing startpos, confirm with UICharSelect.cpp
			Point_short startpos = position - new Point_short ((short)(6 + width / 2), 0);

			tag[0].draw (startpos);
			tag[1].draw (new DrawArgument (startpos + new Point_short (6, 0), new Point_short (width, 0)));
			tag[2].draw (new DrawArgument (startpos + new Point_short ((short)(width + 6), 0)));

			name.draw (new Point_short (position));
		}

		public void set_selected (bool s)
		{
			selected = s;

			if (s)
			{
				name.change_color (Color.Name.WHITE);
			}
			else
			{
				name.change_color (Color.Name.EAGLE);
			}
		}

		private OutlinedText name = new OutlinedText ();
		private BoolPairNew<List<Texture>> textures = new BoolPairNew<List<Texture>> (new List<Texture> (), new List<Texture> ());
		private bool selected;
	}
}