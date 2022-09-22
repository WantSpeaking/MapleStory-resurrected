using ms;
using NodeCanvas.Framework;
using ParadoxNotion;
using ParadoxNotion.Design;
using UnityEngine;


namespace ms_Unity
{

	[Category ("MobAction")]
	public class BTA_MobCheckDistanceToController : ConditionTask
	{
		private Mob mob => blackboard.GetVariable<Mob> (typeof (Mob).Name).value;

		private Player player => Stage.get ().get_player ();

		public CompareMethod checkType = CompareMethod.LessThan;
		public BBParameter<float> distance = 1;

		[SliderField (0, 0.1f)]
		public float floatingPoint = 0.05f;

		private short width => mob.get_Range ().width();
		protected override string info
		{
			get { return "Distance" + OperationTools.GetCompareString (checkType) + distance + " MobUnit to Controller"; }
		}

		protected override bool OnCheck ()
		{
			return OperationTools.Compare (player.get_position ().distance (mob.get_position ()), distance.value * width, checkType, floatingPoint);
		}

		/*public override void OnDrawGizmosSelected() {
            if ( agent != null ) {
                Gizmos.DrawWireSphere((Vector2)agent.position, distance.value);
            }
        }*/
	}
}