using UnityEngine;
using System.Collections;

public class SpawnPoints : MonoBehaviour {

    static public SpawnPoints S;
    public Vector3[] spawnPoints;
    public int curRoom; // 0 is main room, 1 is left, 2 is right
    public int prevRoom;

    public bool playerHasBow;
    public bool playerHasOrb;

    void Awake() {
        // Set the singleton
        if (!S) S = this;

        // Must consist through levels
        DontDestroyOnLoad(this.gameObject);

        playerHasBow = true;
        playerHasOrb = true;
    }
}
