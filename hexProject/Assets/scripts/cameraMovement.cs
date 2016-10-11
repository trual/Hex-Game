using UnityEngine;
using System.Collections;

public class cameraMovement : MonoBehaviour {

	public float speed;



	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey(KeyCode.D)){
			this.transform.localPosition += new Vector3(speed, 0,0);
		}
		else if (Input.GetKey(KeyCode.A)) {
			this.transform.localPosition -= new Vector3(speed, 0,0);
		}
		else if (Input.GetKey(KeyCode.S)) {
			this.transform.localPosition -= new Vector3(0, 0, speed);
		}
		else if (Input.GetKey(KeyCode.W)) {
			this.transform.localPosition += new Vector3(0, 0, speed);
		}
	}
}
