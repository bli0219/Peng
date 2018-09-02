using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour {

    public GameObject player;
    public float enemySpeed = 0.3f;
    public float mass;
    PlayerControl playerCtrl;
    Enemy[] enemies;
    bool angry = false;

    void Start () {
        enemies = GetComponentsInChildren<Enemy>();
        foreach(Enemy e in enemies) {
            e.GetComponent<Rigidbody2D>().mass = mass;
        }
        playerCtrl = player.GetComponent<PlayerControl>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Mathf.Abs(transform.position.x - player.transform.position.x)<1f) {
            if (playerCtrl.killer && !angry) {
                WakeUpEnemies();
                angry = true;
                Debug.Log(transform.position.x - player.transform.position.x);
            }
        }
	}

    public void WakeUpEnemies() {
        foreach(Enemy e in enemies) {
            e.GetAngry(enemySpeed);   
        }
    }

    public void ResetEnemies() {
        angry = false;
        foreach (Enemy e in enemies) {
            e.Reborn();
        }
    }
}
