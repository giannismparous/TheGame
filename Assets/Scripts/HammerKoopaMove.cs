using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HammerKoopaMove : MonoBehaviour
{

    public float dirX;
    public float moveSpeedValue;
    private float moveSpeed;
    private Rigidbody2D rb;
    private bool facingRight = false;
    private Vector3 parentLocalScale;
    private float colliderTimer;
    public Transform groundCheck;
    public LayerMask groundLayer;
    public float pauseDuration;
    private float timer;
    public float rechargingDuration;
    private float rechargingTimer;
    private float fallRadius;
    private Animator anim;
    private bool attack;
    private bool recharging;
    public GameObject hammerPrefab;
    public float AttackThrowXUpper;
    public float AttackThrowXLower;
    public float AttackThrowYUpper;
    public float AttackThrowYLower;
    private System.Random rand;
    private bool attackNow;

    void Start()
    {
        parentLocalScale = transform.parent.gameObject.transform.localScale;
        rb = transform.parent.GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        moveSpeed = moveSpeedValue;
        colliderTimer = 0;
        timer = 0;
        fallRadius = 0.1f;
        attack = false;
        recharging = false;
        rechargingTimer = 0;
        rand = new System.Random();
        attackNow = true;
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

        if (!attack)
        {
            timer -= Time.deltaTime;
            if (rb.velocity.y == 0 && !Physics2D.OverlapCircle(groundCheck.position, fallRadius, groundLayer))
            {
                dirX *= -1;
                timer = pauseDuration;
                rb.velocity = new Vector2(0, rb.velocity.y);
            }

            if (timer <= 0)
            {
                rb.velocity = new Vector2(dirX * moveSpeed, rb.velocity.y);
                fallRadius = 0.1f;
                anim.SetBool("isRunning", true);
            }
            else
            {
                fallRadius = 100f;
                anim.SetBool("isRunning", false);
            }
        }
        else {
            rechargingTimer -= Time.deltaTime;
            if (recharging && rechargingTimer <= 0)
            {
                recharging = false;
                attackNow = true;
            }
        }
        colliderTimer -= Time.deltaTime;
        
    }

    void LateUpdate()
    {
        if (timer<=0 && !recharging)CheckWhereToFace();
    }

    void CheckWhereToFace()
    {

        if (!attack)
        {
            if (dirX > 0)
                facingRight = true;
            else if (dirX < 0)
                facingRight = false;
        }
        else
        {
            if (dirX > 0)
                facingRight = false;
            else if (dirX < 0)
                facingRight = true;
        }

        if (((facingRight) && (parentLocalScale.x < 0)) || ((!facingRight) && (parentLocalScale.x > 0)))
            parentLocalScale.x *= -1;

        transform.parent.localScale = parentLocalScale;
    }

    void ThrowHammer() {
        GameObject tempHammer = Instantiate(hammerPrefab) as GameObject;
        Transform attackPosTransform = transform.parent.Find("AttackPosition").transform;
        tempHammer.transform.position = new Vector2(attackPosTransform.position.x, attackPosTransform.position.y);
        float tempX = (float)(rand.NextDouble() * (AttackThrowXUpper - AttackThrowXLower) + AttackThrowXLower);
        float tempY = (float)(rand.NextDouble() * (AttackThrowYUpper - AttackThrowYLower) + AttackThrowYLower);
        if (facingRight)tempHammer.GetComponent<Rigidbody2D>().velocity = new Vector2(-tempX, tempY);
        else tempHammer.GetComponent<Rigidbody2D>().velocity = new Vector2(tempX, tempY);
    }

    public void Attack() {
        rb.velocity = new Vector2(0, rb.velocity.y);
        attack = true;
        anim.SetBool("isRunning", false);
        timer = 0; //No idea
        if (!recharging)
        {
            if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime>=1 || attackNow)
            {
                anim.SetBool("isAttacking", true);
                anim.SetBool("isRecharging", false);
                attackNow = false;
            }
            else if (anim.GetCurrentAnimatorStateInfo(0).length <= anim.GetCurrentAnimatorStateInfo(0).normalizedTime)
            {
                anim.SetBool("isAttacking", false);
                anim.SetBool("isRecharging", true);
                recharging = true;
                rechargingTimer = rechargingDuration;
                ThrowHammer();
            }
        }
    }

    public void StopAttack()
    {
        
        if (rechargingTimer <= 0)
        {
            attack = false;
            recharging = false;
            anim.SetBool("isAttacking", false);
            anim.SetBool("isRecharging", false);
        }
    }


    public void SetDirX(float dirX)
    {
        this.dirX= dirX;
    }
}