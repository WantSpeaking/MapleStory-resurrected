using System;
using System.Collections.Generic;
using System.Threading;
using MapleLib.WzLib;

namespace server.life
{
	/*using MapleCharacter = client.MapleCharacter;
	using MapleDisease = client.MapleDisease;
	using MonsterStatus = client.status.MonsterStatus;
	using GameConstants = constants.game.GameConstants;
	using ChannelServices = net.server.services.type.ChannelServices;
	using OverallService = net.server.services.task.channel.OverallService;*/
	using Randomizer = tools.Randomizer;
	/*using MapleMap = server.maps.MapleMap;
	using MapleMapObject = server.maps.MapleMapObject;
	using MapleMapObjectType = server.maps.MapleMapObjectType;
	using MapleMist = server.maps.MapleMist;
	using ArrayMap = tools.ArrayMap;*/

	/// 
	/// <summary>
	/// @author Danny (Leifde)
	/// </summary>
	public class MobSkill
	{

		private int skillId, skillLevel, mpCon;
		private IList<int> toSummon = new List<int>();
		private int spawnEffect, hp, x, y;
		private long duration, cooltime;
		private float prop;
		private Point lt, rb;
		private int limit;

		public MobSkill(int skillId, int level)
		{
			this.skillId = skillId;
			this.skillLevel = level;
		}

		public virtual int MpCon
		{
			set
			{
				this.mpCon = value;
			}
			get
			{
				return mpCon;
			}
		}

		public virtual void addSummons(IList<int> toSummon)
		{
			foreach (int summon in toSummon)
			{
				this.toSummon.Add(summon);
			}
		}

		public virtual int SpawnEffect
		{
			set
			{
				this.spawnEffect = value;
			}
			get
			{
				return spawnEffect;
			}
		}

		public virtual int Hp
		{
			set
			{
				this.hp = value;
			}
		}

		public virtual int X
		{
			set
			{
				this.x = value;
			}
			get
			{
				return x;
			}
		}

		public virtual int Y
		{
			set
			{
				this.y = value;
			}
			get
			{
				return y;
			}
		}

		public virtual long Duration
		{
			set
			{
				this.duration = value;
			}
			get
			{
				return duration;
			}
		}

		public virtual long CoolTime
		{
			set
			{
				this.cooltime = value;
			}
			get
			{
				return cooltime;
			}
		}

		public virtual float Prop
		{
			set
			{
				this.prop = value;
			}
		}

		public virtual void setLtRb(Point lt, Point rb)
		{
			this.lt = lt;
			this.rb = rb;
		}

