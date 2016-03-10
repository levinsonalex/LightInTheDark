// See http://answers.unity3d.com/questions/381183/two-textures-on-one-surface.html
using UnityEngine;
using System.Collections;

public class SideRoomTextObjects : MonoBehaviour
{

    Transform tfLight;
    public GameObject pedestalCamGameObject;

    void Start()
    {
        // Find the 'Pedestal Camera', the game camera used when
        // the eye is placed on a pedestal.
        if (pedestalCamGameObject)
        {
            tfLight = pedestalCamGameObject.transform;
        }
    }

    void Update()
    {
        // If light is pointing at the cube (?)
        if (tfLight && pedestalCamGameObject.activeSelf)
        {
            GetComponent<Renderer>().materials[1].SetVector("_LightPos", tfLight.position);
            GetComponent<Renderer>().materials[1].SetVector("_LightDir", tfLight.forward);
        }
        // Default
        if (!tfLight || !pedestalCamGameObject.activeSelf)
        {
            GetComponent<Renderer>().materials[1].SetVector("_LightPos", tfLight.position);
            GetComponent<Renderer>().materials[1].SetVector("_LightDir", Vector3.down);
        }
    }
}
