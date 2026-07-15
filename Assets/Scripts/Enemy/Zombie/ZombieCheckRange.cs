using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieCheckRange : MonoBehaviour
{
    public ChaseEnemy enemy;

    private void Awake()
    {
        enemy = GetComponentInParent<ChaseEnemy>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        enemy.foundPlayer = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        enemy.foundPlayer = false;
    }
}
