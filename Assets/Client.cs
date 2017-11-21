using UnityEngine;
using System.Collections;
using System.Net.Sockets;
using System;

public class Client : MonoBehaviour
{
	//*** 宣告變數: 跟連線有關的變數, Unity為Client
	private ClientThread ct;
	private bool isSend;
	public bool ctrl_tpad_flag;
	public bool ctrl_grip_flag;

	//*** 宣告變數: 為了使用jointsRotate.cs中的變數
	GameObject Robot;
	jointsRotate Robot_jR;

	///initial(home) theta = { 0, 90, 0, 0, 0, 0 }
	public float[] theta_tar_a;  // = new float[] { 0, 90, 0, 0, 0, 0 };  //VR robot的 target theta(actual) 讀取RV-2A實際角度 
	float[] theta_0 = new float[] { 0, 90, 0, 0, 0, 0 };      //VR robot的 initial theta(home)  //!!!??
	float[] theta_user = new float[] { 0, 0, 0, 0, 0, 0 };    //theta_tar -> targetAngle(PC) 的相對角度(中繼站)
	int devider;

	void Awake()
	{
		Robot = GameObject.Find("RV_2A");
		Robot_jR = Robot.GetComponent<jointsRotate>();
		//*** Robot_jR.xxx : 即可取得jointsRotate.cs中的變數 xxx
		// Ex. Robot_jR.theta_tar
	}

	private void Start()
	{
		theta_tar_a = new float[] { 0, 90, 0, 0, 0, 0, 0 };	//[Caution!] the public var should initialize here!!! ; Gripper: theta_now_a[6], 1: close; 0: open 
		/// 之後整合要移走: ON/OFF - line +開關條件
		ct = new ClientThread(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp, "127.0.0.1", 5566);  //localhost
		ct.StartConnect();
		isSend = true;
		ctrl_tpad_flag = false;
		ctrl_grip_flag = false;
		devider = 0;
	}

	private void Update()
	{
		ct.Receive ();
		if (ct.receiveMessage != null) {  //***** Update 'actual angle' *****  /// W/out protect yet!
			string[] recv_msg = ct.receiveMessage.Split (',');  //用逗號分割字串; using System;
			//string[] recv_msg = ct.receiveMessage.Split (new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);  //用逗號分割字串; using System;
			////Debug.Log ("Recv raw:" + ct.receiveMessage);
			if (recv_msg.Length > 6) {
				for (int i = 0; i < 6; i++)
					theta_tar_a [i] = -float.Parse (recv_msg [i]) + theta_0 [i];  //Convert: string -> float
				theta_tar_a [6] = float.Parse (recv_msg [6]);  //update actual Gripper status
			} else {
				Debug.Log (" ![ERROR]! : recv_msg.Length = " + recv_msg.Length);
			}

			/*Debug.Log ("recv_msg.Length = " + recv_msg.Length);
			for(int x=0; x<recv_msg.Length; x++)
				Debug.Log (x + " ~ " + recv_msg [x]);
			*/
			ct.receiveMessage = null;
		}

		if (isSend == true) {    //***** Send 'target angle' to PC *****  //0509
			for (int i = 0; i < 6; i++) {
				theta_user [i] = -(Robot_jR.theta_tar [i] - theta_0 [i]);
			}

			StartCoroutine (SendTargetAngle ());//延遲發送訊息  //0508 Pick&Place
			/*if (devider % 5000 == 0) {  //-----0508 Following-----
				StartCoroutine (SendTargetAngle ());//延遲發送訊息
				devider = 0;
			} else {
				devider++;			
			}*/
		}

	}

	private IEnumerator SendTargetAngle()  //***** Send 'target angle' to PC:  Function ~
	{
		isSend = false;
		yield return new WaitForSeconds(0.01F);

		string send_msg = theta_user[0].ToString("f4") + "," + theta_user[1].ToString("f4") + "," + theta_user[2].ToString("f4") + "," + theta_user[3].ToString("f4") + ","+ theta_user[4].ToString("f4") + "," + theta_user[5].ToString("f4") + "," + Robot_jR.theta_tar[6].ToString("f4") + "\0";
		//string send_msg = theta_user[0].ToString("f4") + "," + theta_user[1].ToString("f4") + "," + theta_user[2].ToString("f4") + "," + theta_user[3].ToString("f4") + ","+ theta_user[4].ToString("f4") + "," + theta_user[5].ToString("f4") + ",999.9999";
		if (Robot_jR.inRange & (ctrl_tpad_flag | ctrl_grip_flag)) {  //*** if(OutOfRange) don't send!
			ct.Send (send_msg);
		
			if (ctrl_tpad_flag) {
				ctrl_tpad_flag = false;  //0508_Pick&Place//
			}
			if (ctrl_grip_flag)  ctrl_grip_flag = false;
			Debug.Log ("Send raw" + send_msg);
		}
		isSend = true;
	}

	private void OnApplicationQuit() //Sent to all game objects before the application is quit.
	{
		ct.StopConnect();  //Close Socket
	}
}