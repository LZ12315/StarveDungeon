using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedUp : MonoBehaviour,IFoodNature
{
    public float SpeedUpValue = 20;

    public void OnFoodNature(Object thisObject)
    {
        thisObject.flySpeed += SpeedUpValue;
    }
}
