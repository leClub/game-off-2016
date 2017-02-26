using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MissionManager : MonoBehaviour {

	// Timer
	[System.NonSerialized]
	public Text timerText;
	public float maxTimeInSeconds;
	private string timeRemaining;
	private float timeInSeconds, minutes, seconds;

	// Score
	[System.NonSerialized]
	public Text scoreText;
	private int currentScore;
	public int scoreStep = 400;

	// Mission status
	private string missionStatus;

	// Mission target
	private GameObject[] targets;
	private GameObject targetPlanet;

    // Mission resolution time to appear
    public int timeAfterMissionResolution = 3;

	// Use this for initialization
	void Start () {
		// Start listn to target hit event
        EventManager.hitTargetEvent += HitTarget;
        EventManager.crashEvent += Crash;
        EventManager.outOfLimitsEvent += OutOfLimits;
        EventManager.outOfTimeEvent += OutOfTime;

        // Load timer
        timerText = GameObject.Find("Timer").GetComponent<Text>();	
		timeInSeconds = maxTimeInSeconds;

		// Load score
		scoreText = GameObject.Find("Score").GetComponent<Text>();

        // Choose a target
        PickTarget();
	}

	// Choose the target planet randomly
	void PickTarget() {
		// Choose the planet gameobject
		targets = GameObject.FindGameObjectsWithTag("Planet");
		int index = Random.Range (0, targets.Length);
		targetPlanet = targets[index];

		// Set the game manager target
		GameManager.gameManager.TargetPlanet = targetPlanet;
	}

	// Update is called once per frame
	void Update () {
		DisplayTimer ();
        DisplayScore();

        if (timeInSeconds < 1) {
            OutOfTime();
        }
    }

	// Calculate and display the timer
	private void DisplayTimer () {
		timeInSeconds -= Time.deltaTime;

		int minutes = Mathf.FloorToInt(timeInSeconds / 60F);
		int seconds = Mathf.FloorToInt(timeInSeconds - minutes * 60);
		string timeRemaining = string.Format("{0:0}:{1:00}", minutes, seconds);

		timerText.text = timeRemaining;
	}

	private void DisplayScore() {
		scoreText.text = currentScore.ToString();
	}

    void HitTarget() {
        Debug.Log("Hit target !");
        // Choose the next target
        PickTarget();

		// Update time and score
		timeInSeconds += 25;
		currentScore += scoreStep;
		GameManager.gameManager.Score = currentScore;
    }

    void Crash() {
        Debug.Log("Crash !");
        missionStatus = "CRASH";
        StartCoroutine(ToGameOver(timeAfterMissionResolution));
    }

    void OutOfLimits() {
        Debug.Log("Out of limits !");
        missionStatus = "OUTOFLIMITS";
        StartCoroutine(ToGameOver(timeAfterMissionResolution));
    }

    void OutOfTime() {
        Debug.Log("Out of time !");
        missionStatus = "OUTOFTIME";
        StartCoroutine(ToGameOver(timeAfterMissionResolution));
    }

    IEnumerator ToGameOver(int seconds) {
        yield return new WaitForSeconds(seconds);
        GameManager.gameManager.MissionResolution = missionStatus;
        SceneManager.LoadScene("GameOver", LoadSceneMode.Single);
    }
}
