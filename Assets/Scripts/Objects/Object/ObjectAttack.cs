using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectAttack : Attack
{
    private Enemy enemy;
    public Object thisObject;

    protected override void Awake()
    {
        base.Awake();
        thisObject = GetComponent<Object>();
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Enemy>() != null)
        {
            other.GetComponent<Enemy>().TakeDamage(this);
        }
    }
}
