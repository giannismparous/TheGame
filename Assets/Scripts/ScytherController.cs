using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Pathfinding;

public class ScytherController : MonoBehaviour
{

    private float distance;
    private Rigidbody2D rb;
    private Animator anim;
    private Transform marioBodyTransform;
    private AIPath aiPath;
    public float moveSpeedValue1;
    public float moveSpeedValue2;

    void Start()
    {
        rb = transform.parent.GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        GameObject player = GameObject.Find("Mario");
        marioBodyTransform = player.transform.Find("MarioBody").transform;
        aiPath = transform.parent.GetComponent<AIPath>();
        aiPath.maxSpeed = moveSpeedValue1;
        transform.parent.GetComponent<AIDestinationSetter>().target = marioBodyTransform;
    }

    void FixedUpdate()
    {

        distance = (float)Math.Sqrt(Math.Pow(marioBodyTransform.position.x - transform.parent.transform.position.x, 2) + Math.Pow(marioBodyTransform.position.y - transform.parent.transform.position.y, 2));

        if (distance <= 4) {
            aiPath.maxSpeed = moveSpeedValue2;
            anim.SetBool("isAttacking", true);
        }
        else if (distance <= 10) {
            aiPath.maxSpeed = moveSpeedValue1;
            anim.SetBool("isAttacking", false);
        }
        else {
            aiPath.maxSpeed = 0;
            anim.SetBool("isAttacking", false);
        }

    }

    void LateUpdate()
    {

        if (aiPath.desiredVelocity.x > 0)
        {
            transform.parent.transform.localScale = new Vector2(-1f, 1f);
        }
        else {
            transform.parent.transform.localScale = new Vector2(1f, 1f);
        }

    }

}