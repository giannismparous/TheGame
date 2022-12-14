using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using EZCameraShake;

public class BombMove : MonoBehaviour
{

    private Rigidbody2D rb;
    private bool facingRight = false;
    private Vector3 parentLocalScale;
    private float colliderTimer;
    public float dirX;
    private float moveSpeed;
    public float moveSpeedValue;
    public float moveSpeedTriggeredValue;
    private bool triggered;
    public float triggeredTimerValue;
    private float triggeredTimer;
    private Animator anim;
    private bool exploding;
    public float explodingTimerValue;
    private float explodingTimer;
    public float explosionRadius;
    public float explosionForce;
    public LayerMask explosionLayer;
    public GameObject explosionEffect;
    public MarioMove marioMove;
    public Transform marioTransform;
    public int explosionDamage1;
    public int explosionDamage2;
    public int explosionDamage3;
    private Transform explosionPositionTransform;

    //CameraShaker
    public float explosionMagnitude;
    public float explosionRoughness;
    public float explosionFadeInTime;
    public float explosionFadeOutTime;

    void Start()
    {
        parentLocalScale = transform.parent.gameObject.transform.localScale;
        rb = transform.parent.GetComponent<Rigidbody2D>();
        moveSpeed = moveSpeedValue;
        colliderTimer = 0;
        triggered = false;
        exploding = false;
        anim = GetComponent<Animator>();
        GameObject player = GameObject.Find("Mario");
        marioMove = player.transform.Find("MarioBody").GetComponent<MarioMove>();
        marioTransform = player.transform;
        explosionPositionTransform = transform.parent.Find("ExplosionPosition").transform;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (colliderTimer<=0 && (collision.CompareTag("Obstacle") || collision.CompareTag("Enemy")))
        {
            colliderTimer = 0.2f;
            dirX *= -1f;
        }
    }

    void FixedUpdate()
    {
        rb.velocity = new Vector2(dirX * moveSpeed, rb.velocity.y);
        colliderTimer -= Time.deltaTime;
        if (!exploding && triggered && triggeredTimer > 0) triggeredTimer -= Time.deltaTime;
        else if (!exploding && triggered && triggeredTimer <= 0)
        {
            moveSpeed = 0;
            anim.SetTrigger("exploding");
            exploding = true;
            explodingTimer = explodingTimerValue;
        }
        else if (exploding) {
            explodingTimer -= Time.deltaTime;
            if (explodingTimer <= 0) {
                Explode();
            }
            
        }

    }

    void LateUpdate()
    {
        CheckWhereToFace();
    }

    void CheckWhereToFace()
    {

        if (!triggered || exploding)
        {
            if (dirX > 0)
                facingRight = true;
            else if (dirX < 0)
                facingRight = false;
        }
        else {
            if (dirX > 0)
                facingRight = false;
            else if (dirX < 0)
                facingRight = true;
        }

        if (((facingRight) && (parentLocalScale.x < 0)) || ((!facingRight) && (parentLocalScale.x > 0)))
            parentLocalScale.x *= -1;

        transform.parent.localScale = parentLocalScale;
    }

    void Explode() {
        Collider2D[] explosionAffectedObjects = Physics2D.OverlapCircleAll(transform.position,explosionRadius,explosionLayer);
        foreach (Collider2D obj in explosionAffectedObjects) {
            Vector2 direction = obj.transform.position - explosionPositionTransform.position;
            if (obj.gameObject.name.Equals("MarioBody"))
            {
                obj.gameObject.GetComponent <MarioMove> ().RegisterExplosionKnockback(direction, 1000);
            }
            else
            {
                obj.transform.parent.GetComponent<Rigidbody2D>().AddForce(direction*explosionForce);
            }
        }
        //for mario
        float distance = (float)Math.Sqrt(Math.Pow(marioTransform.position.x - transform.parent.transform.position.x, 2) + Math.Pow(marioTransform.position.y - transform.parent.transform.position.y, 2));
        if (distance <= explosionRadius / 3) marioMove.TakeDamage(explosionDamage1);
        else if (distance <= explosionRadius / 2) marioMove.TakeDamage(explosionDamage2);
        else if (distance <= explosionRadius) marioMove.TakeDamage(explosionDamage3);
        CameraShaker.Instance.ShakeOnce(explosionMagnitude, explosionRoughness, explosionFadeInTime,explosionFadeOutTime);
        GameObject explosionEffectInstance = Instantiate(explosionEffect, transform.position, Quaternion.identity); //meaning no rotation
        Destroy(explosionEffectInstance, 10);
        Destroy(transform.parent.gameObject);
    }

    void onDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }

    public void Trigger() {
        triggered = true;
        moveSpeed = moveSpeedTriggeredValue;
        triggeredTimer = triggeredTimerValue;
        anim.SetTrigger("triggered");
    }

    public void Jumped() {
        moveSpeed = 0;
        anim.SetTrigger("jumped");
    }

   
}