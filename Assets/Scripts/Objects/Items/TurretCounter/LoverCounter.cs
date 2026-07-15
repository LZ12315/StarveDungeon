using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class LoverCounter : TurretCounter
{
    [Header("制作台特性")]
    public int createBulletNumber = 2;
    public int createBulletCounter = 2;

    protected override void Awake()
    {
        base.Awake();
        createBulletCounter = createBulletNumber;
    }

    private void Initialize()
    {
        createBulletCounter = createBulletNumber;
    }

    protected override IEnumerator CookBullet()
    {
        CreateNewBullet();
        nowBulletObject.SetActive(false);

        foreach (var property in Properties)
        {
            if (property.function != null)
            {
                IFoodFunction newFunction = ComponentCopier.CopyComponent<IFoodFunction>(property.food.gameObject, nowBulletObject);
            }
            if (property.nature != null)
            {
                IFoodNature newNature = ComponentCopier.CopyComponent<IFoodNature>(property.food.gameObject, nowBulletObject);
            }
        }
        progressBarUI.gameObject.SetActive(true);
        isCook = true;

        yield return new WaitForSeconds(cookDuration);

        isCook = false;
        canTake = true;
        createBulletCounter--;
        cookTimer = 0;
        progressPercent = 0;
        progressBarUI.gameObject.SetActive(false);
        nowBulletObject.SetActive(true);
    }

    protected override void TakeAwayObject()
    {
        nowBullet.SetParent(player);
        ClearNowObject();
        nowBulletObject = null;
        nowBullet = null;
        if(createBulletCounter > 0)
        {
            StartCoroutine(CookBullet());
        }
        else
        {
            Properties.Clear();
        }
    }
}
