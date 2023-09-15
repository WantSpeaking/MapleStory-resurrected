using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class CopyStreamingAssetToPersistent : Singleton<CopyStreamingAssetToPersistent>
{
	public static string GetTextForStreamingAssets (string path)
	{
		var uri = new System.Uri (Path.Combine (Application.streamingAssetsPath, path));
		UnityWebRequest request = UnityWebRequest.Get (uri);
		request.SendWebRequest ();//读取数据
		if (request.error == null)
		{
			while (true)
			{
				if (request.downloadHandler.isDone)//是否读取完数据
				{
					Debug.Log (request.downloadHandler.text);
					return request.downloadHandler.text;
				}
			}
		}
		else
		{
			return null;
		}

	}
	string getStreamingPath_for_www ()
	{
		string pre = "file://";
#if UNITY_EDITOR
		pre = "file://";
#elif UNITY_ANDROID
        pre = "";
#elif UNITY_IPHONE
	    pre = "file://";
#endif
		string path = pre + Application.streamingAssetsPath + "/";
		return path;
	}
	public static IEnumerator CopyFile (string fileName, Action<string,float> onChecking = null, Action<string,bool> onCheckComplete = null)
	{
		var sourcePath = Path.Combine(Application.persistentDataPath, fileName);
		var targetPath = Path.Combine(Application.streamingAssetsPath, fileName);
        if (File.Exists(sourcePath) && new FileInfo (sourcePath).Length != 0)
		{
			yield break;
		}
		if (Application.platform == RuntimePlatform.Android)
		{
			using (UnityWebRequest request = UnityWebRequest.Get(sourcePath))
			{
				request.timeout = 3;
				request.downloadHandler = new DownloadHandlerFile(targetPath);//直接将文件下载到外存
				request.SendWebRequest();
				if (request.result is UnityWebRequest.Result.ConnectionError or UnityWebRequest.Result.ProtocolError or UnityWebRequest.Result.DataProcessingError)
				{
					yield break;
				}

				//下载完成后执行的回调
				while (!request.isDone)
				{
					onChecking?.Invoke($"正在复制{fileName}", request.downloadProgress);
					yield return null;
				}

				request.Abort();
				//默认值是true，调用该方法不需要设置Dispose()，Unity就会自动在完成后调用Dispose()释放资源。
				request.disposeDownloadHandlerOnDispose = true;
				request.Dispose();
				onCheckComplete?.Invoke($"{fileName}复制完毕", true);

			}
		}
		else
		{
			if (!File.Exists(Application.streamingAssetsPath + "/" + fileName))
			{
                yield break;
            }
			
			File.Copy (Application.streamingAssetsPath + "/" + fileName, Application.persistentDataPath + "/" + fileName, true);
		}
	}
	public static void CopyFile (string fileName, string destinationFolder)
	{
/*		if (File.Exists (Application.persistentDataPath + "/" + fileName))
		{
			return;
		}*/
		if (Application.platform == RuntimePlatform.Android)
		{

			using (UnityWebRequest request = UnityWebRequest.Get (Application.streamingAssetsPath + "/" + fileName))
			{
				request.timeout = 3;
				request.downloadHandler = new DownloadHandlerFile (destinationFolder + "/" + fileName);//直接将文件下载到外存
				request.SendWebRequest ();

				float time = Time.time;
				//下载完成后执行的回调
				while (!request.isDone)
				{
				}
				request.Abort ();
				//默认值是true，调用该方法不需要设置Dispose()，Unity就会自动在完成后调用Dispose()释放资源。
				request.disposeDownloadHandlerOnDispose = true;
				request.Dispose ();
			}
		}
		else
		{
			File.Copy (Application.streamingAssetsPath + "/" + fileName, Application.persistentDataPath + "/" + fileName, true);
		}
	}
	public static void CopyResourcesFile (string source_pathFolder, string source_fileName, string save_FilenameExtension)
	{
		var path = source_pathFolder + source_fileName;

		var wzFile = Resources.Load<TextAsset> (path);
		FileStream fsDes = File.Create (Application.persistentDataPath + "/" + source_fileName + save_FilenameExtension);
		fsDes.Write (wzFile.bytes, 0, wzFile.bytes.Length);
		fsDes.Flush ();
		fsDes.Close ();
	}
	public void Copy (string fileName)
	{
		string src = getStreamingPath_for_www () + fileName;
		string des = Application.persistentDataPath + "/" + fileName;

		var uri = new System.Uri (src);
		UnityWebRequest request = UnityWebRequest.Get (uri);
		request.SendWebRequest ();//读取数据
		if (request.error == null)
		{
			while (true)
			{
				if (request.downloadHandler.isDone)//是否读取完数据
				{
					if (!File.Exists (des))
					{
						FileStream fsDes = File.Create (des);
						fsDes.Write (request.downloadHandler.data, 0, request.downloadHandler.data.Length);
						fsDes.Flush ();
						fsDes.Close ();
					}

					Debug.Log (request.downloadHandler.text);
					//return request.downloadHandler.text;
					return;
				}
			}
		}
		else
		{
			//return null;
		}
	}
}
