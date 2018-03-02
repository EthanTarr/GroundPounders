using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntiPulseMove : MonoBehaviour
{

    public float speed = 5;
    public float angularSpeed = 5;
    public float Amplitude = 1;
    public float Wavelength = 2f;
    public Color color = Color.white;
    public Transform centerOfGravity;
    private bool forward = true;
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
    void Update () {
        if (centerOfGravity == null) {
            if (transform.position.x > -TerrainGenerator.boundary && forward) {
                transform.Translate(new Vector3(Time.deltaTime * -speed, 0, 0));
            } else if (forward) {
                forward = false;
                if (Amplitude < .1f) {
                    Destroy(this.gameObject);
                }
                //Amplitude = Amplitude / 4;
            }else if (!forward && transform.position.x < TerrainGenerator.boundary){
                
                transform.Translate(new Vector3(Time.deltaTime * speed, 0, 0));
                GameObject Pulse = Instantiate(WaveGenerator.instance.pulse, transform.position, Quaternion.identity);
                Pulse.GetComponent<PulseMove>().color = color;
                Pulse.GetComponent<PulseMove>().Amplitude = Amplitude / 2;
                Pulse.GetComponent<PulseMove>().speed = speed / 2;
                
                Destroy(this.gameObject);
                
            } else if (!forward) {
                forward = true;
                if (Amplitude < .1f) {
                    Destroy(this.gameObject);
                }
                //Amplitude = Amplitude / 4;
            }
            //this.color = Color.Lerp(color, Color.white, Time.deltaTime / 32);
        } else {
            if (Amplitude < .1f || angularSpeed < 10) {
                Destroy(this.gameObject);
            }
            transform.RotateAround(centerOfGravity.position, new Vector3(0, 0, 1), -angularSpeed * Time.deltaTime * TerrainGenerator.instance.pulseSpeedMultiplier);
        }
        setPositions();
    }

    void setPositions() {
        Collider2D[] hitSquares = Physics2D.OverlapCircleAll(transform.position, Wavelength, 1 << 8);
        foreach (Collider2D square in hitSquares)
        {
            if (square.GetComponent<SquareBehavior>() != null)
                square.GetComponent<SquareBehavior>().getPosition(-Amplitude, speed, Wavelength, transform.position);
        }
    }

    bool first;
    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.GetComponent<SquareBehavior>() != null)
        {
            other.gameObject.GetComponent<SquareBehavior>().firstBlock = true;
            //other.gameObject.GetComponent<SpriteRenderer>().color = color;
        }
        else if (other.GetComponent<PulseMove>() != null)
        {
            if (!first)
            {
                first = true;
            }
            else if (!startup && Mathf.Abs(other.GetComponent<PulseMove>().Amplitude - Amplitude) <= 1.5f)
            {
                other.GetComponent<PulseMove>().Amplitude /= 2;
                Amplitude /= 2;
                other.GetComponent<PulseMove>().speed /= 1.25f;

                speed /= 1.25f;
                angularSpeed /= 3f;
                other.GetComponent<PulseMove>().angularSpeed /= 3f;

                if (Amplitude < .1f || speed < 1.25f) {
                    Destroy(this.gameObject);
                }
            }
            else if (other.GetComponent<PulseMove>().Amplitude - Amplitude > 2)
            {
                Destroy(this.gameObject);
            }
        }
    }
}
