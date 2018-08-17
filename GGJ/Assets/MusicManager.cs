using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour {

    AudioSource musicAudioSource;
    public AudioClip stageMusic;

	void Start () {
        musicAudioSource = settings.instance.GetComponent<AudioSource>();
        if (musicAudioSource != null) {
            updateMusic();
        }
	}

    void updateMusic() {
        if(musicAudioSource.clip != stageMusic)
            StartCoroutine("updateMusicCor");
    }

    IEnumerator updateMusicCor() {
        while (musicAudioSource.volume > 0) {
            musicAudioSource.volume -= 0.05f;
            yield return new WaitForEndOfFrame();
        }

        musicAudioSource.clip = stageMusic;
        musicAudioSource.Play();

        while (musicAudioSource.volume < settings.instance.volume) {
            musicAudioSource.volume += 0.05f;
            yield return new WaitForEndOfFrame();
        }
        musicAudioSource.volume = settings.instance.volume;
    }
}
