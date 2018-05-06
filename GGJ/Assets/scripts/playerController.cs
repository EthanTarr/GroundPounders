using System;
using System.Collections;
using UnityEngine;


public class playerController : MonoBehaviour{

    protected Rigidbody2D rigid;
    [HideInInspector] public SpriteScript spriteAnim;
    public LayerMask groundCheck;

    public string playerControl;
    public int playerNum;
    bool canDoubleJump;
    protected bool touchingGround;
    public float topWidth;

    [Header("Control")]
    public float speed = 5;
    public float accelerate = 0;
    public float decelerate = 0;
    protected float xSpeed;
    [Space()]
    public float maxJumpHeight = 10;
    public float minJumpHeight;

    [Header("Gravity")]
    public float gravityStrength;
    public Vector2 gravityDirection;
    public Transform centerOfGravity;
    bool fastFall;
    bool canFastFall;

    [Header("Smash Properties")]
    public float minSmashSpeed = 4;
    protected float SmashSpeed;
    public float maxSmashSpeed = 7; 
    [Space()]
    public float minSmashPower = 3;
    protected float smashPower = 3;
    public float maxSmashPower = 3;

    public float waveBounciness;
    [HideInInspector] public bool jumped;
    public float playerBounceAmplifier = 2;

    protected float maxSmashVulnerabilityTime;
    [HideInInspector] public bool smashing;
    protected float SmashCooldownTime = 0;
    public float waveSpeed = 7;
    public float maxChargeTime = 0.75f;
    float chargeValue;

    protected bool vunrabilityFrames = false;

    protected float previousAmplitude = 0;
    bool canMakeWave = true;
    protected bool canSmash = true;

    [Header("Dash Properties")]
    public float dashSpeed = 7;
    public float dashCharge = 0.35f;
    protected bool canDash = true;
    protected float dashDirection;
    protected float dashDecel;

    [Header("Visual Effects")]

    public GameObject shockWave;
    public ParticleSystem chargeParticle;
    protected Color baseColor;
    public Color fullColor;
    public GameObject dashCancelParticle;
    public GameObject dashTrailParticle;
    public GameObject deathParticle;
    public GameObject collisionParticle;
    public GameObject chargeCircle;

    public AudioClip deathExplosion;

    [Header("Sound Effects")]
    public AudioClip[] softLanding;
    public AudioClip loadPower;
    public AudioClip smash;
    public AudioClip jump;
    public AudioClip charge;
    public AudioClip cancel;
    public AudioClip extraBounce;
    public AudioClip squash;
    public AudioClip unsqaush;

    [Header("Bounciness")]
    public float bounciness;
    [HideInInspector] public Vector2 bounceDirection;
     
    private SquashAndStretch sqetch;

    [Header("Modifiers")]
    public bool airControl;
    public bool seperateDashCooldown;
    public bool canDashOnGround;
    public bool instantBounceKill;
    public bool fullChargeInvinc;
    public bool holdMaxSmash;
    public bool tightDash;
    public bool doubleJump;

    [Space()]
    public GameObject pulse;
    public GameObject antiPulse;
    [HideInInspector] public bool inLobby;

    public bool active = true;

    protected virtual void Start() {
        rigid = GetComponent<Rigidbody2D>();
        spriteAnim = GetComponentInChildren<SpriteScript>();
        active = true; 
        changeModifiers();

        setColors(GetComponent<SpriteRenderer>().color); 
        sqetch = GetComponent<SquashAndStretch>();
        chargeParticle.startColor = Color.Lerp(baseColor, Color.white, 0.5f);

        chargeVisualEffects(0);
        chargeCircle.transform.localScale = Vector3.zero;
    }

    protected void changeModifiers() {
        airControl = GameManager.instance.airControl;
        seperateDashCooldown = GameManager.instance.seperateDashCooldown;
        canDashOnGround = GameManager.instance.canDashOnGround;
        maxSmashPower = GameManager.instance.maxSmashPower;
        waveBounciness = GameManager.instance.bounciness;
        maxSmashSpeed = GameManager.instance.maxSmashSpeed;
        fullChargeInvinc = GameManager.instance.fullChargeInvinc;
        holdMaxSmash = GameManager.instance.holdMaxSmash;
        maxChargeTime = GameManager.instance.maxChargeTime;
        tightDash = GameManager.instance.tightDash;
        instantBounceKill = GameManager.instance.instantBounceKill;
        dashSpeed = GameManager.instance.dashDistance;
        doubleJump = GameManager.instance.doubleJump;
    }

