using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class screenTransition : MonoBehaviour {

    public static screenTransition instance;
    public Material TransitionMaterial;
    [Range(0, 1)] public float fade;
    [Range(0, 1)] public float cutoff;
    public Texture transitionAlpha;
    public Color screenColor;
    public bool reverseDirection;

    private IEnumerator currentFade;

    public Texture[] transitionTextures;

    private void Start() {
        if (Application.isPlaying) {
            instance = this;
            cutoff = 1;
            transitionAlpha = transitionTextures[Random.Range(0, transitionTextures.Length - 1)];
            StartCoroutine("fadeIn");
        }
    }

    IEnumerator fadeIn() {
        cutoff = 1;
        yield return new WaitForSeconds(0.25f);
        while (cutoff > 0) {
            cutoff -= 0.055f;
            yield return new WaitForEndOfFrame();
        }   
    }

    public IEnumerator fadeOut(string nextLevel) {
        if (currentFade == null) {
            currentFade = fadeOut(nextLevel, 0, false);
            yield return currentFade;
        }
    }

    public IEnumerator fadeOut(string nextLevel, float timeAfter, bool fadeMusic) {
        cutoff = 1;
        reverseDirection = !reverseDirection;
        yield return new WaitForSeconds(0.25f);
        while (cutoff > 0) {
            cutoff -= 0.055f;

            if(fadeMusic)
                settings.instance.GetComponent<AudioSource>().volume -= 0.05f;

            yield return new WaitForEndOfFrame();
        }

        while (fadeMusic && settings.instance.GetComponent<AudioSource>().volume > 0) {
                settings.instance.GetComponent<AudioSource>().volume -= 0.05f;

            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForSeconds(timeAfter);
        currentFade = null;
        Application.LoadLevel(nextLevel);
    }

    public IEnumerator fadeOut(int nextLevel)
    {
        cutoff = 1;
        reverseDirection = !reverseDirection;
        yield return new WaitForSeconds(0.25f);
        while (cutoff > 0)
        {
            cutoff -= 0.055f;
            yield return new WaitForEndOfFrame();
        }
        Application.LoadLevel(nextLevel);
    }


    private void Update()
    {
        TransitionMaterial.SetFloat("_Cutoff", cutoff);
        TransitionMaterial.SetFloat("_fade", fade);
        TransitionMaterial.SetFloat("_reverse", reverseDirection ? 1 : 0);
        TransitionMaterial.SetColor("_Color", screenColor);
        TransitionMaterial.SetTexture("_TransitionTex", transitionAlpha);
    }

    /*
    private void OnRenderImage(RenderTexture source, RenderTexture destination) { 
        TransitionMaterial.SetFloat("_Cutoff", cutoff);
        TransitionMaterial.SetFloat("_fade", fade);
        TransitionMaterial.SetFloat("_reverse", reverseDirection ? 1 : 0);
        TransitionMaterial.SetColor("_Color", screenColor);
        TransitionMaterial.SetTexture("_TransitionTex", transitionAlpha);
        Graphics.Blit(source, destination, TransitionMaterial);
    }
    */
}
