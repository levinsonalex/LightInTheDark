using UnityEngine;

//http://answers.unity3d.com/questions/283086/headbobber-script-in-c.html

public class CameraBobScript : MonoBehaviour
{

    public Vector3 restPosition; //local position where your camera would rest when it's not bobbing.
    public float transitionSpeed = 20f; //smooths out the transition from moving to not moving.
    public float bobSpeed = 4.8f; //how quickly the player's head bobs.
    public float bobAmount = 0.05f; //how dramatic the bob is. Increasing this in conjunction with bobSpeed gives a nice effect for sprinting.

    float timer = Mathf.PI / 2; //initialized as this value because this is where sin = 1. So, this will make the camera always start at the crest of the sin wave, simulating someone picking up their foot and starting to walk--you experience a bob upwards when you start walking as your foot pushes off the ground, the left and right bobs come as you walk.

    void Awake()
    {

    }

    void Update()
    {
        if (PlayerScript.S.GetComponent<Rigidbody>().velocity.magnitude > .5) //moving
        {
            timer += bobSpeed * Time.deltaTime;

            //use the timer value to set the position
            Vector3 newPosition = new Vector3(Mathf.Cos(timer) * bobAmount, restPosition.y + Mathf.Abs((Mathf.Sin(timer) * bobAmount)), restPosition.z); //abs val of y for a parabolic path
            transform.localPosition = newPosition;
        }
        else
        {
            timer = Mathf.PI / 2; //reinitialize

            Vector3 newPosition = new Vector3(Mathf.Lerp(transform.localPosition.x, restPosition.x, transitionSpeed * Time.deltaTime), Mathf.Lerp(transform.localPosition.y, restPosition.y, transitionSpeed * Time.deltaTime), Mathf.Lerp(transform.localPosition.z, restPosition.z, transitionSpeed * Time.deltaTime)); //transition smoothly from walking to stopping.
            transform.localPosition = newPosition;
        }

        if (timer > Mathf.PI * 2) //completed a full cycle on the unit circle. Reset to 0 to avoid bloated values.
            timer = 0;
    }
}