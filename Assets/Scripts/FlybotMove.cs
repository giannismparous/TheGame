using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using EZCameraShake;

public class FlybotMove : MonoBehaviour
{

    private Rigidbody2D rb;
    private Animator anim;
    private bool facingRight = false;
    private bool isAttacking;
    private Vector3 localScale;
    private Transform marioBodyTransform;
    private MarioMove marioMove;
    private Transform explosionPositionTransform;
    private System.Random rand;
    public float dirX;
    public float moveSpeedValue;
    public float fallSpeed;
    public int explosionDamage;
    public float explosionRadius;
    public float explosionForce;
    public LayerMask explosionLayer;
    public GameObject explosionEffect;
    //CameraShaker
    public float explosionMagnitude;
    public float explosionRoughness;
    public float explosionFadeInTime;
    public float explosionFadeOutTime;

    void Start()
    {
        localScale = transform.parent.transform.localScale;
        rb = transform.parent.GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        isAttacking = false;
        GameObject player = GameObject.Find("Mario");
        marioBodyTransform = player.transform.Find("MarioBody").transform;
        marioMove = player.transform.Find("MarioBody").GetComponent<MarioMove>();
        rand = new System.Random();
        if ((rand.Next() % 2) ==0)explosionPositionTransform = transform.parent.transform.Find("ExplosionPosition1").transform;
        else explosionPositionTransform = transform.parent.transform.Find("ExplosionPosition2").transform;
    }


    void FixedUpdate()
    {
        
        if (!isAttacking)rb.velocity = new Vector2(dirX * moveSpeedValue, rb.velocity.y);
        else rb.velocity = new Vector2(0, -fallSpeed);

        if (!isAttacking && transform.position.x <= marioBodyTransform.position.x + 0.1 && transform.position.x >= marioBodyTransform.position.x - 0.1) {
            isAttacking = true;
            anim.SetTrigger("kamikaze");
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

        if (((facingRight) && (localScale.x < 0)) || ((!facingRight) && (localScale.x > 0)))
            localScale.x *= -1;

        transform.localScale = localScale;
    }

    void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("Player") || other.CompareTag("Obstacle") || other.CompareTag("Ground"))Explode();

    }

    void Explode()
    {
        Collider2D[] explosionAffectedObjects = Physics2D.OverlapCircleAll(transform.position, explosionRadius, explosionLayer);
        foreach (Collider2D obj in explosionAffectedObjects)
        {
            Vector2 direction = obj.transform.position - explosionPositionTransform.position;
            if (obj.gameObject.name.Equals("MarioBody"))
            {
                obj.gameObject.GetComponent<MarioMove>().RegisterExplosionKnockback(direction, 1000);
            }
            else
            {
                obj.transform.parent.GetComponent<Rigidbody2D>().AddForce(direction * explosionForce);
            }
        }
        
        float distance = (float)Math.Sqrt(Math.Pow(marioBodyTransform.position.x - transform.parent.transform.position.x, 2) + Math.Pow(marioBodyTransform.position.y - transform.parent.transform.position.y, 2));
        marioMove.TakeDamage(explosionDamage);
        CameraShaker.Instance.ShakeOnce(explosionMagnitude, explosionRoughness, explosionFadeInTime, explosionFadeOutTime);
        GameObject explosionEffectInstance = Instantiate(explosionEffect, transform.position, Quaternion.identity); //meaning no rotation
        Destroy(explosionEffectInstance, 10);
        Destroy(transform.parent.gameObject);
    }

}