using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;
using static UnityEngine.Rendering.DebugUI;

public class BurstAttackController : MonoBehaviour
{
    // Define your burst parameters
    [Header("Burst Settings")]
    public float timeBetweenAttacks = 0.2f;
    public int minBurstCount = 1;
    public int maxBurstCount = 3;

    public int attackA = 10;
    public int attackB = 15;
    public int attackC = 20;
    public int attackD = 0;

    public bool hasAttacked = false;

    public bool isEnemyTurn = true;

    public BattleState2 state;


    // Array of methods or attack logic you want to randomize
    public delegate void AttackMethod();
    public AttackMethod[] attacks;

    void Start()
    {
        // Populate our array with the specific attack methods
        attacks = new AttackMethod[]
        {
            AttackA,
            AttackB,
            AttackC,
            AttackD,
        };
    }

    void Update()
    {
        // Example: Trigger the burst attack when pressing Space
        if(isEnemyTurn == true)
        {
            state = BattleState2.ENEMYTURN;
            TriggerRandomBurst();
        }
    }

    public void TriggerRandomBurst()
    {
        // Start the coroutine that handles the burst logic over time
        StartCoroutine(PerformBurstRoutine());
    }
    public IEnumerator PerformBurstRoutine()
    {
        // Randomize how many attacks will happen in this burst
        int burstCount = Random.Range(minBurstCount, maxBurstCount + 1);

        for (int i = 0; i < burstCount; i++)
        {
            // Pick a random index and call the corresponding method
            int randomIndex = Random.Range(0, attacks.Length);
            attacks[randomIndex]?.Invoke();

            // Wait before the next attack in the burst
            yield return new WaitForSeconds(timeBetweenAttacks);
        }
    }

    // --- Specific Attack Methods ---
    public void AttackA()
    {
        if (state == BattleState2.ENEMYTURN)
        {
            Debug.Log("Executing Attack A!");
            // Add your damage/animation/projectile logic here
            int attacKA = 10;
            hasAttacked = true;
        }
        else
        {
            Debug.Log("Cannot execute Attack A, it's not the enemy's turn!");
            int attackB = 10;
            hasAttacked = false;
            return;
        } 
    }

    public void AttackB()
    {
        if(state == BattleState2.ENEMYTURN) 
        {
            Debug.Log("Executing Attack B!");
            int attackB = 15;
            hasAttacked = true;
        }

        else
        {
            Debug.Log("Cannot execute Attack B, it's not the enemy's turn!");
            int attackB = 15;
            hasAttacked = false;
            return;
        }
    }

    public void AttackC()
    {
        if (state == BattleState2.ENEMYTURN)
        {
            Debug.Log("Executing Attack C!");
            int attackC = 20;
            hasAttacked = true;
        }

        else
        {
            Debug.Log("Cannot execute Attack C, it's not the enemy's turn!");
            int attackC = 20;
            hasAttacked = false;
            return;
        }
    }

    public void AttackD()
    {
        if (state == BattleState2.ENEMYTURN)
        {
            Debug.Log("Executing Attack D!");
            int attackC = 0;
            hasAttacked = true;
        }

        else
        {
            Debug.Log("Cannot execute Attack D, it's not the enemy's turn!");
            int attackC = 0;
            hasAttacked = false;
            return;
        }  
    }
}

