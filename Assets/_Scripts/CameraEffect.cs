using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraEffect : MonoBehaviour {

    public float shakeLength = 0.1f;
    public float shakeDistance = 0.05f;
    public float shakeRate = 0.01f;
    private bool shaking = false;

    private void Update() {
        if (Input.GetKeyDown(KeyCode.T)) {
            CameraShake(shakeLength, shakeDistance);
        }
    }

    public void CameraKnock(float duration, float distance, Vector3 direction) {
        shaking = true;
        transform.position = transform.position += distance * direction;
        StartCoroutine("LerpBack", duration);
    }

    IEnumerator LerpBack(float duration) {
        float t = 1f;
        while (t > 0) {
            t -= Time.deltaTime / duration;
            transform.position = Vector3.Lerp(transform.parent.position, transform.position, t);
            yield return 0;
        }
    }

    public void CameraShake(float length, float distance) {
        //shakeDistance = distance;
        InvokeRepeating("ShakeOnce", 0, shakeRate);
        Invoke("StopShake", length);
    }

    void ShakeOnce() {
        if (shakeDistance > 0) {
            Vector3 camPos = gameObject.transform.position;

            float offsetX = Random.value * shakeDistance * 2 - shakeDistance;
            float offsetY = Random.value * shakeDistance * 2 - shakeDistance;
            camPos.x += offsetX;
            camPos.y += offsetY;

            transform.position = camPos;
        }
    }

    void StopShake() {
        CancelInvoke("ShakeOnce");
        transform.localPosition = Vector3.zero;
    }
}
