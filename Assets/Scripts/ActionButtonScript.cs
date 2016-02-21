using UnityEngine;
using System.Collections;

public class ActionButtonScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (PlayerScript.S.GetComponent<PlayerScript>().mainCamera.activeSelf)
        {
            transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward,
               Camera.main.transform.rotation * Vector3.up);
        }
    }
}