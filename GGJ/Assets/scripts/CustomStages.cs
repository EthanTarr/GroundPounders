using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CustomStages : MonoBehaviour {

    public int numberOfStages = 0;
    public abstract float customPlatformPos(int mapIndex, float floorIndex);
}
