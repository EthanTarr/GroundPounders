using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class halfPlanetSpin : MonoBehaviour {

    public float speed = 1;

    private void Update()
    {
        if (!pause.instance.Pause.active)
        {
            playerController[] players = FindObjectsOfType<playerController>();
            foreach (playerController player in players)
            {
            }
        }
    }
}
