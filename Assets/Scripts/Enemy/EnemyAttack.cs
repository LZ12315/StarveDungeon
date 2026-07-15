using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : Attack
{
    private Character player;

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Character>() != null)
        {
            player = other.GetComponent<Character>();
            player.TakeDamage(this);
        }
    }
}