    void LateUpdate() {
        if (Time.timeScale > 0) {
            float HorizInput = Input.GetAxis("Horizontal" + playerControl);
            if (centerOfGravity != null) {
                gravityDirection = (transform.position - centerOfGravity.position).normalized;
            }
            checkGround();

            bounceDirection = new Vector2(Mathf.Lerp(bounceDirection.x, 0, Time.deltaTime * (smashing ? 25 : 2.25f) * (Mathf.Abs(xSpeed) > 0.5f ? 2f : 1)),
                Mathf.Lerp(bounceDirection.y, 0, Time.deltaTime * (smashing ? 2 : 35)));
            dashDirection = Mathf.Lerp(dashDirection, 0, Time.deltaTime * dashDecel);

            if (!smashing && !vunrabilityFrames && playerControl != "" && rigid.bodyType == RigidbodyType2D.Dynamic){
                movement(HorizInput);
            } if (!smashing && !vunrabilityFrames && active) {
                rigid.velocity = transform.TransformDirection(new Vector2(xSpeed, transform.InverseTransformDirection(rigid.velocity).y));
            }

            if ((canSmash || !seperateDashCooldown || !tightDash) && !smashing) {
                if (centerOfGravity == null) {
                        rigid.velocity += gravityDirection * gravityStrength;
                        if (Input.GetAxis("Vertical" + playerControl) < -0.7f) {
                            rigid.velocity += gravityDirection * 0.25f;
                        }
                } else {
                    Vector2 gravityDirection = (-transform.position + centerOfGravity.position).normalized;
                    rigid.velocity -= (Vector2)transform.up * gravityStrength * Time.deltaTime;
                    Vector2 dirUp = -gravityDirection;
                    this.transform.up = Vector2.Lerp(this.transform.up, dirUp, Time.deltaTime * 500);
                }
            }

            if (Input.GetButtonDown("Pause" + playerControl) && pause.instance != null) {
                pause.instance.togglePause(this);
            }
        }

        if (inLobby && Input.GetButtonDown("Dropout" + playerControl)) {
            StartCoroutine(dropoutOfLobby());
        }
    }

    void jumpDelay() {
        jumped = true;
    }

