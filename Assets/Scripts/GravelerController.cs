using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GravelerController : MonoBehaviour
{

    private Rigidbody2D rb;
    private Animator anim;
    private AnimatorClipInfo[] animatorinfo;
    private string current_animation;
    private bool facingRight = false;
    private Vector3 parentLocalScale;
    private float colliderTimer;
    private float attackTimer;
    private float distance;
    private Transform attackPositionTransform;
    private GameObject groundCrackGameObject;
    public float dirX;
    public float attackTimerValue;
    public int numberOfGroundCracks;
    public float groundCrackDestroyTimer;
    public float groundCrackNextTimer;
    public Transform marioBodyTransform;
    public GameObject groundCrackPrefab;

    void Start()
    {
        parentLocalScale = transform.parent.gameObject.transform.localScale;
        rb = transform.parent.GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        marioBodyTransform = GameObject.Find("Mario").transform.Find("MarioBody").transform;
        attackPositionTransform = transform.parent.Find("AttackPosition").transform;
        colliderTimer = 0;
        attackTimer = 0;
    }

    void FixedUpdate()
    {
        if (attackTimer > 0) attackTimer -= Time.deltaTime;
        
        distance = (float)Math.Sqrt(Math.Pow(marioBodyTransform.position.x - transform.parent.transform.position.x, 2) + Math.Pow(marioBodyTransform.position.y - transform.parent.transform.position.y, 2));

        if (distance <= 5 && attackTimer<=0) {
            Attack();
        }

        animatorinfo = anim.GetCurrentAnimatorClipInfo(0);
        current_animation = animatorinfo[0].clip.name;

        if (current_animation.Equals("graveler_attack") && anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1) {
            CreateCrack();
        }

        colliderTimer -= Time.deltaTime;

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

    void Attack() {
        anim.SetTrigger("isAttacking");
        attackTimer = attackTimerValue;
    }

    void CreateCrack() {
        anim.SetTrigger("stoppedAttack");
        groundCrackGameObject = Instantiate(groundCrackPrefab) as GameObject;
        groundCrackGameObject.GetComponent<GroundCrackController>().SetCrack(numberOfGroundCracks, groundCrackNextTimer, groundCrackDestroyTimer);
        groundCrackGameObject.transform.position = new Vector2(attackPositionTransform.position.x, attackPositionTransform.position.y);
    }

}