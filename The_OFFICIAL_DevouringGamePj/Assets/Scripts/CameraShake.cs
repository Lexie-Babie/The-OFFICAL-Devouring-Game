using System.Collections;
using UnityEngine;
using System.Collections.Generic;


public class CameraShake : MonoBehaviour
{

    public BattleState2 state;
    public bool isEnemyTurn = false;
    public float shakeDuration = 2.5f;
    public float shakeMagnitude = 2f;
    public float dampingSpeed = 1.0f;

    public Camera maincamera;
    public bool isShaking = false;

    public void TriggerShake()
    {
        StartCoroutine(Shake());
    }

    private IEnumerator Shake()
    {
        float elapsedTime = 0.0f;
        Vector3 initialPosition = maincamera.transform.localPosition;

        while(elapsedTime < shakeDuration)
        {
            float magnitude = shakeMagnitude * Mathf.Exp(-dampingSpeed * elapsedTime);
            float xOffset = Random.Range(-1f, 1f) * magnitude;
            float yOffset = Random.Range(-1f, 1f) * magnitude;

            maincamera.transform.localPosition = new Vector3(initialPosition.x + xOffset, initialPosition.y + yOffset, initialPosition.z);
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        maincamera.transform.localPosition = initialPosition;
    }

}
