using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameBestOFUI : MonoBehaviour {

    public GameObject gamesUI;
    public Slider slider;

    private void Start() {
        gamesUI.GetComponent<Text>().text = "" + GameManager.instance.increaseGames(3);
    }

    public void increaseGames() {
        int increment = (int)slider.value;
        gamesUI.GetComponent<Text>().text = "" + GameManager.instance.increaseGames(increment);
    }
}
