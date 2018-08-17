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
    public Toggle fullScreenToggle;

    void Start() {
    
        foreach (Resolution res in Screen.resolutions) {
            resolutions.Add(new Vector2Int(res.width, res.height));
        }
        resolutions.Reverse();

        Vector2Int curRes = new Vector2Int(Screen.width, Screen.height);
        if (!resolutions.Contains(curRes)) {
            resolutions.Add(curRes);
        }

        
        // Debug.LogError("" + Screen.width + " x " + Screen.height);
        if (resolutionUI != null) {
            resolutionUI.text = "" + Screen.width + " x " + Screen.height;
        }

        fullScreen = Screen.fullScreen;
        fullScreenToggle.isOn = fullScreen;

        resoultionSlider.minValue = -1;
        resoultionSlider.maxValue = resolutions.Count;
        changeRes(resolutions.IndexOf(curRes));
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
        sliderLoop();
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
    }

    void sliderLoop()
    {
        if (resoultionSlider.value == resoultionSlider.maxValue)
        {
            resoultionSlider.value = resoultionSlider.minValue + 1;
        }
        else if (resoultionSlider.value == resoultionSlider.minValue)
        {
            resoultionSlider.value = resoultionSlider.maxValue - 1;
        }
    }
}
