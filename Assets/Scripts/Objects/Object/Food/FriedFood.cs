using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriedFood : Object
{
    [Header("×Óµ¯×·×Ù")]
    public Vector3 controlPoint;
    public Vector3 originPos;
    public Vector3 target;
    public float cuvature;
    public float percent = 0;
    public float percentSpeed = 0;
    public float traceSpeed;
    public float traceRadius;

    private List<Collider2D> attackedEnemy = new List<Collider2D>();
    private bool haveTarget;

    protected override void Awake()
    {
        base.Awake();
        bulletHP = availableFrequency;
    }

    protected override void Update()
    {
        base.Update();

        if (isFly)
        {
            TraceBullet();
        }
    }

    private void TraceBullet()
    {
        if(!haveTarget)
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, traceRadius);
            foreach(var collider in colliders)
            {
                if (collider.GetComponent<Enemy>() != null && !attackedEnemy.Contains(collider))
                {
                    target = collider.transform.position;
                    originPos = transform.position;
                    controlPoint = GetMiddlePosition(originPos, target);
                    percentSpeed = traceSpeed / (target - originPos).magnitude;
                    attackedEnemy.Add(collider);
                    percent = 0;
                    haveTarget = true;
                    break;
                }
            }
        }

        if(haveTarget)
        {
            percent += percentSpeed * Time.deltaTime;
            if (percent > 1)
            {
                percent = 1;
                haveTarget = false;
            }
            transform.position = Bezier(percent,originPos,controlPoint,target);
        }
    }

    private Vector3 GetMiddlePosition(Vector3 p0, Vector3 p2)
    {
        Vector3 m = Vector3.Lerp(p0, p2, 0.3f);
        Vector3 normal = Vector2.Perpendicular(p0 - p2).normalized;
        float rd = Random.Range(-2f, 2f);
        return m + (p0 - p2).magnitude * cuvature * rd * normal;
    }

    private Vector2 Bezier(float t,Vector3 p0,Vector3 p1,Vector3 p2)
    {
        var p12 = Vector3.Lerp(p0, p1, t);
        var p23 = Vector3.Lerp(p1, p2, t);
        return Vector3.Lerp(p12, p23, t);
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        bulletHP--;
        if (bulletHP <= 0 && isFly)
        {
            Destroy(gameObject);
        }
    }
}
