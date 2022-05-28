using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Newtonsoft.Json;

namespace ms
{
    public class LocalStatus
	{
        [JsonProperty]
        public LocalStatusSetting Setting { get; private set; }

		public float LifeTime = 0;
		public decimal Value = 0;

		public object Owner { get; set; }

		public bool IsEnd => LifeTime <= 0;


		public LocalStatus (LocalStatusSetting setting, float duration, decimal value, object owner, object source)
		{
			Setting = setting;

			Owner = owner;
			LifeTime = duration;
			Value = value;
		}

		public bool TryMerge (float duration, decimal value, object source)
		{
			if (!Setting.CanMerge)
			{
				return false;
			}
			
			switch (Setting.ValueMergeType)
			{
				case LocalStatusSetting.ValueMergeTypes.Ignore:
					//do nothing
					break;
				case LocalStatusSetting.ValueMergeTypes.Override:
					Value = value;
					break;
				case LocalStatusSetting.ValueMergeTypes.Add:
					Value += value;
					break;
			}
			//duration
			switch (Setting.DurationMergeType)
			{
				case LocalStatusSetting.DurationMergeTypes.Ignore:
					//do nothing
					break;
				case LocalStatusSetting.DurationMergeTypes.Override:
					LifeTime = duration;
					break;
				case LocalStatusSetting.DurationMergeTypes.Add:
					LifeTime += duration;
					break;
			}
			return true;
		}

		public void Update (float dt)
		{
			LifeTime -= dt;
		}

		public void OnStart ()
		{
		}

		public void OnRemove ()
		{
		}

		public void OnMerge ()
		{
		}

        public void Reset()
        {
            LifeTime = 0;
            Value = 0;
        }
	}
}

public class LocalStatusSettingObject : SettingObject<LocalStatusSetting>
{
    public LocalStatusSetting.StatusTypes statusType;
    public string tags = "";

    public bool CanMerge = true;
    public LocalStatusSetting.ValueMergeTypes valueMergeType = LocalStatusSetting.ValueMergeTypes.Ignore;
    public LocalStatusSetting.DurationMergeTypes durationMergeType = LocalStatusSetting.DurationMergeTypes.Ignore;
    public LocalStatusSetting.SourceMergeTypes sourceMergeType = LocalStatusSetting.SourceMergeTypes.Ignore;

    //cache
    public bool enableCache = true;

    public LocalStatusSettingObject(LocalStatusSetting.ValueMergeTypes valueMergeType = LocalStatusSetting.ValueMergeTypes.Add, LocalStatusSetting.DurationMergeTypes durationMergeType = LocalStatusSetting.DurationMergeTypes.Override)
    {
        this.valueMergeType = valueMergeType;
        this.durationMergeType = durationMergeType;
    }
}

public class LocalStatusSetting : Setting<LocalStatusSettingObject>
{
    public enum StatusTypes
    {
        Monster,
        Turret,
        Game,
        Charge_Blow,
    }


    public enum ValueMergeTypes
    {
        Ignore,
        Override,
        Add,
    }

    public enum DurationMergeTypes
    {
        Ignore,
        Override,
        Add,
    }

    public enum SourceMergeTypes
    {
        Ignore,
        Override,
        OnlyMergeSameSource,
        RecordEverySource,
    }

    protected override void Init ()
    {
        
    }

    public StatusTypes StatusType => SettingObject.statusType;
    public bool CanMerge => SettingObject.CanMerge;
    public ValueMergeTypes ValueMergeType => SettingObject.valueMergeType;
    public DurationMergeTypes DurationMergeType => SettingObject.durationMergeType;
    public SourceMergeTypes SourceMergeType => SettingObject.sourceMergeType;

    public bool EnableCache => SettingObject.enableCache;
}

[Serializable]
public abstract class SettingObject : ISettingObject
{
    [JsonIgnore]

    protected Setting _setting;
    public Setting GetSetting (bool forceInit = false)
    {
        if (_setting == null)
        {
            _setting = Setting.Create (SettingType, this);
        }
        if (/*!Application.isPlaying || */forceInit)
        {
            InitSetting ();
        }
        return _setting;
    }

    [JsonIgnore]
    public virtual bool IsValid => true;
    [JsonIgnore]
    public virtual string SettingName => GetType ().Name;
    [JsonIgnore]
    public abstract Type SettingType { get; }

    public void InitSetting ()
    {
        Setting.Init (_setting);
    }
}

public abstract class Setting
{
    public static Setting Create (Type type, ISettingObject settingObject)
    {
        var setting = Activator.CreateInstance (type) as Setting;
        setting.SettingObject = settingObject;
        setting.Init ();
        return setting;
    }

    public static void Init (Setting setting)
    {
        setting?.Init ();
    }

    public virtual bool IsValid => SettingObject?.IsValid ?? false;

    protected virtual void Init () { }
    public string SettingName => SettingObject.SettingName;

    protected ISettingObject SettingObject { get; set; }


}

#region interface
public interface ISettingObject
{
    bool IsValid { get; }
    string SettingName { get; }
    Type SettingType { get; }
    Setting GetSetting (bool newSetting = false);
}

public interface ISettingObject<TSetting> : ISettingObject
    where TSetting : Setting, new()
{
    new TSetting GetSetting (bool newSetting = false);
}

public interface ISettingObjectShowname
{
    string Showname { get; }
}
#endregion

#region typed class
/*public class SettingScriptableObject<TSetting> : SettingScriptableObject, ISettingObject<TSetting>
    where TSetting : Setting, new()
{
    public override Type SettingType => typeof (TSetting);
    public new TSetting GetSetting (bool forceInit = false) => (TSetting)base.GetSetting (forceInit);
}*/

public class SettingObject<TSetting> : SettingObject, ISettingObject<TSetting>
    where TSetting : Setting, new()
{
    public override Type SettingType => typeof (TSetting);
    public new TSetting GetSetting (bool forceInit = false) => (TSetting)base.GetSetting (forceInit);
}

public class Setting<TSettingObject> : Setting
{
    protected new TSettingObject SettingObject => (TSettingObject)base.SettingObject;
}

#endregion