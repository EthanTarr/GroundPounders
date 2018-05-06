using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class windController : MonoBehaviour {

    [HideInInspector] public Vector2 windDirection;
    public bool manageWindDirection;
    public float speed;

    private void Start()
    {
        if (manageWindDirection) {
            windDirection = this.transform.right;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        Debug.DrawRay(transform.position, transform.right * 10, Color.red);
        if (collision.GetComponent<playerController>() != null) {
            Vector2 newDir = collision.transform.position;
            newDir += windDirection * speed;
            collision.transform.position = newDir;
        }
    }
}
