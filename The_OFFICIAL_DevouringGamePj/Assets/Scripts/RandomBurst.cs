using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomBurst : MonoBehaviour
{
    public IEnumerator RandomBurstAttack(Transform playerTarget)
    {
        // 1. Determine a random number of hits for this "burst"
        int hitCount = Random.Range(2, 5); // 2 to 4 hits

        for (int i = 0; i < hitCount; i++)
        {
            // 2. Perform the individual hit logic
            Debug.Log($"Hit {i + 1} of {hitCount}!");

            // Apply damage or trigger an animation here
            // Example: playerTarget.GetComponent<PlayerStats>().TakeDamage(5);

            // 3. Wait for a short duration between hits
            yield return new WaitForSeconds(0.2f);
        }

        // 4. End the enemy's turn once the burst is complete
        // GameController.Instance.EndEnemyTurn();
    }
}
