using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class MissionManager : MonoBehaviour {


	GameManager gameManager;

	// Timer
	public Text timerText;
	public float maxTimeInSeconds;
	private string timeRemaining;
	private float timeInSeconds, minutes, seconds;

	// Mission status
	private string missionStatus;

	// Mission target
	private GameObject[] targets;
	private GameObject targetPlanet;

	// Use this for initialization
	void Start () {
        gameManager = Camera.main.GetComponent<GameManager>();

        timerText = GameObject.Find("Timer").GetComponent<Text>();	
		timeInSeconds = maxTimeInSeconds;

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
	}

	// Calculate and display the timer
	private void DisplayTimer () {
		timeInSeconds -= Time.deltaTime;

		int minutes = Mathf.FloorToInt(timeInSeconds / 60F);
		int seconds = Mathf.FloorToInt(timeInSeconds - minutes * 60);
		string timeRemaining = string.Format("{0:0}:{1:00}", minutes, seconds);

		timerText.text = timeRemaining;
	}
}
