using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : BaseCounter, IInteractable
{
    public Animator anim;

    [Header("子弹")]
    public FoodSO bulletPrefab;
    public Bullet nowBullet;
    public GameObject nowBulletObject;
    public GameObject originBulletObject;
    public int poolSize;
    public int bulletNumber;
    private Queue<GameObject> pool = new Queue<GameObject>();
    private Queue<GameObject> bullets = new Queue<GameObject>();

    [Header("炮塔属性")]
    public GameObject Mouth;
    public float detectRadius;
    public float attackInterval;
    private Vector3 target;
    private float attackIntervalCounter;
    private bool haveTarget;
    private bool canAttack;

    protected override void Awake()
    {
        base.Awake();
        canAttack = true;
        attackIntervalCounter = attackInterval;
        anim = GetComponent<Animator>();
    }

    protected override void Update()
    {
        base.Update();
        AttackInterval();
        FindEnemy();
        SetAnimation();
    }

    public void OnConfirmAction(PlayerInteract player)
    {
        CounterInteract(player);
    }
    public override void CounterInteract(PlayerInteract player)
    {
        ObjectInteract(player);
    }

    public void OnSpecialAction(PlayerInteract player)
    {
        SetSubject(player);
    }

    public override void ObjectInteract(IObjectParent player)
    {
        this.player = player;
        if (player.ReturnNowObject1() != null && player.ReturnNowObject1().GetComponent<Bullet>() != null && nowBullet == null)
        {
            GetObject();
        }
    }

    public override void GetObject()
    {
        if (player.ReturnNowObject1().GetComponent<Bullet>() && player.ReturnNowObject1().GetComponent<Bullet>().hasUsed == true)
            return;

        nowObject = player.ReturnNowObject1();
        nowObject.SetParent(this);
        player.ClearNowObject();

        nowBullet = nowObject.GetComponent<Bullet>();
        nowBulletObject = nowObject.gameObject;
        nowBullet.FitOut(this);
        nowBulletObject.SetActive(false);

        CreateBullet(nowBulletObject);
    }

    private void SetAnimation()
    {
        anim.SetBool("haveBullet",bullets.Count!= 0);
    }

    #region 攻击功能

    private void CreateBullet(GameObject bullet)
    {
        if(bullet.GetComponent<FireNumberUp>() != null)
        {
            bulletNumber += bullet.GetComponent<FireNumberUp>().addNumber;
        }

        for (int i = 0;i < bulletNumber; i++)
        {
            GameObject newBullet = Instantiate(bullet,transform.position,Quaternion.identity);
            bullets.Enqueue(newBullet);
            newBullet.SetActive(false);
        }
    }

    private GameObject TakeOutBullet()
    {
        if(bullets.Count != 0)
        {
            GameObject obj = bullets.Dequeue();
            obj.SetActive(true);
            return obj;
        }
        return null;
    }


    private void AttackInterval()
    {
        if (!canAttack)
        {

        }
        if (attackIntervalCounter > 0)
        {
            attackIntervalCounter -= Time.deltaTime;
        }
        else
        {
            attackIntervalCounter = 0;
            attackIntervalCounter = attackInterval;
            if (haveTarget && bullets.Count != 0)
            {
                anim.SetTrigger("attack");
                //Fire();
            }
        }
    }

    private void FindEnemy()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, detectRadius);
        haveTarget = false;
        Vector3 closestEnemyPosition = Vector3.positiveInfinity;

        foreach (Collider2D collider in colliders)
        {
            if (collider.GetComponent<Enemy>() != null)
            {
                haveTarget = true;
                float distance = Vector3.Distance(Mouth.transform.position, collider.transform.position);
                if (distance < Vector3.Distance(Mouth.transform.position, closestEnemyPosition))
                {
                    closestEnemyPosition = collider.transform.position;
                }
            }
        }

        if (haveTarget && target != Vector3.positiveInfinity)
        {
            target = closestEnemyPosition;
        }
    }

    public void Fire()
    {
        Vector2 castDir = (Vector2)(target - Mouth.transform.position).normalized;
        GameObject bullet = TakeOutBullet();
        //GameObject bullet = GetFromPool();
        bullet.transform.position = Mouth.transform.position;
        bullet.transform.right = castDir;
        if (castDir.x < -0)
        {
            Vector3 theScale = bullet.transform.localScale;
            theScale.y *= -1;
            bullet.transform.localScale = theScale;
        }
        bullet.GetComponent<Bullet>().Cast(castDir);
        canAttack = false;

        if(bullets.Count == 0)
        {
            ClearNowObject();
            nowBullet = null;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, detectRadius);
    }
    #endregion

    #region 子弹对象池
    private void FillPool(GameObject poolObject)
    {
        originBulletObject = poolObject;
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(poolObject);
            nowObject.SetParent(this);
            obj.SetActive(false);
            pool.Enqueue(obj);
        }
    }

    public GameObject GetFromPool()
    {
        if (pool.Count > 0)
        {
            GameObject obj = pool.Dequeue();
            obj.SetActive(true);
            return obj;
        }
        else
        {
            GameObject obj = Instantiate(nowBulletObject);
            obj.SetActive(true);
            return obj;
        }
    }

    public void ReturnObject(GameObject obj)
    {
        obj = originBulletObject;
        obj.SetActive(false);
        pool.Enqueue(obj);
    }

    public void ClearPool()
    {
        while (pool.Count > 0)
        {
            GameObject obj = pool.Dequeue();
            Destroy(obj);
        }
    }
    #endregion

    
}
