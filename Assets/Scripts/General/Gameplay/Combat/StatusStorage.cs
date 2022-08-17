using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;
using Utility;

namespace ms
{
    [JsonObject (MemberSerialization = MemberSerialization.OptIn)]
	public class LocalStatusStorage
	{
		[JsonProperty]
		List<LocalStatus> localStatuses = new List<LocalStatus> ();
		Dictionary<LocalStatusSetting, List<LocalStatus>> settingDict = new Dictionary<LocalStatusSetting, List<LocalStatus>> ();

		public IEnumerable<LocalStatus> LocalStatuses => localStatuses;
		public object Owner { get; set; }

		public LocalStatusStorage (object owner)
		{
			Owner = owner;
		}

		public Func<LocalStatusSetting, bool> canAddLocalStatus;

		#region internal
		void _Add (LocalStatus LocalStatus)
		{
            localStatuses.Add (LocalStatus);
			//by type
			if (!settingDict.ContainsKey (LocalStatus.Setting))
			{
				settingDict.Add (LocalStatus.Setting, new List<LocalStatus> ());
			}
			settingDict[LocalStatus.Setting].Add (LocalStatus);

			LocalStatus.OnStart ();
		}

		void _Remove (LocalStatus localStatus)
		{
            localStatuses.Remove (localStatus);
			//by type
			if (settingDict.TryGetValue (localStatus.Setting, out var list))
			{
				list.Remove (localStatus);
			}

			localStatus.OnRemove ();
		}

		LocalStatus _Get (LocalStatusSetting setting)
		{
			if (settingDict.TryGetValue (setting, out var list))
			{
				return list.FirstOrDefault ();
			}
			return null;
		}
		#endregion

		public List<LocalStatus> GetLocalStatuses (LocalStatusSetting setting, List<LocalStatus> list = null)
		{
			list = list ?? new List<LocalStatus> ();
			if (setting == null)
			{
				return list;
			}

			var LocalStatuses = settingDict.TryGetValue (setting);
			if (LocalStatuses != null)
			{
				list.AddRange (LocalStatuses);
			}
			return list;
		}

		public List<LocalStatus> GetLocalStatuses (string settingName, List<LocalStatus> list = null)
		{
			return GetLocalStatuses (new LocalStatusSetting(), list);
		}

		public LocalStatus GetLocalStatus (LocalStatusSetting setting)
		{
			if (setting == null)
			{
				return null;
			}
			return settingDict.TryGetValue (setting)?.FirstOrDefault ();
		}

		public LocalStatus GetLocalStatus (string settingName)
		{
			return GetLocalStatus (new LocalStatusSetting());
		}

		public void Add (LocalStatusSetting setting, float duration, decimal value, object source)
		{
			if (setting == null)
			{
				return;
			}

			if (!(canAddLocalStatus?.Invoke (setting) ?? true))
			{
				//Debug.LogError ($"Cannot add localStatus: {setting.SettingName}");
				return;
			}

			var localStatus = _Get (setting);
			if (localStatus != null)
			{
				if (localStatus.TryMerge (duration, value, source))
				{
					localStatus.OnMerge ();
					return;
				}
			}
			//add new
			localStatus = new LocalStatus (setting, duration, value, Owner, source);
			_Add (localStatus);
		}

		public bool Contains (LocalStatusSetting setting)
		{
			if (setting == null)
			{
				return false;
			}
			return settingDict.TryGetValue (setting, out var list) ? list.Count > 0 : false;
		}

		public void Remove (LocalStatus LocalStatus)
		{
			_Remove (LocalStatus);
		}

		public void Remove (LocalStatusSetting setting)
		{
			if (settingDict.TryGetValue (setting, out var list))
			{
				for (int i = list.Count - 1; i >= 0; i--)
				{
					_Remove (list[i]);
				}
			}
		}

		public void Update (float dt)
		{
			using (var removeSet = BufferSet<LocalStatus>.Spawn ())
			{
				foreach (var localStatus in LocalStatuses)
				{
					localStatus.Update (dt);
					if (localStatus.IsEnd)
					{
						removeSet.Add (localStatus);
					}
				}

				foreach (var LocalStatus in removeSet)
				{
					_Remove (LocalStatus);
				}
			}
		}

		public void Clear ()
		{
			localStatuses.Clear ();
			foreach (var list in settingDict.Values)
			{
				list.Clear ();
			}
		}


		[OnDeserialized]
		void OnDeserializedMethod (StreamingContext context)
		{
			settingDict = LocalStatuses.GroupBy (s => s.Setting).ToDictionary (g => g.Key, g => new List<LocalStatus> (g));
		}
	}
}