    bool squishing = false;
    protected virtual void movement(float horizInput) {
        
        touchingGround = checkGround();
        if (active) {
            //Horizontal Movement
            if (Mathf.Abs(horizInput) > 0.1f && (canSmash || !seperateDashCooldown)) {
                xSpeed += speed * horizInput / (accelerate * 4);
                spriteAnim.GetComponent<SpriteRenderer>().flipX = horizInput < 0;
                slopeCheck();
            } else {
                xSpeed = Mathf.Lerp(xSpeed, 0, Time.deltaTime * decelerate);
            }
            xSpeed = Mathf.Clamp(xSpeed, -speed, speed);

            //Vertical Movement
            if (touchingGround) {
                jumped = false;
                canDoubleJump = true;
                fastFall = false;
                if (Input.GetButtonDown("Jump" + playerControl) && (canSmash || !seperateDashCooldown || !tightDash)) {
                    rigid.velocity += (Vector2)transform.up * maxJumpHeight;
                    audioManager.instance.Play(jump, 0.5f, UnityEngine.Random.Range(0.93f, 1.05f));
                    Invoke("jumpDelay", 0.05f);
                }

                if (Input.GetAxis("Vertical" + playerControl) < -0.7f) {
                    if (sqetch.animatedStretch < 0.5f)
                    {

                        sqetch.setAnimatedStretch(0.5f);
                        if (!squishing) {
                            squishing = true;
                            audioManager.instance.Play(squash, 0.4f, UnityEngine.Random.Range(0.9f, 1.1f));
                        }
                    }
                } else {
                    if(squishing) {
                        squishing = false;
                        audioManager.instance.Play(unsqaush, 0.4f, UnityEngine.Random.Range(0.9f, 1.1f));
                    }
                }
                //dashingOnGround
                if (canDash && Input.GetButtonDown("Dash" + playerControl) && (canDashOnGround && canSmash)) {
                    StartCoroutine(dash(Input.GetAxis("Dash" + playerControl), true, true));
                }
            } else {
                if (canSmash) {
                    if (Input.GetButtonDown("Jump" + playerControl) && doubleJump && canDoubleJump) {
                        canDoubleJump = false;
                        rigid.velocity += (Vector2)transform.up * maxJumpHeight / 1.5f;
                        audioManager.instance.Play(jump, 0.5f, UnityEngine.Random.Range(0.93f, 1.05f));

                        GameObject dashExpurosion = Instantiate(dashCancelParticle, transform.position, transform.rotation);
                        dashExpurosion.GetComponent<ParticleSystem>().startColor = spriteAnim.GetComponent<SpriteRenderer>().color;
                        Destroy(dashExpurosion, 1.5f);
                    }

                    if (canDash && Input.GetButtonDown("Dash" + playerControl) && !smashing) {
                        StartCoroutine(dash(Input.GetAxis("Dash" + playerControl), true, false));
                    }

                    if (Input.GetButtonDown("Smash" + playerControl) && !smashing) {
                        StartCoroutine(chargeSmash(Input.GetAxis("Horizontal" + playerControl)));
                    }               
                }
            }
            //shortHop
            if (Input.GetButtonUp("Jump" + playerControl) && transform.InverseTransformDirection(rigid.velocity).y > minJumpHeight && jumped) {
                rigid.velocity = (Vector2)transform.up * minJumpHeight;
            }
            spriteAnimationManager(horizInput, touchingGround);
        }
    }

    private void FixedUpdate() {
        if (Time.timeScale == 1) {
            rigid.velocity += (Vector2)transform.up * bounceDirection.y;
            if (active) {
                rigid.velocity += (Vector2)transform.right * (dashDirection + bounceDirection.x);
            }
            checkGround();
        }
    }

    protected void spriteAnimationManager(float HorizInput, bool touchingGround) {
        if (!touchingGround) {
            spriteAnim.SetAnimation("jump");
            return;
        }

        if (vunrabilityFrames) {
            spriteAnim.SetAnimation("crush");
            return;
        }

        spriteAnim.SetAnimation(Mathf.Abs(HorizInput) > 0.1f ? "walk" : "idle");
        spriteAnim.SetFramesPerSecond(20);

        if (Mathf.Abs(HorizInput) > 0.1f && Mathf.Sign(xSpeed) != Mathf.Sign(HorizInput)) {
            spriteAnim.SetFramesPerSecond(10);
            spriteAnim.SetAnimation("walk");
        }
    }

    IEnumerator dropoutOfLobby() {
        float t = 1.5f;
        
        while (t > 0) {
            t -= Time.deltaTime;
            if (!Input.GetButton("Dropout" + playerControl)) {
                active = true;
                yield break;
            }
            yield return null;
        }
        audioManager.instance.Play(deathExplosion, 0.5f, UnityEngine.Random.Range(0.96f, 1.04f));

        GameObject particle = Instantiate(deathParticle, transform.position, Quaternion.identity) as GameObject;
        particle.GetComponent<ParticleSystem>().startColor = Color.Lerp(baseColor, Color.white, 0.5f);
        GetComponent<SpriteRenderer>().color = fullColor;

        FindObjectOfType<controlAssigmentManager>().characterDropout(this);
        Destroy(this.gameObject);
    }

    protected void chargeVisualEffects(float toggle) {
        spriteAnim.GetComponent<SpriteRenderer>().color = Color.Lerp(baseColor, fullColor, toggle);
        chargeParticle.startSize = Mathf.Lerp(0.25f, 0.75f, toggle * 10);
        float changeSize = Mathf.Lerp(0, 1, toggle * 2);
        chargeCircle.transform.localScale = Vector3.one * Mathf.Lerp(0.6f,0.1f, toggle);
        Color chargeColor= GetComponent<SpriteRenderer>().color;
        chargeColor.a = Mathf.Lerp(0,0.5f,toggle);
        chargeCircle.GetComponent<SpriteRenderer>().color = chargeColor;
        chargeParticle.transform.localScale = new Vector3(changeSize, changeSize, changeSize);
    }

