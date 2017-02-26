using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class GameOverController : MonoBehaviour {

    private Text gameOverText, scoreText, moneyText, totalMoneyText;
    private bool moneyTransfert = false;

    // Use this for initialization
    void Start() {
        // Declare all ui elements
        gameOverText = GameObject.Find("GameOverText").GetComponent<Text>();
        scoreText = GameObject.Find("ScoreText").GetComponent<Text>();
        moneyText = GameObject.Find("MoneyText").GetComponent<Text>();

        // If game manager is instantiated
        if (GameManager.gameManager != null) { 
            // Fill game over text
            if (GameManager.gameManager.MissionResolution != null) { 
                switch(GameManager.gameManager.MissionResolution) {
                    case "SUCCESS":
                        gameOverText.text = "Mission accomplie !";
                        break;
                    case "CRASH":
                        gameOverText.text = "Plus qu'à attendre qu'on vienne vous chercher... !";
                        break;
                    case "OUTOFLIMITS":
                        gameOverText.text = "Vous vous êtes égaré ?";
                        break;
                    case "OUTOFTIME":
                        gameOverText.text = "Oups, les colis n'ont pas été livrés à temps !";
                        break;
                    case "FAIL":
                        break;
                }
            }

            // Fill previous mission money
            GameManager.gameManager.Load();
            scoreText.text = "+" + GameManager.gameManager.Score.ToString() + "$";
            moneyText.text = GameManager.gameManager.Money.ToString() + "$";
            StartCoroutine(MoneyTransfertTimeout(3));
        }
    }

    void MoneyTransfert() {
        GameManager.gameManager.Money += GameManager.gameManager.Score;
        GameManager.gameManager.Score = 0;
        scoreText.text = "+" + GameManager.gameManager.Score.ToString() + "$";
        moneyText.text = GameManager.gameManager.Money.ToString() + "$";
        GameManager.gameManager.Save();
    }

    IEnumerator MoneyTransfertTimeout(int seconds) {
        yield return new WaitForSeconds(seconds);
        MoneyTransfert();
    }
}
