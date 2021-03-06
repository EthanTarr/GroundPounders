﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class endingUI : MonoBehaviour {

    public static endingUI instance;
    private Animator cameraAnim;
    public AudioClip ding;
    [HideInInspector] public bool endConditionMet;

    bool inputallowed;

    string levelName;
    public int numPlayers = 2;

    public float spacing;
    float width;
    public float size = 0;

    [Space()]
    public Text[] scoreText;
    public Text[] dashText;

    void Start() {
        cameraAnim = Camera.main.GetComponent<Animator>();
        instance = this;
    }

    void Update() {
        if (inputallowed && Input.anyKeyDown) {
            inputallowed = false;
            if (GameManager.instance.highestScore() >= GameManager.instance.gamesToWin) {
                if (AchievemtManager.instance != null) {
                    AchievemtManager.instance.updateMapStats(Application.loadedLevel);
                    AchievemtManager.instance.checkMaps();
                }
                StartCoroutine(screenTransition.instance.fadeOut("VictoryScreen", 1f, true));
            } else {
                if(GameManager.instance.randomMap)
                    StartCoroutine(screenTransition.instance.fadeOut(Random.Range(3, Application.levelCount - 1)));
                else
                    StartCoroutine(screenTransition.instance.fadeOut(levelName));
            }
        }
    }

    public int getRandomStage() {
        int[] validStages = new int[Application.levelCount - 2];
        for (int i = 0; i < validStages.Length - 1; i++) {
            if (i == Application.loadedLevel)
            {
                i++;
                validStages[i - 1] = 2 + i;
            }
            else {
                validStages[i] = 2 + i;
            }
            
        }
        return validStages[Random.Range(2, validStages.Length)];
    }

    public IEnumerator checkPlayersLeft() {      
        yield return new WaitForSeconds(0.75f);

        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        if (players.Length == 1) {
            endConditionMet = true;
            StopCoroutine("ending");
            StartCoroutine(ending(players[0].GetComponent<playerController>().playerNum));
        } else if(players.Length == 0) {
            StartCoroutine(ending(-1));
        }
        
              
    }

    IEnumerator ending(int playerId) {
        setupLayout();
        yield return new WaitForSeconds(0.5f);
        cameraAnim.Play("endingTransition");
        yield return new WaitForSeconds(0.25f);
        for (int i = 0; i < transform.childCount; i++)  {
            transform.GetChild(i).gameObject.SetActive(true);
        }

        if (playerId != -1) {
            for (int i = 0; i < GameManager.instance.numOfPlayers; i++) {
                scoreText[i].text = "" + GameManager.instance.playerScores[i];
            }

            yield return new WaitForSeconds(0.5f);
            GameManager.instance.playerScores[playerId]++;
            audioManager.instance.Play(ding, 0.5f, 1);
        }
        inputallowed = true;
        for (int i = 0; i < GameManager.instance.numOfPlayers;i++) {
            scoreText[i].text = "" + GameManager.instance.playerScores[i];
        }
        yield return new WaitForSeconds(1f);

        if (AchievemtManager.instance != null) {
            AchievemtManager.instance.updateRoundCount();
        }
    }

    void setupLayout() {
        numPlayers = GameManager.instance.numOfPlayers;
        levelName = Application.loadedLevelName;
        width = spacing * 4 * numPlayers;

        // Dash positioning
        for (int i = 0; i < numPlayers - 1; i++) {
            if (i > scoreText.Length) {
                Debug.LogError("not enough scoreText");
            }
            Text text = dashText[i];
            text.transform.position = transform.position - Vector3.right * (width / 2 - width / (numPlayers - 1) * i) 
                + Vector3.right * (width / (numPlayers - 1)) / 2;
        }

        // Dash Placement
        for (int i = 0; i < numPlayers; i++) {
            if (i > scoreText.Length) {
                Debug.LogError("not enough scoreText");
            }
            Text text = scoreText[i];
            text.text = "" + GameManager.instance.playerScores[i];
            text.color = playerSpawner.instance.characterColors[i];
            text.transform.position = transform.position - Vector3.right * (width / 2 - width / (numPlayers - 1) * i);
        }
    }

    void OnDrawGizmos() {
        width = spacing * 4 * numPlayers;
            
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector3(width, 1, 1));

        for (int i = 0; i < numPlayers; i++) {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position - Vector3.right * (width / 2 - width / (numPlayers - 1) * i), 1);
        }

        for (int i = 0; i < numPlayers - 1; i++) {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(transform.position - Vector3.right * (width / 2 - width / (numPlayers - 1) * i) 
                + Vector3.right * (width/(numPlayers - 1))/2, new Vector3(1.0f,0.7f,1));
        }
    }
    
}
