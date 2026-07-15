using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPhysicsCheck : MonoBehaviour
{
    public CapsuleCollider2D capsule;
    public Enemy enemy;

    [Header("¼ì²â²ÎÊý")]
    public bool isWallUp;
    public bool isWallDown;
    public float checkRadius;
    public LayerMask wallLayer;

    [Header("Æ«²îÐÞÕý")]
    public Vector2 upOffset;
    public Vector2 downOffset;
    int time = 0;

    private void Awake()
    {
        enemy = GetComponent<Enemy>();
    }

    private void Update()
    {
        //CheckWall();
    }

    public void CheckWall()
    {
        isWallUp = Physics2D.OverlapCircle((Vector2)transform.position + upOffset, checkRadius,wallLayer);
        isWallDown = Physics2D.OverlapCircle((Vector2)transform.position -  downOffset, checkRadius,wallLayer);
        
        if ((isWallUp||isWallDown)&&(time<1))
        {
            //enemy.TurnBack();
            Debug.Log("isWall");
            time++;
        }
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawSphere((Vector2)transform.position + upOffset, checkRadius);
        Gizmos.DrawSphere((Vector2)transform.position - downOffset, checkRadius);
    }

}
