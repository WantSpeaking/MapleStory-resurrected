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

	public string sortingLayerName_Background = "Background";
    public string sortingLayerName_Layer0 = "Layer0";
    public string sortingLayerName_Layer1 = "Layer1";
    public string sortingLayerName_Layer2 = "Layer2";
    public string sortingLayerName_Layer3 = "Layer3";
    public string sortingLayerName_Layer4 = "Layer4";
    public string sortingLayerName_Layer5 = "Layer5";
    public string sortingLayerName_Layer6 = "Layer6";
    public string sortingLayerName_Layer7 = "Layer7";
    public string sortingLayerName_Layer8 = "Layer8";
    public string sortingLayerName_Portal = "Portal";
    public string sortingLayerName_Foreground = "Foreground";
    public string sortingLayerName_DefaultPlayer = "Layer8";

    public const string sortingLayerName_Default = "Layer7";
	public const string layerNameDefault = "Default";

	[Tooltip("true 使用wz画Obj，false使用ab")]
	public bool Use_wz_or_ab_draw_ObjTile = true;
	public bool Use_Unlit_or_Pixelate_Material = true;
    public bool Use_ab_or_assetDatabase = true;

    public WZProviderType wZProviderType = WZProviderType.wz;
    public string msLayerToSortingLayerName (ms.Layer.Id id)
	{
		var n = sortingLayerName_Layer0;
		switch (id)
		{
			case ms.Layer.Id.ZERO:
				n = sortingLayerName_Layer0;
                break;
			case ms.Layer.Id.ONE:
                n = sortingLayerName_Layer1;
                break;
			case ms.Layer.Id.TWO:
                n = sortingLayerName_Layer2;
                break;
			case ms.Layer.Id.THREE:
                n = sortingLayerName_Layer3;
                break;
			case ms.Layer.Id.FOUR:
                n = sortingLayerName_Layer4;
                break;
			case ms.Layer.Id.FIVE:
                n = sortingLayerName_Layer5;
                break;
			case ms.Layer.Id.SIX:
                n = sortingLayerName_Layer6;
                break;
			case ms.Layer.Id.SEVEN:
                n = sortingLayerName_Layer7;
                break;
			default:
				break;
		}
		return n;
	}

	public void LogTime (string message)
	{
		stopwatch.Stop();
		AppDebug.Log($"{message} used time:{stopwatch.ElapsedMilliseconds}");
		stopwatch.Restart();
    }
}

public enum WZProviderType
{
	wz,
	xml,
	jason
}