using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class GameManager : MonoBehaviour {

    public static GameManager gameManager;

	[System.NonSerialized]
	public int time, scrore, totalMoney, money;
	[System.NonSerialized]
	public string missionResolution;
	[System.NonSerialized]
	public GameObject targetPlanet;

	// Use this for initialization
	void Awake () {
        if (gameManager == null) {
            DontDestroyOnLoad(gameObject);
            gameManager = this;
        } else if(gameManager != this) {
            Destroy(gameObject);
        }
	}

    public void Save() {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + "/playerInfo.dat", FileMode.OpenOrCreate);

        PlayerData data = new PlayerData();
        data.Money = Money;

        bf.Serialize(file, data);
        file.Close();
    }

    public void Load() {
        if (File.Exists(Application.persistentDataPath + "/playerInfo.dat")){
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/playerInfo.dat", FileMode.Open);
            PlayerData data = (PlayerData)bf.Deserialize(file);
            file.Close();

            Money = data.Money;
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

    public int Money {
		get {
			return money;
		}
		set {
			money = value;
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

[Serializable]
class PlayerData {
    public int Money;
}