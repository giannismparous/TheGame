using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PinkGhostMove : MonoBehaviour
{

    private Rigidbody2D rb;
    private Animator anim;
    private AnimatorClipInfo[] animatorinfo;
    private string current_animation;
    private bool facingRight = false;
    private Vector3 parentLocalScale;
    private float moveSpeed;
    private float colliderTimer;
    private float attackTimer;
    private float scareDuration;    
    private bool isScaring;
    private float distance;
    private Transform redGhostSpitPositionTransform;
    private Transform marioBodyTransform;
    private GameObject redGhostSpitGameObject;
    public float dirX;
    public float moveSpeedValue1;
    public float moveSpeedValue2;
    public float distanceValue;
    private int attackType;
    public float attackTimerValue;
    public float scareDurationValue;
    public float stunDuration;
    public Transform groundCheck;
    public LayerMask groundLayer;
    public GameObject redGhostSpitPrefab;
    public MarioMove marioMove;

    void Start()
    {
        parentLocalScale = transform.parent.gameObject.transform.localScale;
        rb = transform.parent.GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        colliderTimer = 0;
        isScaring = false;
        redGhostSpitPositionTransform = transform.parent.transform.Find("SpitPosition").transform;
        GameObject player = GameObject.Find("Mario");
        marioBodyTransform = player.transform.Find("MarioBody").transform;
        marioMove = player.transform.Find("MarioBody").GetComponent<MarioMove>();
        moveSpeed = moveSpeedValue1;
        attackType = 1;
        attackTimer = attackTimerValue;
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

        distance = (float)Math.Sqrt(Math.Pow(marioBodyTransform.position.x - transform.parent.transform.position.x, 2) + Math.Pow(marioBodyTransform.position.y - transform.parent.transform.position.y, 2));

        if (distance <= distanceValue)
        {
            if (marioBodyTransform.position.x <= transform.position.x) dirX = -1;
            else dirX = 1;

            moveSpeed = moveSpeedValue2;
            anim.SetBool("isChasing", true);

            if (!isScaring) {
                if (attackTimer <= 0)
                {
                    if (attackType == 1) {
                        anim.SetBool("isScaring", true);
                        isScaring = true;
                        scareDuration = scareDurationValue;
                        attackType++;
                    }
                    else if(attackType == 2){
                        anim.SetBool("isSpitting", true);
                        attackType--;
                    }

                    attackTimer = attackTimerValue;
                }
                else attackTimer -= Time.deltaTime;
            }

            if (isScaring) {
                if (scareDuration <= 0)
                {
                    isScaring = false;
                    anim.SetBool("isScaring", false);
                }
                else
                {
                    scareDuration -= Time.deltaTime;
                    if (dirX * marioMove.GetDirX() < 0) marioMove.Stun(stunDuration);
                }
            }

        }
        else
        {
            if (rb.velocity.y == 0 && !Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer)) dirX *= -1;
            moveSpeed = moveSpeedValue1;
            anim.SetBool("isChasing", false);
            anim.SetBool("isScaring", false);
            anim.SetBool("isSpitting", false);
            isScaring = false;
        }

        animatorinfo = anim.GetCurrentAnimatorClipInfo(0);
        current_animation = animatorinfo[0].clip.name;

        if (current_animation.Equals("pink_ghost_spit") && anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
        {
            Spit();
            anim.SetBool("isSpitting", false);
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

    public void Spit()
    {
        redGhostSpitGameObject = Instantiate(redGhostSpitPrefab) as GameObject;
        redGhostSpitGameObject.transform.position = new Vector2(redGhostSpitPositionTransform.position.x, redGhostSpitPositionTransform.position.y);
        redGhostSpitGameObject.GetComponent<RedGhostSpitMove>().SetDirX(dirX);
    }

}