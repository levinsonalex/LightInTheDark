using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Worm : MonoBehaviour {

    public List<GameObject> mandibles;
    public float mandibleAngleMin = -80;
    public float mandibleAngleMax = 40;

    public GameObject bodyPartPrefab;
    public List<GameObject> bodyParts;

	// Use this for initialization
	void Start () {
	    foreach(Transform t0 in transform)
        {
            if (t0.name == "Head")
            {
                foreach (Transform t in t0.transform)
                {
                    if (t.name.Substring(0, Mathf.Min(t.name.Length, 8)) == "Mandible")
                    {
                        mandibles.Add(t.gameObject);
                    }
                }
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
	    for(int i = 0; i < mandibles.Count; i++)
        {
            mandibles[i].transform.localEulerAngles = new Vector3(mandibleAngleMin + (mandibleAngleMax - mandibleAngleMin) * (Mathf.Sin(Time.time*4) + 1)/2, mandibles[i].transform.localEulerAngles.y, mandibles[i].transform.localEulerAngles.z);
        }
	}
}
