using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicianCounter : TurretCounter
{
    [Header("制作台特性")]
    public VoidEventSO nextWaveSO;
    public int rapidProductNumber;
    private int rapidProductCounter;

    protected override void Awake()
    {
        base.Awake();
        rapidProductCounter = rapidProductNumber;
    }

    private void OnEnable()
    {
        nextWaveSO.OnEventRaised += Initialize;
    }

    private void OnDisable()
    {
        nextWaveSO.OnEventRaised -= Initialize;
    }

    public void Initialize()
    {
        rapidProductCounter = rapidProductNumber;
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
        isCook = true;

        if(rapidProductCounter > 0)
        {
            rapidProductCounter--;
            yield return new WaitForSeconds(0.1f);
        }
        else
        {
            progressBarUI.gameObject.SetActive(true);
            yield return new WaitForSeconds(cookDuration);
            progressBarUI.gameObject.SetActive(false);
        }

        isCook = false;
        cookTimer = 0;
        progressPercent = 0;
        canTake = true;
        nowBulletObject.SetActive(true);
        Properties.Clear();
    }
}
