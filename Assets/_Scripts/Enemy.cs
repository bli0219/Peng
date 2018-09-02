using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    public GameObject exclamation;
    Animator anim;
    Rigidbody2D rb;
    SpriteRenderer sr;
    Sprite sprite;
    float knockbackTime = 0f;
    float runSpeed = 0f;
    bool angry = false;
    bool dead = false;
    Vector3 startPos;

    void Awake() {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    public void GetAngry(float speed) {
        exclamation.SetActive(true);
        runSpeed = speed;
        Invoke("StartMove", 0.4f);
    }

    void StartMove() {
        exclamation.SetActive(false);
        angry = true;
    }

    void Start() {
        sprite = sr.sprite;
        startPos = transform.position;
    }

    void Update() {
        if (angry && !dead) {
            rb.velocity = new Vector2(-1 * runSpeed, 0f);
        }    
    }

    public void KnockBack(Vector2 force) {
        if (Time.time - knockbackTime > 0.8f) {
            knockbackTime = Time.time;
            rb.AddForce(force);
        }
    }

    void Die() {
        if (dead) {
            return;
        }
        if (!angry) {
            //EnemyManager.Instance.WakeUpEnemies();
            FindObjectOfType<PlayerControl>().killer = true;
        }
        gameObject.tag = "KnockedEnemy";
        anim.SetBool("Attacked", true);
        TaskManager.Instance.OneMore();
        StartCoroutine("EliminationEffect");
        angry = false;
        dead = true;
    }

    void OnCollision(Collision2D collision) {
        GameObject go = collision.collider.gameObject;
        if (go.tag == "KnockedEnemy") {
            if (collision.relativeVelocity.magnitude > 1.2f) {
                Die();
            }
        } else if (go.tag == "Player") {
            if (!dead && angry) {
                go.GetComponent<PlayerControl>().Die();
            } 
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "AttackArea") {
            Die();
        }
    }

    void OnCollisionStay2D(Collision2D collision) {
        OnCollision(collision);
    }

    void OnCollisionEnter2D(Collision2D collision) {
        OnCollision(collision);
    }

    public void Reborn() {
        gameObject.SetActive(true);
        sr.sprite = sprite;

        transform.position = startPos;
        rb.velocity = Vector2.zero;
        angry = false;
        dead = false;
    }

    IEnumerator EliminationEffect() {
        yield return new WaitForSeconds(0.4f);
        anim.SetBool("Attacked", false);
        gameObject.SetActive(false);
    }
}
