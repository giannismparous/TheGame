using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DiglettMove : MonoBehaviour
{

    private float dirX;
    private float moveSpeed;
    private Rigidbody2D rb;
    private bool facingRight = false;
    private Vector3 parentLocalScale;
    private float colliderTimer;
    private float goDownTimer;
    private float goUpTimer;
    private bool isDown;
    private System.Random rand;
    private Animator anim;
    private AnimatorClipInfo[] animatorinfo;
    private string current_animation;
    private Collider2D diglettBodyCollider;
    private float distance;
    public float moveSpeedValue1;
    public float moveSpeedValue2;
    public float distanceValue;
    public float goDownTimerValue;
    public float goUpTimerValue;
    public Transform groundCheck;
    public LayerMask groundLayer;
    public Transform marioTransform;

    void Start()
    {
        parentLocalScale = transform.parent.gameObject.transform.localScale;
        rb = transform.parent.GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        dirX = -1f;
        colliderTimer = 0;
        rand = new System.Random();
        SetGoDownTimer();
        isDown = false;
        Physics2D.IgnoreCollision(transform.parent.transform.Find("Feet").GetComponent<Collider2D>(), GameObject.Find("Mario").transform.Find("MarioBody").GetComponent<Collider2D>());
        diglettBodyCollider = transform.GetComponent<Collider2D>();
        marioTransform = GameObject.Find("Mario").transform;
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

        distance = (float)Math.Sqrt(Math.Pow(marioTransform.position.x - transform.parent.transform.position.x, 2) + Math.Pow(marioTransform.position.y - transform.parent.transform.position.y, 2));

        if (distance <= 5) moveSpeed = moveSpeedValue2;
        else moveSpeed = moveSpeedValue1;

        if (rb.velocity.y == 0 && !Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer))
        {
            dirX *= -1;
        }

        rb.velocity = new Vector2(dirX * moveSpeed, rb.velocity.y);

        colliderTimer -= Time.deltaTime;

        if (!isDown && goDownTimer <= 0)
        {
            isDown = true;
            SetGoUpTimer();
            anim.SetTrigger("diglettGoesDown");
            diglettBodyCollider.enabled = false;
        }
        else if (isDown && goUpTimer <= 0)
        {
            isDown = false;
            SetGoDownTimer();
            anim.SetTrigger("diglettGoesUp");
            diglettBodyCollider.enabled = true;
        }
        else if (!isDown && goDownTimer > 0) goDownTimer -= Time.deltaTime;
        else if (isDown && goUpTimer > 0) goUpTimer -= Time.deltaTime;

        animatorinfo = anim.GetCurrentAnimatorClipInfo(0);
        current_animation = animatorinfo[0].clip.name;
        if (current_animation.Equals("diglett_goes_up") && anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1) anim.SetTrigger("diglettIsUp");
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

    void SetGoUpTimer() {
        goUpTimer = (float)(rand.NextDouble() * 10 + goUpTimerValue - 5);
    }

    void SetGoDownTimer() {
        goDownTimer = (float)(rand.NextDouble() * 10 + goDownTimerValue - 5);
    }

    public void Jumped() {
        anim.SetTrigger("diglettJumped");
        SetGoUpTimer();
        isDown = true;
        diglettBodyCollider.enabled = false;
    }

}