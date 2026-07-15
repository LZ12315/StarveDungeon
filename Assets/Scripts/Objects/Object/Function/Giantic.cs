using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Giantic : MonoBehaviour, IFoodNature
{
    public float gianticNumber = 1.25f;
        
    public void OnFoodNature(Object thisObject)
    {
        thisObject.transform.localScale *= gianticNumber;
    }
}