    protected IEnumerator chargeSmash(float currentDirection) {
        bounceDirection.x = bounceDirection.x / 12;    
        bounceDirection.y *= 10;
        
        dashDirection = 0;
        bool direction = GetComponent<SpriteRenderer>().flipX;

        fastFall = false;
        smashing = true;
        bool maxed = false;
        chargeValue = 0;

        SmashSpeed = minSmashSpeed;
        smashPower = minSmashPower;
        float hold = maxChargeTime + (holdMaxSmash ? 0.5f : 0);

        float initialY = Mathf.Max(0, transform.InverseTransformDirection(rigid.velocity).y);

        while (chargeValue <= hold) {
            float lerp = (chargeValue / maxChargeTime);
            chargeVisualEffects(lerp);

            SmashSpeed = Mathf.Lerp(minSmashSpeed, maxSmashSpeed, lerp);
            smashPower = Mathf.Lerp(minSmashPower, maxSmashPower, lerp);
            SmashCooldownTime = Mathf.Lerp(0.25f, maxSmashVulnerabilityTime, lerp);

            if (!Input.GetButton("Smash" + playerControl) || smashing == false) {
                audioManager.instance.Play(charge, 0.5f * (chargeValue / maxChargeTime));
                break;
            }

            //Dash out of charge
            if (Input.GetButton("Dash" + playerControl) && canDash) {
                chargeValue = 0;
                chargeCircle.transform.localScale = Vector3.zero;
                StartCoroutine(dash(chargeValue * 20, direction, false));
                yield break;
            }

            currentDirection = Input.GetAxis("Dash" + playerControl);
            chargeValue += Time.deltaTime;

            if (chargeValue >= maxChargeTime && !maxed) {
                audioManager.instance.Play(charge, 0.5f * (chargeValue / maxChargeTime), 1.1f);

                GameObject wave = Instantiate(shockWave, transform.position, transform.rotation) as GameObject;
                wave.transform.SetParent(transform);
                wave.GetComponent<SpriteRenderer>().color = new Color(GetComponent<SpriteRenderer>().color.r, GetComponent<SpriteRenderer>().color.g, GetComponent<SpriteRenderer>().color.b, wave.GetComponent<SpriteRenderer>().color.a);

                Color color = GetComponent<SpriteRenderer>().color;
                color.a = 0.5f;
                spriteAnim.GetComponent<SpriteRenderer>().color = color;
                color = GetComponent<SpriteRenderer>().color;
                color.a = 255f;
                spriteAnim.GetComponent<SpriteRenderer>().color = color;

                maxed = true;
            }

            rigid.velocity = ((Vector2)transform.right * transform.InverseTransformDirection(rigid.velocity).x) * 0.95f + (Vector2)transform.up * initialY + (Vector2)transform.InverseTransformDirection((Vector3)bounceDirection);

            initialY /= 1.1f;
            if (airControl) {
                rigid.velocity += (Vector2)transform.right * Input.GetAxis("Horizontal" + playerControl) * 0.2f;
            }

            yield return null;
        }

        chargeValue = Mathf.Min(chargeValue, maxChargeTime);
        StartCoroutine(smashOutOFCharge(chargeValue));
    }

    protected IEnumerator dash(float chargeValue, bool direction) {
        print("dashing");
        yield return dash(chargeValue, direction, false);
        chargeCircle.transform.localScale = Vector3.zero;
        chargeValue = 0;
    }

