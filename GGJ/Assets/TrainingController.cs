using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainingController : MonoBehaviour {

    public GameObject round1;
    public GameObject round2;
    public GameObject round3;

    private void Start() {
        StartCoroutine(tutorialMainLoop());
    }

    IEnumerator tutorialMainLoop() {
        yield return waitForPlayer();
        yield return new WaitForSeconds(1);

        //time to learn how to move left to right
        //time to learn how to jump
        yield return jumpRound();

        //practice jumping through hoops
        //time to learn how to smash
        yield return new WaitForSeconds(1);
        yield return smashRound();

        //time to learn how to bump
        yield return new WaitForSeconds(1);
        yield return bumpRound();

        print("Tutorial Done");
        yield return new WaitForSeconds(3);
        FindObjectOfType<controlAssigmentManager>().clearCharacterSelection();
        StartCoroutine(screenTransition.instance.fadeOut("Title", 0.25f, true));
    }

    IEnumerator waitForPlayer() {
        while (FindObjectOfType<playerController>() == null) {
            yield return new WaitForSeconds(0.1f);
        }
    }

    IEnumerator jumpRound() {
        round1.SetActive(true);
        while (round1 != null) {
            yield return new WaitForSeconds(0.1f);
        }
    }

    IEnumerator smashRound() {
        round2.SetActive(true);
        while (round2.transform.childCount > 0) {
            yield return new WaitForSeconds(0.1f);
        }
    }

    IEnumerator bumpRound() {
        round3.SetActive(true);
        while (round3.transform.childCount > 0) {
            yield return new WaitForSeconds(0.1f);
        }
    }
}
