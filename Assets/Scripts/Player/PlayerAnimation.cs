using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    public Animator anim;
    public Rigidbody2D rb;
    public int foodHoldNumber;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        SetAnimate();
    }

    public void HurtAnim()
    {
        anim.SetTrigger("GetHurt");
    }

    private void SetAnimate()
    {
        anim.SetFloat("velocity", Mathf.Abs(rb.velocity.x) + Mathf.Abs(rb.velocity.y));
        anim.SetInteger("foodHoldNumber",foodHoldNumber);
    }
}
