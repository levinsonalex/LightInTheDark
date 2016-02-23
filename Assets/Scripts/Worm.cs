using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Worm : MonoBehaviour {

    public GameObject head;
    public List<GameObject> mandibles;
    public float mandibleAngleMin = -80;
    public float mandibleAngleMax = 20;

    public int nBodyParts = 10;
    public GameObject bodyPartPrefab;
    public List<GameObject> bodyParts;

    public float groundSpeed = 8;
    public float airSpeed;
    public float frequencyMult;
    public float a = 0;
    public float aTo = 0;
    public float aSin = 0;
    public float time = 0;

    public GameObject target;

    private float yGround;
    public bool underground {  get { return transform.position.y < yGround; } }
    private bool undergroundLast = false;

    public List<Vector3> positions;

	// Use this for initialization
	void Start () {
        a = aTo = Mathf.PI;// transform.localEulerAngles.y;
        yGround = 0;/// transform.position.y;
        foreach (Transform t0 in transform)
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
        for(int i = 0; i < nBodyParts; i++)
        {
            bodyParts.Add(Instantiate<GameObject>(bodyPartPrefab));
            bodyParts[i].transform.position = transform.position;
            bodyParts[i].transform.localScale = new Vector3(10, 10, 10) * Filter(i * 1.0f / (nBodyParts - 1)) * transform.localScale.x;
        }
    }
	
	// Update is called once per frame
	void Update () {
        time += Time.deltaTime;


        var rb = GetComponent<Rigidbody>();

        if (rb.velocity.sqrMagnitude > 1)
        {
            head.transform.LookAt(head.transform.position + rb.velocity);
            head.transform.Rotate(new Vector3(0, 90, 90));
        }

        var posTo = target.transform.position;// PlayerScript.S.transform.position;
        aTo = Mathf.Atan2(posTo.z - transform.position.z, posTo.x - transform.position.x);
        a += Utils.angleDiff(a, aTo) * 0.005f;
        //aSin += underground ? 0.1f * Mathf.Sin(time) : 0;

        for (int i = 0; i < mandibles.Count; i++)
        {
            var nsin = (Mathf.Sin(time * 4 + 0.5f * Mathf.Sin(i  % (mandibles.Count / 2)) * 2 * Mathf.PI / (mandibles.Count / 2)) + 1) / 2;
            mandibles[i].transform.localEulerAngles = new Vector3((mandibleAngleMin + (mandibleAngleMax - mandibleAngleMin) * nsin + 360) % 360, mandibles[i].transform.localEulerAngles.y, mandibles[i].transform.localEulerAngles.z);
        }

        var screenShakeTriggerY = 5 * transform.localScale.x;
        var screenShakeDistanceMax = 1000;
        if (transform.position.y < screenShakeTriggerY && positions.Count > 0 && positions[positions.Count-1].y >= screenShakeTriggerY)
        {
            var _n = Mathf.Max(0, 1 - (transform.position - PlayerScript.S.transform.position).magnitude / screenShakeDistanceMax);
            var _t = _n * _n * 2;
            ScreenShake.Shake(_t);
            GetComponent<AudioSource>().Play();
        }

        rb.useGravity = !underground;
        rb.velocity = new Vector3(
            Mathf.Cos(a + aSin) * groundSpeed,
            Mathf.Cos(time * frequencyMult) * airSpeed,//underground ? rb.velocity.y + airSpeed : 0,//
            Mathf.Sin(a + aSin) * groundSpeed
        );
        positions.Add(transform.position);

        float startOffset = 0.1f;
        float stretchMult = 0.6f;
        float bodyLength = 100 * transform.localScale.x;
        while (GetPathLength() > bodyLength)
            positions.RemoveAt(0);
        float bodyPercent = bodyLength / GetPathLength();
        for (int i = 0; i < nBodyParts; i++)
        {
            var n = i * 1.0f / (nBodyParts - 1);
            n = 1 - Defilter(1-n);
            bodyParts[i].transform.position = GetPositionAlongPath(1 - n * bodyPercent * stretchMult - startOffset);
        }
        /*for(int i = 0; i < positions.Count-1; i++)
        {
            var o = (i + 1) % positions.Count;
            Debug.DrawLine(positions[i], positions[o]);
        }*/

        undergroundLast = underground;
    }

    float Filter(float t)
    {
        return Mathf.Sqrt(1 - t);
    }
    float Defilter(float t)
    {
        return t * t;
    }
    float GetPathLength(bool loop=false)
    {
        float dPositions = 0;
        for (int i = 0; i < positions.Count - (loop ? 0 : 1); i++)
        {
            var o = (i + 1) % positions.Count;
            dPositions += (positions[o] - positions[i]).magnitude;
        }
        return dPositions;
    }
    Vector3 GetPositionAlongPath(float t, bool loop=false)
    {
        float dPositions = GetPathLength(loop);
        float dSum = 0;
        for (int i = 0; i < positions.Count-(loop ? 0 : 1); i++)
        {
            var o = (i + 1) % positions.Count;
            var diff = positions[o] - positions[i];
            var dDiff = diff.magnitude;
            var dSumPrev = dSum;
            dSum += dDiff;
            if (t <= dSum / dPositions && t >= dSumPrev / dPositions)
            {
                float percent = (t - dSumPrev / dPositions) * dPositions / dDiff;
                return positions[i] + diff.normalized * percent * dDiff;
            }
        }
        return Vector3.zero;
    }
}
