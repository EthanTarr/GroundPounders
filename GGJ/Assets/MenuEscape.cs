using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class MenuEscape : MonoBehaviour {

    [Range(0,1)]
    public float loadingBar;
    public float loadingSpeed = 1.5f;
    public maskTransform startTrans;
    public maskTransform endTrans;

    public GameObject arrowMask;
    IEnumerator menued;

    // Update is called once per frame
    void Update () {
        if (menued == null) {
            string pressedEscape = checkEscapeButtons();
            if (pressedEscape != null && !FindObjectOfType<stageSelectManager>().stageSelect.activeSelf) {
                menued = backToMenu(pressedEscape);
                StartCoroutine(menued);
            }
        }

        arrowMask.transform.localPosition = Vector3.Lerp(startTrans.position, endTrans.position, loadingBar);
        arrowMask.transform.localScale = Vector3.Lerp(startTrans.scale, endTrans.scale, loadingBar);
    }

    private string checkEscapeButtons() {
        foreach (string control in controlAssigmentManager.controllers) { // these are unassigned controllers
            if (Input.GetButtonDown("Pause" + control))
                return "Pause" + control;
            if (Input.GetButtonDown("Dropout" + control))
                return "Dropout" + control;
        }

        return null;
    }

    IEnumerator backToMenu(string pressedButton) {
        float t = 0;
        while (t < loadingSpeed) {
            t += Time.deltaTime;
            loadingBar = Mathf.Lerp(0,1, t/loadingSpeed);
            if (!Input.GetButton( pressedButton)) {
                while (loadingBar > 0) {
                    loadingBar -= Time.deltaTime * 4;
                    menued = null;
                    yield return null;
                }
                yield break;
            }
            yield return null;
        }
        Destroy(GameManager.instance.gameObject);
        controllerHandler.controlOrder.Clear();
        StartCoroutine(screenTransition.instance.fadeOut("Title"));
    }

    [System.Serializable]
    public struct maskTransform
    {
        public Vector3 scale;
        public Vector3 position;
    }

}