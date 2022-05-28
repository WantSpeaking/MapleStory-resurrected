using System;
using System.Collections.Generic;
using System.IO;

public class GameDataTableParser : IDisposable
{


    #region GameDataTableParser 构造函数
    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="path"></param>
    public GameDataTableParser(string path)
    {
        m_FieldNameDic = new Dictionary<string, int>();

        //------------------
        //第1步：读取数据流
        //------------------
        byte[] buffer = LocalFileMgr.Instance.GetBuffer(path);

        //------------------
        //第2步：解压缩
        //------------------
        buffer = ZlibHelper.DeCompressBytes(buffer);

        //------------------
        //第3步：xor解密
        //------------------
        buffer = SecurityUtil.Xor(buffer);

        //------------------
        //第4步：解析数据到数组
        //------------------
        using (MMO_MemoryStream ms = new MMO_MemoryStream(buffer))
        {
            Row = ms.ReadInt();
            Column = ms.ReadInt();

            GameData = new String[Row, Column];
            m_FieldName = new string[Column];

            for (int i = 0; i < Row; i++)
            {
                for (int j = 0; j < Column; j++)
                {
                    string str = ms.ReadUTF8String();

                    if (i == 0)
                    {
                        //表示读取的是字段
                        m_FieldName[j] = str;
                        m_FieldNameDic[str] = j;

                    }
                    else if (i > 2)
                    {
                        //表示读取的是内容
                        GameData[i, j] = str;
                    }
                }
            }
        }
    }
    #endregion

    #region 字段或属性
    /// <summary>
    /// 行数
    /// </summary>
    public int Row { get; private set; }

    /// <summary>
    /// 列数
    /// </summary>
    public int Column { get; private set; }

    /// <summary>
    /// 字段名称
    /// </summary>
    private string[] m_FieldName;

    /// <summary>
    /// 字段名称
    /// </summary>
    public string[] FieldName
    {
        get { return m_FieldName; }
    }

    /// <summary>
    /// 游戏数据
    /// </summary>
    public string[,] GameData { get; private set; }

    /// <summary>
    /// 当前行号
    /// </summary>
    private int m_CurRowNo = 3;

    /// <summary>
    /// 字段名称字典
    /// </summary>
    private Dictionary<string, int> m_FieldNameDic;

    /// <summary>
    /// 是否结束
    /// </summary>
    public bool Eof
    {
        get
        {
            return m_CurRowNo == Row;
        }
    }
    #endregion

    #region Next 转到下一条
    /// <summary>
    /// 转到下一条
    /// </summary>
    public void Next()
    {
        if (Eof) return;
        m_CurRowNo++;
    }
    #endregion

    #region GetFieldValue 获取字段值
    /// <summary>
    /// 获取字段值
    /// </summary>
    /// <param name="fieldName"></param>
    /// <returns></returns>
    public string GetFieldValue(string fieldName)
    {
        try
        {
            if (m_CurRowNo < 3 || m_CurRowNo >= Row) return null;
            return GameData[m_CurRowNo, m_FieldNameDic[fieldName]];
        }
        catch { return null; }
    }
    #endregion

    #region Dispose 释放
    /// <summary>
    /// 释放
    /// </summary>
    void IDisposable.Dispose()
    {
        m_FieldNameDic.Clear();
        m_FieldNameDic = null;

        m_FieldName = null;
        GameData = null;
    }
    #endregion
}