		public virtual int Limit
		{
			set
			{
				this.limit = value;
			}
			get
			{
				return limit;
			}
		}

/*//JAVA TO C# CONVERTER WARNING: 'final' parameters are ignored unless the option to convert to C# 7.2 'in' parameters is selected:
//ORIGINAL LINE: public void applyDelayedEffect(final client.MapleCharacter player, final MapleMonster monster, final boolean skill, int animationTime)
		public virtual void applyDelayedEffect(MapleCharacter player, MapleMonster monster, bool skill, int animationTime)
		{
			ThreadStart toRun = () =>
			{
			if (monster.Alive)
			{
				applyEffect(player, monster, skill, null);
			}
			};

			OverallService service = (OverallService) monster.Map.ChannelServer.getServiceAccess(ChannelServices.OVERALL);
			service.registerOverallAction(monster.Map.Id, toRun, animationTime);
		}

		public virtual void applyEffect(MapleCharacter player, MapleMonster monster, bool skill, IList<MapleCharacter> banishPlayers)
		{
			MapleDisease disease = null;
			IDictionary<MonsterStatus, int> stats = new ArrayMap<MonsterStatus, int>();
			IList<int> reflection = new LinkedList<int>();
			switch (skillId)
			{
				case 100:
				case 110:
				case 150:
					stats[MonsterStatus.WEAPON_ATTACK_UP] = Convert.ToInt32(x);
					break;
				case 101:
				case 111:
				case 151:
					stats[MonsterStatus.MAGIC_ATTACK_UP] = Convert.ToInt32(x);
					break;
				case 102:
				case 112:
				case 152:
					stats[MonsterStatus.WEAPON_DEFENSE_UP] = Convert.ToInt32(x);
					break;
				case 103:
				case 113:
				case 153:
					stats[MonsterStatus.MAGIC_DEFENSE_UP] = Convert.ToInt32(x);
					break;
				case 114:
					if (lt != null && rb != null && skill)
					{
						IList<MapleMapObject> objects = getObjectsInRange(monster, MapleMapObjectType.MONSTER);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int hps = (getX() / 1000) * (int)(950 + 1050 * Math.random());
						int hps = (X / 1000) * (int)(950 + 1050 * GlobalRandom.NextDouble);
						foreach (MapleMapObject mons in objects)
						{
							((MapleMonster) mons).heal(hps, Y);
						}
					}
					else
					{
						monster.heal(X, Y);
					}
					break;
				case 120:
					disease = MapleDisease.SEAL;
					break;
				case 121:
					disease = MapleDisease.DARKNESS;
					break;
				case 122:
					disease = MapleDisease.WEAKEN;
					break;
				case 123:
					disease = MapleDisease.STUN;
					break;
				case 124:
					disease = MapleDisease.CURSE;
					break;
				case 125:
					disease = MapleDisease.POISON;
					break;
				case 126: // Slow
					disease = MapleDisease.SLOW;
					break;
				case 127:
					if (lt != null && rb != null && skill)
					{
						foreach (MapleCharacter character in getPlayersInRange(monster))
						{
							character.dispel();
						}
					}
					else
					{
						player.dispel();
					}
					break;
				case 128: // Seduce
					disease = MapleDisease.SEDUCE;
					break;
				case 129: // Banish
					if (lt != null && rb != null && skill)
					{
						foreach (MapleCharacter chr in getPlayersInRange(monster))
						{
							banishPlayers.Add(chr);
						}
					}
					else
					{
						banishPlayers.Add(player);
					}
					break;
				case 131: // Mist
					monster.Map.spawnMist(new MapleMist(calculateBoundingBox(monster.Position), monster, this), x * 100, false, false, false);
					break;
				case 132:
					disease = MapleDisease.CONFUSE;
					break;
				case 133: // zombify
					disease = MapleDisease.ZOMBIFY;
					break;
				case 140:
					if (makeChanceResult() && !monster.isBuffed(MonsterStatus.MAGIC_IMMUNITY))
					{
						stats[MonsterStatus.WEAPON_IMMUNITY] = Convert.ToInt32(x);
					}
					break;
				case 141:
					if (makeChanceResult() && !monster.isBuffed(MonsterStatus.WEAPON_IMMUNITY))
					{
						stats[MonsterStatus.MAGIC_IMMUNITY] = Convert.ToInt32(x);
					}
					break;
				case 143: // Weapon Reflect
					stats[MonsterStatus.WEAPON_REFLECT] = 10;
					stats[MonsterStatus.WEAPON_IMMUNITY] = 10;
					reflection.Add(x);
					break;
				case 144: // Magic Reflect
					stats[MonsterStatus.MAGIC_REFLECT] = 10;
					stats[MonsterStatus.MAGIC_IMMUNITY] = 10;
					reflection.Add(x);
					break;
				case 145: // Weapon / Magic reflect
					stats[MonsterStatus.WEAPON_REFLECT] = 10;
					stats[MonsterStatus.WEAPON_IMMUNITY] = 10;
					stats[MonsterStatus.MAGIC_REFLECT] = 10;
					stats[MonsterStatus.MAGIC_IMMUNITY] = 10;
					reflection.Add(x);
					break;
				case 154:
					stats[MonsterStatus.ACC] = Convert.ToInt32(x);
					break;
				case 155:
					stats[MonsterStatus.AVOID] = Convert.ToInt32(x);
					break;
				case 156:
					stats[MonsterStatus.SPEED] = Convert.ToInt32(x);
					break;
				case 200: // summon
					int skillLimit = this.Limit;
					MapleMap map = monster.Map;

					if (GameConstants.isDojo(map.Id))
					{ // spawns in dojo should be unlimited
						skillLimit = int.MaxValue;
					}

					if (map.SpawnedMonstersOnMap < 80)
					{
						IList<int> summons = Summons;
						int summonLimit = monster.countAvailableMobSummons(summons.Count, skillLimit);
						if (summonLimit >= 1)
						{
							bool bossRushMap = GameConstants.isBossRush(map.Id);

							Collections.shuffle(summons);
							foreach (int? mobId in summons.subList(0, summonLimit))
							{
								MapleMonster toSpawn = MapleLifeFactory.getMonster(mobId);
								if (toSpawn != null)
								{
									if (bossRushMap)
									{
										toSpawn.disableDrops(); // no littering on BRPQ pls
									}
									toSpawn.Position = monster.Position;
									int ypos, xpos;
									xpos = (int) monster.Position.X;
									ypos = (int) monster.Position.Y;
									switch (mobId)
									{
										case 8500003: // Pap bomb high
											toSpawn.Fh = (int) Math.Ceiling(GlobalRandom.NextDouble * 19.0);
											ypos = -590;
											break;
										case 8500004: // Pap bomb
											xpos = (int)(monster.Position.X + Randomizer.Next(1000) - 500);
											if (ypos != -590)
											{
												ypos = (int) monster.Position.Y;
											}
											break;
										case 8510100: //Pianus bomb
											if (Math.Ceiling(GlobalRandom.NextDouble * 5) == 1)
											{
												ypos = 78;
												xpos = (int) Randomizer.Next(5) + (Randomizer.Next(2) == 1 ? 180 : 0);
											}
											else
											{
												xpos = (int)(monster.Position.X + Randomizer.Next(1000) - 500);
											}
											break;
									}
									switch (map.Id)
									{
										case 220080001: //Pap map
											if (xpos < -890)
											{
												xpos = (int)(Math.Ceiling(GlobalRandom.NextDouble * 150) - 890);
											}
											else if (xpos > 230)
											{
												xpos = (int)(230 - Math.Ceiling(GlobalRandom.NextDouble * 150));
											}
											break;
										case 230040420: // Pianus map
											if (xpos < -239)
											{
												xpos = (int)(Math.Ceiling(GlobalRandom.NextDouble * 150) - 239);
											}
											else if (xpos > 371)
											{
												xpos = (int)(371 - Math.Ceiling(GlobalRandom.NextDouble * 150));
											}
											break;
									}
									toSpawn.Position = new Point(xpos, ypos);
									if (toSpawn.Id == 8500004)
									{
										map.spawnFakeMonster(toSpawn);
									}
									else
									{
										map.spawnMonsterWithEffect(toSpawn, SpawnEffect, toSpawn.Position);
									}
									monster.addSummonedMob(toSpawn);
								}
							}
						}
					}
					break;
				default:
					Console.WriteLine("Unhandled Mob skill: " + skillId);
					break;
			}
			if (stats.Count > 0)
			{
				if (lt != null && rb != null && skill)
				{
					foreach (MapleMapObject mons in getObjectsInRange(monster, MapleMapObjectType.MONSTER))
					{
						((MapleMonster) mons).applyMonsterBuff(stats, X, SkillId, Duration, this, reflection);
					}
				}
				else
				{
					monster.applyMonsterBuff(stats, X, SkillId, Duration, this, reflection);
				}
			}
			if (disease != null)
			{
				if (lt != null && rb != null && skill)
				{
					int i = 0;
					foreach (MapleCharacter character in getPlayersInRange(monster))
					{
						if (!character.hasActiveBuff(2321005))
						{ // holy shield
							if (disease.Equals(MapleDisease.SEDUCE))
							{
								if (i < 10)
								{
									character.giveDebuff(MapleDisease.SEDUCE, this);
									i++;
								}
							}
							else
							{
								character.giveDebuff(disease, this);
							}
						}
					}
				}
				else
				{
					player.giveDebuff(disease, this);
				}
			}
		}
*/
		/*private IList<MapleCharacter> getPlayersInRange(MapleMonster monster)
		{
			return monster.Map.getPlayersInRange(calculateBoundingBox(monster.Position));
		}*/

