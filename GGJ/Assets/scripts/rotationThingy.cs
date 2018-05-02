using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotationThingy : MonoBehaviour {

    public float speed = 1;

    private void Start()
    {
        transform.Rotate(Vector3.forward * Random.Range(0, 360));
    }

    // Update is called once per frame
    void Update () {
        transform.Rotate(Vector3.forward * Time.deltaTime * speed);
	}
}
