


using System;
using System.Net;
using System.Net.Sockets;


#if ! USE_ASIO

namespace ms
{
	public class SocketWinsock
	{
		#region 接收消息所需变量

		//接收数据包的字节数组缓冲区
		private byte[] m_ReceiveBuffer = new byte[2048];

		//接收数据包的缓冲数据流
		//private MMO_MemoryStream m_ReceiveMS = new MMO_MemoryStream ();

		//接收消息的队列
		//private Queue<byte[]> m_ReceiveQueue = new Queue<byte[]> ();

		private int m_ReceiveCount = 0;

		#endregion

		/// <summary>
		/// 客户端socket
		/// </summary>
		private Socket m_Client;

		public Action OnConnectOK;

		public bool open (string iaddr, string port)
		{
			//如果socket已经存在 并且处于连接中状态 则直接返回
			if (m_Client != null && m_Client.Connected) return false;
			m_Client = new Socket (AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			try
			{
				m_Client.Connect (new IPEndPoint (IPAddress.Parse (iaddr), int.Parse (port)));
				//m_CheckSendQuene = OnCheckSendQueueCallBack;

				//ReceiveMsg ();
				AppDebug.Log ("开始连接");
				OnConnectOK?.Invoke ();
			}
			catch (Exception ex)
			{
				AppDebug.Log ("连接失败=" + ex.Message);
			}

			return true;

			/*WSADATA wsa_info = new WSADATA();
			sock = INVALID_SOCKET;

			addrinfo addr_info = null;
			addrinfo ptr = null;
			addrinfo hints = new addrinfo();

			int result = WSAStartup(MAKEWORD(2, 2), wsa_info);

			if (result != 0)
			{
				return false;
			}

			ZeroMemory(hints, sizeof(addrinfo));

			hints.ai_family = AF_UNSPEC;
			hints.ai_socktype = SOCK_STREAM;
			hints.ai_protocol = IPPROTO_TCP;

			result = getaddrinfo(iaddr, port, hints, addr_info);

			if (result != 0)
			{
				WSACleanup();

				return false;
			}

			for (ptr = addr_info; ptr != null; ptr = ptr.ai_next)
			{
				sock = socket(ptr.ai_family, ptr.ai_socktype, ptr.ai_protocol);

				if (sock == INVALID_SOCKET)
				{
					WSACleanup();

					return false;
				}

				result = connect(sock, ptr.ai_addr, (int)ptr.ai_addrlen);

				if (result == SOCKET_ERROR)
				{
					closesocket(sock);

					sock = INVALID_SOCKET;

					continue;
				}

				break;
			}

			freeaddrinfo(addr_info);

			if (sock == INVALID_SOCKET)
			{
				WSACleanup();

				return false;
			}

			result = recv(sock, (string)buffer, 32, 0);

			if (result == NetConstants.HANDSHAKE_LEN)
			{
				return true;
			}
			else
			{
				WSACleanup();

				return false;
			}*/
		}

		public bool close ()
		{
			if (m_Client != null && m_Client.Connected)
			{
				m_Client.Shutdown (SocketShutdown.Both);
				m_Client.Close ();
			}

			return true;
			/*int error = closesocket (sock);

			WSACleanup ();

			return error != SOCKET_ERROR;*/
		}

		public bool dispatch (byte[] bytes, int length)
		{
			Send (bytes);
			return true;
		}
		public int receive (bool success)
		{
			//异步接收数据
			//m_Client.BeginReceive (m_ReceiveBuffer, 0, m_ReceiveBuffer.Length, SocketFlags.None, ReceiveCallBack, m_Client);
			//return 1;
			return m_Client.Receive (buffer, 0, buffer.Length, SocketFlags.None, out var socketError);

			/*timeval timeout = new timeval (0, 0);
			fd_set sockset = new fd_set ();

			FD_SET (sock, sockset);

			int result = select (0, sockset, 0, 0, timeout);

			if (result > 0)
			{
				result = recv (sock, buffer, NetConstants.MAX_PACKET_LENGTH, 0);
			}

			if (result == SOCKET_ERROR)
			{
				success = false;

				return 0;
			}
			else
			{
				return result;
			}*/
		}

		public byte[] get_buffer ()
		{
			return buffer;
		}

		private ulong sock;

		//private string buffer = new string (new char[NetConstants.MAX_PACKET_LENGTH]);
		byte[] buffer = new byte[NetConstants.MAX_PACKET_LENGTH];
		
		
		
		
		#region Send 真正发送数据包到服务器

		/// <summary>
		/// 真正发送数据包到服务器
		/// </summary>
		/// <param name="buffer"></param>
		private void Send (byte[] buffer)
		{
			m_Client.BeginSend (buffer, 0, buffer.Length, SocketFlags.None, SendCallBack, m_Client);
		}

		#endregion

		#region SendCallBack 发送数据包的回调

		/// <summary>
		/// 发送数据包的回调
		/// </summary>
		/// <param name="ar"></param>
		private void SendCallBack (IAsyncResult ar)
		{
			m_Client.EndSend (ar);

			//继续检查队列
			//OnCheckSendQueueCallBack();
		}

		#endregion

		#region ReceiveMsg 接收数据

		/// <summary>
		/// 接收数据
		/// </summary>
		private void ReceiveMsg ()
		{
			//异步接收数据
			m_Client.BeginReceive (m_ReceiveBuffer, 0, m_ReceiveBuffer.Length, SocketFlags.None, ReceiveCallBack, m_Client);
		}

		#endregion

		#region ReceiveCallBack 接收数据回调

		/// <summary>
		/// 接收数据回调
		/// </summary>
		/// <param name="ar"></param>
		private void ReceiveCallBack (IAsyncResult ar)
		{
			int len = m_Client.EndReceive (ar);

			if (len > 0)
			{
			}
			else
			{
				//客户端断开连接
				AppDebug.Log (string.Format ("服务器{0}断开连接", m_Client.RemoteEndPoint.ToString ()));
			}

			/*try
			{
				int len = m_Client.EndReceive (ar);

				if (len > 0)
				{
					//已经接收到数据

					//把接收到数据 写入缓冲数据流的尾部
					m_ReceiveMS.Position = m_ReceiveMS.Length;

					//把指定长度的字节 写入数据流
					m_ReceiveMS.Write (m_ReceiveBuffer, 0, len);

					//如果缓存数据流的长度>2 说明至少有个不完整的包过来了
					//为什么这里是2 因为我们客户端封装数据包 用的ushort 长度就是2
					if (m_ReceiveMS.Length > 2)
					{
						//进行循环 拆分数据包
						while (true)
						{
							//把数据流指针位置放在0处
							m_ReceiveMS.Position = 0;

							//currMsgLen = 包体的长度
							int currMsgLen = m_ReceiveMS.ReadUShort ();

							//currFullMsgLen 总包的长度=包头长度+包体长度
							int currFullMsgLen = 2 + currMsgLen;

							//如果数据流的长度>=整包的长度 说明至少收到了一个完整包
							if (m_ReceiveMS.Length >= currFullMsgLen)
							{
								//至少收到一个完整包

								//定义包体的byte[]数组
								byte[] buffer = new byte[currMsgLen];

								//把数据流指针放到2的位置 也就是包体的位置
								m_ReceiveMS.Position = 2;

								//把包体读到byte[]数组
								m_ReceiveMS.Read (buffer, 0, currMsgLen);

								lock (m_ReceiveQueue)
								{
									m_ReceiveQueue.Enqueue (buffer);
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
									m_ReceiveMS.Read (remainBuffer, 0, remainLen);

									//清空数据流
									m_ReceiveMS.Position = 0;
									m_ReceiveMS.SetLength (0);

									//把剩余字节数组重新写入数据流
									m_ReceiveMS.Write (remainBuffer, 0, remainBuffer.Length);

									remainBuffer = null;
								}
								else
								{
									//没有剩余字节

									//清空数据流
									m_ReceiveMS.Position = 0;
									m_ReceiveMS.SetLength (0);

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
					ReceiveMsg ();
				}
				else
				{
					//客户端断开连接
					AppDebug.Log (string.Format ("服务器{0}断开连接", m_Client.RemoteEndPoint.ToString ()));
				}
			}
			catch
			{
				//客户端断开连接
				AppDebug.Log (string.Format ("服务器{0}断开连接", m_Client.RemoteEndPoint.ToString ()));
			}*/
		}

		#endregion

		
	}
}
#endif
