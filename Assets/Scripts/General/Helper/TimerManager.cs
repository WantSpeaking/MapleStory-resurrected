using System;
using System.Collections;
using System.Collections.Generic;
using ms;
using UnityEngine;

public class TimerManager : Singleton<TimerManager>
{
	private TimedQueue timedQueue = new ms.TimedQueue ();

	public TimedQueue.Timed register (Action in_action, long delay)
	{
		return timedQueue.register (in_action, delay);
	}

	public void update ()
	{
		timedQueue.update ();
	}
}
