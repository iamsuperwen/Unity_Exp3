using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class reachTarget : MonoBehaviour {

	GameObject endEff;
	int state;
	float Dpos,Dori;
	public Transform[] tp = new Transform[2];  //targetpoints: tp2, tp3;   Now is at the first point (tp1)
	public Transform text_next;

	/*class targetPoint{
		public Vector3 Pos;
		public Quaternion Ori;
	}*/


	void Awake () {
		endEff = GameObject.Find("end_effector");

	}

	// Use this for initialization
	void Start () {
		state = 0;

		///targetPoint[] tar = new targetPoint ();

	}
		
	// Update is called once per frame
	void Update () {

		if (state == 3)  {
			// Next Level(FOV, FPS)
			//...
			Instantiate (text_next, transform.position, transform.rotation);
			state++;


		} else{
			Dpos = Mathf.Abs (Vector3.Distance (endEff.transform.position, transform.position));
			Dori = Mathf.Abs (Quaternion.Angle (endEff.transform.rotation, transform.rotation));

			if ((Dpos < 1) && (Dori < 5)) {  //都達到
				this.GetComponent<Renderer> ().material = (Material)Resources.Load ("black");

				state++;
				//Next Point:
				if(state>0){
					transform.position = tp [state-1].position;
					transform.rotation = tp [state-1].rotation;
				}

			} else if (Dpos < 1) {  //pos達到
				this.GetComponent<Renderer> ().material = (Material)Resources.Load ("target_pos");
			} else if (Dori < 5) {  //ori達到
				this.GetComponent<Renderer> ().material = (Material)Resources.Load ("target_ori");
			} else {    //都沒達到
				this.GetComponent<Renderer> ().material = (Material)Resources.Load ("target_orange");
			}

		}

	
	}
}
