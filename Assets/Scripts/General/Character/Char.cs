#define USE_NX

using System;
using System.Collections.Generic;
using NodeCanvas.BehaviourTrees;
using NodeCanvas.Framework;
using UnityEngine;

namespace ms
{
	// Base for characters, e.g. the player and other clients on the same map.
	public abstract class Char : MapObject
	{
		// Player states which determine animation and state 
		// Values are used in movement packets (Add one if facing left)
		public enum State : sbyte
		{
			WALK = 2,
			STAND = 4,
			FALL = 6,
			ALERT = 8,
			PRONE = 10,
			SWIM = 12,
			LADDER = 14,
			ROPE = 16,
			DIED = 18,
			SIT = 20
		}

		public static State by_value (sbyte value)
		{
			return (State)value;
		}

		public override void draw (double viewx, double viewy, float alpha)
		{
#if BackgroundStatic
			Point_short absp = phobj.get_position ();
#else
			absp = phobj.get_absolute (viewx, viewy, alpha);
#endif
			//AppDebug.Log ($"Char draw absp:{absp}\t phobj.x:{phobj.x.get()}\t phobj.y:{phobj.y.get()}");
			effects.drawbelow (absp, alpha);

			Color color;

			if ((bool)invincible)
			{
				float phi = invincible.alpha () * 30;
				float rgb = (float)(0.9f - 0.5f * Math.Abs (Math.Sin (phi))) /*abs(sinf(phi))*/;

				color = new Color (rgb, rgb, rgb, 1.0f);
			}
			else
			{
				color = new Color (Color.Code.CWHITE);
			}
			var da_look = new DrawArgument(absp, color).SetParent(MapGameObject);

            if (this is Player )
			{
				da_look.SetDontDestoryOnLoad(true);
            }
			look.draw (da_look, alpha);
			//look.draw (new DrawArgument (new Point_short (absp), new Color (color),get_layer (), 0), alpha);

			afterimage.draw (look.get_frame (), new DrawArgument (absp, facing_right).SetParent (MapGameObject), alpha, look);

			if ((bool)ironbody)
			{
				float ibalpha = ironbody.alpha ();
				float scale = 1.0f + ibalpha;
				float opacity = 1.0f - ibalpha;

				look.draw (new DrawArgument (new Point_short (absp), scale, scale, opacity).SetParent (MapGameObject), alpha);
			}

			foreach (var pet in pets)
			{
				if ((pet?.get_itemid () ?? 0) != 0)
					pet?.draw (viewx, viewy, alpha);
			}


			// If ever changing code for namelabel confirm placements with map 10000
			namelabel.draw (absp + new Point_short (0, -4));
			chatballoon.draw (absp - new Point_short (0, 85));

			effects.drawabove (absp, alpha);
			foreach (var number in damagenumbers)
			{
				number.draw (viewx, viewy, alpha);
			}
			
		}

		public void draw_preview (Point_short position, float alpha)
		{
			look_preview.draw (position, false, Stance.Id.STAND1, Expression.Id.DEFAULT);
		}

		public void draw_shadow (float alpha)
		{
			_charShadowLook?.draw (alpha,look);
		}
		
		public bool update (Physics physics, float speed)
		{
			damagenumbers.remove_if (number => number.update(), (d) => { d?.Dispose(); });

			effects.update ();
			chatballoon.update ();
			invincible.update ();
			ironbody.update ();


			/*foreach(var pet in pets)
			{
				if (pet.get_itemid())
				{
					switch (state)
					{
						case State.LADDER:
						case State.ROPE:
							pet.set_stance(PetLook.Stance.HANG);
							break;
						case State.SWIM:
							pet.set_stance(PetLook.Stance.FLY);
							break;
						default:
							if (pet.get_stance() == PetLook.Stance.HANG || pet.get_stance() == PetLook.Stance.FLY)
								pet.set_stance(PetLook.Stance.STAND);

							break;
					}

					pet.update(physics, get_position());
				}
			}*/

			ushort stancespeed = 0;

			if (speed >= 1.0f / Constants.TIMESTEP)
				stancespeed = (ushort)(Constants.TIMESTEP * speed);

			afterimage.update (look.get_frame (), stancespeed);
			var aniend = look.update ((ushort)(stancespeed * Constants.get ().animSpeed)); //todo 2 this is action speed
			if (aniend)
			{
				OnAniEnd ();
			}
			return aniend;
		}

		void OnAniEnd ()
		{
			OnEnd?.Invoke (this);
			OnEnd = null;
			current_skill_id = 0;
			current_afterimageName = null;
			current_stance_name = null;
		}

		public float get_stancespeed ()
		{
			if (attacking)
				return get_real_attackspeed ();

			switch (state)
			{
				case State.WALK:
					return (float)(Math.Abs (phobj.hspeed));
				case State.LADDER:
				case State.ROPE:
					return (float)(Math.Abs (phobj.vspeed));
				default:
					return 1.0f;
			}
		}