    protected IEnumerator dash(float chargeValue, bool direction, bool onGround) {
        StartCoroutine("spawnAfterImages");
        GetComponent<SpriteRenderer>().flipX = direction;
        float dir = Input.GetAxisRaw("Horizontal" + playerControl);

        if (Mathf.Abs(dir) < 0.5f) {
            dir = (spriteAnim.GetComponent<SpriteRenderer>().flipX ? -1 : 1);
        }
        dashDirection += dashSpeed * dir / (onGround ? 1.5f : 1);
        dashDirection *= 3f;
        dashDecel = tightDash ? 10f : 5f;
        rigid.velocity = transform.TransformDirection(new Vector2(0, onGround ? -10 : Mathf.Max(0, transform.InverseTransformDirection(rigid.velocity).y) / 2));
       
        smashing = false;
        chargeVisualEffects(0);
        SmashSpeed = 0;
        smashPower = 0;

        spriteAnim.GetComponent<SpriteRenderer>().color = changeOpacity(spriteAnim.GetComponent<SpriteRenderer>().color, 0.65f);

        audioManager.instance.Play(this.cancel, 0.25f);

        GameObject dashExpurosion = Instantiate(dashCancelParticle, transform.position, transform.rotation);
        dashExpurosion.GetComponent<ParticleSystem>().startColor = spriteAnim.GetComponent<SpriteRenderer>().color;
        Destroy(dashExpurosion, 1.5f);

        createDashParticle(2);

        if (!seperateDashCooldown) {
            canSmash = false;
            StartCoroutine(recovery(dashCharge));
            vunrabilityFrames = false;

            yield return new WaitForSeconds(0.25f);

            chargeParticle.gameObject.transform.localScale = new Vector3(rigid.velocity.x, 0);
            yield return new WaitForSeconds(0.05f);
            StopCoroutine("spawnAfterImages");
        } else {
            canSmash = false;
            canDash = false;
            vunrabilityFrames = false;

            float j = 0;
            while (j < 0.1f) {
                j += Time.deltaTime;
                rigid.velocity = new Vector2(rigid.velocity.x, rigid.velocity.y/1.1f +  bounceDirection.y);
                yield return new WaitForEndOfFrame();
            }

            canSmash = true;
            chargeParticle.gameObject.transform.localScale = new Vector3(rigid.velocity.x, 0);
            yield return new WaitForSeconds(0.3f);
            StopCoroutine("spawnAfterImages");

            yield return new WaitForSeconds(0.95f);
            canDash = true;
        }
        spriteAnim.GetComponent<SpriteRenderer>().color = changeOpacity(spriteAnim.GetComponent<SpriteRenderer>().color, 225);
        audioManager.instance.Play(loadPower, 0.5f, UnityEngine.Random.Range(0.96f, 1.03f));
    }

    protected IEnumerator smashOutOFCharge(float chargeValue) {
        if (chargeValue > maxChargeTime / 2)
            StartCoroutine("spawnAfterImages");

        yield return new WaitForSeconds(0.05f);
        rigid.velocity = transform.TransformDirection(new Vector3(0, -SmashSpeed));
        bounceDirection.y = 0;
        spriteAnim.SetAnimation("smash");
        createDashParticle(1.5f);
        
        chargeVisualEffects(0);
        chargeCircle.transform.localScale = Vector3.zero;
    }

    public void createDashParticle(float lifetime) {
        GameObject dashTrail = Instantiate(dashTrailParticle, transform.position, transform.rotation);
        dashTrail.transform.parent = this.transform;
        dashTrail.GetComponent<ParticleSystem>().startColor = changeOpacity(baseColor, 0.75f);
        Destroy(dashTrail, lifetime);
    }

    [Space()]
    //this is really lazy to position raycasts to size
    [HideInInspector] public float downLazy = 0.56f;
	public bool checkGround() {
        bool grounded = false;
        for (int i = 0; i < 5; i++) {
            float dir = -1 + (i % 2) * 2;
            Vector3 downward = transform.position - transform.up * downLazy + transform.right * 0.1f * Mathf.Ceil(i/2f) * dir;
            RaycastHit2D hit = Physics2D.Raycast(downward, -transform.up, 0.4f, groundCheck);
            Debug.DrawRay(downward, -transform.up, Color.red);

            if (hit) {
                grounded = true;
                if (hit.transform.GetComponent<SquareBehavior>() != null)
                {
                    SquareBehavior square = hit.transform.GetComponent<SquareBehavior>();
                    float minimumAmp = (centerOfGravity == null) ? 2 : 0.2f;

                    if (square.GetComponent<SquareBehavior>().TotalAmplitude > minimumAmp) {
                        minimumAmp = centerOfGravity == null ? 1.75f : 0.75f;
                        bounceDirection += Vector2.up * square.GetComponent<SquareBehavior>().TotalAmplitude / minimumAmp;

                        if (centerOfGravity != null) {
                            bounceDirection += Vector2.up * TerrainGenerator.instance.pulseAmplitudeMultiplier / 4;
                        }

                        bounceDirection.y *= waveBounciness / (jumped ? 3 : 1);
                        square.GetComponent<SpriteRenderer>().color = Color.red;
                        grounded = false;
                    }
                    break;
                }
            }        
        }
        return grounded;
    }

