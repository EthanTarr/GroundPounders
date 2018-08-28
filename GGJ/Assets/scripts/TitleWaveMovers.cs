using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleWaveMovers : MonoBehaviour {

    public bool disable;
    public bool backWave;
    [HideInInspector]
    public float originalYPos;
    [HideInInspector]
    public float yPos;

    
	// Use this for initialization
	void Start () {
        if (!backWave) {
            if (GetComponent<TextMesh>() != null) {
                GetComponent<TextMesh>().color = ButtonActions.instance.textColor;
            }
            if (GetComponent<SpriteRenderer>() != null) {
                GetComponent<SpriteRenderer>().color = ButtonActions.instance.textColor;
            }
        }
         else
                    GetComponent<SpriteRenderer>().color = ButtonActions.instance.waveColor;

        originalYPos = transform.localPosition.y;
        yPos = originalYPos;
	}
	
	// Update is called once per frame
	void LateUpdate () {
        if (disable)
            return;

        if (!backWave)
           yPos = originalYPos + Mathf.Sin(Time.timeSinceLevelLoad * (transform.localPosition.x + 10) / 2) / 8;
        else {
           yPos = originalYPos + Mathf.Sin(Time.timeSinceLevelLoad * (transform.localPosition.x + 10) / 8) / 4;
        }

        Vector3 targetPos = new Vector3 (transform.localPosition.x, yPos, transform.localPosition.z);
        transform.localPosition = Vector3.Lerp(transform.localPosition, targetPos, Time.deltaTime * 5);
	}
}
