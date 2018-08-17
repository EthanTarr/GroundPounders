using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class settings : MonoBehaviour {

    public static settings instance;
    [Range(0, 1)] public float volume;
    [Range(0, 1)] public float fx;
    [HideInInspector] public AudioSource musicAudio;

    void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        } else {
            Destroy(this.gameObject);
        }

        musicAudio = GetComponent<AudioSource>();
        volume = PlayerPrefs.GetFloat("Volume", 0.75f);
        fx = PlayerPrefs.GetFloat("SoundFX", 0.75f);
        musicAudio.volume = volume;
    }
}
