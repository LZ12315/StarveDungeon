using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropPoint : BaseCounter,IInteractable,IObjectParent,IRewardSubject
{
    public Rigidbody2D rb;
    public FoodSO foodSO;
    public Transform rewardPoint;
    public IReward nowReward;

    protected override void Awake()
    {
        base.Awake();
    }

    public void OnConfirmAction(PlayerInteract player)
    {
        ObjectInteract(player);
    }

    public void OnSpecialAction(PlayerInteract player)
    {
    }

    public override void ObjectInteract(IObjectParent player)
    {
        this.player = player;

        if (nowObject != null && player.ReturnNowObject1() == null)
        {
            TakeAwayObject();
        }
        else if (nowObject == null && player.ReturnNowObject1() != null)
        {
            GetObject();
        }
        else if (nowObject != null && player.ReturnNowObject1() != null)
        {
            Debug.Log("You have something on hand");
        }
    }

    protected override void TakeAwayObject()
    {
        base.TakeAwayObject();
        Destroy(gameObject);
    }

    //public void Place()
    //{
    //    GameObject newFoodPrefab = Instantiate(foodSO.prefab, transform.position, Quaternion.identity);

    //    nowObject = newFoodPrefab.GetComponent<Object>();
    //    nowObject.transform.position = transform.position;
    //}

    #region Rewards

    public void ComeNewReward(IReward reward)
    {
        nowReward = reward;
    }

    public void UseReward()
    {
        if (nowReward == null)
        {
            return;
        }

        nowReward.RewardApply();
        nowReward.ClearNowSubject();
        ClearNowReward();
    }

    public Vector3 ReturnRewardPoint()
    {
        return rewardPoint != null ? rewardPoint.position : transform.position;
    }

    public IReward ReturnNowReward()
    {
        return nowReward;
    }

    public void ClearNowReward()
    {
        nowReward = null;
    }

    public Transform ReturnSubjectTransform()
    {
        return transform;
    }

    #endregion
}
