using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBounce : MonoBehaviour {

    private Vector3 initialPosition;
    private Vector3 forcePosition;

	// Use this for initialization
	void Start () {
        initialPosition = transform.position;
        WaveGenerator.wave += planetBounce;
	}

    void Update() {
        Debug.DrawRay(transform.position, forcePosition);
        forcePosition = Vector3.Lerp(forcePosition, Vector3.zero, Time.deltaTime);
        transform.position = Vector3.Lerp(transform.position, initialPosition + forcePosition, Time.deltaTime * 3);
    }

    public void planetBounce(float amplitude, Vector2 impactPosition) {
        print("hoi");
        Vector3 dir = Vector3.Normalize(impactPosition - (Vector2)transform.position);
        forcePosition = dir * 3;
    }
}
