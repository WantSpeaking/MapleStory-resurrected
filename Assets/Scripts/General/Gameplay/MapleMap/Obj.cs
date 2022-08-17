#define USE_NX




using ms;
using ms.Helper;
using MapleLib.WzLib;

namespace ms
{
	// Represents a map decoration (object) on a map
	public class Obj
	{
		private int orderInLayer;

		public Obj (WzImageProperty node_100000000img_0_obj_0)
		{
			int.TryParse (node_100000000img_0_obj_0.Name, out orderInLayer);

			//UnityEngine.AppDebug.Log(node_100000000img_0_obj_0["oS"] + ".img" + "\t" + node_100000000img_0_obj_0["l0"] + "\t" + node_100000000img_0_obj_0["l1"] + "\t" + node_100000000img_0_obj_0["l2"]);

			animation = new Animation (ms.wz.wzFile_map["Obj"][$"{node_100000000img_0_obj_0["oS"]}.img"]?[node_100000000img_0_obj_0["l0"].ToString ()]?[node_100000000img_0_obj_0["l1"].ToString ()]?[node_100000000img_0_obj_0["l2"].ToString ()]);
			//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: The following line was determined to be a copy constructor call - this should be verified and a copy constructor should be created if it does not yet exist:
			//ORIGINAL LINE: pos = Point_short(src["x"], src["y"]);

			pos = new Point_short (node_100000000img_0_obj_0["x"], node_100000000img_0_obj_0["y"]);
			flip = node_100000000img_0_obj_0["f"];
			//z = (byte)(255- node_100000000img_0_obj_0["z"].GetShort ().ToByte ());//orderInLayer wz和unity正好相反
			z = (node_100000000img_0_obj_0["z"]); //orderInLayer wz和unity正好相反
			if (z == 0)
			{
				z = (node_100000000img_0_obj_0["zM"]);
			}
		}

		// Update animation
		public void update ()
		{
			animation.update ();
		}

		// Draw the object at the specified position
		//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
		//ORIGINAL LINE: void draw(Point_short viewpos, float inter) const
		public void draw (Point_short viewpos, float inter, int layerId)
		{
			//var tempPoint = new Point_short ((short)(pos.x () + viewpos.x ()), (short)(pos.y () + viewpos.y ()));
			//animation.draw (new DrawArgument (tempPoint, layerId, z), inter);
			
			//animation.draw(new DrawArgument(pos + viewpos, flip,layerId, z), inter);
			animation.draw(new DrawArgument(pos + viewpos, flip), inter);
		}

		// Return the depth of the object
		//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
		//ORIGINAL LINE: byte getz() const
		public byte getz ()
		{
			return z;
		}

		private Animation animation = new Animation ();
		private Point_short pos = new Point_short ();
		private byte z;
		private bool flip;
	}
}

#if USE_NX
#endif