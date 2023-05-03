using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}
