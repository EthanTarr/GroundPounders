using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class afterImageFade : MonoBehaviour {

	// Use this for initialization
	void Start () {
        SpriteRenderer trailPartRenderer = GetComponent<SpriteRenderer>();
        StartCoroutine("FadeTrailPart", trailPartRenderer);
    }

    IEnumerator FadeTrailPart(SpriteRenderer trailPartRenderer)
    {
        while (trailPartRenderer != null && trailPartRenderer.color.a > 0) {
            trailPartRenderer.color = changeOpacity(trailPartRenderer.color, trailPartRenderer.color.a - 0.01f);
            yield return new WaitForEndOfFrame();
        }
        Destroy(trailPartRenderer.gameObject);
    }

    public Color changeOpacity(Color color, float opacity)
    {
        color.a = opacity;
        return color;
    }
}