		public virtual int SkillId
		{
			get
			{
				return skillId;
			}
		}

		public virtual int SkillLevel
		{
			get
			{
				return skillLevel;
			}
		}


		public virtual IList<int> Summons
		{
			get
			{
				return new List<int>(toSummon);
			}
		}


		public virtual int HP
		{
			get
			{
				return hp;
			}
		}





		public virtual Point Lt
		{
			get
			{
				return lt;
			}
		}

		public virtual Point Rb
		{
			get
			{
				return rb;
			}
		}


		public virtual bool makeChanceResult()
		{
			return prop == 1.0 || GlobalRandom.NextDouble < prop;
		}

		private Rectangle calculateBoundingBox(Point posFrom)
		{
			Point mYlt = new Point(lt.X + posFrom.X, lt.Y + posFrom.Y);
			Point mYrb = new Point(rb.X + posFrom.X, rb.Y + posFrom.Y);
			Rectangle bounds = new Rectangle(mYlt.X, mYlt.Y, mYrb.X - mYlt.X, mYrb.Y - mYlt.Y);
			return bounds;
		}

		/*private IList<MapleMapObject> getObjectsInRange(MapleMonster monster, MapleMapObjectType objectType)
		{
			return monster.Map.getMapObjectsInBox(calculateBoundingBox(monster.Position), Collections.singletonList(objectType));
		}*/
	}

}

//Helper class added by Java to C# Converter:

//---------------------------------------------------------------------------------------------------------
//	Copyright © 2007 - 2019 Tangible Software Solutions, Inc.
//	This class can be used by anyone provided that the copyright notice remains intact.
//
//	This class is used to replace calls to the static java.lang.Math.random method.
//---------------------------------------------------------------------------------------------------------
internal static class GlobalRandom
{
	private static System.Random randomInstance = null;

	public static double NextDouble
	{
		get
		{
			if (randomInstance == null)
				randomInstance = new System.Random();

			return randomInstance.NextDouble();
		}
	}
}
