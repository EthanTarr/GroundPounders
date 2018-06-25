using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraStartManager : MonoBehaviour {

    public AudioClip go;

    public void contactWaveGenerator() {
        WaveGenerator.instance.StartWave();
    }

    public void activatePlayers() {
        if (playerSpawner.instance == null) {
            Debug.LogError("No Spawner");
            return;
        }

        playerSpawner.instance.activatePlayers();
    }

    public void playGo() {
        audioManager.instance.Play(go, 0.5f, 1);
    }
}
