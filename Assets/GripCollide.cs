using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GripCollide : MonoBehaviour {

	public bool CollideFlag;
	public int CubeNo;  //Cube Number: 1 or 2

	// Use this for initialization
	void Start () {
		CollideFlag = false;
		CubeNo = 0;
	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnCollisionEnter(Collision other){
		CollideFlag = true;
		if (other.gameObject.name == "TarCube1")
			CubeNo = 1;
		else if (other.gameObject.name == "TarCube2")
			CubeNo = 2;
	}

	void OnCollisionStay(Collision other){
		
	}

	void OnCollisionExit(Collision other){
		CollideFlag = false;
	}

}

