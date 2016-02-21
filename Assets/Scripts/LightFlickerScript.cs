using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LightFlickerScript : MonoBehaviour {
    public float u;
    public List<float> points;
    [Header("This is a header")]
    public float pow = 1;
    public float sinStrength = 0.2f;
    public float timeLimit = 5.0f;

    // Use this for initialization
    void Start () {
	
	}

    // Update is called once per frame
    void Update()
    {
        u = Time.time % timeLimit;
        //        u = Mathf.Pow(u,pow);
        //        u = 1 - Mathf.Pow(1-u, pow);
        //u = u + Mathf.Sin(2 * Mathf.PI * u) * sinStrength;
        float p = Interp(u, points);
        this.GetComponent<Light>().intensity = p;
    }

    float Interp(float u, List<float> pS, int i0 = 0, int i1 = -1)
    {
        if (i1 == -1)
        {
            i1 = pS.Count - 1;
        }
        if (i0 == i1) return pS[i0];
        float pL = Interp(u, pS, i0, i1 - 1);
        float pR = Interp(u, pS, i0 + 1, i1);
        float pLR = ((timeLimit - u) * pL + u * pR) / timeLimit;
        return pLR;
    }
}
