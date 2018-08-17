using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSFX : MonoBehaviour {

    private AudioSource sound;
	// Use this for initialization
	void Awake () {
        sound = GetComponent<AudioSource>();
	}

    public void PlaySoundEffect(AudioClip clip) {
        sound.volume = settings.instance.fx;
        sound.clip = clip;
        sound.Play();
    }
}
