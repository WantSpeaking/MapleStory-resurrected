//===================================================
//作    者：边涯  http://www.u3dol.com  QQ群：87481002
//创建时间：2016-02-06 08:52:07
//备    注：
//===================================================

using UnityEngine;
using System.Collections;
using System.Net.Sockets;
using System;
using System.Net;
using System.Collections.Generic;
using System.Text;
using ms;
using ms.Helper;

/// <summary>
/// 网络传输Socket
/// </summary>
public class NetWorkSocket : SingletonMono<NetWorkSocket>
{
	#region 发送消息所需变量

	//发送消息队列
	private Queue<byte[]> m_SendQueue = new Queue<byte[]> ();

	//检查队列的委托
	private Action m_CheckSendQuene;

	//压缩数组的长度界限
	private const int m_CompressLen = 200;

	#endregion

	#region 接收消息所需变量

	//接收数据包的字节数组缓冲区
	private byte[] m_ReceiveBuffer = new byte[NetConstants.MAX_PACKET_LENGTH];

	//接收数据包的缓冲数据流
	private MMO_MemoryStream m_ReceiveMS = new MMO_MemoryStream ();

	//接收消息的队列
	private Queue<byte[]> m_ReceiveQueue = new Queue<byte[]> ();

	private int m_ReceiveCount = 0;

	//public byte[] get_buffer () => m_ReceiveBuffer;
	private Cryptography cryptography => Session.get().getCrypt();
	private PacketSwitch packetswitch = new PacketSwitch ();

	#endregion

	/// <summary>
	/// 客户端socket
	/// </summary>
	private Socket m_Client;

	public Action OnConnectOK;

	protected override void OnStart ()
	{
		base.OnStart ();
	}

	protected override void OnUpdate ()
	{
		base.OnUpdate ();

		#region 从队列中获取数据

		while (true)
		{
			if (m_ReceiveCount <= 5)
			{
				m_ReceiveCount++;
				lock (m_ReceiveQueue)
				{
					if (m_ReceiveQueue.Count > 0)
					{
						/*//得到队列中的数据包
						byte[] buffer = m_ReceiveQueue.Dequeue();

						//异或之后的数组
						byte[] bufferNew = new byte[buffer.Length - 3];

						bool isCompress = false;
						ushort crc = 0;

						using (MMO_MemoryStream ms = new MMO_MemoryStream(buffer))
						{
						    isCompress = ms.ReadBool();
						    crc = ms.ReadUShort();
						    ms.Read(bufferNew, 0, bufferNew.Length);
						}

						//先crc
						int newCrc = Crc16.CalculateCrc16(bufferNew);

						if (newCrc == crc)
						{
						    //异或 得到原始数据
						    bufferNew = SecurityUtil.Xor(bufferNew);

						    if (isCompress)
						    {
						        bufferNew = ZlibHelper.DeCompressBytes(bufferNew);
						    }

						    ushort protoCode = 0;
						    byte[] protoContent = new byte[bufferNew.Length - 2];
						    using (MMO_MemoryStream ms = new MMO_MemoryStream(bufferNew))
						    {
						        //协议编号
						        protoCode = ms.ReadUShort();
						        ms.Read(protoContent, 0, protoContent.Length);

						        SocketDispatcher.Instance.Dispatch(protoCode, protoContent);
						    }
						}
						else
						{
						    break;
						}*/

						//得到队列中的数据包
						byte[] buffer = m_ReceiveQueue.Dequeue ();
                        var packet_bytes = buffer.ToSbyteArray();

                        if (Session.get().getCrypt() != null)
						{
                            //AppDebug.Log($"\trawPacket:{packet_bytes.ToDebugLog()}");

                            cryptography.decrypt(packet_bytes, packet_bytes.Length);
                            //AppDebug.Log($"\tdecryptPacket:{packet_bytes.ToDebugLog()}");

                        }
                        packetswitch.forward (packet_bytes.ToByteArray(), buffer.Length);
					}
					else
					{
						break;
					}
				}
			}
			else
			{
				m_ReceiveCount = 0;
				break;
			}
		}

		#endregion
	}


