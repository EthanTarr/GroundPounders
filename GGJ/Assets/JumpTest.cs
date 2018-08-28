using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpTest : MonoBehaviour {

    playerController player;
    private float baseY;
    public float yOffset;
    public float numberOfGoofs;

    private void Start() {
        player = FindObjectOfType<playerController>();
        baseY = transform.position.y;

        Vector3 pos = transform.position;
        pos.y += 100;
        transform.position = pos;
        dodge = lerpToPos(baseY, 7);
        StartCoroutine(dodge);
    }

    private void FixedUpdate() {
        if (player.transform.position.y + yOffset > this.transform.position.y && numberOfGoofs > 0 && dodge == null) {
            dodge = dodgeThePlayer();     
            StartCoroutine(dodge);
            numberOfGoofs--;
        }

        if (player.transform.position.y > this.transform.position.y) {
            StartCoroutine(finish());
        }
    }

    IEnumerator dodge;
    IEnumerator dodgeThePlayer() {
        Vector3 pos;
        while (player.transform.position.y + yOffset > baseY) {
            pos = transform.position;
            pos.y = Mathf.Lerp(pos.y, player.transform.position.y + yOffset, Time.deltaTime * 14);
            transform.position = pos;
            yield return null;
        }

        while (transform.position.y >= baseY + 0.1f)  {
            pos = transform.position;
            pos.y = Mathf.Lerp(pos.y, baseY, Time.deltaTime * 24);
            transform.position = pos;
            yield return null;
        }

        dodge = null;
    }

    IEnumerator lerpToPos(float yPos, float speed) {
        while (Mathf.Abs(transform.position.y - yPos) > 0.1f) {
            Vector3 pos = transform.position;
            pos.y = Mathf.Lerp(pos.y, yPos, Time.deltaTime * speed);

            transform.position = pos;

            yield return null;
        }
        dodge = null;
    }

    IEnumerator finish() {
        yield return null;
        Destroy(this.gameObject);
    }
}
