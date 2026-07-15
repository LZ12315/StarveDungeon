using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PristessCounter : TurretCounter
{
    [Header("制作台属性")]
    public float debuffValue = 0.8f;

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
        DeBuff();

        progressBarUI.gameObject.SetActive(true);
        isCook = true;

        yield return new WaitForSeconds(cookDuration);

        isCook = false;
        progressBarUI.gameObject.SetActive(false);
        cookTimer = 0;
        progressPercent = 0;
        canTake = true;
        nowBulletObject.SetActive(true);
        Properties.Clear();
    }

    private void DeBuff()
    {
        nowBullet.flySpeed *= debuffValue;
        nowBullet.GetComponent<ObjectAttack>().damage *= debuffValue;
        nowBulletObject.transform.localScale *= debuffValue;
    }
}