		private float skillSpeedMultiplier = 1;
		public void Set_SkillSpeedMultiplier (float skillSpeedMultiplier)
		{
			this.skillSpeedMultiplier = skillSpeedMultiplier;
		}
		public float get_real_attackspeed ()
		{
			sbyte speed = get_integer_attackspeed ();
			var real_attackspeed = 1.7f - (float)(speed) / 10;
			return real_attackspeed * skillSpeedMultiplier;
		}

		public ushort get_attackdelay (uint no)
		{
			byte first_frame = afterimage.get_first_frame ();
			ushort delay = look.get_attackdelay (no, first_frame);
			float fspeed = get_real_attackspeed ();

			return (ushort)(delay / fspeed);
		}


		public float get_total_attackdelay (SpecialMove move, string actionstr)
		{
			if (move is Skill skill)
			{
				return skill.GetKeydownEffect ()?.GetAniLength ()??100 / 1000f;
			}
			else
			if (SkillCooldown.SkillCooldowns.TryGetValue (move.get_id (), out var cooldown))
			{
				return cooldown;
			}
			return look.get_total_attackdelay (actionstr);
		}

		public override sbyte update (Physics physics)
		{
			update (physics, 1.0f);

			return get_layer ();
		}

		public sbyte get_layer ()
		{
			return (sbyte)(is_climbing () ? 7 : phobj.fhlayer);
		}

		public void show_attack_effect (Animation toshow, sbyte z)
		{
			float attackspeed = get_real_attackspeed ();

			effects.add (toshow, new DrawArgument (facing_right).SetParent (MapGameObject), z, attackspeed);
		}

		public void show_effect_id (CharEffect.Id toshow)
		{
			effects.add (chareffects[toshow]);
		}

		public void show_iron_body ()
		{
			ironbody.set_for (500);
		}

		public void show_damage (int damage)
		{
			short start_y = (short)(phobj.get_y () - 60);
			short x = (short)(phobj.get_x () - 10);

			damagenumbers.AddLast (new DamageNumber (DamageNumber.Type.TOPLAYER, damage, start_y, x));

			look.set_alerted (5000);
			invincible.set_for (2000);
		}

		public void speak (string line)
		{
			//AppDebug.Log ($"speak:!item 2000003 {line}");
			chatballoon.change_text (line);
		}

		public void change_look (MapleStat.Id stat, int id)
		{
			switch (stat)
			{
				case MapleStat.Id.SKIN:
					look.set_body (id);
					break;
				case MapleStat.Id.FACE:
					look.set_face (id);
					break;
				case MapleStat.Id.HAIR:
					look.set_hair (id);
					break;
			}
		}

		public void set_state (byte statebyte)
		{
			if (statebyte % 2 == 1)
			{
				set_direction (false);

				statebyte -= 1;
			}
			else
			{
				set_direction (true);
			}

			State newstate = by_value ((sbyte)statebyte);
			set_state (newstate);
		}

		public void set_expression (int expid)
		{
			Expression.Id expression = Expression.byaction ((uint)expid);
			look.set_expression (expression);
		}

		public void attack (string action, bool set_alerted = true, Action<Char> onEnd = null, int skill_id = 0, string afterimageName = null, string stance_name = null)
		{
			OnEnd = onEnd;
			look.set_action (action);

			attacking = true;

			if (set_alerted)
				look.set_alerted (5000);

			current_skill_id = skill_id;
			current_afterimageName = afterimageName;
			current_stance_name = stance_name;

			//set_afterimage (skill_id);
		}

		Action<Char> OnEnd;
		public void attack (Stance.Id stance, Action<Char> onEnd = null, int skill_id = 0, string afterimageName = null, string stance_name = null)
		{
			OnEnd = onEnd;
			look.attack (stance);

			attacking = true;
			look.set_alerted (5000);

			current_skill_id = skill_id;
			current_afterimageName = afterimageName;
			current_stance_name = stance_name;
			//set_afterimage (skill_id);
		}

		public void attack (bool degenerate)
		{
			look.attack (degenerate);

			attacking = true;
			look.set_alerted (5000);
		}

