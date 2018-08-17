using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class victoryScreenManager : MonoBehaviour
{

    public GameObject followConfetti;
    public GameObject confetti;
    public playerController winner;
    public GameObject placeUI;
    public GameObject TextToEscape;
    public GameObject[] spotLights;
    public GameObject[] animSpotLights;
    [Space()]
    public AudioClip drumroll;
    public AudioClip end;
    public AudioClip ding;


    void Start() {
        followConfetti.SetActive(false);
        confetti.SetActive(false);
        playerSpawner.instance.activatePlayers();
        playerNum[] p = FindObjectsOfType<playerNum>();

        foreach (playerNum popp in p) {
            Destroy(popp.gameObject);
        }

        TextToEscape.SetActive(false);
        StartCoroutine("placementSequence");
    }

    IEnumerator placementSequence() {
        playerController[] players = playerSpawner.instance.players;

        Array.Sort(players, (a,b) => GameManager.instance.playerScores[a.playerNum].CompareTo(GameManager.instance.playerScores[b.playerNum]));
        yield return new WaitForSeconds(1.5f);

        GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(1f);
        for (int i = 0; i < 4; i++) {
            yield return new WaitForSeconds(0.5f * (i + 1));
            if (i < players.Length) {
                GameObject newUI = Instantiate(placeUI, players[i].transform);
                newUI.transform.position = players[i].transform.position;
                newUI.transform.localScale = Vector3.one * 0.005542449f;
                newUI.transform.position += Vector3.up * 1.5f;
                newUI.GetComponent<TextMesh>().text = "" + (players.Length - i);
                newUI.GetComponent<TextMesh>().color = players[i].spriteAnim.GetComponent<SpriteRenderer>().color;
                if (i == players.Length - 1)
                {
                    winner = players[i];
                    GetComponent<AudioSource>().loop = false;
                    GetComponent<AudioSource>().clip = end;
                    GetComponent<AudioSource>().Play();
                }
                audioManager.instance.Play(ding, 1, 0.85f + 0.1f * i);
            }
        }

        yield return new WaitForSeconds(0f);
        TextToEscape.SetActive(true);

        int winnerNum = (players[players.Length - 1].playerNum + 1);
        string playerButton = (players[winnerNum].playerControl.Contains("Joy")) ? "Start" : "ESC";

        TextToEscape.GetComponent<TextMesh>().text = "-player " + winnerNum + " hit "+ playerButton + " to return to lobby-";
        foreach (GameObject light in spotLights)
        {
            light.transform.parent.transform.parent.GetComponent<Animator>().enabled = false;
        }
    }

    void Update()
    {
        if (winner != null) {
            followConfetti.SetActive(true);
            followConfetti.transform.position = winner.transform.position + Vector3.up * 11;
            followConfetti.transform.SetParent(winner.transform, false);
            confetti.SetActive(true);
            confetti.transform.position = winner.transform.position + Vector3.up * 11;
            int i = 3;
            foreach (GameObject light in spotLights) {
                light.transform.position = Vector3.Lerp(light.transform.position, winner.transform.position + Vector3.forward * 10, Time.deltaTime * (0.5f + i));
                i += UnityEngine.Random.Range(1, 3);
                light.transform.parent.GetComponent<SpriteRenderer>().enabled = false;
            }

            if (Input.GetButtonDown("Pause" + winner.playerControl)) {
                StartCoroutine(screenTransition.instance.fadeOut("Controller Setup"));
                GameManager.instance.clearScore();
            }
        }
    }
}
