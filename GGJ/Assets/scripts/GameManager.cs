using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public static GameManager instance;
    public int[] playerScores;

    public bool randomMap;
    public int gamesToWin;
    [Space()]
    public int numOfPlayers = 2;
    public GameObject[] selectedCharacters;

    [Header("Modifiers")]
    public int maxSmashPower = 25;
    public float bounciness = 0.85f;
    public bool airControl;
    public bool seperateDashCooldown;
    public bool canDashOnGround;
    public bool instantBounceKill;
    public float maxSmashSpeed = 30f;
    public float maxChargeTime = 1.5f;
    public bool fullChargeInvinc;
    public bool holdMaxSmash;
    public bool tightDash;
    public bool doubleJump;
    public float dashDistance = 15;
    public float suddenDeathTimer = 60;

    void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(this);
        } else {
			Destroy(this.gameObject);
        }
        playerScores = new int[numOfPlayers];
     }

    private void Start() {
        if(GameObject.Find("GameOptions") != null)
            GameObject.Find("GameOptions").SetActive(false);
    }

    public int totalScores() {
        int total = 0;
        foreach (int i in playerScores) {
            total += i;
        }

        return total;
    }

    public int highestScore() {
        int max = 0;
        foreach (int i in playerScores) {
            max = Mathf.Max(max, i);
        }
        return max;
    }

    public void maxGames() {
        int.TryParse(GameObject.Find("GameCounter").GetComponent<UnityEngine.UI.InputField>().text, out gamesToWin);
    }

    public int increaseGames(int increment) {
        gamesToWin = increment;
        return gamesToWin;
    }

    public void setRandom(bool random) {
        randomMap = random;
    }

    public void clearScore() {
        for (int j = 0; j < numOfPlayers; j++) {
            playerScores[j] = 0;
        }
    }
}
