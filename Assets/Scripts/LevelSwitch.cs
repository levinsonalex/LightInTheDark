using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LevelSwitch : MonoBehaviour {

    // Set in Inspector
    public string levelName;
    public int levelIndex;

    void OnTriggerEnter(Collider coll) {
        if (coll.gameObject.tag != "Player") return;
        SpawnPoints.S.prevRoom = SpawnPoints.S.curRoom;
        SpawnPoints.S.curRoom = levelIndex;
        SceneManager.LoadScene(levelName);
    }
}
