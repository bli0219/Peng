using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour {

    public static EnemyManager Instance;
    public GameObject enemyPrefab;
    public GameObject levelObject;
    float enemyDistance = 0.3f;
    //List<Enemy> enemies;
    Level[] levels;

    void Awake() {
        Instance = this;
        levels = levelObject.GetComponentsInChildren<Level>();
        for (int i = 0; i != levels.Length; i++) {
            levels[i].mass = 0.8f + 0.3f * i;
            levels[i].enemySpeed = 0.3f + 0.05f * i;
        }
    }

    void Start() {
        
    }

    public void ActivateLevels() {
        levelObject.SetActive(true);
    }

    public void ResetEnemies() {
        foreach(Level l in levels) {
            l.ResetEnemies();
        }
    }
}