		int current_skill_id;
		string current_afterimageName;
		string current_stance_name;
		/// <summary>
		/// todo 应该根据 当前 技能id，stance，设置 afterImage
		/// </summary>
		/// <param name="skill_id"></param>
		public void set_afterimage (int skill_id)
		{
			int weapon_id = look.get_equips ().get_weapon ();

			if (weapon_id <= 0)
				return;

			WeaponData weapon = WeaponData.get (weapon_id);

			bool hasAfterimageId = !string.IsNullOrEmpty (current_afterimageName);
			bool hasAfterimageStanceId = !string.IsNullOrEmpty (current_stance_name);

			string stance_name = hasAfterimageStanceId ? current_stance_name : Stance.names[look.get_stance (1f)];
			short weapon_reqlevel = weapon.get_equipdata ().get_reqstat (MapleStat.Id.LEVEL);
			string ai_name = hasAfterimageId ? current_afterimageName : weapon.get_afterimage ();

			afterimage?.Dispose ();
			afterimage = new Afterimage (skill_id, ai_name, stance_name, weapon_reqlevel);

			//AppDebug.Log ($"set_afterimage:ai_name:{ai_name}\tstance_name:{stance_name}");
		}

		/*public void set_afterimage (int skill_id)
		{
			int weapon_id = look.get_equips ().get_weapon ();

			if (weapon_id <= 0)
				return;

			WeaponData weapon = WeaponData.get (weapon_id);
			short weapon_reqlevel = weapon.get_equipdata ().get_reqstat (MapleStat.Id.LEVEL);

			afterimage = new Afterimage (skill_id, afterimageName, stance_name, weapon_reqlevel);
		}*/

		public Afterimage get_afterimage ()
		{
			return afterimage;
		}

		public virtual void set_direction (bool f)
		{
			facing_right = f;
			look.set_direction (f);
		}

		public virtual void set_state (State st)
		{
			state = st;

			Stance.Id stance = Stance.by_state ((sbyte)state);
			look.set_stance (stance);
		}

		public void add_pet (byte index, int iid, string name, int uniqueid, Point_short pos, byte stance, int fhid)
		{
			if (index > 2)
				return;

			pets[index] = new PetLook (iid, name, uniqueid, new Point_short (pos), stance, fhid);
		}

		public void remove_pet (byte index, bool hunger)
		{
			if (index > 2)
				return;

			pets[index] = new PetLook ();

			if (hunger)
			{
				// TODO: Empty
			}
		}
		public bool has_pet (int petid)
		{
			for (int i = 0; i < pets.Length; i++)
			{
				var pet = pets[i];
				if (pet != null)
				{
					if (pet.get_itemid () == petid)
					{
						return true;
					}
				}
			}
			return false;
		}
		public virtual bool is_invincible ()
		{
			return invincible == false;
		}

		public bool is_sitting ()
		{
			return state == State.SIT;
		}

		public bool is_climbing ()
		{
			return state == State.LADDER || state == State.ROPE;
		}

		public bool is_twohanded ()
		{
			return look.get_equips ().is_twohanded ();
		}

		public Weapon.Type get_weapontype ()
		{
			int weapon_id = look.get_equips ().get_weapon ();

			if (weapon_id <= 0)
				return Weapon.Type.NONE;

			return WeaponData.get (weapon_id).get_type ();
		}

		public bool getflip ()
		{
			return facing_right;
		}

		public string get_name ()
		{
			return namelabel.get_text ();
			//return string.Empty;
		}


		// Obtain a reference to this character's look
		public CharLook get_look ()
		{
			return look;
		}

		// Return a reference to this characters's physics
		public PhysicsObject get_phobj ()
		{
			return phobj;
		}

		public static void init ()
		{
			CharLook.init ();

			var file_BasicEffimg = ms.wz.wzFile_effect["BasicEff.img"];
			foreach (var iter in CharEffect.PATHS_One)
			{
				if (iter.Value != null)
				{
					chareffects[iter.Key] = new Animation (file_BasicEffimg[iter.Value]);
				}
			}

			foreach (var iter in CharEffect.PATHS_Two)
			{
				if (iter.Value != null)
				{
					var pathSplit = iter.Value.Split ('/');

					chareffects[iter.Key] = new Animation (file_BasicEffimg[pathSplit[0]][pathSplit[1]]);
				}
			}
		}

		public const short charWidth = 40;
		public const short charHeight = 90;
		public Point_short charHalf = new Point_short (charWidth / 2, charHeight / 2);

		/// <summary>
		/// return char Rectangle range
		/// </summary>
		/// <returns></returns>
		public Rectangle_short bounds ()
		{
			var chaPos = absp;

			AppDebug.Log ($"chaPos:{chaPos}");
			short l = (short)(chaPos.x () - charWidth / 2);
			short t = (short)(chaPos.y () - charHeight);
			short r = (short)(chaPos.x () + charWidth / 2);
			short b = (short)(chaPos.y ());
			return new Rectangle_short (l, r, t, b);
		}

		public Point_short headBarPos ()
		{
			return absp + new Point_short (0, -charHeight);
		}

		private static EnumMap<CharEffect.Id, Animation> chareffects = new EnumMap<CharEffect.Id, Animation> ();

		public abstract sbyte get_integer_attackspeed ();

