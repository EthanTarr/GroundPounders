using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConfigureVideoSettings : MonoBehaviour {
    
    public List<Vector2Int> resolutions;

    private int resIndex;
    public bool fullScreen;
    [Space()]
    public Text resolutionUI;
    public Slider resoultionSlider;

    void Start() {
        Resolution currentRes = Screen.currentResolution;
        Vector2Int res = new Vector2Int(currentRes.width, currentRes.height);

        if (!resolutions.Contains(res)) {
            resolutions.Add(res);
            resIndex = resolutions.Count - 1;
        }

        if (resolutionUI != null) {
            resolutionUI.text = "" + res.x + " x " + res.y;
        }
        fullScreen = Screen.fullScreen;
        resoultionSlider.minValue = 0;
        resoultionSlider.maxValue = resolutions.Count - 1;
        changeRes(0);
    }
    
    public void toggleFullScreen(Toggle change) {
        fullScreen = change.isOn;
        changeRes(0);
    }

    public void changeRes() {
        resIndex = (int)resoultionSlider.value;
        changeRes(0);
    }

    public void changeRes(int i) {
        resIndex += i;

        if (resIndex < 0) {
            resIndex = resolutions.Count - 1;
        } else {
            resIndex %= resolutions.Count;
        }
        resoultionSlider.value = resIndex;
        print("set resolution to" + resolutions[resIndex]);
        Vector2Int res = resolutions[resIndex];
        Screen.SetResolution(res.x, res.y, fullScreen);
        if (resolutionUI != null) {
            resolutionUI.text = "" + res.x + " x " + res.y;
        }
        PlayerPrefs.SetInt("xRes", res.x);
        PlayerPrefs.SetInt("xRes", res.y);
        //PlayerPrefs.SetBool("fullScreen", fullScreen);
    }
}
