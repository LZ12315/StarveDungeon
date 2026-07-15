using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using static Enum;

public class EnemyPatrolState : ChaseEnemyBaseState
{
    public override void OnEnter(ChaseEnemy enemy)
    {
        currentEnemy = enemy;
        currentEnemy.currentSpeed = currentEnemy.normalSpeed;
        currentEnemy.checkDir = new Vector2(1, 0);
    }

    public override void LogicUpdate()
    {
        if (currentEnemy.foundPlayer)
        {
            currentEnemy.SwitchState(EnemyState.Chase);
        }
    }

    public override void PhysicsUpdate()
    {

    }

    public override void OnExit()
    {

    }

}
