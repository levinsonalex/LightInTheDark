using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour {

    public float speed = 5f;
    public GameObject mainCamera;

    public GameObject nearPedestal;
    public GameObject droppedEye;

    public static GameObject S;
    private Rigidbody rb;

	public bool hasRedPowerUp;
	public bool hasYellowPowerUp;
	public bool hasBluePowerUp;

	// Use this for initialization
	void Start () {
		hasRedPowerUp = false;
		hasYellowPowerUp = false;
		hasBluePowerUp = false;
        if (S)
        {
            Debug.Log("Two player objects.");
        } else
        {
            S = gameObject;
        }

        rb = GetComponent<Rigidbody>();
	}

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && nearPedestal)
        {
            if (rb)
            {
                rb.velocity = Vector3.zero;
            }
            Debug.Log("Pressed E Nearby A Pedestal");
            if (!droppedEye)
            {
                droppedEye = nearPedestal.transform.FindChild("PedestalCamera").gameObject;
                droppedEye.SetActive(true);
                mainCamera.SetActive(false);
            }
            else
            {
                mainCamera.SetActive(true);
                droppedEye.SetActive(false);
                droppedEye = null;
            }
        }
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        if (rb)
        {
            //Regular Movement
            if (!droppedEye)
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
            else //Dropped Eye Movement
            {

            }
        }
	}

    void OnTriggerEnter(Collider coll)
    {
        if(coll.tag == "Pedestal")
        {
            nearPedestal = coll.gameObject;
        }
    }

    void OnTriggerExit(Collider coll)
    {
        if(coll.tag == "Pedestal")
        {
            nearPedestal = null;
        }
    }

	void OnCollisionEnter(Collision coll) {
		print ("Player on collision function called");
		switch (coll.collider.tag) {
		case "RedPowerUp":
			print ("Player encountered a red power-up!");
			hasRedPowerUp = true;
			break;
		default:
			print ("Unidentified collision");
			break;
		}
	}
}
