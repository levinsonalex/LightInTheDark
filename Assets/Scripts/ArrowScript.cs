using UnityEngine;
using System.Collections;

public class ArrowScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    if(Vector3.Distance(PlayerScript.S.transform.position, transform.position) > 200)
        {
            Destroy(gameObject);
        }
	}
}
