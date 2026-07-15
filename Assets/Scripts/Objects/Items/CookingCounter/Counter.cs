using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Counter : BaseCounter, IInteractable,IObjectParent
{
    public Counter secondCounter;

    protected override void Awake()
    {
        base.Awake();
    }

    public void OnConfirmAction(PlayerInteract player)
    {
        CounterInteract(player);
    }

    public void OnSpecialAction(PlayerInteract player)
    {
        ChangeObjectCounter();
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

    public void ChangeObjectCounter()
    {
        if ((nowObject != null) && (secondCounter.nowObject == null))
        {
            nowObject.SetParent(secondCounter);
            nowObject = null;
        }
    }
}
