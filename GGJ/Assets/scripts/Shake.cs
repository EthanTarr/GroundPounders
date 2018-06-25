using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Shake : MonoBehaviour {
	public static Shake instance;
	public Vector3 startTransform;
    public ParticleSystem dustParticles;

    public delegate void shakeEvent();
    public shakeEvent screenShake;

    [Space()]
    public AudioClip[] rumbles;

    void Awake(){
        instance = this;
        startTransform = transform.position;
	}

	public void shake(float t, float strength){
        if(dustParticles != null)
            dustParticles.Emit(UnityEngine.Random.Range(5, 8));

        audioManager.instance.Play(rumbles[Random.Range(0, rumbles.Length - 1)], 0.25f, Random.Range(0.96f, 1.03f));

        if (screenShake != null)
            screenShake.Invoke();

        StartCoroutine(screenshake(t, strength));	
	}
	
	IEnumerator screenshake(float t, float strength){
		float z = transform.position.z;
		while (t > 0){
			t -= Time.deltaTime*10;
			
			transform.position = new Vector2(startTransform.x,startTransform.y) + Random.insideUnitCircle*strength/8;
			transform.position += new Vector3(0,0,z);
			yield return null;
		}
		transform.position = startTransform;
	}
}
