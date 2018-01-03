using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class resultsSign : objectShake
{

    void Awake()
    {
        //startTransform = transform.localPosition;
    }
    void Start () {
        StartCoroutine(moveToPostiionEnum(this.gameObject, transform.position + -Vector3.up * 7,1.25f, true));
	}

    public IEnumerator moveToPostiionEnum(GameObject targetObject, Vector3 targetPos, float speed, bool lerp)
    {
        while (Vector3.Distance(targetObject.transform.localPosition, targetPos) > 0.1f)
        {
            if (lerp)
            {
                targetObject.transform.localPosition = Vector3.Lerp(targetObject.transform.localPosition, targetPos, Time.deltaTime * speed);
            }
            else
            {
                targetObject.transform.localPosition = Vector3.MoveTowards(targetObject.transform.localPosition, targetPos, Time.deltaTime * speed);
            }
            yield return new WaitForEndOfFrame();
        }
    }

    protected override IEnumerator screenshake(float t, float strength) {
        if (Random.Range(0, 101) < 90) {
            startTransform = transform.localPosition;
            float z = transform.localPosition.z;
            while (t > 0)
            {
                t -= Time.deltaTime * 10;

                transform.localPosition = new Vector2(startTransform.x, startTransform.y) + Random.insideUnitCircle * strength / 8;
                transform.localPosition += new Vector3(0, 0, z);
                yield return null;
            }

            transform.localPosition = startTransform;
        } else {
            StartCoroutine(moveToPostiionEnum(this.gameObject, transform.position + -Vector3.up * 1, 10, true));
        }
    }
}
