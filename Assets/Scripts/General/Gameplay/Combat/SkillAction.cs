using System.Collections.Generic;
using System.Linq;
using MapleLib.WzLib;




namespace ms
{
	public abstract class SkillAction : System.IDisposable
	{
		public virtual void Dispose ()
		{
		}

		public abstract void apply (ref Char target, Attack.Type atype);

		public virtual string actionStr { get; } = string.Empty;	
	}

	public class NoAction : SkillAction
	{
		public override void apply (ref Char UnnamedParameter1, Attack.Type UnnamedParameter2)
		{
		}
	}

	public class RegularAction : SkillAction
	{
		public override void apply (ref Char target, Attack.Type atype)
		{
			Weapon.Type weapontype = target.get_weapontype ();
			bool degenerate;

			switch (weapontype)
			{
				case Weapon.Type.BOW:
				case Weapon.Type.CROSSBOW:
				case Weapon.Type.CLAW:
				case Weapon.Type.GUN:
					degenerate = atype != Attack.Type.RANGED;
					break;
				default:
					degenerate = false;
					break;
			}

			target.attack (degenerate);
			//target.PlaySkillBTree ("Test");
		}
	}

	public class SingleAction : SkillAction
	{
		public bool setAlerted = true;
		public SingleAction (WzObject src)
		{
			action = src["action"]["0"].ToString ();
		}
		public SingleAction (string action, bool setAlerted)
		{
			this.action = action;
			this.setAlerted = setAlerted;
		}
		public override void apply (ref Char target, Attack.Type atype)
		{
			target.attack (action, setAlerted);
		}

		private string action;

		public override string actionStr => action;
	}

	public class TwoHandedAction : SkillAction
	{
		public TwoHandedAction (WzObject src)
		{
			actions[false] = src["action"]["0"].ToString ();
			actions[true] = src["action"]["1"].ToString ();
		}

		public override void apply (ref Char target, Attack.Type atype)
		{
			bool twohanded = target.is_twohanded ();
			string action = actions[twohanded];

			target.attack (action);
		}

		private BoolPair<string> actions = new BoolPair<string> ();

		public override string actionStr => actions[false];

	}

	public class ByLevelAction : SkillAction
	{
		public ByLevelAction (WzObject src, int id)
		{
			foreach (var sub in src["level"])
			{
				int level = string_conversion.or_zero<int> (sub.Name);
				actions[level] = sub["action"].ToString ();
			}

			skillid = id;
		}

		public override void apply (ref Char target, Attack.Type atype)
		{
			int level = target.get_skilllevel (skillid);
			if (actions.TryGetValue (level, out var action))
			{
				target.attack (action);
			}
		}

		private SortedDictionary<int, string> actions = new SortedDictionary<int, string> ();
		private int skillid;

		public override string actionStr => actions.Values.FirstOrDefault();

	}
}