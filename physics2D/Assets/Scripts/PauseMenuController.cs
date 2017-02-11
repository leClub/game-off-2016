using UnityEngine;
using System.Collections;

public class PauseMenuController : MonoBehaviour {

    public GameObject pauseMenuObject;
    private bool isActive = false;

	// Use this for initialization
	void Start () {

    }
	
	// Update is called once per frame
	void Update () {
        if (isActive == true) {
            pauseMenuObject.SetActive(true);
            Time.timeScale = 0;
        } else {
            pauseMenuObject.SetActive(false);
            Time.timeScale = 1;
        }
    }

    public void GamePause() {
        isActive = true;
    }

    public void GameResume() {
        isActive = false;
    }
}
