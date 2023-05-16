using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class GameUtil : SingletonMono<GameUtil>
{
	public int DrawOrder;
	public bool enableDebugPacket = true;
	public bool TestMode = true;
	public string TestAccount = "admin2";
	public string TestPassword = "admin2";
	public string Version = "2023/4/8/18/06";
	public bool drawMap = true;
	public bool enableUpdate = true;
	public UnityEngine.Camera mainCamera;
	[FormerlySerializedAs ("testTalkNpcId")] public int testTalkNpcId_Instance = 2091005;
	public int testTalkNpcId_FunctionCenter = 2091006;
	[FormerlySerializedAs ("testTalkNpcFileName")] public string testTalkNpcFileName_FunctionCenter = "extend/10200";
}
