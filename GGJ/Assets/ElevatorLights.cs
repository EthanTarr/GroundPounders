using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorLights : MonoBehaviour {

    private controlAssigmentManager controllerAssignment;
    public SpriteRenderer[] lights;
    public Color baseColor;
    public Color[] playerColors;

    void Awake () {
        controllerAssignment = GetComponentInParent<controlAssigmentManager>();
        playerColors = controllerAssignment.colors;
        controlAssigmentManager.spawned += updateLights;
    }

    void updateLights() {
        int playerCount = controllerAssignment.players.Count;
        for (int i = 0; i < lights.Length; i++) {
            lights[i].color = (i < playerCount) ? playerColors[i] : baseColor;
        }
    }

    private void OnDestroy()
    {
        controlAssigmentManager.spawned -= updateLights;
    }
}
