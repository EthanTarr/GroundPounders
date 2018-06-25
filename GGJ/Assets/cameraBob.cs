using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraBob : MonoBehaviour {

    private Camera cam;
    private float timer;
    public float bobDistance;
    public float bobSpeed;
    private float startY;
    public float startYOffset;

    void Start() {
        cam = Camera.main;
        startY = cam.transform.position.y + startYOffset;
    }

    // Update is called once per frame
    void Update () {
        timer += Time.deltaTime * bobSpeed;
        transform.position = new Vector3(cam.transform.position.x, startY - startYOffset + bobDistance * Mathf.Sin(timer), cam.transform.position.z);
    }
}
