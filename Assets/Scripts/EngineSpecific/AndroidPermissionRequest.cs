using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Android;

public class AndroidPermissionRequest : MonoBehaviour
{
	public List<string> RequestedPermissions1 = new List<string> () { Permission.ExternalStorageWrite, Permission.ExternalStorageRead };
	public Dictionary<string, AndroidRuntimePermissions.Permission> permissionString_Result = new Dictionary<string, AndroidRuntimePermissions.Permission> ();
	static int getSDKInt ()
	{
#if UNITY_EDITOR
		return 0;
#else
		using (var version = new AndroidJavaClass ("android.os.Build$VERSION"))
		{
			return version.GetStatic<int> ("SDK_INT");
		}
#endif
	}

	private static int REQUEST_CODE = 1024;
	public static string ACTION_MANAGE_APP_ALL_FILES_ACCESS_PERMISSION = "android.settings.MANAGE_APP_ALL_FILES_ACCESS_PERMISSION";
	// Start is called before the first frame update
	void Awake ()
	{
		AndroidRuntimePermissions.Permission[] result_All = AndroidRuntimePermissions.RequestPermissions (RequestedPermissions1.ToArray ());
		
		var result_NotGranted = permissionString_Result.Where (p => p.Value == AndroidRuntimePermissions.Permission.ShouldAsk || p.Value == AndroidRuntimePermissions.Permission.Denied).ToArray ();
		Not_Granted = result_NotGranted.Any ();
		var p_ShouldAsk = "请稍后打开应用管理设置，开启以下权限。权限请求结果：\r\n";
		foreach (var pair in permissionString_Result)
		{
			p_ShouldAsk += $"权限：{pair.Key}\r\n";
		}
		message = p_ShouldAsk;

#if UNITY_ANDROID
		if (getSDKInt () >= 30)
		{
			//Environment.isExternalStorageManager ()
			AndroidJavaClass Environment = new AndroidJavaClass ("android.os.Environment");
			var isExternalStorageManager = Environment.CallStatic<bool> ("isExternalStorageManager");
			if (!isExternalStorageManager)
			{
				AndroidJavaClass UnityPlayer = new AndroidJavaClass ("com.unity3d.player.UnityPlayer");
				var activity = UnityPlayer.GetStatic<AndroidJavaObject> ("currentActivity");

				AndroidJavaObject intent = new AndroidJavaObject ("android.content.Intent", ACTION_MANAGE_APP_ALL_FILES_ACCESS_PERMISSION);
				// 传入类的完整包名
				AndroidJavaClass Uri = new AndroidJavaClass ("android.net.Uri");
				var UriObj = Uri.CallStatic<AndroidJavaObject> ("parse", "package:" + Application.identifier);
				//Debug.Log ("package:" + Application.identifier);
				intent.Call<AndroidJavaObject> ("setData", UriObj);
				activity.Call ("startActivityForResult", intent, REQUEST_CODE);
			}
		}
#endif
	}

	IEnumerator CopyFile ()
	{
		message = "数据解压中，请耐性等待";
		yield return new WaitForSeconds (2f);

		//测试
		//CopyStreamingAssetToPersistent.CopyFile ("Base.wz", destinationFolderPath);

		/*	CopyStreamingAssetToPersistent.CopyFile ("Map.wz", destinationFolderPath);
			CopyStreamingAssetToPersistent.CopyFile ("Sound.wz", destinationFolderPath);
			CopyStreamingAssetToPersistent.CopyFile ("UI_New.wz", destinationFolderPath);*/

		/*		CopyStreamingAssetToPersistent.CopyFile ("Settings", destinationFolderPath);
				CopyStreamingAssetToPersistent.CopyFile ("Base.wz", destinationFolderPath);
				CopyStreamingAssetToPersistent.CopyFile ("Character1.wz", destinationFolderPath);
				CopyStreamingAssetToPersistent.CopyFile ("Effect.wz", destinationFolderPath);
				CopyStreamingAssetToPersistent.CopyFile ("Etc.wz", destinationFolderPath);
				CopyStreamingAssetToPersistent.CopyFile ("Item.wz", destinationFolderPath);
				CopyStreamingAssetToPersistent.CopyFile ("List.wz", destinationFolderPath);
				CopyStreamingAssetToPersistent.CopyFile ("Map001.wz", destinationFolderPath);
				CopyStreamingAssetToPersistent.CopyFile ("MapLatest.wz", destinationFolderPath);
				CopyStreamingAssetToPersistent.CopyFile ("Mob.wz", destinationFolderPath);
				CopyStreamingAssetToPersistent.CopyFile ("Morph.wz", destinationFolderPath);
				CopyStreamingAssetToPersistent.CopyFile ("Npc.wz", destinationFolderPath);
				CopyStreamingAssetToPersistent.CopyFile ("Quest.wz", destinationFolderPath);
				CopyStreamingAssetToPersistent.CopyFile ("Reactor.wz", destinationFolderPath);
				CopyStreamingAssetToPersistent.CopyFile ("Skill.wz", destinationFolderPath);
				CopyStreamingAssetToPersistent.CopyFile ("String.wz", destinationFolderPath);
				CopyStreamingAssetToPersistent.CopyFile ("UI_Endless.wz", destinationFolderPath);*/

		message = "数据解压完成，可以退出并卸载该数据包";
	}
	
	public int fontSize = 20;
	string message;
	bool Not_Granted;
	private void OnGUI ()
	{
		GUI.skin.label.fontSize = fontSize;
		GUI.skin.button.fontSize = fontSize;
		GUILayout.BeginScrollView (new Vector2 ());
		GUILayout.BeginVertical ();
		//GUILayout.Label (message);
		if (Not_Granted)
		{
			if (GUILayout.Button (message))
			{
				AndroidRuntimePermissions.OpenSettings ();
				Not_Granted = true;
			}
		}
		GUILayout.EndVertical ();
		GUILayout.EndScrollView ();
	}
}
