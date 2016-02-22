using UnityEngine;
using System.Collections;

public class HiddenTextObject : MonoBehaviour {

	Transform tfLight;
 
	void Start()
	{
		// find the revealing light named "RevealingLight":
		var goLight = GameObject.Find("FlashlightHead");
		if (goLight) tfLight = goLight.transform;
	}

	void Update()
	{
		if (tfLight)
		{
			GetComponent<Renderer>().material.SetVector("_LightPos", tfLight.position);
			GetComponent<Renderer>().material.SetVector("_LightDir", tfLight.forward);
		}
	}

}
