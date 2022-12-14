using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BlastoiseMove : MonoBehaviour
{

    private float moveSpeed;
    private Rigidbody2D rb;
    private Animator anim;
    private bool facingRight = false;
    private Vector3 parentLocalScale;
    private float colliderTimer;
    private bool isAttacking;
    private float waterBallTimer;
    public float dirX;
    public float moveSpeedValue;
    public float waterBallTimerValue;
    public Transform groundCheck;
    public LayerMask groundLayer;
    public Transform marioBodyTransform;
    public GameObject waterBallPrefab;

    void Start()
    {
        parentLocalScale = transform.parent.gameObject.transform.localScale;
        rb = transform.parent.GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        moveSpeed = moveSpeedValue;
        colliderTimer = 0;
        waterBallTimer = 0;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (colliderTimer <= 0 && (collision.CompareTag("Obstacle") || collision.CompareTag("Enemy")))
        {
            colliderTimer = 0.2f;
            dirX *= -1f;
        }
    }

    void FixedUpdate()
    {

        if (!isAttacking && rb.velocity.y == 0 && !Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer)) dirX *= -1;

        rb.velocity = new Vector2(dirX * moveSpeed, rb.velocity.y);

        colliderTimer -= Time.deltaTime;
        waterBallTimer -= Time.deltaTime;

        if (isAttacking && waterBallTimer <= 0) {
            Rigidbody2D temp1 = transform.parent.GetComponent<Rigidbody2D>();
            GameObject temp2 = Instantiate(waterBallPrefab) as GameObject;
            temp2.transform.position = new Vector2(temp1.position.x, temp1.position.y);
            waterBallTimer = waterBallTimerValue;
        }
    }

    void LateUpdate()
    {
        CheckWhereToFace();
    }

    void CheckWhereToFace()
    {
        if (dirX > 0)
            facingRight = false;
        else if (dirX < 0)
            facingRight = true;

        if (((facingRight) && (parentLocalScale.x < 0)) || ((!facingRight) && (parentLocalScale.x > 0)))
            parentLocalScale.x *= -1;

        transform.parent.localScale = parentLocalScale;
    }

    public void InitiateAttack() {
        isAttacking = true;
        moveSpeed = 0;
        anim.SetBool("isAttacking",true);
    }

    public void StopAttack()
    {
        isAttacking = false;
        moveSpeed = moveSpeedValue;
        anim.SetBool("isAttacking",false);
    }
}