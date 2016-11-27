using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void ToMain() {
        Debug.Log("Start !");
        SceneManager.LoadScene("Main", LoadSceneMode.Single);
    }

    public void ToHome() {
        Debug.Log("Back home");
        SceneManager.LoadScene("Home", LoadSceneMode.Single);
    }

    public void QuitGame() {
        Application.Quit();
    }
}
