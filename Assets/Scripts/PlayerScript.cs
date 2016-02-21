using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour {

    public float speed = 5f;
    public bool nearPedestal = false;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void FixedUpdate () {
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb)
        {
            float forwardMovement = Input.GetAxis("Vertical") * speed;
            float horizontalMovement = Input.GetAxis("Horizontal") * speed;

            Vector3 motion = Camera.main.transform.TransformDirection(new Vector3(horizontalMovement, 0, forwardMovement));
            rb.velocity = motion;
        }
	}

    void OnTriggerEnter(Collider coll)
    {
        if(coll.tag == "Pedestal")
        {
            nearPedestal = true;
        }
    }

    void OnTriggerExit(Collider coll)
    {
        if(coll.tag == "Pedestal")
        {
            nearPedestal = false;
        }
    }
}
