using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SuddenDeathUI : MonoBehaviour {

    public GameObject gamesUI;
    public Slider slider;

    private void Start() {
        slider.value = GameManager.instance.suddenDeathTimer;
        gamesUI.GetComponent<Text>().text = "" + GameManager.instance.suddenDeathTimer;
    }

    public void setSDTime() {
        sliderLoop();
        GameManager.instance.suddenDeathTimer = (int)slider.value;
        gamesUI.GetComponent<Text>().text = "" + GameManager.instance.suddenDeathTimer;
    }

    void sliderLoop() {
        if (slider.value == slider.maxValue) {
            slider.value = slider.minValue + 1;
        } else if (slider.value == slider.minValue) {
            slider.value = slider.maxValue - 1;
        }
    }
}
