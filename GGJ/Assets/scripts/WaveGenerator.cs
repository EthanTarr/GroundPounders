using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WaveGenerator : MonoBehaviour
{

	public GameObject pulse;
	public GameObject antiPulse;
    public float wavelengthModifier = .25f;
    public static WaveGenerator instance;

    public delegate void waveImapct(float amplitude, Vector2 position);
    public static waveImapct wave;

    public Color fadeIn;

	// Use this for initialization
	void Start () {
        instance = this;
    }

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown ("t")) {
			makeWave (); 
		}
	}

    public void timeFreeze(float timescale, float freezeDuration) {
        StartCoroutine(zaWardo(timescale, freezeDuration));
    }

    IEnumerator zaWardo(float timescale, float freezeDuration)
    {
        Time.timeScale = timescale;
        yield return new WaitForSecondsRealtime(freezeDuration);
        Time.timeScale = 1;
    }

    public void changeTime(float time)
    {
        Time.timeScale = time;
    }

    void makeWave() {
        makeWave(transform.position, -1, fadeIn, 5, null);
	}

    public void StartWave(){
        makeWave(transform.position, 0, fadeIn, 5, null);
    }

    public void makeWave(Vector2 position, float amplitude, Color color, float velocity, Transform centerOfGravity) {
        if (wave != null)
            wave.Invoke(amplitude, position);

        GameObject Pulse = Instantiate(pulse, new Vector3(position.x, position.y, 0), Quaternion.identity);

        float wavelength = Mathf.Clamp(wavelengthModifier * amplitude, 1.5f, 2.15f);

        Pulse.GetComponent<PulseMove> ().Amplitude = amplitude;
        Pulse.GetComponent<PulseMove>().color = color;
        Pulse.GetComponent<PulseMove>().speed = velocity;
        Pulse.GetComponent<PulseMove>().angularSpeed = velocity * 7;
        Pulse.GetComponent<PulseMove>().centerOfGravity = centerOfGravity;
        Pulse.GetComponent<PulseMove>().Wavelength = wavelength;

        GameObject AntiPulse = Instantiate (antiPulse, new Vector3(position.x, position.y, 0), Quaternion.identity);
		AntiPulse.GetComponent<AntiPulseMove> ().Amplitude = amplitude;
        AntiPulse.GetComponent<AntiPulseMove>().color = color;
        AntiPulse.GetComponent<AntiPulseMove>().speed = velocity;
        AntiPulse.GetComponent<AntiPulseMove>().angularSpeed = velocity * 7;
        AntiPulse.GetComponent<AntiPulseMove>().centerOfGravity = centerOfGravity;
        AntiPulse.GetComponent<AntiPulseMove>().Wavelength = wavelength;
    }
}



