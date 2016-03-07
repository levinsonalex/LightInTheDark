using UnityEngine;
using System.Collections;

public class Sword : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider c)
    {
        Debug.Log("Hit " + c.gameObject.tag + "!");
        if (c.gameObject.tag == "Worm")
        {
        }
    }
}
