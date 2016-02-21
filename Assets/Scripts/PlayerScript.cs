using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour {

    public float speed = 5f;
    public bool nearPedestal = false;

    public static GameObject S;

	// Use this for initialization
	void Start () {
        if (S)
        {
            Debug.Log("Two player objects.");
        } else
        {
            S = gameObject;
        }
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb)
        {
            float forwardMovement = Input.GetAxis("Vertical") * speed;
            float horizontalMovement = Input.GetAxis("Horizontal") * speed;
            GameObject cameraFlat = new GameObject("TEMP");
            cameraFlat.transform.rotation = Camera.main.gameObject.transform.rotation;
            cameraFlat.transform.rotation = Quaternion.Euler(0, cameraFlat.transform.rotation.eulerAngles.y, cameraFlat.transform.rotation.eulerAngles.z);
            Vector3 motion = cameraFlat.transform.TransformDirection(new Vector3(horizontalMovement, rb.velocity.y, forwardMovement));
            rb.velocity = motion;
            Destroy(cameraFlat);
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
