using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetController : MonoBehaviour {

    GameManager gameManager;

    private EventManager em;

    private bool isTarget = false;

    // Use this for initialization
    void Start() {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        gameObject.transform.Find("Highlight").GetComponent<Canvas>().enabled = false;

        em = GameObject.Find("EventManager").GetComponent<EventManager>();
    }

    // Update is called once per frame
    void Update() {
        // Check if current planet is the target
		if (gameManager.TargetPlanet.name == gameObject.transform.name) {
			isTarget = true;

			gameObject.transform.Find ("Highlight").GetComponent<Canvas> ().enabled = true;
		} else {
			isTarget = false;
			gameObject.transform.Find ("Highlight").GetComponent<Canvas> ().enabled = false;
		}
			
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (gameManager.TargetPlanet.name == gameObject.transform.name && other.tag == "Player") {
            em.hitTarget();
        }
    }
}
