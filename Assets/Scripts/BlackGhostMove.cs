using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BlackGhostMove : MonoBehaviour
{

    private float dirX;
    private float moveSpeed;
    private float distance;
    private Rigidbody2D rb;
    private Animator anim;
    private bool facingRight = false;
    private Vector3 parentLocalScale;
    private Transform marioBodyTransform;
    private float spawnGhostTimer;
    public float distanceValue;
    public float moveSpeedValue;
    public float spawnGhostTimerValue;

    void Start()
    {
        parentLocalScale = transform.parent.gameObject.transform.localScale;
        rb = transform.parent.GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        dirX = -1f;
        moveSpeed = 0;
        GameObject player = GameObject.Find("Mario");
        marioBodyTransform = player.transform.Find("MarioBody").transform;
    }

    void Update() {
        if (distance <= distanceValue) anim.SetBool("isMoving", true);
        else anim.SetBool("isMoving",false);
    }

    void FixedUpdate()
    {

        distance = (float)Math.Sqrt(Math.Pow(marioBodyTransform.position.x - transform.parent.transform.position.x, 2) + Math.Pow(marioBodyTransform.position.y - transform.parent.transform.position.y, 2));

        if (distance <= distanceValue)
        {
            if (marioBodyTransform.position.x <= transform.position.x) dirX = -1;
            else dirX = 1;

            moveSpeed = moveSpeedValue;

            if (spawnGhostTimer <= 0)
            {
                spawnGhostTimer = spawnGhostTimerValue;
            }
            else spawnGhostTimer -= Time.deltaTime;
        }
        else
        {
            moveSpeed = 0;
            spawnGhostTimer = spawnGhostTimerValue;
        }

        rb.velocity = new Vector2(dirX * moveSpeed, rb.velocity.y);

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