using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class windFlag : MonoBehaviour {

    public float minScale, maxScale;
    public float dampen;

    public float baseY;
    public float range;

    private void Start() {
        Vector2 position = transform.parent.transform.position;
        position.x += Random.Range(-range/2, range/2);
        transform.parent.transform.position = position;
    }

    void Update () {
        float yStretch = 1;
        //yStretch = (transform.position.y - baseY)/dampen;
        //yStretch = Mathf.Clamp(yStretch, minScale, maxScale);
        //changeYScale(yStretch * Mathf.Sign(windZoneManager.instance.windDirection.x));
	}

    void changeYScale(float scale) {
        Vector2 curScale = transform.localScale;
        curScale.y = scale;
        transform.localScale = curScale;
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position + Vector3.down * 3, new Vector3(range,1,1));
    }
}
