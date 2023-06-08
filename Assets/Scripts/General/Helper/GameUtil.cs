using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
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
	public int testTalkNpcId_Instance = 2091005;
	public int testTalkNpcId_FunctionCenter = 2091006;
	public string testTalkNpcFileName_FunctionCenter = "extend/10200";

    public Stopwatch stopwatch = new Stopwatch();
	public string debugMessage = "";

	public bool sendAttackPacket = true;

	public float spawnMobInterval = 0.2f;
    public float spawnNPCInterval = 0.5f;

	public int bgDrawX;
    public int bgDrawY;

	public bool PrintPlayerPos;

	public float combo_radius = 5f;
	public float superCombo_radius = 5f;
    public float combo_roate_speed = 1f;
	public short combo_buff_attack_increase_per = 100;
}
