using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodBox : BaseCounter,IInteractable,IObjectParent
{
    public bool canCreate;
    public float createInterval;
    public float createTimer; 

    public FoodSO foodSO;

    protected override void Awake()
    {
        base.Awake();
        canCreate = true;
    }

    protected override void Update()
    {
        base.Update();

        if(createTimer > 0)
        {
            createTimer -= Time.deltaTime;
            if(createTimer <= 0)
            {
                canCreate = true;
                createTimer = 0;
            }
        }
    }

    public void OnConfirmAction(PlayerInteract player)
    {
        CounterInteract(player);
    }

    public void OnSpecialAction(PlayerInteract player)
    {
        SetSubject(player);
    }

    public override void CounterInteract(PlayerInteract player)
    {
        ObjectInteract(player);
    }

    public override void ObjectInteract(IObjectParent player)
    {
        this.player = player;

        if (nowObject == null)
        {
            CreateObject();
        }
        else if (nowObject != null && (player.ReturnNowObject1() == null || player.ReturnNowObject2() == null))
        {
            TakeAwayObject();
        }
        else if (nowObject == null && (player.ReturnNowObject1() != null || player.ReturnNowObject2() != null))
        {
            GetObject();
        }
        else if (nowObject != null && player.ReturnNowObject1() != null && player.ReturnNowObject2() != null)
        {
            Debug.Log("You have something on hand");
        }
    }

    private void CreateObject()
    {
        if(canCreate)
        {
            GameObject newFoodPrefab = Instantiate(foodSO.prefab, objectPoint.position, Quaternion.identity);

            nowObject = newFoodPrefab.GetComponent<Object>();
            nowObject.SetParent(this);
            nowObject.transform.position = objectPoint.position;

            canCreate = false;
            createTimer = createInterval;
        }
    }
}
