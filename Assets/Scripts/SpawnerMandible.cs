using UnityEngine;
using System.Collections;

public class SpawnerMandible : MonoBehaviour {

    public GameObject explosionPrefab;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Hit()
    {
        var explosion = Instantiate<GameObject>(explosionPrefab);
        explosion.transform.position = transform.position;
        explosion.transform.localScale = transform.lossyScale * 0.25f;
        Destroy(gameObject);
    }
}
