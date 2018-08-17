using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IcebergWaveReaction : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.GetComponent<playerController>() != null) {
            print("hoi");
            WaveGenerator.instance.makeWave(collision.transform.position, 5, Color.blue, 3, null);
        }

    }
}
