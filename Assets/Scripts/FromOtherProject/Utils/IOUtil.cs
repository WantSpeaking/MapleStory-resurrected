//===================================================
//作    者：边涯  http://www.u3dol.com  QQ群：87481002
//创建时间：2017-03-03 09:08:41
//备    注：
//===================================================
using UnityEngine;
using System.Collections;
using System.IO;
using System;

public class IOUtil
{
    #region CreateTextFile 创建文本文件
    /// <summary>
    /// 创建文本文件
    /// </summary>
    /// <param name="filePath"></param>
    /// <param name="content"></param>
    public static void CreateTextFile(string filePath, string content)
    {
        DeleteFile(filePath);

        using (FileStream fs = File.Create(filePath))
        {
            using (StreamWriter sw = new StreamWriter(fs))
            {
                sw.Write(content.ToString());
            }
        }
    }
    #endregion

    #region DeleteFile 删除文件
    /// <summary>
    /// 删除文件
    /// </summary>
    /// <param name="filePath"></param>
    public static void DeleteFile(string filePath)
    {
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }
    }
    #endregion

    #region CopyDirectory 拷贝文件夹
    /// <summary>
    /// 拷贝文件夹
    /// </summary>
    /// <param name="sourceDirName"></param>
    /// <param name="destDirName"></param>
    public static void CopyDirectory(string sourceDirName, string destDirName)
    {
        try
        {
            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
                File.SetAttributes(destDirName, File.GetAttributes(sourceDirName));

            }

            if (destDirName[destDirName.Length - 1] != Path.DirectorySeparatorChar)
                destDirName = destDirName + Path.DirectorySeparatorChar;

            string[] files = Directory.GetFiles(sourceDirName);
            foreach (string file in files)
            {
                if (File.Exists(destDirName + Path.GetFileName(file)))
                    continue;
                FileInfo fileInfo = new FileInfo(file);
                if (fileInfo.Extension.Equals(".meta", StringComparison.CurrentCultureIgnoreCase))
                    continue;

                File.Copy(file, destDirName + Path.GetFileName(file), true);
                File.SetAttributes(destDirName + Path.GetFileName(file), FileAttributes.Normal);
            }

            string[] dirs = Directory.GetDirectories(sourceDirName);
            foreach (string dir in dirs)
            {
                CopyDirectory(dir, destDirName + Path.GetFileName(dir));
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    #endregion

    //todo GetFileText
    public static string GetFileText(string textPath)
    {
        string text = File.ReadAllText(textPath);
        return text;
    }
    public static string[] GetLines(string text)
    {
        string[] arrLines = text.Split('\n');//每行一条数据，\n做分割
        return arrLines;
    }
}