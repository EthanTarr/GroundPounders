using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoomInEffect : MonoBehaviour {

    public GameObject top;
    public float vertOffset;
    
    public GameObject left;
    public GameObject right;
    public float horizOffset;
    [Space()]
    public float suddenDeathTime = 90;
    public float zoomSpeed = 4;
    public GameObject suddenDeathHUD;

    void Start() {
        Invoke("cameraZoom", suddenDeathTime);
    }

    public void cameraZoom() {
        if(suddenDeathHUD != null)
            StartCoroutine(BlinkDisplay(1, 3));

        StartCoroutine(cameraZoomEnum(Camera.main, zoomSpeed, 0.25f));
    }

    IEnumerator BlinkDisplay(float displayTime, float blinkNumber) {

        for (int i = 0; i < blinkNumber; i++) {
            yield return displayNum(0.5f);
            yield return new WaitForSeconds(0.5f);
        }
    }

    IEnumerator displayNum(float seconds) {
        suddenDeathHUD.SetActive(true);
        yield return new WaitForSeconds(seconds);
        suddenDeathHUD.SetActive(false);
    }

    public IEnumerator cameraZoomEnum(Camera cam, float targetZoom, float zoomSpeed) {
        float initialSize = cam.orthographicSize;
        while (Mathf.Abs(cam.orthographicSize - targetZoom) > 0.01f) {
            float verticalExtent = cam.orthographicSize;
            float horzExtent = verticalExtent * Screen.width / Screen.height;
            top.transform.position = transform.position + (vertOffset + verticalExtent) * Vector3.up + Vector3.forward * 20;
            left.transform.position = transform.position + (horizOffset + horzExtent) * Vector3.right + Vector3.forward * 20;
            right.transform.position = transform.position - (horizOffset + horzExtent) * Vector3.right + Vector3.forward * 20;

            cam.orthographicSize -= Time.deltaTime * zoomSpeed;
            Vector3 pos = cam.transform.position;
            pos.y -= 0.0025f;
            cam.transform.position = pos;
            yield return null;
        }
        cam.orthographicSize = targetZoom;
        yield return null;
    }
}
