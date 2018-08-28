using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class letterWaveMovers : MonoBehaviour {

    private Animator anim;
    public GameObject centerObject;
    public float distanceDelay = 1;
    float distanceFromCenter;
    public static float maxDistanceFromCenter;
    public float yPos;
    public static letterWaveMovers[] order;

    private void Awake() {
        anim = GetComponent<Animator>();
        distanceFromCenter = Mathf.Abs(transform.position.x - centerObject.transform.position.x);

        maxDistanceFromCenter = Mathf.Max(maxDistanceFromCenter, distanceFromCenter);
        float distanceOrder = distanceFromCenter / maxDistanceFromCenter;
        anim.SetFloat("AnimOffset", distanceOrder);
    }

    void Start () {
        if (order == null && Application.isPlaying) {
            order = FindObjectsOfType<letterWaveMovers>();
            System.Array.Sort(order, (x, y) => { return (int)((x.distanceFromCenter - y.distanceFromCenter) * 1000); });
            StartCoroutine(cycleThroughLetters());
        }
        if (GetComponent<TextMesh>() != null)
        {
            GetComponent<TextMesh>().color = ButtonActions.instance.textColor;
        }
        if (GetComponent<SpriteRenderer>() != null)
        {
            GetComponent<SpriteRenderer>().color = ButtonActions.instance.textColor;
        }
    }

    void shake() {
        Shake.instance.shake(2, 3);
    }

    IEnumerator cycleThroughLetters() {
        yield return new WaitForSeconds(distanceDelay);
        order[0].startAnim();
        Invoke("shake", 0.7f);
        for (int i = 1; i < order.Length; i+=2) {
            yield return new WaitForSeconds(distanceDelay);
            order[i].startAnim();
            if(i+1<order.Length) {
                order[i+1].startAnim();
            }
        }
    }

    void startAnim() {
        anim.Play("logoIntroAnim");
    }

	void LateUpdate () {
        Vector3 pos = transform.position;
        pos.y = yPos;
        transform.position = pos;
	}

    private void OnDestroy()
    {
        order = null;
    }
}