	protected override void BeforeOnDestroy ()
	{
		base.BeforeOnDestroy ();
		DisConnect ();
	}

	#region Connect 连接到socket服务器

	/// <summary>
	/// 连接到socket服务器
	/// </summary>
	/// <param name="ip">ip</param>
	/// <param name="port">端口号</param>
	public bool Connect (string ip, int port)
	{
		//如果socket已经存在 并且处于连接中状态 则直接返回
		if (m_Client != null && m_Client.Connected) return true;

		m_Client = new Socket (AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

		try
		{
			m_Client.Connect (new IPEndPoint (IPAddress.Parse (ip), port));
			m_CheckSendQuene = OnCheckSendQueueCallBack;

            //ReceiveMsg ();
			Debug.Log ($"开始连接: ip:{ip} port:{port}");
			if (OnConnectOK != null)
			{
				OnConnectOK ();
			}
			return true;
		}
		catch (Exception ex)
		{
            Debug.LogError ("连接失败=" + ex.Message);
            return false;
        }
	}

	public byte[] get_buffer(int size = NetConstants.MAX_PACKET_LENGTH)
	{
		if (m_Client != null && m_Client.Connected)
		{
			m_Client.Receive(m_ReceiveBuffer, size, SocketFlags.None);
        }
        return m_ReceiveBuffer;
    }
    #endregion

    /// <summary>
    /// 断开连接
    /// </summary>
    public void DisConnect ()
	{
		if (m_Client != null && m_Client.Connected)
		{
			m_Client.Shutdown (SocketShutdown.Both);
			m_Client.Close ();
		}
	}

	#region OnCheckSendQueueCallBack 检查队列的委托回调

	/// <summary>
	/// 检查队列的委托回调
	/// </summary>
	private void OnCheckSendQueueCallBack ()
	{
		lock (m_SendQueue)
		{
			//如果队列中有数据包 则发送数据包
			if (m_SendQueue.Count > 0)
			{
				//发送数据包
				Send (m_SendQueue.Dequeue ());
			}
		}
	}

	#endregion

	#region MakeData 封装数据包

	/// <summary>
	/// 封装数据包
	/// </summary>
	/// <param name="data"></param>
	/// <returns></returns>
	private byte[] MakeData (byte[] data)
	{
		byte[] retBuffer = null;

		//1.如果数据包的长度 大于了m_CompressLen 则进行压缩
		bool isCompress = data.Length > m_CompressLen ? true : false;
		if (isCompress)
		{
			data = ZlibHelper.CompressBytes (data);
		}

		//2.异或
		data = SecurityUtil.Xor (data);

		//3.Crc校验 压缩后的
		ushort crc = Crc16.CalculateCrc16 (data);

		using (MMO_MemoryStream ms = new MMO_MemoryStream ())
		{
			ms.WriteUShort ((ushort)(data.Length + 3));
			ms.WriteBool (isCompress);
			ms.WriteUShort (crc);
			ms.Write (data, 0, data.Length);

			retBuffer = ms.ToArray ();
		}

		return retBuffer;
	}

	#endregion

	#region SendMsg 发送消息 把消息加入队列

	/// <summary>
	/// 发送消息
	/// </summary>
	/// <param name="buffer"></param>
	public void SendMsg (byte[] buffer)
	{
		//得到封装后的数据包
		// byte[] sendBuffer = MakeData(buffer);

		lock (m_SendQueue)
		{
			//把数据包加入队列
			m_SendQueue.Enqueue (buffer);

			if (m_CheckSendQuene == null) return;
			//启动委托（执行委托）
			m_CheckSendQuene.BeginInvoke (null, null);
		}
	}

	#endregion

	#region Send 真正发送数据包到服务器

	/// <summary>
	/// 真正发送数据包到服务器
	/// </summary>
	/// <param name="buffer"></param>
	public void Send (byte[] buffer)
	{
		//var encodeBuffer = System.Text.Encoding.Unicode.GetBytes (Convert.ToBase64String (buffer));
		//Debug.Log ($"send encodeBuffer:{encodeBuffer.ToDebugLog ()}");
		//AppDebug.Log ($"BeginSend:{buffer.ToDebugLog ()}");
		//m_Client.BeginSend (buffer, 0, buffer.Length, SocketFlags.None, SendCallBack, m_Client);
		m_Client.Send (buffer);
		OnCheckSendQueueCallBack ();
	}

	#endregion

	#region SendCallBack 发送数据包的回调

	/// <summary>
	/// 发送数据包的回调
	/// </summary>
	/// <param name="ar"></param>
	private void SendCallBack (IAsyncResult ar)
	{
		m_Client.EndSend (ar, out var socketError);
		//AppDebug.Log ($"SendCallBack error:{socketError.GetTypeCode()}");
		//继续检查队列
		OnCheckSendQueueCallBack ();
	}

	#endregion

	//====================================================

	#region ReceiveMsg 接收数据

	/// <summary>
	/// 接收数据
	/// </summary>
	public void ReceiveMsg ()
	{
		//异步接收数据
		m_Client.BeginReceive (m_ReceiveBuffer, 0, m_ReceiveBuffer.Length, SocketFlags.None, out var socketError, ReceiveCallBack, m_Client);
		//Debug.Log ($"ReceiveMsg: {socketError}");
	}

	#endregion

	#region ReceiveCallBack 接收数据回调

	private byte[] strangeBytes = new byte[] {14, 0, 83, 0, 1, 0, 49, 70, 114, 122};
	private byte[] strangeBytes1 = new byte[] {49,52,56,51,49,52,57};
		

	/// <summary>
	/// 接收数据回调
	/// </summary>
	/// <param name="ar"></param>
	private void ReceiveCallBack (IAsyncResult ar)
	{
		//try
		{
			/*if (m_Client.Available <= 0) return; 
			if (m_Client == null) return;*/
			if (m_Client != null && m_Client.Connected)
			{
                int len = m_Client.EndReceive(ar);
                if (len > 0)
                {
                    Debug.Log($"\tReceive len:{len}\t Receive Content:{m_ReceiveBuffer.ToSbyteArray().ToDebugLog()} \t GetString:{Encoding.ASCII.GetString(m_ReceiveBuffer)}");

                    //已经接收到数据

                    //把接收到数据 写入缓冲数据流的尾部
                    m_ReceiveMS.Position = m_ReceiveMS.Length;
                    if (StringExt.BytesCompare_Base64(m_ReceiveBuffer, strangeBytes))
                    {

                        //Session.get().setcrypt(new Cryptography(m_ReceiveBuffer.ToSbyteArray()));


                        ReceiveMsg();
                        return;
                    }

                    if (StringExt.BytesCompare_Base64(m_ReceiveBuffer, strangeBytes1))
                    {
                        ReceiveMsg();
                        return;
                    }

                    byte[] originalArray = new byte[len];
                    Array.Copy(m_ReceiveBuffer, 0, originalArray, 0, len);
                    //Debug.Log ($"Start Receive");
                    //Debug.Log ($"\toriginalArray :(length :{originalArray.Length}) {originalArray.ToDebugLog ()}");
                    //把指定长度的字节 写入数据流

                    m_ReceiveMS.Write(originalArray, 0, originalArray.Length);

                    //如果缓存数据流的长度>2 说明至少有个不完整的包过来了
                    //为什么这里是2 因为我们客户端封装数据包 用的ushort 长度就是2
                    if (m_ReceiveMS.Length > 4)
                    {
                        //进行循环 拆分数据包
                        while (true)
                        {
                            //把数据流指针位置放在0处
                            m_ReceiveMS.Position = 0;

                            //currMsgLen = 包体的长度
                            byte[] tempHeaderBytes = new byte[4];
                            m_ReceiveMS.Read(tempHeaderBytes, 0, 4);
                            //int currMsgLen = m_ReceiveMS.ReadUShort();
                            int currMsgLen = cryptography.check_length(tempHeaderBytes.ToSbyteArray());
                            //int currMsgLen = len - 4;

                            //int currMsgLen = m_ReceiveMS.ReadInt ();
                            if (currMsgLen > NetConstants.MAX_PACKET_LENGTH)
                            {
                                //清空数据流
                                m_ReceiveMS.Position = 0;
                                m_ReceiveMS.SetLength(0);

                                break;
                            }

                            //currFullMsgLen 总包的长度=包头长度+包体长度
                            int currFullMsgLen = 4 + currMsgLen;

                            //如果数据流的长度>=整包的长度 说明至少收到了一个完整包
                            if (m_ReceiveMS.Length >= currFullMsgLen)
                            {
                                //至少收到一个完整包

                                //定义包体的byte[]数组
                                byte[] buffer = new byte[currMsgLen];

                                //把数据流指针放到4的位置 也就是包体的位置
                                m_ReceiveMS.Position = 4;

                                //把包体读到byte[]数组
                                m_ReceiveMS.Read(buffer, 0, currMsgLen);
                                //cryptography.decrypt(buffer.ToSbyteArray(),buffer.Length);
                                lock (m_ReceiveQueue)
                                {
                                    m_ReceiveQueue.Enqueue(buffer);
                                }
                                //==============处理剩余字节数组===================

                                //剩余字节长度
                                int remainLen = (int)m_ReceiveMS.Length - currFullMsgLen;
                                if (remainLen > 0)
                                {
                                    //把指针放在第一个包的尾部
                                    m_ReceiveMS.Position = currFullMsgLen;

                                    //定义剩余字节数组
                                    byte[] remainBuffer = new byte[remainLen];

                                    //把数据流读到剩余字节数组
                                    m_ReceiveMS.Read(remainBuffer, 0, remainLen);

                                    //清空数据流
                                    m_ReceiveMS.Position = 0;
                                    m_ReceiveMS.SetLength(0);

                                    //把剩余字节数组重新写入数据流
                                    m_ReceiveMS.Write(remainBuffer, 0, remainBuffer.Length);

                                    remainBuffer = null;
                                }
                                else
                                {
                                    //没有剩余字节

                                    //清空数据流
                                    m_ReceiveMS.Position = 0;
                                    m_ReceiveMS.SetLength(0);

                                    break;
                                }
                            }
                            else
                            {
                                //还没有收到完整包
                                break;
                            }
                        }
                    }

                    //进行下一次接收数据包
                    ReceiveMsg();
                }
                else
                {
                    //客户端断开连接
                    Debug.LogError ($"Disconnected! EndReceive len<=0");
                }
            }
			else
			{
                /*lock(m_ReceiveQueue)
				{
                    m_ReceiveQueue.Clear();
                }*/
                Debug.LogError($"Disconnected! m_Client != null:{m_Client != null}\t m_Client.Connected:{m_Client.Connected} ");
            }
			
		}
		//catch (Exception ex)
		{
			//客户端断开连接
			//Debug.Log ($"服务器{m_Client.RemoteEndPoint}断开连接\t Message:{ex.Message} {ex.Data} {ex.StackTrace}");
		}
	}

	#endregion

	public int receive (bool success)
	{
		//异步接收数据
		//m_Client.BeginReceive (m_ReceiveBuffer, 0, m_ReceiveBuffer.Length, SocketFlags.None, ReceiveCallBack, m_Client);
		//return 1;
		return m_Client.Receive (m_ReceiveBuffer, 0, m_ReceiveBuffer.Length, SocketFlags.None, out var socketError);
	}

   /* public void setcrypt(Cryptography c)
    {
        cryptography = c;
    }*/
}