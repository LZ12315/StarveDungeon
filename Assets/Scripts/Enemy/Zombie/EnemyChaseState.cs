using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using static Enum;

public class EnemyChaseState : ChaseEnemyBaseState
{

    public override void OnEnter(ChaseEnemy enemy)
    {
        currentEnemy = enemy;
        enemy.currentSpeed = enemy.chaseSpeed;
    }

    public override void LogicUpdate()
    {
        //currentEnemy.LockOn();
        if(!currentEnemy.foundPlayer)
        {
            currentEnemy.SwitchState(EnemyState.Patrol);
        }
    }

    public override void PhysicsUpdate()
    {

    }

    public override void OnExit()
    {

    }

}
