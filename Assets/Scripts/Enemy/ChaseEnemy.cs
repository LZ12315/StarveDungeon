using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Lifetime;
using UnityEngine;
//using static Enum;

public class ChaseEnemy : Enemy
{

    [Header("状态参数")]
    public ChaseEnemyBaseState currentState;
    public ChaseEnemyBaseState patrolState;
    public ChaseEnemyBaseState chaseState;

    [Header("追击检测")]
    public float chaseSpeed;
    public float checkDistance;
    public bool foundPlayer;

    protected override void Awake()
    {
        base.Awake();
        patrolState = new EnemyPatrolState();
        chaseState = new EnemyChaseState();
    }

    protected override void OnEnable()
    {
        currentState = patrolState;
        base.OnEnable();
        currentState.OnEnter(this);
    }

    protected override void Update()
    {
        base.Update();
        currentState.LogicUpdate();
        LockOn();
        ChaseCheck();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        currentState.PhysicsUpdate();
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        currentState.OnExit();
    }

    public void SwitchState(EnemyState state)
    {
        var newState = state switch
        {
            EnemyState.Patrol => patrolState,
            EnemyState.Chase => chaseState,
            _ => null
        };

        currentState.OnExit();
        currentState = newState;
        currentState.OnEnter(this);
    }

    private void ChaseCheck()
    {
        float distance = Vector3.Distance(transform.position,GameManager.instance.playerPosLate);
        if (distance <= checkDistance)
        {
            foundPlayer = true;
        }
        else
        {
            foundPlayer = false;
        }
    }

}
