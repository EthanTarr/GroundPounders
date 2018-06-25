using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class stageSelectManager : MonoBehaviour
{

    StandaloneInputModule input;
    playerController curPlayer;
    public GameObject stageSelect;
    public GameObject extraOptions;

    public GameObject firstMap;
    public GameObject hitA;

    public GameObject cursor;

    public string selectedLevel;

    // Use this for initialization
    void Start()
    {
        input = EventSystem.current.gameObject.GetComponent<StandaloneInputModule>();
    }

    IEnumerator turnOnInput(string controller) {
        input.horizontalAxis = "Horizontal" + controller;
        input.submitButton = "Enter" + controller;
        input.cancelButton = "Cancel" + controller;
        yield return new WaitForSeconds(0.05f);
        extraOptions.SetActive(false);
        stageSelect.SetActive(true);
        EventSystem.current.SetSelectedGameObject(firstMap);
        yield return new WaitForSeconds(1f);
        input.enabled = true;
    }

    private void Update() {
        if (curPlayer != null && Input.GetButtonDown("Cancel" + curPlayer.playerControl)) {
            curPlayer.active = true;
            curPlayer.transform.gameObject.layer = LayerMask.NameToLayer("Player");
            curPlayer.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
            curPlayer = null;
            stageSelect.GetComponent<Animator>().Play("exitAnim");
            Invoke("turnOffInput", 0.25f);
        }
    }

    public void changeSelectedLevel(string level) {
        print("level here");
        selectedLevel = level;
        if (curPlayer != null && GameManager.instance.numOfPlayers >= 2) {
            print("level here");
            input.enabled = false;
            StartCoroutine(screenTransition.instance.fadeOut(selectedLevel));
        }
    }

    void turnOffInput() {
        extraOptions.SetActive(true);
        stageSelect.SetActive(false);
        hitA.SetActive(true);
        hitA.GetComponent<Animator>().Play("SelectAnim");

        input.horizontalAxis = "Horizontal";
        input.submitButton = "Enter";
        input.cancelButton = "Cancel";
        input.submitButton = "EnterArrow";
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<playerController>() && curPlayer == null && !hitA.activeSelf)
        {
            hitA.SetActive(true);
            hitA.GetComponent<Animator>().Play("SelectAnim");
        }
    }

    private void OnTriggerStay2D(Collider2D collision) {
        if (collision.gameObject.GetComponent<playerController>() && curPlayer == null) {
            if (Input.GetButton("Jump" + collision.GetComponent<playerController>().playerControl)) {
                stageSelect.GetComponent<Animator>().Play("introAnim");
                curPlayer = collision.gameObject.GetComponent<playerController>();
                curPlayer.transform.gameObject.layer = LayerMask.NameToLayer("PlayerExtra");
                curPlayer.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                curPlayer.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
                curPlayer.active = false;
                StartCoroutine(turnOnInput(curPlayer.playerControl));
                curPlayer.spriteAnim.SetAnimation("think");
                curPlayer.spriteAnim.SetFramesPerSecond(3);
                hitA.SetActive(false);

                Color cursorColor = curPlayer.GetComponent<SpriteRenderer>().color;
                cursor.GetComponent<Image>().color = cursorColor;
            }
            else
            {
                hitA.SetActive(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<playerController>())
        {
            hitA.SetActive(false);
        }
    }
}
