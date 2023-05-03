using System;
using System.Collections.Generic;
using System.Threading;

/*
 This file is part of the OdinMS Maple Story Server
 Copyright (C) 2008 Patrick Huy <patrick.huy@frz.cc>
 Matthias Butz <matze@odinms.de>
 Jan Christian Meyer <vimes@odinms.de>

 This program is free software: you can redistribute it and/or modify
 it under the terms of the GNU Affero General Public License as
 published by the Free Software Foundation version 3 as published by
 the Free Software Foundation. You may not use, modify or distribute
 this program under any other version of the GNU Affero General Public
 License.

 This program is distributed in the hope that it will be useful,
 but WITHOUT ANY WARRANTY; without even the implied warranty of
 MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 GNU Affero General Public License for more details.

 You should have received a copy of the GNU Affero General Public License
 along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */
namespace server.life
{
	/*using MapleBuffStat = client.MapleBuffStat;
	using MapleCharacter = client.MapleCharacter;
	using MapleClient = client.MapleClient;
	using MapleFamilyEntry = client.MapleFamilyEntry;
	using MapleJob = client.MapleJob;
	using Skill = client.Skill;
	using SkillFactory = client.SkillFactory;
	using MonsterStatus = client.status.MonsterStatus;
	using MonsterStatusEffect = client.status.MonsterStatusEffect;
	using YamlConfig = config.YamlConfig;
	using Crusader = constants.skills.Crusader;
	using FPMage = constants.skills.FPMage;
	using Hermit = constants.skills.Hermit;
	using ILMage = constants.skills.ILMage;
	using NightLord = constants.skills.NightLord;
	using NightWalker = constants.skills.NightWalker;
	using Priest = constants.skills.Priest;
	using Shadower = constants.skills.Shadower;
	using WhiteKnight = constants.skills.WhiteKnight;
	using MonitoredReentrantLock = net.server.audit.locks.MonitoredReentrantLock;
	using Channel = net.server.channel.Channel;
	using MapleParty = net.server.world.MapleParty;
	using MaplePartyCharacter = net.server.world.MaplePartyCharacter;
	using EventInstanceManager = scripting.@event.EventInstanceManager;
	using BanishInfo = server.life.MapleLifeFactory.BanishInfo;
	using MapleMap = server.maps.MapleMap;
	using MapleMapObjectType = server.maps.MapleMapObjectType;
	using IntervalBuilder = tools.IntervalBuilder;
	using MaplePacketCreator = tools.MaplePacketCreator;
	using Pair = tools.Pair;
	using Randomizer = tools.Randomizer;
	using LockCollector = net.server.audit.LockCollector;
	using MonitoredLockType = net.server.audit.locks.MonitoredLockType;
	using MonitoredReentrantLockFactory = net.server.audit.locks.factory.MonitoredReentrantLockFactory;
	using ChannelServices = net.server.services.type.ChannelServices;
	using MobAnimationService = net.server.services.task.channel.MobAnimationService;
	using MobClearSkillService = net.server.services.task.channel.MobClearSkillService;
	using MobStatusService = net.server.services.task.channel.MobStatusService;
	using OverallService = net.server.services.task.channel.OverallService;
	using MapleMonsterAggroCoordinator = net.server.coordinator.world.MapleMonsterAggroCoordinator;
	using MapleLootManager = server.loot.MapleLootManager;
	using MapleSummon = server.maps.MapleSummon;*/

	public class MapleMonster
	{

		private int mid;
		private MapleMonsterStats stats;
		public MapleMonster (int mid, MapleMonsterStats stats)
		{
			this.mid = mid;
			this.stats = stats;
		}
	}
}
