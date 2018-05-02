using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spaceFlag : MonoBehaviour {

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (transform.parent == null) {
            print("hoi");
        }
    }
}
