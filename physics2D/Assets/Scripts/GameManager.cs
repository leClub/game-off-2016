using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

	[System.NonSerialized]
	public int time, scrore, userXP;
	[System.NonSerialized]
	public string missionResolution;
	[System.NonSerialized]
	public GameObject targetPlanet;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (missionResolution == "FAIL") {
			Debug.Log("Game over");
			SceneManager.LoadScene("Gameover", LoadSceneMode.Single);
		}
		if (missionResolution == "WIN") {
			Debug.Log("YOU WIN");
			SceneManager.LoadScene("Success", LoadSceneMode.Single);
		}
    }

    public GameObject TargetPlanet {
		get {
			return targetPlanet;
		}
		set {
			targetPlanet = value;
		}
	}

	public int Time {
		get {
			return time;
		}
		set {
			time = value;
		}
	}

	public int Score {
		get {
			return scrore;
		}
		set {
			scrore = value;
		}
	}

	public int UserXP {
		get {
			return userXP;
		}
		set {
			userXP = value;
		}
	}

	public string MissionResolution {
		get {
			return missionResolution;
		}
		set {
			missionResolution = value;
		}
	}
}
