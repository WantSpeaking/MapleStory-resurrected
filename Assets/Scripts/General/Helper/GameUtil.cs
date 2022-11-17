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
}
