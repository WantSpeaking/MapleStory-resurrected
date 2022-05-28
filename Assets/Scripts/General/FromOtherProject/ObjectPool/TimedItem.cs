using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utility.PoolSystem
{
	public class TimedItem : MonoBehaviour, IPoolCallback
	{
		public enum Status
		{
			Inactive,
			PlayDelay,
			Playing,
			StopDelay,
			DespawnDelay
		}

		#region runtime
		List<ITimedItemCallback> listeners = new List<ITimedItemCallback> ();

		Status status = Status.Inactive;
		bool autoStop = true;
		float t_playDelay = 0;
		float t_stopDelay = 0;
		float t_despawnDelay = 0;
		#endregion

		public void Init (bool autoActivate, bool autoStop, float playDelay, float stopDelay, float despawnDelay)
		{
			this.autoStop = autoStop;
			t_playDelay = playDelay <= 0 ? 0 : playDelay;
			t_stopDelay = stopDelay <= 0 ? 0 : stopDelay;
			t_despawnDelay = despawnDelay <= 0 ? 0 : despawnDelay;

			//activate
			if (autoActivate)
			{
				Activate ();
			}
			else
			{
				status = Status.Inactive;
			}
		}

		public void Activate ()
		{
			GetComponents (listeners);
			foreach (var listener in listeners)
			{
				listener.OnActivate ();
			}

			if (t_playDelay > 0)
			{
				status = Status.PlayDelay;
			}
			else
			{
				Play ();
			}
		}

		void Play ()
		{
			GetComponents (listeners);
			foreach (var listener in listeners)
			{
				listener.OnPlay ();
			}

			if (autoStop)
			{
				status = Status.StopDelay;
				if (t_stopDelay <= 0)
				{
					Stop ();
				}
			}
			else
			{
				status = Status.Playing;
			}
		}

		public void Stop ()
		{
			GetComponents (listeners);
			foreach (var listener in listeners)
			{
				listener.OnStop ();
			}

			if (t_despawnDelay > 0)
			{
				status = Status.DespawnDelay;
			}
			else
			{
				Despawn ();
			}
		}

		public void Despawn ()
		{
			PoolManager.Despawn (gameObject);
		}

		void Update ()
		{
			float dt = Time.deltaTime;
			switch (status)
			{
				case Status.PlayDelay:
					t_playDelay -= dt;
					if (t_playDelay <= 0)
					{
						Play ();
					}
					break;
				case Status.StopDelay:
					t_stopDelay -= dt;
					if (t_stopDelay <= 0)
					{
						Stop ();
					}
					break;
				case Status.DespawnDelay:
					t_despawnDelay -= dt;
					if (t_despawnDelay <= 0)
					{
						Despawn ();
					}
					break;
			}
		}

		void IPoolCallback.OnSpawn ()
		{
			
		}

		void IPoolCallback.OnDespawn ()
		{
			status = Status.Inactive;
			listeners.Clear ();
		}
	}

	public interface ITimedItemCallback
	{
		void OnActivate ();
		void OnPlay ();
		void OnStop ();
	}
}