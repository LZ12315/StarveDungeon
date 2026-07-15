using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Character : MonoBehaviour
{
    public PlayerController playerControl;

    [Header("角色属性")]
    public float maxHealth;
    public float currentHealth;

    [Header("受伤")]
    public bool injureInvulnerable;
    public bool dashInvulnerable;
    public float injureInvulnerableTime;
    public Vector2 attackerPos;

    [Header("事件")]
    public UnityEvent<Character> OnHealthChange;
    public UnityEvent OnTakeDamage;

    private void Awake()
    {
        currentHealth = maxHealth;
        playerControl = GetComponent<PlayerController>();
    }

    public void TakeDamage(Attack attacker)
    {
        attackerPos = attacker.transform.position;

        if (injureInvulnerable || dashInvulnerable)
            return;
        if (attacker.damage < this.currentHealth)
        {
            currentHealth -= attacker.damage;
            OnTakeDamage?.Invoke();
            StartCoroutine(InjureInvulnerable());
        }
        else
        {
            currentHealth = 0;
            Destroy(gameObject);
        }

        OnHealthChange?.Invoke(this);
    }

    public IEnumerator InjureInvulnerable()
    {
        injureInvulnerable = true;
        yield return new WaitForSeconds(injureInvulnerableTime);
        injureInvulnerable = false;
    }

    public IEnumerator DashInvulnerable(float dashTime)
    {
        dashInvulnerable = true;
        this.GetComponent<Collider2D>().enabled = false;
        yield return new WaitForSeconds(2*dashTime);
        this.GetComponent<Collider2D>().enabled = true;
        dashInvulnerable = false;
    }
}
