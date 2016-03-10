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

    void OnCollisionEnter(Collision c)
    {
        if (c.gameObject.tag == "Worm")
        {
            Worm o = c.transform.parent.gameObject.GetComponent<Worm>();
            if (o == null)
            {
                // I have no idea what is going on here. The collider clearly has a gameobject
                // attached with the name "Worm". Each worm gameobject clearly has a "Worm"
                // script component. And yet, this message will print.
                Debug.Log("Somehow the worm object is null");
            }
            else
            {
                o.Hit(gameObject);
            }
        }
        else if (c.gameObject.tag == "SpawnerMandible")
        {
            c.gameObject.GetComponent<SpawnerMandible>().Hit();
        }
        Debug.Log(c.gameObject.name);

        if (c.gameObject.name != "PlayerBody")
        {
            Destroy(gameObject);
        }
    }
}
