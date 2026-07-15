using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public PlayerInputControl playerInput;
    public PlayerInteract playerInteract;
    public Character character;
    public Rigidbody2D rb;
    public Animator anim;

    [Header("移动属性")]
    public float normalSpeed;
    public float currentspeed;
    public Vector2 inputDir;
    public Vector2 faceDir;

    [Header("数值传递")]
    public float passLate;
    private float passTimer;

    [Header("物品投掷")]
    public Object nowObject;
    public Queue<Object> bullets = new Queue<Object>();
    public float attackInterval = 1f;
    public int castNumber = 10;
    public Transform fireMouth;
    private float attackTimer;
    private bool canAttack;
    private Vector2 mousePos;
    private Vector2 castDir;

    [Header("冲刺")]
    public bool canDash;
    public bool isDash;
    public float dashForce;
    public float dashTime;
    public float dashCD;
    public float checkDistance;
    public LayerMask rapartLayer;

    [Header("受伤")]
    public bool isHurt;
    public float hurtForce;

    [Header("事件监听")]
    public SceneLoadEvent sceneLoadEvent;
    public VoidEventSO afterSceneLoadEvent;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerInput = new PlayerInputControl();
        playerInteract = GetComponent<PlayerInteract>();
        character = GetComponent<Character>();
        anim = GetComponent<Animator>();
        currentspeed = normalSpeed;
    }

    private void OnEnable()
    {
        playerInput.Enable();
        playerInput.GamePlay.Fire.started += OnCast;
        playerInput.GamePlay.Dash.started += OnDash;
        sceneLoadEvent.LoadRequestEvent += OnLoadEvent;
        afterSceneLoadEvent.OnEventRaised += OnAfterLoadEvent;
        canDash = true;
    }

    private void OnDisable()
    {
        playerInput.Disable();
        playerInput.GamePlay.Fire.started -= OnCast;
        playerInput.GamePlay.Dash.started -= OnDash;
        sceneLoadEvent.LoadRequestEvent -= OnLoadEvent;
        afterSceneLoadEvent.OnEventRaised -= OnAfterLoadEvent;
    }

    private void Update()
    {
        inputDir = playerInput.GamePlay.Move.ReadValue<Vector2>();
        OnAim();
        PassPlayerInfo();
        TurnAround();
        AttackInterval();
        DashWallCheck();
    }

    private void FixedUpdate()
    {
        if (!isHurt && !isDash)
        {
            Move();
        }
    }

    #region 移动相关

    private void TurnAround()
    {
        if (inputDir.x < 0)
        {
            transform.rotation = new Quaternion(0, 180, 0, 0);
            faceDir.x = 1f;
        }
        if (inputDir.x > 0)
        {
            transform.rotation = new Quaternion(0, 0, 0, 0);
            faceDir.x = -1f;
        }
    }

    private void Move()
    {
        rb.velocity = new Vector2(inputDir.x * currentspeed * Time.deltaTime, inputDir.y * currentspeed * Time.deltaTime);
    }

    #endregion

    #region 攻击相关

    private void OnAim()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);//获取鼠标位置
        castDir = new Vector2(mousePos.x - transform.position.x,mousePos.y - transform.position.y).normalized;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(mousePos, 1f);
        Gizmos.DrawWireSphere(castDir * hurtForce, 1f);
    }

    private void OnCast(InputAction.CallbackContext context)
    {
        if (bullets.Count != 0 && canAttack)
        {
            anim.SetTrigger("attack");
            canAttack = false;
        }
    }

    public void Cast()
    {
        playerInteract.nowObject1.GetComponent<Bullet>().hasUsed = true;
        Object newBullet = bullets.Dequeue();
        newBullet.gameObject.SetActive(true);

        newBullet.transform.position = fireMouth.position;
        newBullet.transform.right = castDir;
        if(castDir.x < -0)
        {
            Vector3 theScale = newBullet.transform.localScale;
            theScale.y *= -1;
            newBullet.transform.localScale = theScale;
        }

        newBullet.Cast(castDir);
        if (bullets.Count == 0)
        {
            bullets.Clear();
            Destroy(playerInteract.nowBullet.gameObject);
            playerInteract.nowBullet = null;
            playerInteract.ClearNowObject();
        }
        //if (playerInteract.nowObject1 != null)
        //{
        //    this.nowObject = playerInteract.nowObject1;
        //    nowObject.transform.position = transform.position;
        //    nowObject.Cast(castDir);
        //    this.nowObject = null;
        //}
        //else if (playerInteract.nowObject2 != null)
        //{
        //    this.nowObject = playerInteract.nowObject2;
        //    nowObject.transform.position = transform.position;
        //    nowObject.Cast(castDir);
        //    this.nowObject = null;
        //}
    }

    private void AttackInterval()
    {
        if(!canAttack)
        {
            attackTimer += Time.deltaTime;
            if(attackTimer >= attackInterval)
            {
                attackTimer = 0;
                canAttack = true;
            }
        }

        if(bullets.Count != 0 && bullets.Count != castNumber)
        {
        }
    }

    public void Reloading(Object bullet)
    {
        for(int i = 0; i < castNumber; i++)
        {
            Object obj = Instantiate(bullet);
            obj.gameObject.SetActive(false);
            bullets.Enqueue(obj);
        }
    }

    #endregion

    #region 冲刺相关

    private void OnDash(InputAction.CallbackContext context)
    {
        if (canDash)
        {
            StartCoroutine(Dash());
        }
    }

    private IEnumerator Dash()
    {
        canDash = false;
        isDash = true;
        float formalGravity = rb.gravityScale;
        rb.gravityScale = 0;
        rb.AddForce(inputDir * dashForce, ForceMode2D.Impulse);
        //StartCoroutine(character.DashInvulnerable(dashTime));


        yield return new WaitForSeconds(dashTime);
        rb.velocity = Vector2.zero;
        rb.gravityScale = formalGravity;
        isDash = false;

        yield return new WaitForSeconds(dashCD);
        canDash = true;
    }

    private void DashWallCheck()
    {
        if (Physics2D.Raycast(transform.position, faceDir, checkDistance, rapartLayer))
        {
            rb.velocity = Vector2.zero;
            this.GetComponent<Collider2D>().enabled = true;
        }
    }

    #endregion

    #region 其他

    private void PassPlayerInfo()
    {
        passTimer += Time.deltaTime;
        if(passTimer >= passLate)
        {
            GameManager.instance.playerPosLate = transform.position;
            passTimer = 0;
        }

        GameManager.instance.playerPosUpdate = transform.position;
        GameManager.instance.playerTransform = transform;
        GameManager.instance.playerMaxHealth = character.maxHealth;
        GameManager.instance.playerCurrentHealth = character.currentHealth;
    }

    public void GetHurt()
    {
        isHurt = true;
        rb.velocity = Vector2.zero;
        Vector2 getHurtDir = new Vector2(transform.position.x - character.attackerPos.x, transform.position.y - character.attackerPos.y).normalized;
        rb.AddForce(getHurtDir * hurtForce, ForceMode2D.Impulse);
    }

    private void OnLoadEvent(GameSceneSO arg0, Vector3 arg1, bool arg2)
    {
        playerInput.GamePlay.Disable();
    }

    private void OnAfterLoadEvent()
    {
        playerInput.GamePlay.Enable();
    }

    #endregion
}
