using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour {

    public void ToMain() {
        Debug.Log("Start !");
        SceneManager.LoadScene("TourMission", LoadSceneMode.Single);
    }

    public void ToHome() {
        Debug.Log("Back home");
        SceneManager.LoadScene("Home", LoadSceneMode.Single);
    }

    public void QuitGame() {
        Debug.Log("Quit");
        Application.Quit();
    }
}
