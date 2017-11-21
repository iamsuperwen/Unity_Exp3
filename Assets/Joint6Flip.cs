using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Joint6Flip : MonoBehaviour {

	//GameObject J6_base;
	public Transform text_flip;
	// Use this for initialization
	void Awake () {
		
		//J6_base = GameObject.FindGameObjectsWithTag ("J6");
	}

	// Use this for initialization
	void Start () {
	}

	// Update is called once per frame
	void Update () {

		Instantiate (text_flip, transform.position, transform.rotation);

	}
}
