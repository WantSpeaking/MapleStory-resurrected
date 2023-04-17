



using UnityEngine;

namespace ms
{
	public class MesoDrop : Drop
	{
		public MesoDrop(int oid, int owner, Point_short start, Point_short dest, sbyte type, sbyte mode, bool pd, Animation icn,int meso) : base(oid, owner, start, dest, type, mode, pd)
		{
			this.icon = new ms.Animation(icn);
			this.meso = meso;
			scale = Mathf.Clamp (meso / 1000000, 1, 15);
		}

		public override void draw(double viewx, double viewy, float alpha)
		{
			if (!active)
			{
				return;
			}

			Point_short absp = phobj.get_absolute(viewx, viewy, alpha);
			//icon.draw(new DrawArgument(angle.get(alpha), absp, opacity.get(alpha),Constants.get ().sortingLayer_MesoDrop,0), alpha);
			icon.draw(new DrawArgument(angle.get(alpha), absp, opacity.get(alpha)).SetParent(MapGameObject).set_xscale (scale).set_yscale (scale), alpha);
		}

		public override void Dispose ()
		{
			icon?.Dispose ();
			base.Dispose ();
		}
		
		private readonly Animation icon;
		private int meso;
		private float scale;

	}
}
