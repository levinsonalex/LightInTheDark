using UnityEngine;
using System.Collections;

public class HiddenTextObject : MonoBehaviour {

	Transform tfLight;
	public GameObject pedestalCamGameObject;
 
	void Start()
	{
		// find the revealing light named "RevealingLight":
		if (pedestalCamGameObject)
		{
			tfLight = pedestalCamGameObject.transform;
		}
	}

	void Update()
	{
		if (tfLight && pedestalCamGameObject.activeSelf)
		{
			GetComponent<Renderer>().material.SetVector("_LightPos", tfLight.position);
			GetComponent<Renderer>().material.SetVector("_LightDir", tfLight.forward);
		}
		else
		{
			GetComponent<Renderer>().material.SetVector("_LightPos", tfLight.position);
			GetComponent<Renderer>().material.SetVector("_LightDir", Vector3.down);
		}
	}

}
