using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskManager : MonoBehaviour {

    public static TaskManager Instance;
    public GameObject player;
    public Text notification;
    public GameObject startPanel;
    public GameObject winPanel;
    public GameObject restartPanel;
    PlayerControl playerCtrl;
    int left = 100;
    Dictionary<int, string> linesForKills = new Dictionary<int, string> {
        {1, "剩余: 1"},
        {2, "剩余: 2"},
        {3, "剩余: 3"},
        {4, "剩余: 4"},
        {5, "剩余: 5"},
        {6, "剩余: 6"},
        {7, "剩余: 7"},
        {8, "剩余: 8"},
        {9, "剩余: 9"},
        {10, "剩余: 10"},
        {20, "剩余: 20"},
        {30, "剩余: 30"},
        {40, "剩余: 40"},
        {50, "剩余: 50"},
        {60, "剩余: 60"},
        {70, "剩余: 70"},
        {80, "剩余: 80"},
        {90, "剩余: 90"},
    };

    void Awake() {
        Instance = this;
        playerCtrl = player.GetComponent<PlayerControl>();
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            StartCoroutine("DisplayText", "按 Alt + F4");
        }
    }

    public void GameStart() {
        startPanel.SetActive(false);
        player.SetActive(true);
        EnemyManager.Instance.ActivateLevels();
        playerCtrl.PlayerReady();
    }

    public void Quit() {
        Application.Quit();
    }

    public void OneMore() {
        left--;
        if (linesForKills.ContainsKey(left )) {
            StartCoroutine("DisplayText", linesForKills[left]);
        }
    }

    IEnumerator DisplayText(string words) {
        notification.text = words;
        notification.gameObject.SetActive(true);
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(1f);
        Time.timeScale = 1f;
        notification.text = "";
        notification.gameObject.SetActive(false);
    }

    public void PlayerDie() {
        if (left == 1) {
            winPanel.SetActive(true);
        } else {
            restartPanel.SetActive(true);
        }
    }

    public void RestartGame() {
        restartPanel.SetActive(false);
        winPanel.SetActive(false);
        left = 100;
        playerCtrl.Reborn();
        EnemyManager.Instance.ResetEnemies();
        Time.timeScale = 1f;
    }
}