    void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.tag.Equals("Floor")) {
            collisionWithFloor(other);
        } else if (other.gameObject.tag.Equals("Player")) {
            collisionWithPlayer(other);
        } else if (other.gameObject.tag.Equals("Spike") && !alreadyDead) {
            die();
        } else if (other.gameObject.tag.Equals("top")) {
            bounceDirection.y = 0;
        } else { //bounce off side walls
            GameObject colParticle = Instantiate(collisionParticle, transform.position, transform.rotation);
            colParticle.GetComponent<ParticleSystem>().startColor = baseColor;
            Destroy(colParticle, 0.25f);
            audioManager.instance.Play(softLanding[UnityEngine.Random.Range(0, softLanding.Length - 1)], 0.75f, UnityEngine.Random.Range(0.96f, 1.03f));
            bounceDirection.x *= -0.75f;
            dashDirection *= -0.75f;
        }
    }

    void collisionWithFloor(Collision2D other) {
        float strength = Mathf.Clamp(other.relativeVelocity.magnitude / 40f, 0, .8f);
        if (smashing) {
            StopCoroutine("chargeSmash");
            StopCoroutine("smashAfterCharge");
            StopCoroutine("spawnAfterImages");
            canSmash = false;

            if (Shake.instance != null)
                Shake.instance.shake(2, 3);
            
            rigid.velocity = Vector3.zero;
            Color color = GetComponent<SpriteRenderer>().color;
            color.a = 0.75f;
            WaveGenerator.instance.makeWave(other.gameObject.GetComponent<SquareBehavior>().initialPosition, strength * smashPower, color, chargeValue >= maxChargeTime ? 10 : 7, centerOfGravity);
            audioManager.instance.Play(smash, 0.75f, UnityEngine.Random.Range(0.95f, 1.05f));

            SmashSpeed = 0;
            smashPower = 0;
            chargeValue = 0;
            StartCoroutine(recovery(SmashCooldownTime));

            if (FindObjectOfType<TerrainTilt>() != null) {
                FindObjectOfType<TerrainTilt>().applySmashForce(this.transform.position, strength);
            }
        } 
    }

    void collisionWithPlayer(Collision2D other) {
        if (smashing && other.transform.GetComponent<playerController>().touchingGround) {
            WaveGenerator.instance.makeWave(transform.position + Vector3.up * -1, 0.75f, Color.white, chargeValue >= maxChargeTime ? 10 : 6, centerOfGravity);
        }

        GameObject colParticle = Instantiate(collisionParticle, other.contacts[0].point, transform.rotation);

        //if fully charged, and th mod is on, player cannot be bounced
        if (chargeValue >= maxChargeTime && fullChargeInvinc) {
            float pushBack = other.relativeVelocity.x * 2;
            pushBack = Mathf.Sign(pushBack) * Mathf.Max(30, Mathf.Abs(pushBack));
            other.transform.GetComponent<playerController>().bounceDirection += pushBack * Vector2.right + Vector2.up * 10;
            bounceDirection.x = 0;
            rigid.velocity = new Vector2(0, -maxSmashSpeed);
            return;
        }

        Vector2 dir = Vector2.zero;

        //horizontal collision
        dir.x = transform.InverseTransformDirection(other.relativeVelocity).x * (smashing ? 0.2f : 1);
        if (!touchingGround) {
            dir.x *= 1.25f;
        }
        dir.x = Mathf.Min(Mathf.Abs(dir.x), 32) * Mathf.Sign(dir.x) * 1.5f;

        bool onTop = false;

        for (int i = 0; i < 5; i++) {
            float direction = -1.6f + (i % 2) * 2.9f;
            Vector3 downward = transform.position - transform.up * downLazy * 1.25f + transform.right * 0.15f * Mathf.Ceil(i / 2f) * direction;
            RaycastHit2D hit = Physics2D.Raycast(downward, -transform.up, 0.4f);
            Debug.DrawRay(downward, -transform.up, Color.blue);
            
            if (hit && hit.collider.gameObject.Equals(other.gameObject)) {
                print(hit.collider.name);
                onTop = true;
                break;
            }
        }

        float aboveMultiplyer = (onTop) ? (instantBounceKill ? 15 : 5) : 1;
        aboveMultiplyer *= playerBounceAmplifier;
        dir.y = transform.InverseTransformDirection(other.relativeVelocity).y;
        dir.y = Mathf.Clamp(dir.y, aboveMultiplyer, 50);

        if (onTop) {
            dir.y = Mathf.Max(dir.y, 25);
            print(dir.y);
            sqetch.setAnimatedStretch(1);
            other.transform.GetComponent<playerController>().sqetch.setAnimatedStretch(smashing ? 50 : 2);
            audioManager.instance.Play(softLanding[UnityEngine.Random.Range(0, softLanding.Length - 1)], 1, UnityEngine.Random.Range(0.96f, 1.03f));

            if (smashing) {
                Shake.instance.shake(1, 2.5f);
                other.transform.GetComponent<Rigidbody2D>().velocity = new Vector3(other.transform.GetComponent<Rigidbody2D>().velocity.x, -20); 
            }
        }

        rigid.velocity = new Vector2(rigid.velocity.x, 0);
        bounceDirection += dir;
        
        //juice   
        colParticle.GetComponent<ParticleSystem>().startColor = baseColor;
        if (onTop) {
            colParticle.GetComponent<ParticleSystem>().startSize = 1.5f;
            colParticle.GetComponent<ParticleSystem>().startSpeed = 70f;
        }
        Destroy(colParticle, 0.75f);
        audioManager.instance.Play(softLanding[UnityEngine.Random.Range(0, softLanding.Length - 1)], onTop ? 1 : 0.5f, UnityEngine.Random.Range(0.96f, 1.03f));
    }

    //checks neighboring wave segments and allows player to walk up slopes to some extent
    protected void slopeCheck() {
        float littleHeight = 0.15f;
        float height = -1;

        for (int i = 1; i < 10; i++) {
            Debug.DrawRay(transform.position - transform.up * 0.66f + transform.up * littleHeight * i, transform.right * Mathf.Sign(rigid.velocity.x) * 0.7f, Color.red);
            RaycastHit2D slopeDetect = Physics2D.Raycast(transform.position - transform.up * 0.6f + transform.up * littleHeight * i, transform.right * Mathf.Sign(rigid.velocity.x), 0.7f, groundCheck);
            if (slopeDetect) {
                height = i;
            }
        }
        

        if (height > -1 && height < 9) {
            checkGround();
            transform.position = Vector2.Lerp(transform.position, new Vector2(transform.position.x, transform.position.y + littleHeight * height + 0.4f), Time.deltaTime * 12);
        }
    }

    IEnumerator recovery(float recoveryTime) {
        smashing = false;
        canSmash = false;
        
        vunrabilityFrames = true;
        yield return new WaitForSeconds(recoveryTime);
        vunrabilityFrames = false;

        yield return new WaitForSeconds(recoveryTime);
        GameObject rechargeCircle = Instantiate(shockWave, transform.position, transform.rotation) as GameObject;
        rechargeCircle.transform.SetParent(transform);
        rechargeCircle.GetComponent<SpriteRenderer>().color = changeOpacity(spriteAnim.GetComponent<SpriteRenderer>().color, rechargeCircle.GetComponent<SpriteRenderer>().color.a);
        spriteAnim.GetComponent<SpriteRenderer>().color = changeOpacity(spriteAnim.GetComponent<SpriteRenderer>().color, 225f);
        SmashCooldownTime = 0.25f;
        canSmash = true;
    }

    protected bool alreadyDead;
    public virtual void die() {
        if (!endingUI.instance.endConditionMet) {
            alreadyDead = true;
            audioManager.instance.Play(deathExplosion, 0.5f, UnityEngine.Random.Range(0.96f, 1.04f));

            GameObject particle = Instantiate(deathParticle, transform.position, Quaternion.identity) as GameObject;
            particle.GetComponent<ParticleSystem>().startColor = Color.Lerp(baseColor, Color.white, 0.5f);

            GetComponent<SpriteRenderer>().color = fullColor;
            Shake.instance.shake(2, 3);

            endingUI.instance.StopAllCoroutines();
            endingUI.instance.StartCoroutine(endingUI.instance.checkPlayersLeft());
            Destroy(this.gameObject);
        }

    }

    IEnumerator spawnAfterImages() {
        float distanceTravelled = 0;
        Vector2 deltaPosition = transform.position;
        float AFTERIMAGE_RATE = 1;
        while (true) {
            distanceTravelled += Vector2.Distance(transform.position, deltaPosition);
            deltaPosition = transform.position;
            if (distanceTravelled > AFTERIMAGE_RATE) {
                distanceTravelled = 0;
                SpawnTrail();
            }
            yield return null;
        }
    }

    void SpawnTrail() {
        GameObject trailPart = new GameObject();
        SpriteRenderer trailPartRenderer = trailPart.AddComponent<SpriteRenderer>();
        trailPartRenderer.sprite = spriteAnim.GetComponent<SpriteRenderer>().sprite;
        trailPartRenderer.flipX = spriteAnim.GetComponent<SpriteRenderer>().flipX;

        trailPartRenderer.color = changeOpacity(GetComponent<SpriteRenderer>().color, 0.75f);

        trailPart.transform.position = transform.position;
        trailPart.transform.localScale = transform.localScale;
        Destroy(trailPart, 2f); // replace 0.5f with needed lifeTime

        StartCoroutine("FadeTrailPart", trailPartRenderer);
    }

    IEnumerator FadeTrailPart(SpriteRenderer trailPartRenderer) {
        while (trailPartRenderer != null && trailPartRenderer.color.a > 0) {
            trailPartRenderer.color = changeOpacity(trailPartRenderer.color, trailPartRenderer.color.a - 0.01f);
            //trailPartRenderer.transform.localScale = trailPartRenderer.transform.localScale - Vector3.one * 0.2f;
            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator checkIfWaved() {
        canMakeWave = false;
        yield return new WaitForSeconds(1.5f); 
        canMakeWave = true;
    }

    public Color changeOpacity(Color color, float opacity) {
        color.a = opacity;
        return color;
    }

    public void setColors(Color baseColor) {
        spriteAnim.GetComponent<SpriteRenderer>().color = baseColor;
        this.baseColor = baseColor;
        chargeParticle.startColor = Color.Lerp(baseColor, Color.white, 0.5f);
    }
 
    private void OnDrawGizmos()
    {
        
        for (int i = 0; i < 5; i++)
        {
            float direction = -1.6f + (i % 2) * 2.9f;
            Vector3 downward = transform.position - transform.up * 0.33f * 1.25f + transform.right * 0.15f * Mathf.Ceil(i / 2f) * direction;
            Debug.DrawRay(downward, -transform.up, Color.blue);
        }
        //Gizmos.color = Color.red;
        //Gizmos.DrawWireCube(transform.position + Vector3.right * transform.GetComponent<playerController>().topWidth / 2, new Vector2(0.1f,1));
        //Gizmos.DrawWireCube(transform.position - Vector3.right * transform.GetComponent<playerController>().topWidth / 2, new Vector2(0.1f, 1));

        //Vector2 angleFromCenter = (transform.position - centerOfGravity.position).normalized;
        //Debug.DrawLine(transform.position, transform.position + downLazy / 1.25f * transform.up, Color.green);
        //Debug.DrawLine(transform.position, transform.position - downLazy / 1.25f * transform.up, Color.blue);
        //Debug.DrawRay(transform.position, angleFromCenter * 5, Color.green);
    }
}

