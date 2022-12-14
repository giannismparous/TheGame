using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SpinyMove : MonoBehaviour
{

    private float dirX;
    private float moveSpeed;
    private Rigidbody2D rb;
    private Animator anim;
    private bool facingRight = false;
    private Vector3 parentLocalScale;
    private float colliderTimer;
    private bool landed;
    

    void Start()
    {
        parentLocalScale = transform.parent.gameObject.transform.localScale;
        rb = transform.parent.GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        dirX = -1f;
        moveSpeed = 3f;
        colliderTimer = 0;
        landed = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (landed &&colliderTimer<=0 && (collision.CompareTag("Obstacle") || collision.CompareTag("Enemy")))
        {
            colliderTimer = 0.2f;
            dirX *= -1f;
        }
    }

    void FixedUpdate()
    {
        if (!landed && rb.velocity.y == 0)
        {
            anim.SetTrigger("landed");
            landed = true;
        }

        rb.velocity = new Vector2(dirX * moveSpeed, rb.velocity.y);

        colliderTimer -= Time.deltaTime;
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