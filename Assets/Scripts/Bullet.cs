using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {
	public Vector3 initialPos;
	void Awake() {
		InvokeRepeating ("CheckOffscreen", 0.5f, 0.5f);
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void CheckOffscreen() {
		Vector3 currPos = gameObject.transform.position;
		//Debug.Log ("(current, initial):" +  " " + currPos.z + " " + initialPos.z);
		if (Mathf.Abs(currPos.z - initialPos.z) > 2f) {
			//Debug.Log ("OFFSCREEN");
			Destroy (this.gameObject);
		}
	}
}
