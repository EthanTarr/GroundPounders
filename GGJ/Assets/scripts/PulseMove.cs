using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PulseMove : MonoBehaviour {

    public float speed = 5;
    public float angularSpeed = 20;
    public float Amplitude = 1;
    public float Wavelength = 2f;
    public Color color = Color.white;
    public Transform centerOfGravity;
    private bool forward = true;
    public AudioClip roll;
    bool startup = true;

    private void Start() {
        Invoke("endStartup", 0.5f);
        if (centerOfGravity != null) {
            Wavelength = 2;
            Amplitude *= TerrainGenerator.instance.pulseAmplitudeMultiplier;
        }
    }

    void endStartup() {
        startup = false;
    }

    // Update is called once per frame
    void Update() {
        if (TerrainGenerator.instance.shape == Shape.Plane) {
            if (transform.position.x < TerrainGenerator.boundary && forward) {
		        transform.Translate (new Vector3 (Time.deltaTime * speed, 0, 0));
		    } else if (forward) {
			    forward = false;
			    if (Amplitude < 1f) {
				    Destroy (this.gameObject);
			    }
			    //Amplitude = Amplitude / 4;
		    } else if (!forward && transform.position.x > -TerrainGenerator.boundary) {
                transform.Translate(new Vector3(Time.deltaTime * speed, 0, 0));
                GameObject Pulse = Instantiate(WaveGenerator.instance.antiPulse, transform.position, Quaternion.identity);
                Pulse.GetComponent<AntiPulseMove>().color = color;
                Pulse.GetComponent<AntiPulseMove>().Amplitude = Amplitude / 2;
                Pulse.GetComponent<AntiPulseMove>().speed = speed / 2;
                Destroy(this.gameObject);
            } else if (!forward) {
			    forward = true;
			    if (Amplitude < 1f) {
				    Destroy (this.gameObject);
			    }
			    //Amplitude = Amplitude / 4;
            }
            //this.color = Color.Lerp(color, Color.white, Amplitude);
        } else if (TerrainGenerator.instance.shape == Shape.Sphere) {
            transform.RotateAround(centerOfGravity.position, new Vector3(0, 0, 1), angularSpeed * Time.deltaTime * TerrainGenerator.instance.pulseSpeedMultiplier);
        }
        setPositions();
        Vector3 size = transform.localScale;
        size.x = Wavelength;
        size.y = Wavelength;
    }

    void setPositions() {
        Collider2D[] hitSquares = Physics2D.OverlapCircleAll(transform.position, Wavelength, 1 << 8);
        foreach (Collider2D square in hitSquares) {
            if (square.GetComponent<SquareBehavior>() != null)
                square.GetComponent<SquareBehavior>().getPosition(Amplitude, speed, Wavelength, transform.position);
        }
    }

    bool first;
    void OnTriggerEnter2D(Collider2D other) {
        if (!startup && other.GetComponent<AntiPulseMove>() != null)
        {
            if (Mathf.Abs(other.GetComponent<AntiPulseMove>().Amplitude - Amplitude) <= 1.25f)
            {
                if (Amplitude < .1f || speed < 1.25f || angularSpeed < 7)
                {
                    Destroy(this.gameObject);
                }
            }
            else if (other.GetComponent<AntiPulseMove>().Amplitude - Amplitude > 2)
            {
                Destroy(this.gameObject);
            }
        }
    }
}
