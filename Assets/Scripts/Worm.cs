using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Worm : MonoBehaviour {

    public GameObject head;
    public List<GameObject> mandibles;
    public float mandibleAngleMin = -80;
    public float mandibleAngleMax = 20;

    public int nBodyPartsToCreate = 10;
    public int nBodyParts { get { return bodyParts.Count; } }
    public GameObject bodyPartPrefab;
    public List<GameObject> bodyParts;

    public float groundSpeedMax;
    public float groundSpeed;
    public float airSpeed;
    public float frequencyMult;
    public float a = 0;
    public float aTo = 0;
    public float aSin = 0;
    public float time = 0;

    public GameObject target;
    public GameObject explosionPrefab;
    
    public bool underground {  get { var epi = GetEpicenter(transform.position); if (!epi.HasValue) { Destroy(gameObject); return true; } return transform.position.y < epi.Value.point.y; } }
    private bool undergroundLast = false;

    public List<Vector3> positions;

    private float health = 100;

    private float goTimer = 1f;

	// Use this for initialization
	void Start () {
        groundSpeed = 0;
        a = aTo = Random.value * Mathf.PI * 2;// transform.localEulerAngles.y;
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
        for(int i = 0; i < nBodyPartsToCreate; i++)
        {
            bodyParts.Add(Instantiate<GameObject>(bodyPartPrefab));
            bodyParts[i].transform.SetParent(transform);
            bodyParts[i].transform.localPosition = Vector3.zero;
            bodyParts[i].transform.localScale = new Vector3(10, 10, 10) * Filter(i * 1.0f / (nBodyPartsToCreate - 1));
        }
    }
	
	// Update is called once per frame
	void Update () {

        if (health > 0)
        {
            time += Time.deltaTime;


            var rb = GetComponent<Rigidbody>();

            groundSpeed += (groundSpeedMax - groundSpeed) * 0.05f;
            if (goTimer > 0)
            {
                goTimer -= Time.deltaTime;
            }
            else
            {
                var rand = Random.insideUnitSphere;
                var posTo = target ? target.transform.position : (SeePlayer() ? PlayerScript.S.transform.position : (positions.Count > 0 ? positions[0] + (rand - Vector3.Project(rand, Vector3.up)) * 50 : transform.position));// PlayerScript.S.transform.position;
                aTo = Mathf.Atan2(posTo.z - transform.position.z, posTo.x - transform.position.x);
                a += Utils.angleDiff(a, aTo) * 0.04f;
                //aSin += underground ? 0.1f * Mathf.Sin(time) : 0;
            }

            for (int i = 0; i < mandibles.Count; i++)
            {
                var nsin = (Mathf.Sin(time * 4 + 0.5f * Mathf.Sin(i % (mandibles.Count / 2)) * 2 * Mathf.PI / (mandibles.Count / 2)) + 1) / 2;
                mandibles[i].transform.localEulerAngles = new Vector3((mandibleAngleMin + (mandibleAngleMax - mandibleAngleMin) * nsin + 360) % 360, mandibles[i].transform.localEulerAngles.y, mandibles[i].transform.localEulerAngles.z);
            }

            /*var screenShakeTriggerY = 5 * transform.localScale.x;
            var screenShakeDistanceMax = 1000;
            if (transform.position.y < screenShakeTriggerY && positions.Count > 0 && positions[positions.Count - 1].y >= screenShakeTriggerY)
            {
                var _n = Mathf.Max(0, 1 - (transform.position - PlayerScript.S.transform.position).magnitude / screenShakeDistanceMax);
                var _t = _n * _n * 2;
                ScreenShake.Shake(_t);
                GetComponent<AudioSource>().Play();
            }*/

            rb.useGravity = !underground;
            var xz = groundSpeed * new Vector3(Mathf.Cos(a + aSin), 0, Mathf.Sin(a + aSin));
            var y = Vector3.up * (underground ? rb.velocity.y / 2f : rb.velocity.y);

            if (goTimer <= 0)
            {
                rb.velocity = xz + y;
            }
            else
            {
                rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z) + y;
            }

            var epi = GetEpicenter(transform.position);
            if(!epi.HasValue)
            {
                Destroy(gameObject);
                return;
            }
            var epi_diff = epi.Value.point - transform.position;
            if (underground && epi_diff.sqrMagnitude > 4)
            {
                transform.position += epi_diff.normalized * Time.deltaTime * 4;
            }
            if (rb.velocity.sqrMagnitude > 1)
            {
                var rot = new Quaternion(head.transform.rotation.x, head.transform.rotation.y, head.transform.rotation.z, head.transform.rotation.w);
                head.transform.LookAt(head.transform.position + rb.velocity);// (positions.Count > 0 ? transform.position - positions[positions.Count - 1] : rb.velocity));
                head.transform.Rotate(new Vector3(0, 90, 90));
                head.transform.rotation = Quaternion.Slerp(rot, head.transform.rotation, 0.1f);
            }
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
                n = 1 - Defilter(1 - n);
                bodyParts[i].transform.position = GetPositionAlongPath(1 - n * bodyPercent * stretchMult - startOffset);
            }
        }
        else
        {
            GetComponent<Rigidbody>().isKinematic = true;
            GameObject o;
            if (bodyParts.Count > 0)
                o = bodyParts[bodyParts.Count - 1];
            else
                o = gameObject;

            var explosion = Instantiate<GameObject>(explosionPrefab);
            explosion.transform.position = o.transform.position;
            explosion.transform.localScale = o.transform.lossyScale * 0.25f;

            if (o != gameObject)
                bodyParts.Remove(o);
            Destroy(o);
        }
        /*for(int i = 0; i < positions.Count-1; i++)
        {
            var o = (i + 1) % positions.Count;
            Debug.DrawLine(positions[i], positions[o]);
        }*/

        undergroundLast = underground;
    }

    public bool SeePlayer()
    {
        var pos = transform.position + Vector3.up * 5;
        var diff = PlayerScript.S.transform.position - pos;
        return !Physics.Raycast(pos, diff, diff.magnitude, 1 << 12);
    }

    public RaycastHit? GetEpicenter(Vector3 pos)
    {
        const int distance = 1000000;
        var origin = new Vector3(transform.position.x, distance, transform.position.z);

        RaycastHit hitInfo;
        LayerMask layerGround = 1 << 12;
        if (Physics.Raycast(origin, Vector3.down, out hitInfo, distance, layerGround))
            return hitInfo;
        return null;
    }

    //g is the object that hit the worm
    //c is the collider that was hit ON the worm
    //damage is the amount of damage the hit will do
    public void Hit(GameObject g=null, Collider c=null, float damage=-1)
    {
        health -= damage < 0 ? health : damage;

        

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
