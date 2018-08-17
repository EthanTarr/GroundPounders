using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class stageSelectCursor : MonoBehaviour {

	// Update is called once per frame
	void LateUpdate () {
        if (EventSystem.current != null)
        {
            Vector2 cursorPos = EventSystem.current.currentSelectedGameObject.transform.position;
            transform.position = Vector2.Lerp(transform.position, cursorPos, Time.deltaTime * 15);
        }
    }
}
