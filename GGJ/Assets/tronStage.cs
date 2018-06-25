using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tronStage : MonoBehaviour {
    public Color[] colors;

	void Start () {
        Camera.main.backgroundColor = colors[Random.Range(0, colors.Length - 1)];
    }
}
