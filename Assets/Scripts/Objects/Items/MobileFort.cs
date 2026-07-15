using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobileFort : MonoBehaviour
{
    public Rigidbody2D rb;
    public GameObject centerPoint;
    public GameObject player;

    public float followDistance;
    public float followSpeed;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        FollowPlayer();

        PassFortInfo();
    }

    private void FollowPlayer()
    {
        float distance = Vector3.Distance(transform.position, GameManager.instance.playerPosUpdate);
        if (distance > followDistance && GameManager.instance.inWave)
        {
            Vector3 nextPosition = Vector3.MoveTowards(transform.position, GameManager.instance.playerPosUpdate, followSpeed * Time.deltaTime);
            transform.position = nextPosition;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position,followDistance);
    }

    private void PassFortInfo()
    {
        GameManager.instance.fortTranform = transform;
        GameManager.instance.fortFollowDistance = followDistance;
    }
}
