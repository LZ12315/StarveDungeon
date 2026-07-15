using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelFortuneCounter : TurretCounter
{
    [Header("制作台属性")]
    public List<GameObject> foods = new List<GameObject>();

    public override void GetObject()
    {
        if (player.ReturnNowObject1() != null)
        {
            nowObject = player.ReturnNowObject1();
            player.ClearNowObject();
            Destroy(nowObject.gameObject);
            GameObject newFood = Instantiate(foods[Random.Range(0, foods.Count)]);
            nowObject = newFood.GetComponent<Object>();
            nowObject.SetParent(this);
        }
        else if (player.ReturnNowObject1() == null && player.ReturnNowObject2() != null)
        {
            nowObject = player.ReturnNowObject2();
            nowObject.SetParent(this);
            player.ClearNowObject();
        }

        PropertyUpdate();
    }

    protected override void PropertyUpdate()
    {
        nowObject.gameObject.SetActive(false);

        if (Properties.Count < storageQuantity)
        {
            Properties.Add(new Property(nowObject, nowObject.GetComponent<IFoodFunction>(), nowObject.GetComponent<IFoodNature>()));
            canTake = false;
        }
        if (Properties.Count >= storageQuantity)
        {
            StartCoroutine(CookBullet());
        }
    }
}
