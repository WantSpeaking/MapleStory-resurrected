using System;
using System.Collections;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;
using ms;
using UnityEngine;


public class AndroidIntentCallToUnZipData : MonoBehaviour
{
	private void OnApplicationFocus (bool focus)
	{
#if (!UNITY_EDITOR && UNITY_ANDROID)
		if (focus)
		{
			StartCoroutine (CreatePushClass (new AndroidJavaClass ("com.unity3d.player.UnityPlayer")));
		}
#endif
	}

	public string destinationPath = "/storage/emulated/0/ForeverStory";
	public string password = "Forever~Story2022";
	public IEnumerator CreatePushClass (AndroidJavaClass UnityPlayer)
	{


		AndroidJavaObject currentActivity = UnityPlayer.GetStatic<AndroidJavaObject> ("currentActivity");
		AndroidJavaObject intent = currentActivity.Call<AndroidJavaObject> ("getIntent");

		var intentPath = intent.Call<string> ("getDataString")?.ToString ();
		var getAction = intent.Call<string> ("getAction")?.ToString ();
		var uriObj = intent.Call<AndroidJavaObject> ("getData");
		var uriPath = uriObj?.Call<string> ("getPath")?.ToString ();
		Debug.Log ($"getDataString:{intentPath}\t getAction:{getAction}\t uriPath:{uriPath}");

		if (getAction == "android.intent.action.VIEW")
		{
			if (!string.IsNullOrEmpty (intentPath))
			{
				message = "��Դ��ѹ�У������ĵȴ���Լ3����";
				yield return new WaitForSeconds (2f);
				/*				var index = intentPath.IndexOf ("storage");
								if (index != -1)
								{
									var storagePath = intentPath.Substring (index);
									Debug.Log ($"storagePath:{storagePath}");
									return null;
								}*/

				try
				{
					if (!Directory.Exists (destinationPath))
					{
						Directory.CreateDirectory (destinationPath);
					}
					//System.IO.Compression.ZipFile.ExtractToDirectory (storagePath, destinationPath, true); //��ѹ
					if (uriPath.Contains ("/external_files"))
					{
						uriPath = uriPath.Replace ("/external_files", "");
					}

					// /root/storage
					if (uriPath.Contains ("/storage"))
					{
						uriPath = uriPath.Substring (uriPath.IndexOf ("/storage"));
					}

					if (uriPath.Contains (".zip"))
					{
						TestUnZipFile (uriPath, destinationPath, password);
						//message = "��ѹ���,�뵽/storage/emulated/0/ForeverStoryĿ¼�鿴��ѹ���ļ�";
					}
					else
					{
						TestCopyFile(uriPath, destinationPath, password);
					}
					
					message = String.Empty;
					unzipFinished.set_for (6000);
				}
				catch (Exception ex)
				{
					Debug.LogError ($"ex:{ex.Message}");
				}
			}
		}

		CopyStreamingAssetToPersistent.CopyFile ("Quest.wz", destinationPath);
	}

	public static void TestUnZipFile (string zipPath, string outPath, string password)
	{
		if (!File.Exists (zipPath))
		{
			Debug.LogError ("û�д��ļ�·����" + zipPath);
			return;
		}
		using (ZipInputStream stream = new ZipInputStream (File.OpenRead (zipPath)))
		{
			stream.Password = password;
			ZipEntry theEntry;
			while ((theEntry = stream.GetNextEntry ()) != null)
			{

				// Debug.Log ("theEntry.Name��" + theEntry.Name);
				string fileName = Path.GetFileName (theEntry.Name);
				// Debug.Log ("fileName��" + fileName);
				string filePath = Path.Combine (outPath, theEntry.Name);
				// Debug.Log ("filePath:" + filePath);
				string directoryName = Path.GetDirectoryName (filePath);
				// Debug.Log ("directoryName��" + directoryName);

				// ����ѹ���ļ����ļ���λ��
				if (directoryName.Length > 0)
				{
					Directory.CreateDirectory (directoryName);
				}
				if (fileName != String.Empty)
				{
					using (FileStream streamWriter = File.Create (filePath))
					{
						int size = 2048;
						byte[] data = new byte[2048];
						while (true)
						{
							size = stream.Read (data, 0, data.Length);
							if (size > 0)
							{
								streamWriter.Write (data, 0, size);
							}
							else
							{
								// Debug.Log (theEntry.Name+"��ѹ��ɣ�");
								break;
							}
						}
					}
				}
			}
			Debug.Log ($"��ѹ��ɣ���ѹ����{outPath}");
		}
	}

	public static void TestCopyFile (string zipPath, string outPath, string password)
	{
		if (!File.Exists (zipPath))
		{
			Debug.LogError ("û�д��ļ�·����" + zipPath);
			return;
		}
		using (FileStream streamReader = File.Create (zipPath))
		{
			using (FileStream streamWriter = File.Create (outPath))
			{
				streamReader.CopyTo (streamWriter);
			}
		}
	}
	
	public int fontSize = 20;
	string message;
	TimedBool unzipFinished = new TimedBool ();
	bool hasStartZip;
	private void OnGUI ()
	{
		unzipFinished.update ();

		GUI.skin.label.fontSize = fontSize;
		GUILayout.BeginVertical ();
		GUILayout.Label (message);

		if (unzipFinished)
		{
			GUILayout.Label ("���ݽ�ѹ���, ������ʼ��Ϸ��ť");
		}
		GUILayout.EndVertical ();
	}
}
