using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlicedFood : Object
{
    //public float bulletHP;

    protected override void Awake()
    {
        base.Awake();
        bulletHP = availableFrequency;
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if(isFly)
        {
            bulletHP--;
            if (bulletHP <= 1)
            {
                Destroy(gameObject);
            }
        }
    }
}
