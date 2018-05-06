using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class configureSoundSettings : MonoBehaviour {

    public Slider musicVol;
    public Slider fxVol;
    public bool startOn = false;


    private void Start() {
        musicVol.value = settings.instance.musicAudio.volume;
        fxVol.value = settings.instance.fx;
        gameObject.SetActive(startOn);
    }

    public void changeMusicVol() {
        settings.instance.volume = musicVol.value;
        settings.instance.musicAudio.volume = musicVol.value;
    }

    public void changeFXVol() {
        settings.instance.fx = fxVol.value;
        fxVol.value = settings.instance.fx;
    }
}
