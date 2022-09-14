using System.Collections.Generic;
using ms;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ms_Unity
{

	[Category ("PlayerAction")]
	public class BTA_ApplyActions : ActionTask
	{
		[StanceIdSelector]
		public Stance.Id StanceId = Stance.Id.SWINGO1;

		[CharActionIdSelector]
		public CharAction.Id ActionId = CharAction.Id.savage;

		[AfterimgIdSelector]
		public Afterimage.Id AfterimageId = Afterimage.Id.none;

		[StanceIdSelector]
		public Stance.Id AfterimageStanceId = Stance.Id.NONE;
		public BBParameter<byte> mobcount = 1;
		public BBParameter<byte> hitcount = 1;
		public BBParameter<float> skillSpeedMultiplier = 1f;

		private SpecialMove move => blackboard.GetVariable<SpecialMove> (typeof (SpecialMove).Name).value;

		public string actionName;

		private Player player => Stage.get ().get_player ();

		private Attack attack => blackboard.GetVariable<Attack> (typeof (Attack).Name).value;

		public List<BodyActionInfo> bodyActionInfos = new List<BodyActionInfo> ();

		public struct BodyActionInfo
		{
			public Stance.Id stanceId;
			public int frame;
			public int delay;
		}

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
			CharLook.get_BodyDrawInfo ().remove_action (actionName);
			if (!string.IsNullOrEmpty (actionName) && bodyActionInfos.Count != 0)
			{
				foreach (var bodyActionInfo in bodyActionInfos)
				{
					var bodyAction = new BodyAction (Stance.names[bodyActionInfo.stanceId], bodyActionInfo.frame, bodyActionInfo.delay);

					CharLook.get_BodyDrawInfo ().add_actionframe (actionName, (byte)bodyActionInfos.IndexOf (bodyActionInfo), bodyAction);
				}
			}

			bool hasAfterimageId = AfterimageId != Afterimage.Id.none;
			bool hasAfterimageStanceId = AfterimageStanceId != Stance.Id.NONE;
			//AppDebug.LogError ($"OnExecute ownerSystemBlackboard:{this.ownerSystemBlackboard.GetHashCode ()}\tblackboard:{blackboard.GetHashCode ()}\tblackboard.InputSD:{blackboard.GetVariable<int> ("InputSD").value}");
			player.Set_SkillSpeedMultiplier (skillSpeedMultiplier.GetValue ());

			if (StanceId != Stance.Id.NONE)
			{
				player.attack (StanceId, OnAttackEnd, move.get_id (), hasAfterimageId ? $"{AfterimageId}" : "", hasAfterimageStanceId ? StanceId.ToString () : "");
			}
			else
			{
				player.attack (string.IsNullOrEmpty (actionName) ? ActionId.ToString () : actionName, true, OnAttackEnd, move.get_id (), hasAfterimageId ? $"{AfterimageId}" : "", hasAfterimageStanceId ? StanceId.ToString () : "");
			}

			player.set_afterimage (move.get_id ());

			if (AfterimageId == Afterimage.Id.none)
			{
				//player.set_afterimage (move.get_id ());
			}
			else
			{
			}

			/*attack.damagetype = Attack.DamageType.DMG_WEAPON;
			attack.skill = 0;
			attack.mobcount = mobcount.value;
			attack.hitcount = hitcount.value;
			attack.stance = (byte)player.get_look ().get_stance ();

			if (attack.type == Attack.Type.CLOSE)
			{
				attack.range = player.get_afterimage ().get_range ();
			}*/
		}

		private void OnAttackEnd (Char c)
		{
			c.Set_SkillSpeedMultiplier (1);
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