using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trainingDummy : playerController {

    private void LateUpdate() {

    }


    private void FixedUpdate()
    {
        if (Time.timeScale > 0) {
            onGround = checkGround();
            spriteAnimationManager(0, onGround);
            bool smashing = smashStateEnum != null;

            bounceDirection = new Vector2(Mathf.Lerp(bounceDirection.x, 0, Time.deltaTime * (smashing ? 25 : 2.25f) * (Mathf.Abs(xSpeed) > 0.5f ? 2f : 1)),
                Mathf.Lerp(bounceDirection.y, 0, Time.deltaTime * (smashing ? 2 : 35)));

            if (!smashing && !vunrabilityFrames && active) {
                rigid.velocity = transform.TransformDirection(new Vector2(xSpeed, transform.InverseTransformDirection(rigid.velocity).y));
            }


            Vector2 gravityDirection = Vector2.down;
            Vector2 dirUp = -gravityDirection;
            this.transform.up = Vector2.Lerp(this.transform.up, dirUp, Time.deltaTime * 500);

            if (smashStateEnum == null)
                rigid.velocity -= (Vector2)transform.up * gravityStrength * Time.deltaTime;

            if (active) {
                rigid.velocity += (Vector2)transform.up * bounceDirection.y;
                rigid.velocity += (Vector2)transform.right * (dashDirection + bounceDirection.x);
            }
        }
    }

    void collisionWithPlayer(Collision2D other)
    {
        // vertical collision
        bool onTop = false;
        if (!checkGround())
        {
            for (int i = 0; i < 5; i++)
            {
                float direction = -1.6f + (i % 2) * 2.9f;
                Vector3 downward = transform.position - transform.up * downLazy * 1.25f + transform.right * 0.2f * Mathf.Ceil(i / 2.5f) * direction;
                RaycastHit2D hit = Physics2D.Raycast(downward, -transform.up, 0.4f);

                if (hit && hit.collider.gameObject.Equals(other.gameObject))
                {
                    onTop = true;
                    break;
                }
            }
        }

        if (onTop) {
            chargeVisualEffects(0);
            //StopCoroutine(smashStateEnum);
            smashStateEnum = null;
            rigid.velocity = new Vector2(rigid.velocity.x, 0);
            Shake.instance.shake(1, 2.5f);
            other.transform.GetComponent<Rigidbody2D>().velocity = new Vector3(other.transform.GetComponent<Rigidbody2D>().velocity.x, -10);
            bounceDirection.y += 50;
            WaveGenerator.instance.timeFreeze(0.2f, 0.15f);
        }

        //juice   
        GameObject colParticle = Instantiate(collisionParticle, other.contacts[0].point, transform.rotation);
        ParticleSystem.MainModule ps = colParticle.GetComponent<ParticleSystem>().main;
        ps.startColor = baseColor;
        if (onTop)
        {
            ps.startSize = 1.5f;
            ps.startSpeed = 70f;
        }
        Destroy(colParticle, 0.75f);
        audioManager.instance.Play(softLanding[UnityEngine.Random.Range(0, softLanding.Length - 1)], onTop ? 1 : 0.5f, UnityEngine.Random.Range(0.96f, 1.03f));
    }

    private void OnEnable() {
        setColors(GetComponent<SpriteRenderer>().color);
        GameObject particle = Instantiate(deathParticle, transform.position, Quaternion.identity) as GameObject;
        ParticleSystem.MainModule ps = particle.GetComponent<ParticleSystem>().main;
        ps.startColor = Color.Lerp(baseColor, Color.white, 0.5f);
    }

    void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.tag.Equals("Player")) {
            collisionWithPlayer(other);
        } else if (other.gameObject.tag.Equals("Spike") && !alreadyDead) {
            die();
        }
        else if (other.gameObject.tag.Equals("top")) {
            die();
        } else if (other.gameObject.tag.Equals("Floor")) {

        } else { //bounce off side walls
            GameObject colParticle = Instantiate(collisionParticle, transform.position, transform.rotation);
            ParticleSystem.MainModule ps = colParticle.GetComponent<ParticleSystem>().main;
            ps.startColor = baseColor;
            Destroy(colParticle, 0.25f);
            audioManager.instance.Play(softLanding[UnityEngine.Random.Range(0, softLanding.Length - 1)], 0.75f, UnityEngine.Random.Range(0.96f, 1.03f));
            bounceDirection.x *= -0.75f;
            dashDirection *= -0.75f;
        }
    }

    public void setColors(Color baseColor)
    {
        this.baseColor = baseColor;
        ParticleSystem.MainModule ps = chargeParticle.GetComponent<ParticleSystem>().main;
        ps.startColor = Color.Lerp(baseColor, Color.white, 0.5f);
    }
}




