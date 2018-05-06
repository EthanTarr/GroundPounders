using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindTurbulenceStages : CustomStages {

    public override float customPlatformPos(int mapIndex, float floorIndex) {

        switch (mapIndex) {
            case 1:
                return transform.position.y;
            case 2:
                return transform.position.y - 0.50f + Mathf.Sin(floorIndex / 10);
            case 3:
                return transform.position.y - 1 + Mathf.Abs(floorIndex / 15);
            case 4:
                return transform.position.y - Mathf.Abs(Mathf.Pow(.03f * floorIndex, 2));
            case 5:
                return transform.position.y - 0.75f - Mathf.Sin(floorIndex / 10);
            case 6:
                return transform.position.y - 0.5f - floorIndex * 0.025f;
            case 7:
                return transform.position.y + floorIndex * 0.025f;
            case 8:
                return transform.position.y - 0.75f - Mathf.Sin((floorIndex + 20) / 10);
            case 9:
                return transform.position.y - 0.75f - Mathf.Sin((floorIndex - 15) / 10);
            case 10:
                return transform.position.y + 0.5f - Mathf.Abs(floorIndex / 25);
        }
        return 0;
    }
}
