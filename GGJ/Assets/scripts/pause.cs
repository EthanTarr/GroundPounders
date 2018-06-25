using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class pause : MonoBehaviour
{

    public GameObject Pause;
    public GameObject settings;
    public AudioClip pauseBeep;
    public static pause instance;
    [Space()]
    public GameObject firstButton;
    public GameObject firstSettingsButton;
    public string currentPlayer;
    StandaloneInputModule input;

    public void Start()
    {
        instance = this;
        input = EventSystem.current.gameObject.GetComponent<StandaloneInputModule>(); ;
    }

    public void togglePause(playerController curPlayer) {
        input.enabled = true;
        currentPlayer = curPlayer.playerControl;

        input.verticalAxis = "Vertical" + currentPlayer;
        input.horizontalAxis = "Horizontal" + currentPlayer;
        input.submitButton = "Enter" + currentPlayer;

        if (currentPlayer == "WASD" || currentPlayer == "Arrow")
            input.verticalAxis = "VerticalArrow";

        Pause.transform.GetChild(0).GetComponent<Text>().text = "- pause p" + (curPlayer.playerNum + 1) + " -";

        foreach (Text text in Pause.GetComponentsInChildren<Text>()) {
            text.color = curPlayer.GetComponent<SpriteRenderer>().color;
        }

        togglePause();
    }

    public void togglePause() {
        if (Pause.activeSelf || settings.activeSelf) {
            input.enabled = false;
            Pause.SetActive(false);
            settings.SetActive(false);
            StartCoroutine("zaWardo");
        }
        else
        {
            Pause.SetActive(true);
            EventSystem.current.SetSelectedGameObject(firstButton);
            //EventSystem.current.gameObject.GetComponent<myInputModule>().submitButton = "Submit";
            Time.timeScale = 0;
        }
        audioManager.instance.Play(pauseBeep, 0.25f, 1);
    }

    IEnumerator zaWardo()
    {
        yield return new WaitForEndOfFrame();
        Time.timeScale = 1;
    }

    public void gotoMenu()
    {
        Pause.SetActive(false);
        Time.timeScale = 1;
        Destroy(GameManager.instance.gameObject);
        controllerHandler.controlOrder.Clear();
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

    public void quit()
    {
        Application.Quit();
    }

    public bool paused() {
        return Pause.activeSelf || settings.activeSelf;
    }

    public void toggleSettings()
    {
        settings.SetActive(!settings.activeSelf);
        Pause.SetActive(!Pause.activeSelf);
        if (settings.activeSelf)
        {
            EventSystem.current.SetSelectedGameObject(firstSettingsButton);
        }
        else
        {
            EventSystem.current.SetSelectedGameObject(firstButton);
        }

        foreach (Text t in GetComponentsInChildren<Text>()) {
            t.color = Pause.GetComponentInChildren<Text>().color;
        }

        foreach (Slider s in FindObjectsOfType<Slider>()) {
            s.fillRect.GetComponent<Image>().color = Pause.GetComponentInChildren<Text>().color;
        }
    }
}
