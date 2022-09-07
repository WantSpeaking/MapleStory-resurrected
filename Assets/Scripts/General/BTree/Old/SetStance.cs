/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using ms;

namespace ms_Unity
{
	[TaskCategory ("PlayerAction")]
	public class SetStance : PlayerAction_Base
	{
		public Stance.Id stance = Stance.Id.STABO1;

		public override TaskStatus OnUpdate ()
		{
			if(player == null)return TaskStatus.Running;
			player.attack (stance);

			return TaskStatus.Success;
		}
	}
}*/