using NodeCanvas.Framework;
using ParadoxNotion.Design;
using ms;
using System.Collections.Generic;

namespace ms_Unity
{

	[Category ("PlayerAction")]
	public class BTA_SkillCommonProcedure : ActionTask
	{
		public BBParameter<SpecialMove> SpecialMove;
		private SpecialMove move => SpecialMove.value;

		private Player player => Stage.get ().get_player ();
		private MapChars chars => Stage.get().get_chars();
		private MapMobs mobs => Stage.get ().get_mobs ();
		private MapReactors reactors => Stage.get ().get_reactors ();

		//Use for initialization. This is called only once in the lifetime of the task.
		//Return null if init was successfull. Return an error string otherwise
		protected override string OnInit ()
		{
			return null;
		}

		//This is called once each time the task is enabled.
		//Call EndAction() to mark the action as finished, either in success or failure.
		//EndAction can be called from anywhere.
		protected override void OnExecute ()
		{
			if (move == null)
			{
				EndAction (false);
			}

			Attack attack = player.prepare_attack (move.is_skill ());

			move.apply_useeffects (player);
			move.apply_actions (player, attack.type);

			player.set_afterimage (move.get_id ());

			move.apply_stats (player, attack);

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

			List<int> mob_targets = Stage.get ().get_combat ().find_closest (mob_objs, new Rectangle_short (range), new Point_short (origin), mobcount, true);//todo 2 跳起来 攻击不到，按键输入还有问题 没法起跳时就攻击
			List<int> reactor_targets = Stage.get ().get_combat ().find_closest (reactor_objs, new ms.Rectangle_short (range), new ms.Point_short (origin), mobcount, false);

			mobs.send_attack (result, attack, mob_targets, mobcount);
			result.attacker = player.get_oid ();
			Stage.get ().get_combat ().extract_effects (player, move, result);

			Stage.get ().get_combat ().apply_use_movement (move);
			Stage.get ().get_combat ().apply_result_movement (move, result);

			new AttackPacket (result).dispatch ();
			/*			if (down == true && pressing == false && move.has_skillPrepareEffect ())//begin
						{
							new SkillEffectPacket (move.get_id (), Stage.get ().get_player ().get_skills ().get_masterlevel (move.get_id ()), 22, attack.toleft ? -128 : 0, 6).dispatch ();
							move.apply_prepareEffect (player);
							AppDebug.Log ("begin");

						}
						else if (down == false && pressing == true && move.has_skillPrepareEffect ())//end
						{
							new Cancel_BuffPacket (move.get_id ()).dispatch ();
							move.apply_keydownendEffect (player);
							AppDebug.Log ("end");

						}
						else if (down == true && pressing == true && move.has_skillPrepareEffect ())//moving
						{
							move.apply_keydownEffect (player);
							AppDebug.Log ("moving");

						}*/
			if (reactor_targets.Count != 0)
			{
				Optional<MapObject> reactor = reactor_objs.get (reactor_targets[0]);
				if (reactor)
				{
					new DamageReactorPacket (reactor.get ().get_oid (), player.get_position (), 0, 0).dispatch ();
				}
			}

			EndAction (true);
		}

		//Called once per frame while the action is active.
		protected override void OnUpdate ()
		{

		}

		//Called when the task is disabled.
		protected override void OnStop ()
		{

		}

		//Called when the task is paused.
		protected override void OnPause ()
		{

		}
	}
}