﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareBehavior : MonoBehaviour {

	public float TotalAmplitude;
    private ArrayList Amplitudes;
	//public float Wavelength = 2f;
	public float FloorOscillation = .02f;
    public float OscillationSpeed = 0.01f;
	[HideInInspector] public float initialY = 0;
    [HideInInspector] public float initialX = 0;
    private float radius;
    public Vector3 initialPosition;
    protected float standardY;
    protected float standardX;
    [HideInInspector] public bool firstBlock;
    [HideInInspector] public Vector3 CenterOfGravity;
    Renderer squareMaterial;
    public Color matColor;
    public Color ampColor;
    private float circleLength;

    Vector2 lastPosition;
	protected void Start () {
        lastPosition = transform.position;

        initialY = transform.localPosition.y;
        initialX = transform.localPosition.x;

        initialPosition = transform.position;

        standardY = transform.position.y;
        standardX = transform.position.x;

        radius = Mathf.Sqrt(Mathf.Pow(transform.localPosition.x, 2) + Mathf.Pow(transform.localPosition.y, 2));

        squareMaterial = transform.GetChild(0).GetComponent<Renderer>();

        Amplitudes = new ArrayList();
        //Debug.Log(this.gameObject.layer);

        //StartCoroutine(physicsCheck());
    }

    public float maxAmplitude = 15f;

    public float maxCircleAmplitude = 12f;

    public float dampen = 1;

    public float circleDampen = 1;

    public void getPosition(float Amplitude, float speed, float Wavelength, Vector3 pulsePosition) {
        if (TerrainGenerator.instance.shape == Shape.Sphere && circleLength == 0) {
            circleLength = Mathf.Sqrt(Mathf.Pow(radius - Mathf.Cos(Wavelength / radius), 2) + Mathf.Pow(radius - Mathf.Sin(Wavelength / radius), 2));
        }

        standardY += FloorOscillation * (Mathf.Sin(Time.time * OscillationSpeed));
        standardX += FloorOscillation * (Mathf.Sin(Time.time * OscillationSpeed));

        float xPos = transform.position.x;
        float xPulsePos = pulsePosition.x;

        if (TerrainGenerator.instance.shape == Shape.Plane) {
            Amplitudes.Add(Amplitude * (speed / 4) * Mathf.Sin(((Mathf.PI / Wavelength) * (xPos - xPulsePos))));
        } else {
            if (pulsePosition.y > CenterOfGravity.y) {
                if ((initialY > 0 && standardX < pulsePosition.x) || (initialY <= 0 && initialX < 0)) {
                    Amplitudes.Add((1 / (circleDampen * radius)) * Amplitude * Mathf.Sin(Mathf.PI * 
                        ((initialPosition - pulsePosition).magnitude / Wavelength)));
                }
                else {
                    Amplitudes.Add(-(1 / (circleDampen * radius)) * Amplitude * Mathf.Sin(Mathf.PI *
                        ((initialPosition - pulsePosition).magnitude / Wavelength)));
                }
            } else {
                if ((initialY <= 0 && standardX > pulsePosition.x) || (initialY > 0 && initialX > 0)) {
                    Amplitudes.Add((1 / (circleDampen * radius)) * Amplitude * Mathf.Sin(Mathf.PI *
                        ((initialPosition - pulsePosition).magnitude / Wavelength)));
                } else {
                    Amplitudes.Add(-(1 / (circleDampen * radius)) * Amplitude * Mathf.Sin(Mathf.PI *
                        ((initialPosition - pulsePosition).magnitude / Wavelength)));
                }
            }
        }        
    }

    void setPosition() {
        TotalAmplitude = 0;
        if (Amplitudes.Count != 0) {
            foreach (float value in Amplitudes) {
                TotalAmplitude += value;
            }
            Amplitudes.Clear();
        }


        if (TerrainGenerator.instance != null && TerrainGenerator.instance.shape == Shape.Sphere) {
            TotalAmplitude = Mathf.Clamp(TotalAmplitude, -maxCircleAmplitude, maxCircleAmplitude);
            Vector3 vector = ((-((-transform.localPosition + new Vector3(0, 0, 0)).normalized)) * TotalAmplitude) / dampen;
            transform.localPosition = new Vector3(Mathf.Lerp(transform.localPosition.x, initialX + vector.x, Time.deltaTime),
                Mathf.Lerp(transform.localPosition.y, initialY + vector.y, Time.deltaTime), 0);
        } else {
            Vector3 vector = ((-((-transform.position + new Vector3(0, 0, 0)).normalized)) * TotalAmplitude) / dampen;

            //transform.position = new Vector3(transform.position.x, Mathf.Lerp(transform.position.y, standardY + vector.y, Time.deltaTime), transform.position.z);
            TotalAmplitude = Mathf.Clamp(TotalAmplitude, -maxAmplitude, maxAmplitude);
            transform.position = transform.right * transform.position.x + Vector3.up *
                Mathf.Lerp(transform.position.y, standardY + vector.y, Time.deltaTime) + transform.forward * transform.position.z;
        }

        getVelocity();

        if (firstBlock)
        {
            //GetComponent<SpriteRenderer>().color = Color.Lerp(GetComponent<SpriteRenderer>().color, floorColor, Time.deltaTime);
        }
    }

    [HideInInspector] public float velocity;
    void getVelocity() {
        velocity =  transform.position.y - lastPosition.y;
        lastPosition = transform.position;
    }

    void LateUpdate(){
        if (pause.instance != null && pause.instance.paused()) {
            return;
        }

        setPosition();
        squareMaterial.material.SetColor("_Color", Color.Lerp(matColor, ampColor, (transform.localPosition.y - initialY)));
    }
}