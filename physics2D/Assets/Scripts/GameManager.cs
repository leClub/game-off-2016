using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

	[System.NonSerialized]
	public int time, scrore, userXP;
	public string missionResolution;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		Debug.Log (missionResolution);
		if (missionResolution == "FAIL") {
			Debug.Log("Back home");
			SceneManager.LoadScene("Home", LoadSceneMode.Single);
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

	public int Scrore {
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
