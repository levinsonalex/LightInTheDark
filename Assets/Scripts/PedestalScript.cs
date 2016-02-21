using UnityEngine;
using System.Collections;

public class PedestalScript : MonoBehaviour {

    bool PlayerNear = false;
    public GameObject AB;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Vector3.Distance(PlayerScript.S.transform.position, transform.position) < 10 && PlayerScript.S.GetComponent<PlayerScript>().nearPedestal)
        {
            if (AB)
            {
                AB.SetActive(true);
            }
        }
        else
        {
            AB.SetActive(false);
        }
	}
}
