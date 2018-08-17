using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeCancel : Shake {

    public override void shake(float t, float strength)
    {
        audioManager.instance.Play(rumbles[Random.Range(0, rumbles.Length - 1)], 0.25f, Random.Range(0.96f, 1.03f));
        if (screenShake != null)
            screenShake.Invoke();
    }
}
