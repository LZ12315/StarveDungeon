using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCounter : MonoBehaviour,IObjectParent,IReward
{
    public GameObject selectedSprite;
    public IObjectParent player;
    public PlayerInteract playerInteract;

    [Header("桌台设置")]
    public Object nowObject;
    public FoodSO nowObjectSO;
    public Transform objectPoint;

    [Header("奖励设置")]
    public IRewardSubject nowSubject;

    protected virtual void Awake()
    {
    }

    private void Start()
    {
    }

    protected virtual void Update()
    {
        //if(!GameManager.instance.inWave && nowSubject != null)
        //{
        //    FollowWithSubject();
        //}

        if (nowSubject != null)
        {
            FollowWithSubject();
        }
    }

    protected virtual void TakeAwayObject()
    {
        nowObject.SetParent(player);
        ClearNowObject();
        nowObject = null;
    }

    public void ClearNowObject()
    {
        if (nowObject != null)
        {
            nowObject = null;
        }
    }

    public void ComeNewObject(Object newFood)
    {
        if (nowObject == null)
        {
            nowObject = newFood;
        }
    }

    public virtual void ObjectInteract(IObjectParent player)
    {

    }

    public virtual void GetObject()
    {
        if(player.ReturnNowObject1() != null)
        {
            nowObject = player.ReturnNowObject1();
            nowObject.SetParent(this);
            player.ClearNowObject();
        }
        else if(player.ReturnNowObject1() == null && player.ReturnNowObject2() != null)
        {
            nowObject = player.ReturnNowObject2();
            nowObject.SetParent(this);
            player.ClearNowObject();
        }
    }

    public virtual void CounterInteract(PlayerInteract player)
    {
        if (GameManager.instance.inWave)
        {
            ObjectInteract(player);
        }
        //if (!GameManager.instance.inWave)
        //{
        //    DestroyOtherReward();
        //    SetSubject(player);
        //}
    }

    /// <summary>
    /// 从此开始都是不会被用于物体交互的函数，主要负责返回值和其他功能
    /// </summary>
    /// <returns></returns>

    #region 桌台交互

    public Vector3 ReturnHoldPlace1()
    {
        return objectPoint.position;
    }

    public Object ReturnNowObject1()
    {
        return nowObject;
    }

    public Vector3 ReturnHoldPlace2()
    {
        return Vector3.zero;
    }

    public Object ReturnNowObject2()
    {
        return null;
    }

    #endregion

    #region 奖励交互

    //public void DestroyOtherReward()
    //{
    //    if (!GameManager.instance.inPrepare)
    //    {
    //        foreach (var reward in WaveManager.instance.waves[WaveManager.instance.currentWaveCount].spawnedRewards)
    //        {
    //            if(this.gameObject == reward)
    //            {
    //                WaveManager.instance.DestroyOtherRewards(this.gameObject);
    //            }
    //        }
    //    }
    //}

    public void SetSubject(IRewardSubject rewardSubject)
    {
        nowSubject = rewardSubject;
        rewardSubject.ComeNewReward(this);
    }

    private void FollowWithSubject()
    {
        if (nowSubject != null)
        {
            this.GetComponent<Collider2D>().enabled = false;
            transform.position = nowSubject.ReturnRewardPoint();
        }
    }

    public void ClearNowSubject()
    {
        nowSubject = null;
    }

    public void RewardApply()
    {
        this.gameObject.transform.SetParent(GameManager.instance.fortTranform);
        this.transform.position = nowSubject.ReturnSubjectTransform().position;
        StartCoroutine(TurnOffCollider());
        //this.GetComponent<Collider2D>().enabled = true;
    }

    private IEnumerator TurnOffCollider()
    {
        Collider2D collider = this.gameObject.GetComponent<Collider2D>();
        collider.enabled = false;
        yield return new WaitForSeconds(0.5f);
        collider.enabled = true;
    }

    #endregion

    #region 其他
    public void OnSelected()
    {
        if(selectedSprite != null)
        {
            selectedSprite.SetActive(true);
        }
    }

    public void NotSelected()
    {
        if (selectedSprite != null)
        {
            selectedSprite.SetActive(false);
        }
    }
    #endregion
}
