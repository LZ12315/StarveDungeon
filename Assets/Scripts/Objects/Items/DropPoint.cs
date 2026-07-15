using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropPoint : BaseCounter,IInteractable,IObjectParent,IRewardSubject
{
    public Rigidbody2D rb;
    public FoodSO foodSO;

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

    #region Ω±¿¯œ‡πÿ

    public void ComeNewReward(IReward reward)
    {
        throw new System.NotImplementedException();
    }

    public void UseReward()
    {
        throw new System.NotImplementedException();
    }

    public Vector3 ReturnRewardPoint()
    {
        throw new System.NotImplementedException();
    }

    public IReward ReturnNowReward()
    {
        throw new System.NotImplementedException();
    }

    public void ClearNowReward()
    {
        throw new System.NotImplementedException();
    }

    public Transform ReturnSubjectTransform()
    {
        return transform;
    }

    #endregion
}
