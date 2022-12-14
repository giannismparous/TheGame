using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CondorBomberBulletMove : MonoBehaviour
{

    private float dirX;
    private float moveSpeed;
    public float moveSpeedValue;
    private Rigidbody2D rb;
    private bool facingRight = false;
    private Vector3 localScale;
    public MarioMove marioMove;
    public float KBForce;
    private float destructionTimer;
    public float destructionTimerValue;

    void Start()
    {
        localScale = transform.transform.localScale;
        rb = transform.GetComponent<Rigidbody2D>();
        moveSpeed = moveSpeedValue;
        destructionTimer = destructionTimerValue;
        GameObject player = GameObject.Find("Mario");
        marioMove = player.transform.Find("MarioBody").GetComponent<MarioMove>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.transform.position.x <= transform.position.x) marioMove.RegisterHitKnockback(KBForce, true);
            else marioMove.RegisterHitKnockback(KBForce, false);
            marioMove.TakeDamage(10);
            Destroy(transform.gameObject);
        }
    }

    void FixedUpdate()
    {
        rb.velocity = new Vector2(dirX * moveSpeed, rb.velocity.y);

        destructionTimer -= Time.deltaTime;
        if (destructionTimer<=0) Destroy(transform.gameObject);
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

        if (((facingRight) && (localScale.x < 0)) || ((!facingRight) && (localScale.x > 0)))
            localScale.x *= -1;

        transform.localScale = localScale;
    }

    public void SetDirX(float dirX) { 
        this.dirX=dirX;
    }

}