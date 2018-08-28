using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public class TitleBackWaves : MonoBehaviour
{

    public bool disable;
    public bool backWave;
    [HideInInspector] public float originalYPos;
    [HideInInspector] public float yPos;
    public float offset = -10;
    public float delay = 1;
    public float activate;

    void Start() {
        if (!backWave)
        {
            if (GetComponent<TextMesh>() != null)
            {
                GetComponent<TextMesh>().color = ButtonActions.instance.textColor;
            }
            if (GetComponent<SpriteRenderer>() != null)
            {
                GetComponent<SpriteRenderer>().color = ButtonActions.instance.textColor;
            }
        } else {
            GetComponent<SpriteRenderer>().color = ButtonActions.instance.waveColor;
        }

        originalYPos = transform.localPosition.y;
        yPos = originalYPos;
        transform.localPosition -= (transform.position.y > 0 ? -1 : 1) * Vector3.up * 10;
        activate = (Time.deltaTime - offset + transform.localPosition.x) * delay;
    }
}

class TitleBackWaveSystem : ComponentSystem {
    struct Component {
        public TitleBackWaves wave;
        public Transform transform;
        public SpriteRenderer sprite;
    }

    protected override void OnUpdate() {
        foreach (var e in GetEntities<Component>()) {
            EntityUpdate(e);
        }
    }

    void EntityUpdate(Component e) {       
        if (Time.time < e.wave.activate) { return; }

        if (e.wave.disable)
            return;

        if (!e.wave.backWave)
            e.wave.yPos = e.wave.originalYPos + Mathf.Sin(Time.timeSinceLevelLoad * (e.transform.localPosition.x + 10) / 2) / 8;
        else
            e.wave.yPos = e.wave.originalYPos + Mathf.Sin(Time.timeSinceLevelLoad * (e.transform.localPosition.x + 10) / 8) / 4;
        
        Vector3 targetPos = new Vector3(e.transform.localPosition.x, e.wave.yPos, e.transform.localPosition.z);
        e.transform.localPosition = Vector3.Lerp(e.transform.localPosition, targetPos, Time.deltaTime * 10); 
    }
}