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
        if (c.gameObject.tag == "Worm" && PlayerScript.S.GetComponent<PlayerScript>().swinging)
        {
			Worm o = c.gameObject.GetComponent<Worm>();
			if (o == null) {
				// I have no idea what is going on here. The collider clearly has a gameobject
				// attached with the name "Worm". Each worm gameobject clearly has a "Worm"
				// script component. And yet, this message will print.
				Debug.Log ("Somehow the worm object is null");
			} else {
				o.Hit (gameObject);
			}
        }
    }
}