		public abstract ushort get_level ();

		public abstract int get_skilllevel (int skillid);

		public void TrySet_CharShadowLook ()
		{
			//如果是无影人 才初始化 影子外观
			//if ((this is Player player && player.get_job ().isA(MapleJob.HERMIT))||(this is OtherChar otherChar && otherChar.get_job ().isA(MapleJob.HERMIT)))
			if (get_job ().isA (MapleJob.HERMIT))
			{
				_charShadowLook = new CharShadowLook ();
			}
		}
		public Char (int o, CharLook lk, string name) : base (o, new Point_short ())
		{
			look = (lk);
			look_preview = (lk);
			namelabel = new Text (Text.Font.A13M, Text.Alignment.CENTER, Color.Name.WHITE, Text.Background.NAMETAG, name);

			behaviorTreeOwner = MapGameObject.AddComponent<BehaviourTreeOwner> ();
			behaviorTreeOwner.blackboard = MapGameObject.AddComponent<Blackboard> ();
			behaviorTreeOwner.repeat = false;

			look.OnActionChanged += () => { set_afterimage (current_skill_id); };

			
			
		}

		public abstract Job get_job ();

		public override void Dispose ()
		{
			base.Dispose ();
			look.OnActionChanged = null;
			
			effects.Dispose ();
			afterimage.Dispose ();
			look.Dispose ();
			chatballoon.Dispose ();
			_charShadowLook?.Dispose ();
		}

		public CharLook look = new CharLook ();
		public CharLook look_preview = new CharLook ();
		private CharShadowLook _charShadowLook;
		public PetLook[] pets = new PetLook[3];

		public State state;
		public bool attacking;
		public bool facing_right;

		public Point_short absp;


		private Text namelabel;
		private ChatBalloon chatballoon = new ChatBalloon ();
		private EffectLayer effects = new EffectLayer ();
		private Afterimage afterimage = new Afterimage ();
		private TimedBool invincible = new TimedBool ();

		private TimedBool ironbody = new TimedBool ();
		private LinkedList<DamageNumber> damagenumbers = new LinkedList<DamageNumber> ();

		BehaviourTreeOwner behaviorTreeOwner;
		public BehaviourTreeOwner GetBehaviourTreeOwner () => behaviorTreeOwner;

		//private string skillBTreePath = "SkillBTree/Skill_";
		//private Dictionary<string, Graph> skillBTreePool = new Dictionary<string, Graph> ();
		/*public void PlaySkillBTree (string skillId)
		{
			if (!skillBTreePool.TryGetValue (skillId, out var externalBehaviorTree))
			{
				externalBehaviorTree = Resources.Load<Graph> ($"{skillBTreePath}{skillId}");
				skillBTreePool.Add (skillId, externalBehaviorTree);
			}

			behaviorTreeOwner.graph = externalBehaviorTree;
			behaviorTreeOwner.StartBehaviour ();
		}*/
		public bool PlaySkillBTree (BehaviourTree BTree, SpecialMove move, int move_id)
		{
			bool result = false;
			if (BTree != null)
			{
				var StopBehaviourWhenEnter = BTree.blackboard.GetVariable<bool> ("StopBehaviourWhenEnter")?.value??false;
				if (StopBehaviourWhenEnter)
				{
					behaviorTreeOwner.StopBehaviour ();
					//AppDebug.LogError ($"StopBehaviour");

				}

				//behaviorTreeOwner.blackboard.SetVariableValue(GetType().Name,this);
				behaviorTreeOwner.repeat = false;
				//AppDebug.LogError ($"after1 graph: {behaviorTreeOwner.graph?.GetHashCode ()}");

				//behaviorTreeOwner.graph = BTree;
				//AppDebug.LogError ($"after2 graph: {behaviorTreeOwner.graph?.GetHashCode ()}");

				behaviorTreeOwner.StartBehaviour (BTree);//todo 会把传入的graph产生一个新实例，所以设置变量要后设置

				behaviorTreeOwner.graph.blackboard.SetVariableValue (typeof (SpecialMove).Name, move);
				behaviorTreeOwner.graph.blackboard.SetVariableValue ("InputSD", move_id);
				//AppDebug.LogError ($"after3 graph: {behaviorTreeOwner.graph?.GetHashCode ()}");
				//AppDebug.LogError ($"PlaySkillGraph blackboard:{graph.blackboard.GetHashCode ()}\tbehaviorTreeOwner.blackboard:{behaviorTreeOwner.blackboard.GetHashCode ()}\tbehaviorTreeOwner.graph.blackboard:{behaviorTreeOwner.graph.blackboard.GetHashCode()}\t InputSD:{behaviorTreeOwner.graph.blackboard.GetVariable<int> ("InputSD").value}");
				result = true;
			}
			return result;
		}
	}
}