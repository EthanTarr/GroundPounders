using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class playerDropout : MonoBehaviour {

    [Range(0, 1)]
    public float loadingBar;
    public float loadingSpeed = 1.5f;
    public maskTransform startTrans;
    public maskTransform endTrans;
    [Space()]
    public GameObject arrowMask;
    playerController player;
    string playerControl;

	// Use this for initialization
	void Start () {
        player = GetComponentInParent<playerController>();
        playerControl = player.playerControl;
        foreach (Transform child in transform.GetComponentInChildren<Transform>()) {
            child.gameObject.SetActive(false);
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (Application.isPlaying && Input.GetButtonDown("Dropout" + playerControl) 
            && !FindObjectOfType<stageSelectManager>().stageSelect.activeSelf) {
            StartCoroutine(dropoutOfLobby());
        }
        arrowMask.transform.localPosition = Vector3.Lerp(startTrans.position, endTrans.position, loadingBar);
        arrowMask.transform.localScale = Vector3.Lerp(startTrans.scale, endTrans.scale, loadingBar);
    }

    IEnumerator dropoutOfLobby() {
        float t = 0f;

        foreach (Transform child in transform.GetComponentInChildren<Transform>())
        {
            child.gameObject.SetActive(true);
        }

        while (t < 1.5f) {
            t += Time.deltaTime;
            loadingBar = Mathf.Lerp(0, 1, t / 1.5f);
            if (!Input.GetButton("Dropout" + playerControl)) {
                player.active = true;
                loadingBar = 0;
                foreach (Transform child in transform.GetComponentInChildren<Transform>()) {
                    child.gameObject.SetActive(false);
                }
                yield break;
            }
            yield return null;
        }

        audioManager.instance.Play(player.deathExplosion, 0.5f, UnityEngine.Random.Range(0.96f, 1.04f));

        GameObject particle = Instantiate(player.deathParticle, player.transform.position, Quaternion.identity) as GameObject;
        ParticleSystem.MainModule ps = particle.GetComponent<ParticleSystem>().main;
        ps.startColor = Color.Lerp(player.baseColor, Color.white, 0.5f);
        player.GetComponent<SpriteRenderer>().color = player.fullColor;

        FindObjectOfType<controlAssigmentManager>().characterDropout(player);
        Destroy(player.gameObject);
    }

    [System.Serializable]
    public struct maskTransform
    {
        public Vector3 scale;
        public Vector3 position;
    }

}
