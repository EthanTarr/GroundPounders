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
    [Space()]
    public GameObject settingsMenu;
    public GameObject firstSetting;
    [Space()]
    public GameObject mapsMenu;
    public GameObject firstMap;
    [Space()]
    public GameObject hitA;
    public GameObject cursor;

    public string selectedLevel;
    private CameraPositioning camPos;

    // Use this for initialization
    void Start() {
        if(!settings.instance.musicAudio.isPlaying)
            settings.instance.musicAudio.Play();

        camPos = GetComponent<CameraPositioning>();
        input = EventSystem.current.gameObject.GetComponent<StandaloneInputModule>();
    }

    private void Update() {
        if (curPlayer != null && Input.GetButtonDown("Cancel" + curPlayer.playerControl)) {
            turnOffInput();
        }
    }

    public void changeSelectedLevel(string level) {
        selectedLevel = level;
        if (curPlayer != null && GameManager.instance.numOfPlayers >= 2) {
            input.enabled = false;
            EventSystem.current.enabled = false;
            StartCoroutine(screenTransition.instance.fadeOut(selectedLevel, 0.5f, true));
        }
    }

    public void switchToSettings(bool toSettings) {
        settingsMenu.SetActive(toSettings);
        mapsMenu.SetActive(!toSettings);
        EventSystem.current.SetSelectedGameObject(toSettings ? firstSetting : firstMap);
        stageSelect.GetComponent<Animator>().Play(toSettings ? "settingsAnim": "defaultAnim");
    }

    IEnumerator turnOnInput(string controller)
    {        
        camPos.repositionCamera("PosSelection",5);
        switchToSettings(false);
        EventSystem.current.SetSelectedGameObject(null); // im lazy.this is pretyt lazy
        EventSystem.current.firstSelectedGameObject = firstMap;

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

    public void turnOffInput() {
        camPos.repositionCamera("PosDefault", 5);
        curPlayer.active = true;
        curPlayer.transform.gameObject.layer = LayerMask.NameToLayer("Player");
        curPlayer.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
        curPlayer = null;
        stageSelect.GetComponent<Animator>().Play("exitAnim");
        Invoke("turnOffInputHelper", 0.25f);
    }

    void turnOffInputHelper() {
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
