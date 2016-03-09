using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Pit : MonoBehaviour {

    public GameObject wormPrefab;
    public GameObject explosionPrefab;

    public List<Vector3> mandibleStartPositions;
    public List<GameObject> mandibles;
    public float mandibleAngleMin;
    public float mandibleAngleMax;
    public float mandibleAngleMinOpening;
    public float mandibleAngleMaxOpening;
    public bool opening;
    private bool createdWormThisOpening = false;

    public List<GameObject> worms;
    private int numWorms = 3;

    private float time = 0;

    // Use this for initialization
    void Start ()
    {
        worms = new List<GameObject>();
        foreach (Transform t in transform)
        {
            if (t.name.Substring(0, Mathf.Min(t.name.Length, 15)) == "SpawnerMandible")
            {
                mandibles.Add(t.gameObject);
                mandibleStartPositions.Add(t.transform.position);
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
        time += Time.deltaTime;

        var needNewWorm = worms.Count < numWorms;

        if(!opening && needNewWorm && Mathf.Floor(Random.value * 100) == 0)
        {
            opening = true;
            time = 0;
        }

        int deadMandibles = 0;
        for (int i = 0; i < mandibles.Count; i++)
        {
            if (!mandibles[i])
            {
                deadMandibles++;
                continue;
            }

            var nsin = (Mathf.Sin(time * 2) + 1) / 2;
            var _min = opening ? mandibleAngleMinOpening : mandibleAngleMin;
            var _max = opening ? mandibleAngleMaxOpening : mandibleAngleMax;
            mandibles[i].transform.localEulerAngles = new Vector3(mandibles[i].transform.localEulerAngles.x + Utils.angleDiffDeg(mandibles[i].transform.localEulerAngles.x, (_min + (_max - _min) * nsin + 360) % 360) * 0.1f, mandibles[i].transform.localEulerAngles.y, mandibles[i].transform.localEulerAngles.z);
            Vector3 manTo = Vector3.zero;
            if (opening)
            {
                manTo = mandibleStartPositions[i] + 
                    new Vector3(0, transform.lossyScale.y * 0.5f * nsin, 0) + (transform.position - mandibleStartPositions[i]) * 0.25f * (1 - (nsin - 0.5f) * 2 * (nsin - 0.5f) * 2);
                if (nsin >= 0.75f && !createdWormThisOpening)
                {
                    var o = Instantiate<GameObject>(wormPrefab);
                    worms.Add(o);
                    o.transform.position = transform.position;
                    o.GetComponent<Rigidbody>().velocity = new Vector3(0, 15, 0);
                    createdWormThisOpening = true;
                }

                if (nsin <= 0.05f)
                {
                    createdWormThisOpening = false;
                    opening = false;
                }

            }
            else
                manTo = mandibleStartPositions[i] + (transform.position + new Vector3(0, transform.lossyScale.y * 0.5f, 0) - mandibleStartPositions[i]) * 0.2f * nsin;
            mandibles[i].transform.position += (manTo - mandibles[i].transform.position) * 0.1f;
        }
        for(int i = 0; i < worms.Count; i++)
        {
            if (worms[i] == null)
                worms.RemoveAt(i--);
        }

        if(deadMandibles >= 4)
        {
            var explosion = Instantiate<GameObject>(explosionPrefab);
            explosion.transform.position = transform.position;
            explosion.transform.localScale = transform.lossyScale;
            for (int i = 0; i < mandibles.Count; i++)
            {
                if (mandibles[i])
                    mandibles[i].GetComponent<SpawnerMandible>().Hit();
            }
            Destroy(gameObject);
        }
    }
}
