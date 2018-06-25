using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class buttonRecenter : MonoBehaviour {
	// Update is called once per frame
	void Update () { 
        if (EventSystem.current.currentSelectedGameObject == null) {
            Debug.Log("Reselecting first input");
            EventSystem.current.SetSelectedGameObject(EventSystem.current.firstSelectedGameObject);
        }
    }
}
