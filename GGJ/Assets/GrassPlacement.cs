using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassPlacement : MonoBehaviour {

    void Start() {
        RaycastHit2D hit = Physics2D.Raycast(transform.position - transform.up, -transform.up);
        if (hit) {
            transform.position = hit.point;
        }
    }

}
