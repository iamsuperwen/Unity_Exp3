using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GripMove : MonoBehaviour {  // ***** Add on TarCube *****

	GameObject endEff;
	GameObject Robot;
	jointsRotate Robot_jR;

	void Awake(){
		endEff = GameObject.Find("end_effector");
		Robot = GameObject.Find("RV_2A");
		Robot_jR = Robot.GetComponent<jointsRotate>();
	}

	// Use this for initialization
	void Start () {
		//transform.parent = endEff.transform;
	}
	
	// Update is called once per frame
	void Update () {
		//transform.parent = endEff.transform;
	}

	void OnCollisionEnter(Collision other){
		if (((other.gameObject.name == "Grip_L") || (other.gameObject.name == "Grip_R"))&(Robot_jR.theta_tar [6]==1)) {
			GetComponent<Collider> ().attachedRigidbody.useGravity = false;
			transform.parent = endEff.transform;
		}
	}

	void OnCollisionExit(Collision other){
		if (((other.gameObject.name == "Grip_L") || (other.gameObject.name == "Grip_R"))|(Robot_jR.theta_tar [6]==0)) {
			GetComponent<Collider> ().attachedRigidbody.useGravity = true;
			transform.parent = null;
		}
	}

	/*void OnCollisionEnter(Collision other){
		if ((other.gameObject.name == "Grip_L") || (other.gameObject.name == "Grip_R")) {
			GetComponent<Collider> ().attachedRigidbody.useGravity = false;
			transform.parent = endEff.transform;
		}
	}

	void OnCollisionExit(Collision other){
		if ((other.gameObject.name == "Grip_L") || (other.gameObject.name == "Grip_R")) {
			GetComponent<Collider> ().attachedRigidbody.useGravity = true;
			transform.parent = null;
		}
	}*/
}
