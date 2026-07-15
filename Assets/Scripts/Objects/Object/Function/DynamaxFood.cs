using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamaxFood : MonoBehaviour,IFoodNature
{
    public float maxiCoeffcient;
    public float waitTime;
    public float flySlowCoeffcient;

    public void OnFoodNature(Object thisObject)
    {
        StartCoroutine(Dynamax(thisObject));
    }

    private IEnumerator Dynamax(Object thisObject)
    {
        yield return new WaitForSeconds(waitTime);
        transform.localScale *= maxiCoeffcient;
        thisObject.flySpeed *= flySlowCoeffcient;
    }
}
