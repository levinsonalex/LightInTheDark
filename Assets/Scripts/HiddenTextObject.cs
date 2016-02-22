// See http://answers.unity3d.com/questions/381183/two-textures-on-one-surface.html
using UnityEngine;
using System.Collections;

public class HiddenTextObject : MonoBehaviour {

	Transform tfLight;
	public GameObject pedestalCamGameObject;
	public GameObject player;
 
	void Start()
	{
		// Find the 'Pedestal Camera', the game camera used when
		// the eye is placed on a pedestal.
		if (pedestalCamGameObject)
		{
			tfLight = pedestalCamGameObject.transform;
		}
		if (!player) {
			player = GameObject.Find ("PlayerBody");
		}
	}

	void Update()
	{
		// If light is pointing at the cube (?)
		if (tfLight && pedestalCamGameObject.activeSelf
			&& player.GetComponent<PlayerScript>().hasBluePowerUp)
		{
			// Updates the first material attached to this object, the hidden material.
			// Specifically, communicates the light's position and direction to the shader.
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
