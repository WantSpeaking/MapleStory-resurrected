#define USE_NX

using MapleLib.WzLib;
using provider;

namespace ms
{
	public class MapEffect
	{
        public MapEffect(string path)
		{
			this.active = false;
			var Effect = wz.wzProvider_map["Effect.img"];

			effect = Effect.getChildByPath(path);

			short width = Constants.get().get_viewwidth();

			position = new Point_short((short)(width / 2), 250);
		}
		public MapEffect()
		{
		}

		public void draw()
		{
			if (!active)
			{
				effect.draw(position, 1.0f);
			}
		}
		public void update()
		{
			if (!active)
			{
				active = effect.update(6);
			}
		}

		private bool active;
		private Animation effect = new Animation();
		private Point_short position = new Point_short();
	}
}


#if USE_NX
#endif
