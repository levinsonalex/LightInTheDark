using UnityEngine;
using System.Collections;

public class DemoScript : MonoBehaviour {

    public GameObject worm;
    public GameObject[] powerups;

    void Update() {
        int powerupCount = 0;
        for (int i = 0; i < 3; i++) {
            if (powerups[i].active == false) {
                powerupCount++;
            }
        }
        if (powerupCount == 3) {
            worm.GetComponent<Worm>().enabled = true;
            worm.SetActive(true);
        }
        else {
            worm.GetComponent<Worm>().enabled = false;
            worm.SetActive(false);
        }
    }
}
