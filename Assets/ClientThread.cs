using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

class ClientThread
{
	public struct Struct_Internet
	{
		public string ip;
		public int port;
	}

	private Socket clientSocket;  //連線使用的Socket
	private Struct_Internet internet;
	public string receiveMessage;
	private string sendMessage;

	private Thread threadReceive;
	private Thread threadConnect;

	public ClientThread(AddressFamily family, SocketType socketType, ProtocolType protocolType, string ip, int port)
	{
		clientSocket = new Socket(family, socketType, protocolType);
		internet.ip = ip;
		internet.port = port;
		receiveMessage = null;
		//nowConnectCount = 0;
	}

	public void StartConnect()
	{
		threadConnect = new Thread(Accept);
		threadConnect.Start();
	}

	public void StopConnect()
	{
		try
		{
			clientSocket.Close();
		}
		catch(Exception)
		{

		}
	}

	public void Send(string message)  //Send 'target angle' to PC(.cpp)
	{
		if (message.Length < 50)  //7*6+5+10=51 (6 joints * '(-)0.0000', round off to the 4th decimal + ',-999.9999' )
			throw new NullReferenceException("ERROR: 'targetAngle[1~6]' send to PC go wrong!");
		else
			sendMessage = message;
		SendMessage();
	}

	public void Receive()  //Recv 'actual angle' from PC(.cpp)
	{
		if (threadReceive != null && threadReceive.IsAlive == true)
			return;
		threadReceive = new Thread(ReceiveMessage);
		threadReceive.IsBackground = true;  ///?
		threadReceive.Start();
	}

	private void Accept()
	{
		try
		{
			clientSocket.Connect(IPAddress.Parse(internet.ip), internet.port);//等待連線，若未連線則會停在這行
		}
		catch (Exception)
		{
		}
	}

	private void SendMessage()  //sendMessage = 'targetAngle'
	{
		try
		{
			if (clientSocket.Connected == true)
			{
				clientSocket.Send(Encoding.ASCII.GetBytes(sendMessage));
			}
		}
		catch (Exception)
		{

		}
	}

	private void ReceiveMessage()  //receiveMessage = 'actualAngle'
	{
		if (clientSocket.Connected == true)
		{
			byte[] bytes = new byte[100];		
			clientSocket.Receive(bytes);
			receiveMessage = Encoding.ASCII.GetString(bytes);
		}
	}
}