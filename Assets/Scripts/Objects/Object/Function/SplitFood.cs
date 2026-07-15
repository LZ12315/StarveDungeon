using System;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;

public class SplitFood : MonoBehaviour,IFoodFunction
{
    private Rigidbody2D rb;
    public int splitNumber;
    public float splitWaitTime;
    public float splitAngle;
    private Object nowObject;

    private void Awake()
    {
        nowObject = GetComponent<Object>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        
    }

    public void OnFoodFunction(Object thisObject)
    {
        StartCoroutine(Split(thisObject));
    }

    private IEnumerator Split(Object thisObject)
    {
        Vector2 originalDirection = rb.velocity.normalized;
        float originalVelocityMagnitude = rb.velocity.magnitude;

        yield return new WaitForSeconds(splitWaitTime);

        float startAngle = (-splitAngle * (splitNumber / 2));

        for (int i = 0; i < splitNumber; i++)
        {
            float angle = startAngle + (splitAngle * 2 * i) + (splitNumber % 2 == 0 ? 0 : splitAngle * 0.5f);
            Quaternion rotation = Quaternion.Euler(0f, 0f, angle);

            GameObject splitedBullet = Instantiate(thisObject.gameObject, transform.position,transform.rotation * rotation);

            Vector2 newDirection = rotation * originalDirection;
            splitedBullet.GetComponent<Rigidbody2D>().velocity = newDirection * originalVelocityMagnitude;
            splitedBullet.GetComponent<Object>().isFly = true;
            splitedBullet.GetComponent<Object>().TimeCounter = thisObject.TimeCounter;
        }
    }
}
