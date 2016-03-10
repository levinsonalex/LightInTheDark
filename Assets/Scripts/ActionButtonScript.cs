using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ActionButtonScript : MonoBehaviour {

	public static bool endGame;
    public GameObject worm;

    void Start() {
        endGame = false;
        try {
            worm.SetActive(false);
        }
        catch {
            // do nothing
        }
    }

	// Update is called once per frame
	void Update () {
        PlayerScript player = PlayerScript.S.GetComponent<PlayerScript>();
        if (player.mainCamera.activeSelf)
        {
            transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward,
               Camera.main.transform.rotation * Vector3.up);
        }
        // Begin end game scene
        else if (!player.mainCamera.activeSelf && (player.forceOrb == null) && (player.bow == null) && SpawnPoints.S.curRoom == 0) {
            player.mainCamera.SetActive(true);
            player.droppedEye.SetActive(false);
            player.droppedEye = null;
            EndGame();
        }
    }

    // Initiate end "cutscene"
    void EndGame() {
        endGame = true;
        worm.SetActive(true);
        worm.GetComponent<Worm>().enabled = true;
        worm.GetComponent<Worm>().target = PlayerScript.S.gameObject;
        worm.GetComponent<Rigidbody>().velocity = new Vector3(0f, 0f, 95f);
        Invoke("GameOver", 1f);
    }

    void GameOver() {
        SceneManager.LoadScene("GameOver");
    }
}