using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public Character character;

    [Header("¹¥»÷ÊôÐÔ")]
    public float damage;
    public string BeingAttack;

    protected virtual void Awake()
    {

    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    { 
    }

}
