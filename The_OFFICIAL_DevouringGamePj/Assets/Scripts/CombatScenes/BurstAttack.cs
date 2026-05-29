using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class BurstAttack : MonoBehaviour
{
    [Header("Burst Settings")]
    public int burstCount = 3;             // Number of hits in the burst
    public float timeBetweenHits = 0.2f;   // Pause between each individual hit
    public float postBurstDelay = 1.0f;    // Pause after the entire burst finishes

    Unit playerUnit;

    public IEnumerator ExecuteBurstAttack(Unit playerUnit)
    {
        // 1. Perform the initial telegraph/wind-up (e.g., play animation)
        Debug.Log("Enemy winds up for a burst attack!");
        yield return new WaitForSeconds(0.5f);

        // 2. Loop through the burst attack count
        for (int i = 0; i < burstCount; i++)
        {
            // Calculate damage & apply to the target
            int damage = CalculateBurstDamage();
            playerUnit.TakeDamage(damage);

            Debug.Log($"Hit {i + 1}! Dealt {damage} damage.");

            // Wait briefly before the next strike in the burst
            yield return new WaitForSeconds(timeBetweenHits);
        }

        // 3. Wait after the burst completes before passing the turn back
        yield return new WaitForSeconds(postBurstDelay);

        Debug.Log("Burst attack finished. Turn passing over.");
 
    }

    private int CalculateBurstDamage()
    {
        // Example: Base damage logic
        return Random.Range(5, 10);
    }
}

