using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fortress : MonoBehaviour
{
    void Start()
    {
        GameManager.instance.fortTranform = transform;
    }

    void Update()
    {
        GameManager.instance.fortTranform = transform;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerInteract>().canUseReward = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerInteract>().canUseReward = false;
        }
    }
}
