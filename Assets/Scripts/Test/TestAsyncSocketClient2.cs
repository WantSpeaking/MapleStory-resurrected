using UnityEngine;
using System.Collections;
using System.Net.Sockets;
using System.Net;
using System;
using System.Text;
using System.Threading;
using ms.Helper;

public class TestAsyncSocketClient2 : MonoBehaviour
{
	/*private Socket client = null;
	public string ip = "127.0.0.1";
	public int port = 8484;
 
	private int size = 1024;
	private byte[] readData = new byte[1024];
	private byte[] data = new byte[1024];
	void Start()                      
	{
	    //  socket2.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true); 多socket 复用同一端口
	    client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
	    IPEndPoint address = new IPEndPoint(IPAddress.Parse(ip), port);
	    //client.Blocking = false;
	    //client.BeginConnect(address, new AsyncCallback(Connected), null); //建立异步连接服务 , Connected 进行监听
	    client.Connect(address); //建立异步连接服务 , Connected 进行监听
	    //connectDone.WaitOne();
	}
	void Connected(IAsyncResult iar)    //建立连接
	{
	   //Socket client = (Socket)iar.AsyncState;
	   //client.EndConnect(iar);
	    client.BeginReceive(data, 0, size, SocketFlags.None, new AsyncCallback(ReceiveData), client);
	    echo("建立连接");
 
	}
	void Send(string str)
	{
	    byte[] msg = Encoding.UTF8.GetBytes(str);
	    client.BeginSend(msg, 0, msg.Length, SocketFlags.None, new AsyncCallback(SendData), client);    //开始发送
	}
	void SendData(IAsyncResult iar) //发送数据
	{
	    Socket remote = (Socket)iar.AsyncState;
	    //int sent = remote.EndSend(iar);         //关闭发送
	    remote.BeginReceive(data, 0, data.Length, SocketFlags.None, new AsyncCallback(ReceiveData), remote);   //开始接收
	}
 
 
 
	void Update()
	{
	    startReceive(); //这步很重要，，，不然会收不到服务器发过来的消息
	}
	bool ReceiveFlag = true;
	void startReceive()
	{
	    if (ReceiveFlag) {
	        ReceiveFlag = false;
	        client.BeginReceive(readData, 0, readData.Length, SocketFlags.None, new AsyncCallback(endReceive), client);
	    }           
	}
 
	void endReceive(IAsyncResult iar) //接收数据
	{
	    ReceiveFlag = true;
	    Socket remote = (Socket)iar.AsyncState;
	    int recv = remote.EndReceive(iar);
	    if (recv > 0)
	    {
	        string stringData = Encoding.UTF8.GetString(readData, 0, recv);
	        text2 += "\n" + "接收服务器数据:+++++++++++++++" + stringData;
	    }
 
	}
	void ReceiveData(IAsyncResult iar) //接收数据
	{
	    Socket remote = (Socket)iar.AsyncState;
	    int recv = remote.EndReceive(iar);          //关闭接收 注意：如果关闭了接收，就不能接收了 测试是这样
 
	    string stringData = Encoding.UTF8.GetString(data, 0, recv);
	    text2 += "\n" + "回收发送数据:+++++++++++++++" + stringData;
 
	}
 
 
	void CloseSocket()                  //关闭socket
	{
	    if (client.Connected)
	    {
	        echo("关闭socket");
	        client.Close();
	    }            
	}
	void OnApplicationQuit()
	{
	    CloseSocket();
	}
 
	void echo(object msg)
	{
	    Debug.Log(msg);
	}
 
	string text = "";
	string text2 = "";
	Vector2 p = new Vector2(600, 300);
	void OnGUI()
	{
	    GUILayout.BeginVertical(GUILayout.Width(500) );
	        text = GUILayout.TextField(text);
	        if (GUILayout.Button("发送数据"))
	        {
	            Send(text);
	        }
	        GUILayout.BeginScrollView(p);
	        text2 = GUILayout.TextArea(text2, GUILayout.Height(300));
	        GUILayout.EndScrollView();
	    GUILayout.EndVertical();
 
	}*/
	MMO_MemoryStream bs = new MMO_MemoryStream ();

	public short readShort ()
	{
		return (short)(bs.ReadByte () + (bs.ReadByte () << 8));
	}

	public void writeShort (int i)
	{
		bs.WriteByte ((byte)(i & 0xFF));
		bs.WriteByte ((byte)((i >> 8) & 0xFF));
	}

	private byte[] buffer = {151, 29};
	//private sbyte[] sbuffer = {-105, 29};
	private sbyte[] sbuffer = {-53,-3};

	private void Start ()
	{
		/*bs.WriteShort (7575);
		bs.Position = 0;
		Debug.Log ($"bs:{bs.ToArray ().ToDebugLog ()}\t value:{bs.ReadShort ()}");*/

		/*writeShort (7575);
		bs.Position = 0;
		Debug.Log ($"bs:{bs.ToArray ().ToDebugLog ()}\t value:{readShort()}");*/

		bs = new MMO_MemoryStream (sbuffer.ToByteArray ());
		//bs.Position = 0;
		//Debug.Log ($"bs:{bs.ToArray ().ToDebugLog ()}\t value:{readShort ()}");

		//Debug.Log ($"{-105 & 255} \t {-105 & 0xff}\t {sbuffer[0] & 0xFF}\t {(sbuffer[1] & 0xFF) << 8}\t {(sbuffer[0] & 0xFF) + ((sbuffer[1] & 0xFF) << 8)}");
		//Debug.Log ($"{sbuffer[0] & 0xFF}\t {(sbuffer[1] & 0xFF) << 8}\t {(sbuffer[0] & 0xFF) + ((sbuffer[1] & 0xFF) << 8)}");
		Debug.Log ($"{unchecked((short)64971)}");

	}
}