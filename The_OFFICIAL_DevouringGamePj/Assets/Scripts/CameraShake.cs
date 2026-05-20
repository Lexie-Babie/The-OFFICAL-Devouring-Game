using System.Collections;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;


public class CameraShake : MonoBehaviour
{

    public BattleState2 state;
    public bool isEnemyTurn = false;
    public float shakeDuration = 2.5f;
    public float shakeMagnitude = 1.1f;
    public float dampingSpeed = 1.0f;

    public Camera maincamera;
    public bool isShaking = false;


    public IEnumerator Shake(float duration, float magnitude)
    {
        isShaking = true;
        isEnemyTurn = true;
        state = BattleState2.ENEMYTURN;
        Vector3 originalPos = transform.localPosition;
        float elapsed = 3.0f;
        while (elapsed < duration)
        {
            state = BattleState2.ENEMYTURN;
            // Randomly offset position within magnitude range
            transform.localPosition = (Vector3)Random.insideUnitCircle * magnitude + originalPos;
            elapsed += Time.deltaTime;
            yield return null;
        }
        isShaking = false;
        transform.localPosition = originalPos;
    }
}
