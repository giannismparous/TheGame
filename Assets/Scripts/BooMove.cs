using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BooMove : MonoBehaviour
{

    private float dirX;
    private float moveSpeed;
    private Rigidbody2D rb;
    private bool facingRight = false;
    private Vector3 parentLocalScale;
    private Animator anim;
    public MarioMove marioMove;
    public float moveSpeedValue;


    void Start()
    {
        parentLocalScale = transform.parent.gameObject.transform.localScale;
        rb = transform.parent.GetComponent<Rigidbody2D>();
        dirX = -1f;
        moveSpeed = moveSpeedValue;
        anim = GetComponent<Animator>();
        GameObject player = GameObject.Find("Mario");
        marioMove = player.transform.Find("MarioBody").GetComponent<MarioMove>();
    }

    void FixedUpdate()
    {

        if ((marioMove.GetFacingRight() && marioMove.transform.position.x < rb.position.x) || (!marioMove.GetFacingRight() && marioMove.transform.position.x > rb.position.x)) anim.SetBool("stopped", true);
        else
        {
            if (marioMove.transform.position.x < rb.position.x) dirX = -1;
            else if (marioMove.transform.position.x > rb.position.x) dirX = 1;
            anim.SetBool("stopped", false);
            rb.transform.position = Vector2.MoveTowards(rb.transform.position, marioMove.transform.position, moveSpeed * Time.deltaTime);
        }

    }

    void LateUpdate()
    {
        CheckWhereToFace();
    }

    void CheckWhereToFace()
    {
        if (dirX > 0)
            facingRight = true;
        else if (dirX < 0)
            facingRight = false;

        if (((facingRight) && (parentLocalScale.x < 0)) || ((!facingRight) && (parentLocalScale.x > 0)))
            parentLocalScale.x *= -1;

        transform.parent.localScale = parentLocalScale;
    }

}