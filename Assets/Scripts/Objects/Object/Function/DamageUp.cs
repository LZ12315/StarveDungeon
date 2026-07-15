using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageUp : MonoBehaviour,IFoodNature
{
    public float damageUpValue = 5f;

    public void OnFoodNature(Object thisObject)
    {
        thisObject.GetComponent<ObjectAttack>().damage += damageUpValue;
    }
}
