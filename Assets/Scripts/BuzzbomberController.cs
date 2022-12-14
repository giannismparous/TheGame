using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Pathfinding;
using EZCameraShake;

public class BuzzbomberController : MonoBehaviour
{

    private float distance;
    private Rigidbody2D rb;
    private Animator anim;
    private Transform marioBodyTransform;
    private AIPath aiPath;
    private bool isChasing;
    public int dirX;
    public float moveSpeedValue1;
    public float moveSpeedValue2;
    public float distanceValue;
    public MarioMove marioMove;


    void Start()
    {
        rb = transform.parent.GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        GameObject player = GameObject.Find("Mario");
        marioBodyTransform = player.transform.Find("MarioBody").transform;
        marioMove = player.transform.Find("MarioBody").GetComponent<MarioMove>();
        aiPath = transform.parent.GetComponent<AIPath>();
        aiPath.enabled = false;
        transform.parent.GetComponent<AIDestinationSetter>().target = marioBodyTransform;
        isChasing = false;
    }

    void FixedUpdate()
    {

        distance = (float)Math.Sqrt(Math.Pow(marioBodyTransform.position.x - transform.parent.transform.position.x, 2) + Math.Pow(marioBodyTransform.position.y - transform.parent.transform.position.y, 2));

        if (!isChasing) {
            if (distance > distanceValue) {

                rb.velocity = new Vector2(dirX * moveSpeedValue1, rb.velocity.y);
            }
            else
            {
                anim.SetBool("isChasing", true);
                isChasing = true;
                aiPath.enabled = true;
                aiPath.maxSpeed = moveSpeedValue2;
            }
        }

    }

    void LateUpdate()
    {

        if (isChasing)
        {
            if (aiPath.desiredVelocity.x > 0)
            {
                transform.parent.transform.localScale = new Vector2(-1f, 1f);
            }
            else
            {
                transform.parent.transform.localScale = new Vector2(1f, 1f);
            }
        }
        else {
            if (dirX>0)transform.parent.transform.localScale = new Vector2(-1f, 1f);
            else transform.parent.transform.localScale = new Vector2(1f, 1f);
        }

    }

}