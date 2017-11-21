using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Display FPS on a Unity UGUI Text Panel
// To use: Drag onto a game object with Text component
//         Press 'F' key to toggle show/hide
public class showFPS : MonoBehaviour 
{
	public Text text;
	public bool show = false;

	private const int targetFPS = 90;  //Vive, steamVR
	private const float updateInterval = 0.5f;

	private int framesCount; 
	private float framesTime;

	private GameObject spawnMe; //aaa
	private Vector3 position; //aaa

	void Start()
	{ 
		// no text object set? see if our gameobject has one to use
		if (text == null) 
		{ 
			text = GetComponent<Text>(); 
		}


		spawnMe = GameObject.Find("SpawnMe"); //aaa
		position = spawnMe.transform.position; //aaa

	}
	/*void Update()
	{
		Time.fixedDeltaTime = 0.05f;
	}*/

	void FixedUpdate()
	{
		if (Input.GetKeyDown(KeyCode.F))
		{
			show = !show;
		}

		// monitoring frame counter and the total time
		framesCount++;
		framesTime += Time.unscaledDeltaTime; 

		// measuring interval ended, so calculate FPS and display on Text
		if (framesTime > updateInterval)
		{
			if (text != null)
			{
				if (show)
				{
					float fps = framesCount/framesTime;
					text.text = System.String.Format("{0:F2} FPS", fps);
					text.color = (fps > (targetFPS-5) ? Color.green :
						(fps > (targetFPS-30) ?  Color.yellow : 
							Color.red));
				}
				else
				{
					text.text = "";
				}
			}
			// reset for the next interval to measure
			framesCount = 0;
			framesTime = 0;
		}

		///aaa
		float fps1 = framesCount/framesTime;
		if (fps1 > 100) {
			for (int i = 0; i < 10; i++) {
				Instantiate (spawnMe, position, Quaternion.identity);
			}
		}
	}
}
//***SOURCE: http://talesfromtherift.com/vr-fps-counter/

