using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ms;

public class PlayerCamera : MonoBehaviour
{
	public float deltaY = 35;
	void Start ()
	{

	}

	Player player;
	void Update ()
	{
		if (player == null)
		{
			player = ms.Stage.Instance.get_player ();
		}

		if (player != null && player.absp != null)
		{
			var abspos = player.absp;
			transform.position = new Vector3 (abspos.x (), -abspos.y () + deltaY, -999);
		}
	}
}
