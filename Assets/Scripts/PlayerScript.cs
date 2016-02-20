using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour {

    public float speed = 5f;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb)
        {
            float forwardMovement = Input.GetAxis("Vertical") * speed;
            float horizontalMovement = Input.GetAxis("Horizontal") * speed;

            Vector3 motion = Camera.main.transform.TransformDirection(new Vector3(horizontalMovement, 0, forwardMovement));
            rb.velocity = motion;
        }
	}
}
