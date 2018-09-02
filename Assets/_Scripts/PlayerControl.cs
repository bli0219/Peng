using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControl : MonoBehaviour {

    public GameObject atkArea;
    AttackEffect atkEffect;
    Rigidbody2D rb;
    BoxCollider2D col;
    Animator anim;

    float jumpForce = 3f;
    public float speedLimit = 1f;
    float dashDivider = 1f;
    float chargeMax= 1f;
    float chargeMin= 0f;
    float punchLength;
    float time = 0f;
    float chargeTime;
    bool dashing = false;
    bool charging = false;
    bool dead = true;
    Vector3 defaultPos;
    public float dashSpeed;
    public bool ready = false;
    public bool killer = false;

    void Awake() {
        col = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        atkEffect = atkArea.GetComponent<AttackEffect>();
    }
    void Start () {
        defaultPos = transform.position;
	}

    public void PlayerReady() {
        dead = false;
    }

	void Update () {
        
        if (!charging && !dashing && !dead) {
            Move();
        } else if (dashing) {
            rb.velocity = new Vector2(transform.localScale.x * dashSpeed, 0f);
        }

        Punch();
        time += Time.deltaTime;

	}

    void Move() {
        float x = Input.GetAxis("Horizontal");
        anim.SetBool("Running", x!=0f);

        rb.AddForce(new Vector2(x*8f, 0));

        if (x > 0f) {
            transform.localScale = new Vector3(1, 1, 1);
        } else if (x < 0f) {
            transform.localScale = new Vector3(-1, 1, 1);
        }

        if (rb.velocity.x > speedLimit) {
            rb.velocity = new Vector2(speedLimit, rb.velocity.y);
        } else if (rb.velocity.x < -speedLimit) {
            rb.velocity = new Vector2(-speedLimit, rb.velocity.y);
        }
    }


    void Punch() {
        if (dead) {
            return;
        }
        
        if (Input.GetKey(KeyCode.Space) && !dashing && !charging) {
            charging = true;
            chargeTime = Time.time;
            anim.SetBool("Charging", true);
            rb.velocity = Vector2.zero;
        }

        if (Input.GetKeyUp(KeyCode.Space) && charging) {
            charging = false;
            dashing = true;
            float chargeLength = Time.time - chargeTime;
            anim.SetBool("Charging", false);
            StartCoroutine("DashLerp", chargeLength);
        }
    }

    IEnumerator DashLerp(float chargeLength) {
        
        float duration = Mathf.Min(chargeLength / dashDivider, chargeMax);
        duration = Mathf.Max(duration, chargeMin);
        atkEffect.SetChargeAmount(duration);
        atkArea.SetActive(true);
        anim.SetBool("Dashing", true);
        yield return new WaitForSeconds(duration);
        StopDash();
    }

    private void OnCollisionEnter2D(Collision2D collision) {

        if (collision.collider.gameObject.tag == "Hell") {
            Die();
        }
    }


    private void OnCollisionStay2D(Collision2D collision) {
        
    }
    public void StopDash() {
        rb.velocity = Vector2.zero;
        atkArea.SetActive(false);
        dashing = false;
        anim.SetBool("Dashing", false);
    }


    public void Reborn() {
        dead = false;
        killer = false;
        anim.SetBool("Die", false);
        transform.position = defaultPos;
        col.isTrigger = false;
        rb.gravityScale = 1f;
    }

    public void Die() {
        dead = true;
        charging = false;
        dashing = false;
        anim.SetBool("Charging", false);
        anim.SetBool("Dashing", false);
        anim.SetBool("Die", true);

        rb.gravityScale = 0f;
        rb.velocity = Vector2.zero;
        col.isTrigger = true;

        TaskManager.Instance.PlayerDie();
    }
}
