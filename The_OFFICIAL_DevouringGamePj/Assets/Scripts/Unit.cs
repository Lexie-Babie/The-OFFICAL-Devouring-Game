using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static BurstAttackController;

public class Unit : MonoBehaviour
{
    public int hpSlider;

    public string unitName;

    public int damage;
    public int heal;
    public int cook;

    public int maxHP;
    public int currentHP;

    public float timeBetweenAttacks = 0.2f;
    public int minBurstCount = 1;
    public int maxBurstCount = 3;

    public int attackA = 10;
    public int attackB = 15;
    public int attackC = 20;
    public int attackD = 0;


    // Array of methods or attack logic you want to randomize
    public delegate void AttackMethod();
    public AttackMethod[] attacks;



    public bool TakeDamage(int damage)
    {
        currentHP -= damage;

        if (currentHP <= 0)
            return true;
        else
            return false;
    }

    public void Heal(int amount)
    {
        currentHP += amount;
        if (currentHP > maxHP)
            currentHP = maxHP;
    }


    public bool CookDamage(int cook)
    {
        currentHP -= cook;
        if (currentHP <= 0)
            return true;
        else
            return false;
    }

    public void BossAttackA(int amount)
    {
       

    }

    public void BossAttackB(int amount)
    {

    }

    public void BossAttackC(int amount)
    {
    }

}
