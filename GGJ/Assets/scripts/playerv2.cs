using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerv2 : MonoBehaviour {

    private Rigidbody2D rigid;
    private SpriteRenderer sprite;
    public LayerMask groundCheck;

    public string playerControl;

    float xSpeed;
    public float acceleration;
    public float friction;
    public float turnSlipperiness = 0.6f;
    public float maxSpeed;
    [Space()]
    public float gravityStrength = 1;
    bool jumped = false;
    public float maxJumpHeight;
    public float minJumpHeight;

    void Start() {
        rigid = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
    }

    void LateUpdate() {
        float HorizInput = Input.GetAxis("Horizontal" + playerControl);
        HorizontalMovement(HorizInput);

        
        if (Input.GetButtonDown("Jump" + playerControl)) {
            rigid.velocity += (Vector2)transform.up * maxJumpHeight;
            Invoke("jumpDelay", 0.05f);
        }

        if (Input.GetButtonUp("Jump" + playerControl) && transform.InverseTransformDirection(rigid.velocity).y > minJumpHeight && jumped) {
            rigid.velocity = (Vector2)transform.up * minJumpHeight;
        }
    }

    private void FixedUpdate() {
        rigid.velocity += gravityStrength * Vector2.down;
        rigid.velocity = new Vector2(xSpeed, rigid.velocity.y) + Vector2.right * xForce;
    }

    float xForce;
    void HorizontalMovement(float HorizInput) { 
        xSpeed += HorizInput * acceleration;
        xSpeed = Mathf.Clamp(xSpeed, -maxSpeed, maxSpeed);

        xForce = Mathf.Lerp(xForce, 0, Time.deltaTime * 7);

        if (Mathf.Abs(xSpeed) > 0) {
            float actualFriction = friction;
            if (Mathf.Abs(HorizInput) > 0.1f && Mathf.Sign(xSpeed) != Mathf.Sign(HorizInput)) {
                actualFriction = -turnSlipperiness;
            }
            xSpeed += -Mathf.Sign(xSpeed) * Mathf.Min(Mathf.Abs(xSpeed), actualFriction);

            if (Mathf.Abs(HorizInput) > 0.1)
                sprite.flipX = xSpeed < 0.1;
        }        
    }

    void addForce(Vector2 direction, float strength) {
        xForce += direction.x * strength;
        rigid.velocity = new Vector2(rigid.velocity.y, strength);
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "Player") {
            Vector2 normalContact = collision.contacts[0].normal;

            float xForce = collision.relativeVelocity.x;
            xForce = Mathf.Min(Mathf.Abs(xForce), 32) * 2f;
            addForce(Vector2.right * normalContact.x, xForce);

            
            bool onTop = collision.transform.position.y + 0.56f / 1.5f < this.transform.position.y - 0.56f / 1.25f;
            float yForce = collision.relativeVelocity.y / 1.5f;

            if (onTop) {
                yForce = Mathf.Clamp(yForce, 13, 50) * 3f;
                print(yForce);
            }
    
            addForce(Vector2.up * normalContact.y, yForce);
        }
    }

    void jumpDelay() {
        jumped = true;
    }

    public bool checkGround()
    {
        bool grounded = false;
        for (int i = 0; i < 5; i++) {
            float dir = -1 + (i % 2) * 2;
            Vector3 downward = transform.position - transform.up * 0.33f + transform.right * 0.1f * Mathf.Ceil(i / 2f) * dir;
            RaycastHit2D hit = Physics2D.Raycast(downward, -transform.up, 0.4f, groundCheck);
            Debug.DrawRay(downward, -transform.up, Color.red);

            if (hit) {
                return true;
            }
        }
        return false;
    }
}
