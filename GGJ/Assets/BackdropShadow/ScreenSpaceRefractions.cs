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

    public bool camSamePosition = true;
    public Vector3 offset;

    [SerializeField]private Color32 changeColor;

    public static bool flipUV = true;

    private string _globalTextureName = "_GlobalRefractionTex";

    void OnEnable()
    {
        GenerateRT();
    }

    private void Start(){
        if (flipUV) {
            offset.z = -offset.z;
            transform.eulerAngles = new Vector3(transform.rotation.x, 180f, 180f);
        }
    }

    private void Update() {
        GetComponent<Camera>().orthographicSize = Camera.main.orthographicSize;

        if (!flipUV) {
            offset.z = -Mathf.Abs(offset.z);
            transform.eulerAngles = new Vector3(transform.rotation.x, 0, 0);
        } else {
            offset.z = Mathf.Abs(offset.z);
            transform.eulerAngles = new Vector3(transform.rotation.x, 180f, 180f);
        }

        if (camSamePosition)
            transform.position = new Vector3( Camera.main.transform.position.x + offset.x, 
                Camera.main.transform.position.y + offset.y, 
                offset.z);

        //transform.GetChild(0).transform.position = -transform.forward * (transform.GetChild(0).transform.position.z - 8);
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