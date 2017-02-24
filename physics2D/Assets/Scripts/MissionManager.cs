using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class MissionManager : MonoBehaviour {


	GameManager gameManager;

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

	// Use this for initialization
	void Start () {
		// Start listn to target hit event
        EventManager.targetReached += Test;

		// Load gamemanager
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

		// Load timer
        timerText = GameObject.Find("Timer").GetComponent<Text>();	
		timeInSeconds = maxTimeInSeconds;

		// Load score
		scoreText = GameObject.Find("Score").GetComponent<Text>();	

		// Choose a target
        pickTarget();
	}

	// Choose the target planet randomly
	void pickTarget () {
		// Choose the planet gameobject
		targets = GameObject.FindGameObjectsWithTag("Planet");
		int index = Random.Range (0, targets.Length);
		targetPlanet = targets[index];

		// Set the game manager target
		gameManager.TargetPlanet = targetPlanet;
	}

	// Update is called once per frame
	void Update () {
		DisplayTimer ();

		if (timeInSeconds < 1) {
			missionStatus = "FAIL";
			gameManager.MissionResolution = missionStatus;
		}


		DisplayScore();
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

    void Test() {
		// Choose the next target
		pickTarget();

		// Update time and score
		timeInSeconds += 25;
		currentScore += scoreStep;
		gameManager.Score = currentScore;
    }
}
