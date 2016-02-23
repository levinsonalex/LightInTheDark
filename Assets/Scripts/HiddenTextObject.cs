// See http://answers.unity3d.com/questions/381183/two-textures-on-one-surface.html
using UnityEngine;
using System.Collections;

public class HiddenTextObject : MonoBehaviour {

	Transform tfLight;
	public GameObject pedestalCamGameObject;
	public GameObject player;
	public int redMaterialIndex;
	public int greenMaterialIndex;
	public int blueMaterialIndex;

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
		// Kind of hacky...
		redMaterialIndex = FindMaterial ("HiddenRedText (Instance)");
		greenMaterialIndex = FindMaterial ("HiddenGreenText (Instance)");
		blueMaterialIndex = FindMaterial ("HiddenBlueText (Instance)");
	}

	void Update()
	{
		// If light is pointing at the cube (?)
		if (tfLight
		    && pedestalCamGameObject.activeSelf
			&& player.GetComponent<PlayerScript> ().hasRedPowerUp
			&& redMaterialIndex != -1) {
			GetComponent<Renderer> ().materials [redMaterialIndex].SetVector ("_LightPos", tfLight.position);
			GetComponent<Renderer> ().materials [redMaterialIndex].SetVector ("_LightDir", tfLight.forward);
		}
		if (tfLight
			&& pedestalCamGameObject.activeSelf
			&& player.GetComponent<PlayerScript>().hasGreenPowerUp
			&& greenMaterialIndex != -1)
		{
			GetComponent<Renderer>().materials[greenMaterialIndex].SetVector("_LightPos", tfLight.position);
			GetComponent<Renderer>().materials[greenMaterialIndex].SetVector("_LightDir", tfLight.forward);
		}
		if (tfLight
			&& pedestalCamGameObject.activeSelf
			&& player.GetComponent<PlayerScript>().hasBluePowerUp
			&& blueMaterialIndex != -1)
		{
			GetComponent<Renderer>().materials[blueMaterialIndex].SetVector("_LightPos", tfLight.position);
			GetComponent<Renderer>().materials[blueMaterialIndex].SetVector("_LightDir", tfLight.forward);
		}
		// Default
		if (!tfLight || !pedestalCamGameObject.activeSelf)
		{
			// Assumes default material is first in array
			for (int i = 0; i < GetComponent<Renderer> ().materials.Length-1; ++i) {
				//print (GetComponent<Renderer> ().materials [i].name);
				GetComponent<Renderer>().materials[i].SetVector("_LightPos", tfLight.position);
				GetComponent<Renderer>().materials[i].SetVector("_LightDir", Vector3.down);
			}
		}
	}

	// Returns true if the object has this material. (May wish to move this elsewhere.)
	int FindMaterial(string name)
	{
		for (int i = 0; i < GetComponent<Renderer>().materials.Length; ++i) {
			//print ("Compare: " + name + ", " + GetComponent<Renderer>().materials[i].name);
			if (GetComponent<Renderer>().materials[i].name == name) {
				print ("Material " + name + " found at index " + i);
				return i;
			}
		}
		return -1;
	}

}
