using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Rapart : MonoBehaviour
{
    public float maxHealth;
    public float currentHealth;
    public UnityEvent<Rapart> OnHealthChange;
    public VoidEventSO gameOverEvent;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(EnemyAttack attacker)
    {
        if (attacker.damage < this.currentHealth)
        {
            currentHealth -= attacker.damage;
        }
        else
        {
            currentHealth = 0;
            gameOverEvent.RaiseEvent();
        }
        GameUIManager.instance.RapartHealthChange(this);
    }

}
