using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LevelSwitch : MonoBehaviour {

    // Set in Inspector
    public string levelname;

    void OnTriggerEnter(Collider coll) {
        SceneManager.LoadScene(levelname);
    }
}
