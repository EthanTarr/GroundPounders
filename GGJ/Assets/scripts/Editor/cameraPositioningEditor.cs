using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CameraPositioning))]
public class cameraPositionsContainerEditor : Editor {

    public override void OnInspectorGUI()
    {
        CameraPositioning cam = (CameraPositioning)target;
        if (GUILayout.Button("Store")) {
            cam.storeCameraData();
        }

        DrawDefaultInspector();
    }
}
