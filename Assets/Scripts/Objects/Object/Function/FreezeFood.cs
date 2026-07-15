using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeFood : MonoBehaviour, IFoodFunction
{
    public void OnFoodFunction(Object thisObject)
    {
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null)
        {
            collision.GetComponent<Enemy>().currentSpeed *= 0.5f;
            Debug.Log("1");
        }
    }
}
