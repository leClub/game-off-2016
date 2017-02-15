using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class MissionManager : MonoBehaviour {

	GameManager gameManager;
	public Camera mainCamera;

	public Text timerText;

	public float maxTimeInSeconds;

	private string timeRemaining;
	private float timeInSeconds, minutes, seconds;

	private string missionStatus;

	// Use this for initialization
	void Start () {
		timerText = GameObject.Find("Timer").GetComponent<Text>();	
		timeInSeconds = maxTimeInSeconds;

		gameManager = mainCamera.GetComponent<GameManager>();
	}
	
	// Update is called once per frame
	void Update () {
		DisplayTimer ();

		if (timeInSeconds < 1) {
			missionStatus = "FAIL";
			gameManager.MissionResolution = missionStatus;
		}
	}

	private void DisplayTimer () {
		timeInSeconds -= Time.deltaTime;

		int minutes = Mathf.FloorToInt(timeInSeconds / 60F);
		int seconds = Mathf.FloorToInt(timeInSeconds - minutes * 60);
		string timeRemaining = string.Format("{0:0}:{1:00}", minutes, seconds);

		timerText.text = timeRemaining;
	}
}
