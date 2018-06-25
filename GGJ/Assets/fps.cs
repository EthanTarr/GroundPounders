using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fps : MonoBehaviour {

    float deltaTime = 0.0f;

	// Update is called once per frame
	void Update () {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
        float msec = deltaTime * 1000.0f;
        float fps = 1.0f / deltaTime;
        string text = string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);

        GetComponent<TextMesh>().text = text;
    }
}
