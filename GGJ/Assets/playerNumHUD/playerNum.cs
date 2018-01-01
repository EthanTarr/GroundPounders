using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerNum : MonoBehaviour {

    private playerController player;
    private TextMesh playerNumText;

    public Sprite keyboardImage;
    public Sprite controllerImage;

    void Start() {
        player = GetComponentInParent<playerController>();
        playerNumText = GetComponentInChildren<TextMesh>();

        GetComponent<SpriteRenderer>().color = player.GetComponent<SpriteRenderer>().color;
        if (player.playerControl.Contains("Joy")) {
            GetComponent<SpriteRenderer>().sprite = controllerImage;
        } else {
            GetComponent<SpriteRenderer>().sprite = keyboardImage;
        }

        playerNumText.text = "" + (player.playerNum + 1);

        StartCoroutine(BlinkDisplay(3, 3));
    }

    IEnumerator BlinkDisplay(float displayTime, float blinkNumber) {
        yield return new WaitForSeconds(displayTime);

        for (int i = 0; i < blinkNumber; i++) {
            yield return displayNum(0.25f);
            yield return new WaitForSeconds(0.25f);
        }
    }

    IEnumerator displayNum(float seconds) {
        GetComponent<SpriteRenderer>().enabled = true;
        playerNumText.gameObject.SetActive(true);

        yield return new WaitForSeconds(seconds);

        GetComponent<SpriteRenderer>().enabled = false;
        playerNumText.gameObject.SetActive(false);
    }
}
