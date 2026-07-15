using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ChaseEnemyBaseState
{
    public ChaseEnemy currentEnemy;

    public abstract void OnEnter(ChaseEnemy enemy);
    public abstract void LogicUpdate();
    public abstract void PhysicsUpdate();
    public abstract void OnExit();
}
