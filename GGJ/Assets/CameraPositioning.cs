using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPositioning : MonoBehaviour {

    public List<CameraPosition> cameraPositions;
    public static Dictionary<string, CameraPosition> posDict;

    public void Start() {
        posDict = new Dictionary<string, CameraPosition>();
        foreach (CameraPosition cp in cameraPositions)
            posDict.Add(cp.name, cp);
    }

    public void storeCameraData() {
        Camera cam = Camera.main;
        CameraPosition newPos = new CameraPosition(cam.transform.position, cam.orthographicSize);
        cameraPositions.Add(newPos);
    }

    public void repositionCamera(string posName, float speed) {
        StopAllCoroutines();
        CameraPosition camData = posDict[posName];

        if (camData == null) {
            Debug.LogError("cameraData not found");
            return;
        }
        StartCoroutine(repostionCameraEnum(camData, speed));
    }

    IEnumerator repostionCameraEnum(CameraPosition camData, float speed) {
        Shake.instance.startTransform = camData.position;
        Camera cam = Camera.main;
        float counter = 0;
        while (Vector3.Distance(cam.transform.position, camData.position) > 0.1f || 
                Mathf.Abs(cam.orthographicSize - camData.orthographicSize) > 0.1f) {
            counter += speed * Time.deltaTime;
            cam.transform.position = Vector3.Lerp(cam.transform.position, camData.position, Time.deltaTime * speed);
            cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, camData.orthographicSize, Time.deltaTime * speed);
            yield return null;
        }
        cam.transform.position = camData.position;
        cam.orthographicSize = camData.orthographicSize;
    }
}

[System.Serializable]
public class CameraPosition {
    public string name;
    public Vector3 position;
    public float orthographicSize;

    public CameraPosition(Vector3 position, float orthographicSize) {
        this.position = position;
        this.orthographicSize = orthographicSize;
    }
}
