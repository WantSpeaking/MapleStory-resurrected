using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using CL.IO.Zip;
using ICSharpCode.SharpZipLib.Zip;
using ms;
using UnityEngine;
using UnityEngine.Networking;
//using Puerts;
using UnityEngine.TextCore.Text;

public class AndroidIntentCallToUnZipData : MonoBehaviour
{
	public string androidIntentCalljs = "androidIntentCalljs.js";

	private void OnApplicationFocus (bool focus)
	{
/*#if (!UNITY_EDITOR && UNITY_ANDROID)
		if (focus)
		{
			StartCoroutine (CreatePushClass (new AndroidJavaClass ("com.unity3d.player.UnityPlayer")));
		}
#endif*/
	}

	public string destinationPath = "/storage/emulated/0/ForeverStory";
	public string password = "Forever~Story2022";
	
	public IEnumerator CreatePushClass (AndroidJavaClass UnityPlayer)
	{
		EvalJs();

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
				yield return new WaitForSeconds (0.1f);
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
						StartCoroutine(TestUnZipFile(uriPath, destinationPath, password));
						//yield return TestUnZipFile (uriPath, destinationPath, password);
						//message = "��ѹ���,�뵽/storage/emulated/0/ForeverStoryĿ¼�鿴��ѹ���ļ�";
					}
					else
					{
                        StartCoroutine(TestCopyFile(uriPath, destinationPath, password));
                        //TestCopyFile(uriPath, destinationPath, password);
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

		//CopyStreamingAssetToPersistent.CopyFile ("Quest.wz", destinationPath);
	}
    public int GetZipFileCount(string zipPath)
    {
        int num = 0;
        try
        {
            using ZipInputStream zipInputStream = new ZipInputStream(File.OpenRead(zipPath));
            ZipEntry zipEntry = null;
            while ((zipEntry = zipInputStream.GetNextEntry()) != null)
            {
                foreach (Match item in Regex.Matches(zipEntry.Name, "[^/]+$"))
                {
                    num++;
                }
            }
        }
        catch
        {
        }

        return num;
    }
    public IEnumerator TestUnZipFile (string zipPath, string outPath, string password, bool haspassword = true,float waitTime = 3f)
	{
		yield return new WaitForSeconds(waitTime);

        if (!File.Exists (zipPath))
		{
			Debug.LogError ("û�д��ļ�·����" + zipPath);
			yield break;
		}
		if (!Directory.Exists(outPath))
		{
			Directory.CreateDirectory(outPath);
        }

        ZipHandler handler = ZipHandler.GetInstance();

        var totalFileCount = GetZipFileCount(zipPath);
        //handler.UnpackAll(zipPath, outPath,num => message= $"��ѹ���ȣ�{num}");
        var readedCount = 0;
        using (ZipInputStream stream = new ZipInputStream(File.OpenRead(zipPath)))
		{
			if (haspassword)
				stream.Password = password;
			ZipEntry theEntry;
			
			while ((theEntry = stream.GetNextEntry()) != null)
			{
				readedCount++;
                message = $"��ѹ���ȣ�{readedCount * 1f / totalFileCount * 100:f2}%";

                // Debug.Log ("theEntry.Name��" + theEntry.Name);
                string fileName = Path.GetFileName(theEntry.Name);
				// Debug.Log ("fileName��" + fileName);
				string filePath = Path.Combine(outPath, theEntry.Name);
				// Debug.Log ("filePath:" + filePath);
				string directoryName = Path.GetDirectoryName(filePath);
				// Debug.Log ("directoryName��" + directoryName);

				// ����ѹ���ļ����ļ���λ��
				if (directoryName.Length > 0)
				{
					Directory.CreateDirectory(directoryName);
				}
				if (fileName != String.Empty)
				{
					if (File.Exists(filePath)) continue;
					using (FileStream streamWriter = File.Create(filePath))
					{
						long readedLength = 0;
						long totalLength = stream.Length;

						int size = 2048;
						byte[] data = new byte[2048];
						while (true)
						{
							size = stream.Read(data, 0, data.Length);
							if (size > 0)
							{
								readedLength += size;
								progress = readedLength * 1f / totalLength * 100;
								//message = $"��ѹ���ȣ�{filePath}\t{progress:f2}%";
								timer += Time.deltaTime;
								if (timer > 1f)
								{
									timer = 0;
									yield return null;
								}

								streamWriter.Write(data, 0, size);
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
			message = $"��ѹ��ɣ���ѹ����{outPath}";

			Debug.Log(message);
		}
	}

	public IEnumerator TestCopyFile (string zipPath, string outPath, string password)
	{
		if (!File.Exists (zipPath))
		{
			Debug.LogError ("û�д��ļ�·����" + zipPath);
			yield break;
		}
		using (FileStream streamReader = File.Create (zipPath))
		{
			using (FileStream streamWriter = File.Create (outPath))
			{
				streamReader.CopyTo (streamWriter);
			}
		}
	}

	private int fileCount = 0;
	private void OnCopyComplete ()
	{
		fileCount++;
		if (fileCount >= wzFiles.Count)
		{
			copyComplete = true;
			message = "���ݽ�ѹ���, ������ʼ��Ϸ��ť";
		}
	}
	public IEnumerator CopyFile (string fileName)
	{
		if (File.Exists(Application.persistentDataPath + "/" + fileName))
		{
			yield break;
		}
		
		if (Application.platform == RuntimePlatform.Android)
		{

			using (UnityWebRequest request = UnityWebRequest.Get (Application.streamingAssetsPath + "/" + fileName))
			{
				request.timeout = 3;
				request.downloadHandler = new DownloadHandlerFile (Application.persistentDataPath + "/" + fileName);//ֱ�ӽ��ļ����ص����
				request.SendWebRequest ();
				
				float time = Time.time;
				//������ɺ�ִ�еĻص�
				while (!request.isDone)
				{
					message = $"��ѹ���ȣ�{request.downloadProgress:f2}%";
					timer += Time.deltaTime;
					if (timer > 1f)
					{
						timer = 0;
						yield return null;
					}
				}
				request.Abort ();
				//Ĭ��ֵ��true�����ø÷�������Ҫ����Dispose()��Unity�ͻ��Զ�����ɺ����Dispose()�ͷ���Դ��
				request.disposeDownloadHandlerOnDispose = true;
				request.Dispose ();
			}
		}
		else
		{
			File.Copy (Application.streamingAssetsPath + "/" + fileName, Application.persistentDataPath + "/" + fileName, true);
		}

		OnCopyComplete ();
	}
	
	public float timer;
	public static float progress;
	public int fontSize = 20;
	string message;
	TimedBool unzipFinished = new TimedBool ();
	bool copyComplete;
	bool clicked;

	private void OnGUI ()
	{
		if (isWzFileCopy)
		{
			unzipFinished.update ();

			GUI.skin.button.fontSize = fontSize;
			GUILayout.BeginVertical ();
			//GUILayout.Label (message);

		
			if (!clicked)
			{
				if (GUILayout.Button (message))
				{
					clicked = true;
				}
			}
			else
			{
			
			}
			/*if (unzipFinished)
			{
				GUILayout.Label ("���ݽ�ѹ���, ������ʼ��Ϸ��ť");
			}*/

			/*	if (ms.Setting<JSDebugOn>.get().load())
				{
		            if (GUILayout.Button("hotreload"))
		            {
						EvalJs();
		            }
		        }*/
        

			GUILayout.EndVertical ();
		}
	}

    //JsEnv jsEnv;

    public bool isWzFileCopy = false;
	private void EvalJs()
	{
        //string tex = File.ReadAllText(Path.Combine(Constants.get().path_MapleStoryFolder, androidIntentCalljs));
        //Debug.Log(tex);
        //jsEnv.Eval(tex);
    }

	private void Awake ()
	{
		/*if (isWzFileCopy)
		{
			message = "���ڽ�ѹ���ݰ��������ĵȴ�10����,��ѹ��ɻ�����ʾ";
		}*/
	}

	private void Start()
    {
        //jsEnv = new JsEnv ();
        //StartCoroutine(TestPCUnZip());
        var assetBundleZipPath = Path.Combine(Application.persistentDataPath, "AssetBundle.zip");
        var assetBundleOutPath = Path.Combine(Application.persistentDataPath, "");
        var baseWzPath = Path.Combine(Application.persistentDataPath, "Base.wz");

        if (!File.Exists(assetBundleZipPath))
		{
            message = $"���ڸ����ʲ���ѹ����";
            CopyStreamingAssetToPersistent.CopyFile("AssetBundle.zip");
            message = $"�����ʲ���ѹ�������";
        }
        if (!File.Exists(Path.Combine(assetBundleOutPath, "AssetBundle","Android.manifest")))
        {
            message = $"���ڽ�ѹ�ʲ���ѹ����";
            StartCoroutine(TestUnZipFile(assetBundleZipPath, assetBundleOutPath, password, false));
        }

        if (!File.Exists(baseWzPath))
		{
            message = $"���ڸ������ݰ�";
            StartCoroutine(TestCopyAllWz(5));

            //MapleStory.Instance.ShowGUINotice ("���Ȱ�װ���¿ͻ��ˣ��򿪿ͻ����Զ���ѹ���ݰ����ٰ�װ������Ϸ���������ʼ��Ϸ");
			return;
		}
        
		
    }

	IEnumerator TestCopyAllWz (float waitSecong)
	{
		yield return new WaitForSeconds (waitSecong);
		
		if (isWzFileCopy)
		{
			copyComplete = false;
			clicked = false;
			fileCount = 0;
			//CheckAndCopyFile ();
			
			foreach (var fileName in wzFiles)
			{
				//StartCoroutine (CopyFile (fileName)) ;
				CopyStreamingAssetToPersistent.CopyFile (fileName);
				message = $"���ƽ��ȣ�{wzFiles.IndexOf (fileName) * 100f / wzFiles.Count:f2}%";
				yield return new WaitForSeconds (1);
				
			}
		}

		copyComplete = true;
		message = "���ݽ�ѹ��ɣ�������ʼ��ť������Ϸ";
		
	}
    public List<string> wzFiles = new List<string> ()
    {
	    "Settings",
	    "Base.wz",
	    "Character1.wz",
	    "Effect.wz",
	    "Etc.wz",
	    "Item.wz",
	    "List.wz",
	    "Map.wz",
	    "Mob.wz",
	    "Morph.wz",
	    "Npc.wz",
	    "Quest.wz",
	    "Reactor.wz",
	    "Skill.wz",
	    "Sound.wz",
	    "String.wz",
	    "UI_083.wz",
	    "UI_Endless.wz"
    };
    
    private void CheckAndCopyFile ()
    {
	    foreach (var fileName in wzFiles)
	    {
		    //StartCoroutine (CopyFile (fileName)) ;
		    CopyStreamingAssetToPersistent.CopyFile (fileName);
	    }
    }
    void OnDestroy()
    {
        //jsEnv.Dispose();
    }

    IEnumerator TestPCUnZip()
	{
		yield return new WaitForSeconds(5);

        yield return TestUnZipFile("G:\\Program Files (x86)\\MapleStory\\MapleStory.zip", "G:\\Program Files (x86)\\MapleStory\\destinationPath", password);

    }
}
