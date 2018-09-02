using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackEffect : MonoBehaviour {

    CameraEffect camEffect;
    private float chargeAmt;
    PlayerControl playerControl;

    public float cameraKnockDistance = 0.04f;
    public float cameraKnockDuration = 0.2f;
    public float enemyKnockMult = 13f;

    void Awake () {
        playerControl = transform.parent.GetComponent<PlayerControl>();
        camEffect = Camera.main.GetComponent<CameraEffect>();
	}

    public void SetChargeAmount(float amt) {
        chargeAmt = amt;
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Enemy" || other.tag == "KnockedEnemy") {
            playerControl.killer = true;
            float direction = transform.parent.localScale.x;
            camEffect.CameraKnock(cameraKnockDuration, cameraKnockDistance, new Vector3(direction, 0f, 0f));
            Vector2 force = new Vector2(enemyKnockMult* 10 * chargeAmt * direction, 0f);
            other.gameObject.GetComponent<Enemy>().KnockBack(force);
            //playerControl.Invoke("StopDash", 0.2f);
            playerControl.StopDash();
        }
    }




}
