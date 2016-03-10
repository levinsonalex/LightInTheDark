using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour {

    public float speed = 5f;
    private float curSpeed;
    public GameObject mainCamera;

    public GameObject curWeapon;
    public GameObject sword;
    public GameObject forceOrb;
    public GameObject bow;


    public GameObject nearPedestal;
    public GameObject droppedEye;

    public static GameObject S;
    private Rigidbody rb;

	public bool hasRedPowerUp;
	public bool hasGreenPowerUp;
	public bool hasBluePowerUp;

	public AudioClip powerupsound;
	AudioSource audiosrc;

    [Header("Sword Variables")]
    public float swordAngleMin;
    public float swordAngleMax;
    public float swordAngleReady;
    public float swordAngleMultSwing;
    public float swordAngleMultReturn;
    public float swordAngleTo;
    public float swordAngle;
    public float swordReturnRate = 5;
    public bool swinging = false;
    private Vector3 normalLocalRotation;

    [Header("Bow Variables")]
    public GameObject arrowPrefab;
    public float arrowSpeed = 15f;

    [Header("ForceOrb Variables")]
    public float forceOrbResetTime = 3f;
    private float forceOrbCooldownTime;

    private float JumpSpeed = 30;


    private bool lockedCursor;

    // Use this for initialization
    void Start()
    {
        //// Spawn in the correct spot
        //if (SpawnPoints.S.prevRoom != 0) {
        //    transform.position = SpawnPoints.S.spawnPoints[SpawnPoints.S.prevRoom];
        //    transform.rotation = (SpawnPoints.S.prevRoom == 1) ?
        //        Quaternion.Euler(0f, 90f, 0f) : Quaternion.Euler(0f, -90f, 0f);
        //}

        normalLocalRotation = sword.transform.localEulerAngles;

        hasRedPowerUp = true;
        hasGreenPowerUp = false;
        hasBluePowerUp = false;

        curSpeed = speed;

        // Audio
        if (GetComponents<AudioSource>().Length > 1)
        {
            audiosrc = GetComponents<AudioSource>()[1];
        }

        if (S)
        {
            Debug.Log("Two player objects.");
        }
        else
        {
            S = gameObject;
        }

        rb = GetComponent<Rigidbody>();

        Screen.lockCursor = lockedCursor = true;
    }

    void Update()
    {
        if (Input.GetKeyDown("escape"))
            Screen.lockCursor = lockedCursor = !lockedCursor;

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

        if(Input.GetKey(KeyCode.Space) && IsGrounded())
        {
            rb.velocity = new Vector3(rb.velocity.x, JumpSpeed, rb.velocity.z);
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            curSpeed = 2 * speed;
        }
        else if(Input.GetKeyUp(KeyCode.LeftShift))
        {
            curSpeed = speed;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha1) && sword)
        {
            
            curWeapon = sword;
            if (sword) sword.SetActive(true);
            if (forceOrb) forceOrb.SetActive(false);
            if (bow) bow.SetActive(false);

            hasRedPowerUp = true;
            hasBluePowerUp = false;
            hasGreenPowerUp = false;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2) && forceOrb)
        {
            curWeapon = forceOrb;
            if(sword) sword.SetActive(false);
            if(forceOrb) forceOrb.SetActive(true);
            if(bow) bow.SetActive(false);

            hasRedPowerUp = false;
            hasBluePowerUp = true;
            hasGreenPowerUp = false;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3) && bow)
        {
            curWeapon = bow;
            if (sword) sword.SetActive(false);
            if (forceOrb) forceOrb.SetActive(false);
            if (bow) bow.SetActive(true);

            hasRedPowerUp = false;
            hasBluePowerUp = false;
            hasGreenPowerUp = true;
        }



        // Weapon-related update functions
        if (curWeapon == sword)
        {
            SwordInput();
        }
        else if(curWeapon == forceOrb)
        {
            ForceOrbInput();
        }
        else if(curWeapon == bow)
        {
            BowInput();
        }
    }

    bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, 2 + GetComponent<CapsuleCollider>().height, 1 << 12);
    }

    // Update is called once per frame
    void FixedUpdate () {
        if (rb)
        {
            //Regular Movement
            if (!droppedEye)
            { 
                float forwardMovement = Input.GetAxis("Vertical") * curSpeed;
                float horizontalMovement = Input.GetAxis("Horizontal") * curSpeed;

                //Dumb workaround to let me set the rotation to ground level only.
                GameObject cameraFlat = new GameObject("TEMP");
                cameraFlat.transform.rotation = Camera.main.gameObject.transform.rotation;
                cameraFlat.transform.rotation = Quaternion.Euler(0, cameraFlat.transform.rotation.eulerAngles.y, cameraFlat.transform.rotation.eulerAngles.z);

                //Moves in foward direction relative to camera.
                Vector3 motion = rb.velocity + cameraFlat.transform.TransformDirection(new Vector3(horizontalMovement, 0, forwardMovement));
                Vector3 diff = motion - Vector3.up * motion.y;
                rb.velocity = diff.normalized * Mathf.Min(diff.magnitude, curSpeed) + Vector3.up * motion.y;

                //No one will ever see the workaround. I've destroyed it.
                Destroy(cameraFlat);
            }
            else //Dropped Eye Movement
            {

            }
        }
	}

    void OnTriggerEnter(Collider coll)
    {
        //Debug.Log("Hit " + coll.gameObject.tag + "!");
        if (coll.tag == "Pedestal")
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
		switch (coll.collider.tag) {
			case "RedPowerUp":
				audiosrc.PlayOneShot (powerupsound, 3f);
				Debug.Log ("Player encountered a red power-up!");
				hasRedPowerUp = true;
				coll.gameObject.SetActive(false); // Don't destoy; it causes issues.
				break;
			case "GreenPowerUp":
				audiosrc.PlayOneShot (powerupsound, 3f);
				Debug.Log ("Player encountered a green power-up!");
				hasGreenPowerUp = true;
				coll.gameObject.SetActive(false);
				break;
			case "BluePowerUp":
				audiosrc.PlayOneShot (powerupsound, 3f);
				Debug.Log ("Player encountered a blue power-up!");
				hasBluePowerUp = true;
				coll.gameObject.SetActive(false);
				break;
			default:
				if (coll.collider.tag != "Untagged")
				{
					Debug.Log("Unidentified collision: " + coll.collider.tag);
				}
				break;
		}
    }

    void ForceOrbInput()
    {
        if(forceOrb && Time.time - forceOrbCooldownTime > forceOrbResetTime && Input.GetMouseButton(0))
        {
            Debug.Log("Explode");
            forceOrbCooldownTime = Time.time;
            forceOrb.transform.GetChild(0).GetComponent<ParticleSystem>().Play();
            //Workaround for weird bug
            forceOrb.transform.GetChild(0).GetComponent<ParticleSystem>().startLifetime = forceOrb.transform.GetChild(0).GetComponent<ParticleSystem>().startLifetime;

            GameObject[] wormList = GameObject.FindGameObjectsWithTag("Worm");
            foreach (GameObject worm in wormList)
            {
                if(worm.transform.parent == null && Vector3.Distance(transform.position, worm.transform.position) < 20f)
                {
                    worm.GetComponent<Rigidbody>().AddExplosionForce(100, transform.position, 10, 2, ForceMode.Impulse);
                }
            }
        } 
        else
        {

        }
    }

    void SwordInput()
    {
        if (Input.GetMouseButtonUp(0))
        {
            swinging = true;
        }
        else if (Input.GetMouseButton(0))
        {
            swordAngleTo = swordAngleReady;
            var deg = Utils.angleDiffDeg(swordAngle, swordAngleTo);
            if (deg != 0)
                swordAngle += deg / 5f;
        }
        else if (swinging)
        {
            swordAngleTo = swordAngleMax;
            var deg = Utils.angleDiffDeg(swordAngle, swordAngleTo);
            swordAngle += deg * swordAngleMultSwing;

            if (Mathf.Abs(deg) < 2)
                swinging = false;
        }
        else
        {
            swordAngleTo = swordAngleMin;

            var deg = Utils.angleDiffDeg(swordAngle, swordAngleTo);
            if (Mathf.Abs(deg) > 20)
                swordAngle += Utils.sign(deg) * Mathf.Min(Mathf.Abs(deg), swordReturnRate);
            else
                swordAngle += Utils.angleDiffDeg(swordAngle, swordAngleTo) * swordAngleMultReturn;
        }
        sword.transform.localRotation = Quaternion.Euler(new Vector3(swordAngle, 0, 0) + normalLocalRotation);
    }

	void BowInput() {
		if (Input.GetMouseButtonUp(0)) {
			Debug.Log ("Shooting from bow");
            GameObject arrow = Instantiate(arrowPrefab, arrowPrefab.transform.position, arrowPrefab.transform.rotation) as GameObject;
            Vector3 motion = mainCamera.transform.TransformDirection(new Vector3(0, 0, 1 * arrowSpeed));
            arrow.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
            arrow.GetComponent<Rigidbody>().velocity = motion;
            arrow.GetComponent<BoxCollider>().isTrigger = false;
        }
	}
}
