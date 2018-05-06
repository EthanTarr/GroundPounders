using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonActions : MonoBehaviour {
    public static ButtonActions instance;

    public GameObject mainButtons;
    public GameObject title;
    public GameObject startPrompt;
    public GameObject initialButtons;

    public GameObject firstSettingsButton;

    public menuColorScheme[] colorScheme;
    public int chosenColor = 0;

    [HideInInspector] public Color backgroundColor;
    [HideInInspector] public Color waveColor;
    [HideInInspector] public Color textColor;

    public AudioClip highlightedSFX;
    public AudioClip selectedSFX;

    // Use this for initialization
    void Awake () {
        instance = this;
        //scoreCard.instance.isConeHeadMode();

        chosenColor = Random.Range(0, colorScheme.Length);

        backgroundColor = colorScheme[chosenColor].backgroundColor;
        waveColor = colorScheme[chosenColor].waveColor;
        textColor = colorScheme[chosenColor].textColor;
        Camera.main.backgroundColor = backgroundColor;

        foreach (Text t in FindObjectsOfType<Text>()) {
            t.color = colorScheme[chosenColor].textColor;
        }

        foreach (Slider s in FindObjectsOfType<Slider>()) {
            s.fillRect.GetComponent<Image>().color = waveColor;
            s.handleRect.GetComponent<Image>().color = textColor;
        }

        if(mainButtons != null)
        mainButtons.SetActive(false);
    }

    // Update is called once per frame
    void Update () {
        if (mainButtons != null && !mainButtons.activeInHierarchy && checkButtonDown("Enter")) {
            mainButtons.SetActive(true);
            StartCoroutine(moveToPositionEnum(title, title.transform.position + Vector3.left * 2.5f, 3, true));
            startPrompt.SetActive(false);
            EventSystem.current.SetSelectedGameObject(mainButtons.transform.GetChild(0).gameObject);
            mainButtons.transform.GetChild(0).GetComponent<Animator>().SetBool("Highlighted", true);
        }
    }

    public bool checkButtonDown(string buttonName) {
        string[] controllers = new string[]{ "Arrow", "WASD", "Joy1", "Joy2", "Joy3", "Joy4" };
        for(int i = 0; i < controllers.Length; i++) {
            if (Input.GetButtonDown(buttonName + controllers[i])) {
                return true;
            }

        }
        return false;
    }

    public void PlayGame() {
        StartCoroutine(screenTransition.instance.fadeOut("Controller Setup"));
	}

    public void backToMenu() {
        StartCoroutine(screenTransition.instance.fadeOut("Title"));
    }

    public void gotoSettings()
    {
        StartCoroutine(screenTransition.instance.fadeOut("audioSettings"));
    }

    public void Quit() {
        Application.Quit();
    }

    private IEnumerator moveToPositionEnum(GameObject targetObject, Vector3 targetPos, float speed, bool lerp)
    {
        while (Vector3.Distance(targetObject.transform.position, targetPos) > 0.1f)
        {
            if (lerp)
            {
                targetObject.transform.position = Vector3.Lerp(targetObject.transform.position, targetPos, Time.deltaTime * speed);
            }
            else
            {
                targetObject.transform.position = Vector3.MoveTowards(targetObject.transform.position, targetPos, Time.deltaTime * speed);
            }
            yield return new WaitForEndOfFrame();
        }
    }

    [System.Serializable]
    public struct menuColorScheme {
        public Color backgroundColor;
        public Color waveColor;
        public Color textColor;
    }
}
