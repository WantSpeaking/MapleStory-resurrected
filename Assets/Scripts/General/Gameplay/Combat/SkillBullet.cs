using System.Collections.Generic;
using MapleLib.WzLib;




namespace ms
{
	public abstract class SkillBullet : System.IDisposable
	{
		public virtual void Dispose ()
		{
		}

		public abstract Animation get (Char user, int bulletid);

		protected class Ball
		{
			public Animation animation = new Animation ();

			public Ball (WzObject src)
			{
				animation = src;
			}

			public Ball ()
			{
			}
		}
	}

	public class RegularBullet : SkillBullet
	{
		public override Animation get (Char user, int bulletid)
		{
			return BulletData.get (bulletid).get_animation ();
		}
	}

	public class SingleBullet : SkillBullet
	{
		public SingleBullet (WzObject src)
		{
			ball = new Ball (src["ball"]);
		}

		public override Animation get (Char user, int bulletid)
		{
			return ball.animation;
		}

		private Ball ball = new Ball ();
	}

	public class BySkillLevelBullet : SkillBullet
	{
		public BySkillLevelBullet (WzObject src, int id)
		{
			skillid = id;

			foreach (var sub in src["level"])
			{
				var level = string_conversion.or_zero<int> (sub.Name);
				bullets[level] = new Ball (sub["ball"]);
			}
		}

		public override Animation get (Char user, int bulletid)
		{
			int level = user.get_skilllevel (skillid);
			Animation anim;
			if (bullets.TryGetValue (level, out var ball))
			{
				anim = ball.animation;
			}
			else
			{
				anim = new Animation ();
			}

			return anim;
		}

		private Dictionary<int, Ball> bullets = new Dictionary<int, Ball> ();
		private int skillid;
	}
}