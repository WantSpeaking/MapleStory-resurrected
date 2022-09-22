using ms;
using NodeCanvas.Framework;
using ParadoxNotion;
using ParadoxNotion.Design;
using UnityEngine;


namespace ms_Unity
{

	[Category ("MobAction")]
	public class BTA_MobCheckInControl : ConditionTask
	{
		private Mob mob => blackboard.GetVariable<Mob> (typeof (Mob).Name).value;

		protected override string info
		{
			get { return "Mob is Controlled by player"; }
		}

		protected override bool OnCheck ()
		{
			return mob?.is_Controlled () ?? false;
			//return OperationTools.Compare(Vector2.Distance(agent.position, checkTarget.value.transform.position), distance.value, checkType, floatingPoint);
		}

		/*public override void OnDrawGizmosSelected() {
            if ( agent != null ) {
                Gizmos.DrawWireSphere((Vector2)agent.position, distance.value);
            }
        }*/
	}
}