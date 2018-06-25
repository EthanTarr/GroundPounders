using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class ScreenSpaceRefractions : MonoBehaviour
{
    [HideInInspector]
    [SerializeField]
    private Camera _camera;
    private int _downResFactor = 1;

    [SerializeField]private Color32 changeColor;

    private string _globalTextureName = "_GlobalRefractionTex";

    void OnEnable()
    {
        GenerateRT();
    }

    private void Update()
    {
        GetComponent<Camera>().orthographicSize = Camera.main.orthographicSize;
    }

    void GenerateRT()
    {
        _camera = GetComponent<Camera>();

        if (_camera.targetTexture != null) {
            RenderTexture temp = _camera.targetTexture;

            _camera.targetTexture = null;
            DestroyImmediate(temp);
        }

        _camera.targetTexture = new RenderTexture(_camera.pixelWidth << _downResFactor, _camera.pixelHeight << _downResFactor, 16);
        _camera.targetTexture.filterMode = FilterMode.Point;


        Shader.SetGlobalTexture(_globalTextureName, _camera.targetTexture);
    }
}