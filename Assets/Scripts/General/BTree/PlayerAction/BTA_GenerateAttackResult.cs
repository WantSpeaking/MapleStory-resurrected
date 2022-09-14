using System.Collections.Generic;
using ms;
using NodeCanvas.Framework;
using ParadoxNotion.Design;


namespace ms_Unity
{

	[Category("PlayerAction")]
	public class BTA_GenerateAttackResult : ActionTask
	{
		private SpecialMove move => blackboard.GetVariable<SpecialMove> (typeof (SpecialMove).Name).value;

		private Attack attack => blackboard.GetVariable<Attack> (typeof (Attack).Name).value;

		private Player player => Stage.get ().get_player ();
		private MapChars chars => Stage.get ().get_chars ();
		private MapMobs mobs => Stage.get ().get_mobs ();
		private MapReactors reactors => Stage.get ().get_reactors ();
		private Combat combat => Stage.get ().get_combat ();

		
		//Use for initialization. This is called only once in the lifetime of the task.
		//Return null if init was successfull. Return an error string otherwise
		protected override string OnInit()
		{
			return null;
		}

		//This is called once each time the task is enabled.
		//Call EndAction() to mark the action as finished, either in success or failure.
		//EndAction can be called from anywhere.
		protected override void OnExecute()
		{
			Point_short origin = new Point_short (attack.origin);
			Rectangle_short range = new Rectangle_short (attack.range);
			short hrange = (short)(range.left () * attack.hrange);

			MapleStory.Instance.attackRange = range;
			//AppDebug.Log ($"center:{center}\t size:{size}\t attackRange:{attackRange}");
			if (attack.toleft)
			{
				range = new Rectangle_short (
					(short)(origin.x () + hrange),
					(short)(origin.x () + range.right ()),
					(short)(origin.y () + range.top ()),
					(short)(origin.y () + range.bottom ()));
			}
			else
			{
				range = new Rectangle_short (
					(short)(origin.x () - range.right ()),
					(short)(origin.x () - hrange),
					(short)(origin.y () + range.top ()),
					(short)(origin.y () + range.bottom ()));
			}

			MapleStory.Instance.attackRangeAfter = range;

			// This approach should also make it easier to implement PvP
			byte mobcount = attack.mobcount;
			AttackResult result = new AttackResult (attack);

			MapObjects mob_objs = mobs.get_mobs ();
			MapObjects reactor_objs = reactors.get_reactors ();

			List<int> mob_targets = combat.find_closest (mob_objs, new Rectangle_short (range), new Point_short (origin), mobcount, true);//todo 2 跳起来 攻击不到，按键输入还有问题 没法起跳时就攻击
			List<int> reactor_targets = combat.find_closest (reactor_objs, new ms.Rectangle_short (range), new ms.Point_short (origin), mobcount, false);

			mobs.send_attack (result, attack, mob_targets, mobcount);
			result.attacker = player.get_oid ();

			combat.extract_effects (player, move, result);

			combat.apply_use_movement (move);
			combat.apply_result_movement (move, result);

			new AttackPacket (result).dispatch ();

			EndAction (true);
		}

		//Called once per frame while the action is active.
		protected override void OnUpdate()
		{
			
		}

		//Called when the task is disabled.
		protected override void OnStop()
		{
			
		}

		//Called when the task is paused.
		protected override void OnPause()
		{
			
		}
	}
}