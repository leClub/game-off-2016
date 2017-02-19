using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetController : MonoBehaviour {

    GameManager gameManager;

    private bool isTarget = false;

    // Use this for initialization
    void Start () {
        gameManager = Camera.main.GetComponent<GameManager>();
        gameObject.transform.Find("Highlight").GetComponent<Canvas>().enabled = false;
    }

    // Update is called once per frame
    void Update () {
        // Check if current planet is the target
        if (gameManager.TargetPlanet.name == gameObject.transform.name) {
            isTarget = true;

            gameObject.transform.Find("Highlight").GetComponent<Canvas>().enabled = true;
        }
    }
}
