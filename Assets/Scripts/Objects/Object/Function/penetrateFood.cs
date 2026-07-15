using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class penetrateFood : MonoBehaviour,IFoodFunction
{
    public void OnFoodFunction(Object thisObject)
    {
        thisObject.GetComponent<Object>().bulletHP = 99;
    }
}
