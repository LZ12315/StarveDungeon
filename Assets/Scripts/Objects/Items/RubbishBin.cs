using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubbishBin : BaseCounter,IInteractable
{

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

        if (nowObject == null && player.ReturnNowObject1() != null)
        {
            GetObject();
            nowObject.CookingDestroy();
            //player.ClearNowObject();
        }
        else if (player.ReturnNowObject1() == null)
        {
            Debug.Log("You had to have something on hand");
        }
    }
}
