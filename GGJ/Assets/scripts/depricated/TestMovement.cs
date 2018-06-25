using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMovement : MonoBehaviour {

    public float radius = 2f;
    public int speed = 2;
    public Transform centerOfGravity;

	// Update is called once per frame
	void FixedUpdate () {
        transform.RotateAround(centerOfGravity.position, new Vector3(0, 0, 1), speed * Time.deltaTime);
    }
}
