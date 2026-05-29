using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Unit : MonoBehaviour
{
    public int hpSlider;

    public string unitName;

    public int damage;
    public int heal;
    public int cook;

    public int maxHP;
    public int currentHP;

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

}
