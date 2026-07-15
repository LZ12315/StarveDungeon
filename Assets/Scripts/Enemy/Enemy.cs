using System.Collections;
using System.Collections.Generic;
using System.IO.Pipes;
using UnityEngine;
using UnityEngine.Events;
//using static Enum;

public class Enemy : MonoBehaviour
{
    public Rigidbody2D rb;
    public Animator anim;
    public PlayerSO playerSO;

    [Header("基本属性")]
    public float maxHealth;
    public float currentHealth;

    [Header("移动属性")]
    public float normalSpeed;
    public float currentSpeed;
    protected Enemy currentEnemy;

    [Header("避障")]
    public float minDistance;
    public bool isAvoid;
    public Vector3 newDirection;
    public bool stop;

    [Header("攻击")]
    public float attackInterval;
    private float attackCounter;
    private bool canAttack;

    [Header("受伤")]
    public bool injureInvulnerable;
    public float injureInvulnerableTime;
    public Vector2 attackerPos;
    public float transformHealthPercent = 0.5f;
    private bool isDead;

    [Header("检测")]
    public Transform target;
    public Vector2 checkDir;
    public float faceDir;

    [Header("事件")]
    public UnityEvent OnTakeDamage;


    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
        currentSpeed = normalSpeed;
    }

    protected virtual void OnEnable()
    {
        checkDir.x = 1f;
        target = FindTarget();
    }

    protected virtual void Update()
    {
        Animation();
        AttackInterval();
    }

    protected virtual void FixedUpdate()
    {
        ObstacleAvoid();
        Move();
        RapartCheck();
    }

    protected virtual void OnDisable()
    {
        
    }

    private void RapartCheck()
    {
        stop = false;
        RaycastHit2D[] hitInfo;
        hitInfo = Physics2D.RaycastAll((Vector2)transform.position, (Vector2)checkDir,2f);
        foreach(RaycastHit2D hit in hitInfo)
        {
            if (hit.collider.GetComponentInParent<Rapart>())
            {
                stop = true;
                if (canAttack)
                {
                    hit.collider.GetComponentInParent<Rapart>().TakeDamage(this.GetComponent<EnemyAttack>());
                    canAttack = false;
                }
            }
        }
    }

    private void AttackInterval()
    {
        attackCounter -= Time.deltaTime;
        if (attackCounter <= 0)
        {
            canAttack = true;
            attackCounter = attackInterval;
        }
    }

    public void TakeDamage(Attack attacker)
    {
        attackerPos = attacker.transform.position;
        if (attacker.damage < this.currentHealth)
        {
            currentHealth -= attacker.damage;
            OnTakeDamage?.Invoke();
        }
        else
        {
            currentHealth = 0;
            Destroy(gameObject);
            EnemyDie();
            isDead = true;
        }
    }

    private void EnemyDie()
    {
        if (!isDead)
        {
            int index = 0;
            foreach (GameObject otherEnemy in GameManager.instance.alivedEnemies)
            {
                if (this.gameObject == otherEnemy)
                {
                    index = GameManager.instance.alivedEnemies.IndexOf(otherEnemy);
                }
            }
            if (index >= 0 && index < GameManager.instance.alivedEnemies.Count)
            {
                GameManager.instance.alivedEnemies.RemoveAt(index);
            }
            WaveManager.instance.EnemiesKilled();
        }
    }

    #region 移动相关
    private void Move()
    {
        if (!stop)
        {
            if (!isAvoid)
                rb.velocity = new Vector2(checkDir.x * currentSpeed * Time.fixedDeltaTime, checkDir.y * currentSpeed * Time.fixedDeltaTime);
            if (isAvoid)
                rb.velocity = new Vector2(newDirection.x * currentSpeed * Time.fixedDeltaTime, newDirection.y * currentSpeed * Time.fixedDeltaTime);
        }
    }

    private Transform FindTarget()
    {
        float minDistance = 100;
        Transform targetTransform = GameManager.instance.playerTransform;
        GameObject[] targetWall = GameObject.FindGameObjectsWithTag("Rapart");
        foreach(var rapart in targetWall)
        {
            float distance = Vector3.Distance(transform.position,rapart.transform.position);
            if(distance < minDistance)
            {
                minDistance = distance;
                targetTransform = rapart.transform;
            }
        }
        return targetTransform;
    }

    public void LockOn()
    {
        checkDir =  new Vector2(target.position.x - transform.position.x, target.position.y - transform.position.y).normalized;

        if(checkDir.x < 0)
        {
            transform.rotation = new Quaternion(0, 180, 0, 0);
            faceDir = 1f;
        }
        if (checkDir.x > 0)
        {
            transform.rotation = new Quaternion(0, 0, 0, 0);
            faceDir = -1f;
        }
        //transform.localScale = new Vector3(faceDir, 1, 1);
    }

    public void Animation()
    {
        anim.SetBool("transform", currentHealth/maxHealth <= transformHealthPercent);
    }

    private void ObstacleAvoid()
    {
        Vector3 totalAvoidanceDir = Vector3.zero;
        int avoidanceCount = 0;

        foreach (GameObject otherEnemy in GameManager.instance.alivedEnemies)
        {
            float distanceToOthers = Vector3.Distance(transform.position, otherEnemy.transform.position);
 
            if(distanceToOthers < minDistance && this.gameObject != otherEnemy)
            {
                newDirection = (transform.position - otherEnemy.transform.position).normalized;
                totalAvoidanceDir += newDirection;
                avoidanceCount++;
            }
        }
        if(avoidanceCount > 0)
        {
            newDirection = (totalAvoidanceDir / avoidanceCount).normalized;
            isAvoid = true;
        }
        else
        {
            isAvoid = false;
        }
    }

    #endregion
}
