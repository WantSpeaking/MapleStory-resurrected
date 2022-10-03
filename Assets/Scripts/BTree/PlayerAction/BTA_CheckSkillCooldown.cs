using ms;
using NodeCanvas.Framework;
using ParadoxNotion.Design;


namespace ms_Unity
{

	[Category ("PlayerAction")]
	public class BTA_CheckSkillCooldown : ConditionTask
	{
		public BBParameter<float> cooldown;
		private SpecialMove move => blackboard.GetVariable<SpecialMove> (typeof (SpecialMove).Name).value;

		private Player player => Stage.get ().get_player ();

		protected override bool OnCheck ()
		{
			return player.has_cooldown (move.get_id ());
		}

		protected override string info => "SkillOnCooldown";
	}
}