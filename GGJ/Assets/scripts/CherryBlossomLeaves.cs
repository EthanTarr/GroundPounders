using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CherryBlossomLevel : MonoBehaviour {

    public ParticleSystem[] treeParticles;

	// Use this for initialization
	void Start () {
        Shake.instance.screenShake += leavesFall;
    }

    public void leavesFall() {
        foreach (ParticleSystem treeParticles in treeParticles) {
            treeParticles.Emit(UnityEngine.Random.Range(5, 10));
        }
    }

    private void OnDestroy() {
        Shake.instance.screenShake -= leavesFall;
    }
}
