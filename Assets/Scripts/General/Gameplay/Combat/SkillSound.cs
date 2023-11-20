namespace ms
{
	// Interface for skill sound
	public abstract class SkillSound : System.IDisposable
	{
		public virtual void Dispose()
		{
		}

		public abstract void play_use();
		public abstract void play_hit();
	}

	// No sound
	public class NoSkillSound : SkillSound
	{
		public override void play_use()
		{
		}
		public override void play_hit()
		{
		}
	}

	// Plays one use and one hit sound
	public class SingleSkillSound : SkillSound
	{
		public SingleSkillSound(string strid)
		{
			var soundsrc = ms.wz.wzProvider_sound["Skill.img"][strid];

			usesound = soundsrc?["Use"];
			hitsound = soundsrc?["Hit"];
		}

		public override void play_use()
		{
			usesound?.play();
		}
		public override void play_hit()
		{
			hitsound?.play();
		}

		private Sound usesound = new Sound();
		private Sound hitsound = new Sound();
	}
}
