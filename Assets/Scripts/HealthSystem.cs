using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public event EventHandler OnDead;
    public event EventHandler OnDamage;
    private UnitBase unitBase;
    private double  maxHealth;

    private void Start()
    {
        unitBase = GetComponent<UnitBase>( );

        maxHealth = unitBase.HP;
    }

    public void Damage(int damageAmount)
    {
        unitBase.HP -= damageAmount;
        Debug.Log( unitBase.HP );
        if ( unitBase.HP < 0)
        {
            unitBase.HP = 0;
        }

        OnDamage?.Invoke(this, EventArgs.Empty);

        if ( unitBase.HP == 0)
        {
            Die();
        }

        Debug.Log( unitBase.HP );
    }

    private void Die()
    {
        OnDead?.Invoke(this, EventArgs.Empty);
    }

    public double GetHealthNormalized()
    {
        return unitBase.HP / maxHealth;
    }

    public double getHealth( ) {
        return unitBase.HP;
    }

    public double getMaxHealth( ) {
        return maxHealth;
    }
}
