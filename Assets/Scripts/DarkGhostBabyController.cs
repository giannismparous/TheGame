using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Pathfinding;
using EZCameraShake;

public class DarkGhostBabyController : MonoBehaviour
{

    private float distance;
    private Rigidbody2D rb;
    private Animator anim;
    private AnimatorClipInfo[] animatorinfo;
    private string current_animation;
    private Transform marioBodyTransform;
    private Transform explosionPositionTransform;
    private AIPath aiPath;
    private bool appeared;
    private bool isChasing;
    public float moveSpeedValue;
    public float distanceValue;
    public int explosionDamage;
    public float explosionRadius;
    public float explosionForce;
    public LayerMask explosionLayer;
    public GameObject explosionEffect;
    public MarioMove marioMove;

    //CameraShaker
    public float explosionMagnitude;
    public float explosionRoughness;
    public float explosionFadeInTime;
    public float explosionFadeOutTime;

    void Start()
    {
        rb = transform.parent.GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        GameObject player = GameObject.Find("Mario");
        marioBodyTransform = player.transform.Find("MarioBody").transform;
        marioMove = player.transform.Find("MarioBody").GetComponent<MarioMove>();
        explosionPositionTransform = transform.parent.transform.Find("ExplosionPosition").transform;
        aiPath = transform.parent.GetComponent<AIPath>();
        aiPath.maxSpeed = 0;
        transform.parent.GetComponent<AIDestinationSetter>().target = marioBodyTransform;
        appeared = false;
        isChasing = false;
    }

    void FixedUpdate()
    {

        distance = (float)Math.Sqrt(Math.Pow(marioBodyTransform.position.x - transform.parent.transform.position.x, 2) + Math.Pow(marioBodyTransform.position.y - transform.parent.transform.position.y, 2));

        if (!appeared && distance <= distanceValue)
        {
            anim.SetTrigger("appeared");
            appeared = true;
        }

        animatorinfo = anim.GetCurrentAnimatorClipInfo(0);
        current_animation = animatorinfo[0].clip.name;

        if (appeared && current_animation.Equals("dark_ghost_baby_appear") && anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
        {
            anim.SetTrigger("isChasing");
            aiPath.maxSpeed = moveSpeedValue;
            isChasing = true;
        }
        else if (isChasing && distance > distanceValue)
        {
            anim.SetTrigger("stoppedChasing");
            aiPath.maxSpeed = 0;
            isChasing = false;
            appeared = false;
        }

    }

    void LateUpdate()
    {

        if (isChasing)
        {
            if (aiPath.desiredVelocity.x > 0)
            {
                transform.parent.transform.localScale = new Vector2(1f, 1f);
            }
            else
            {
                transform.parent.transform.localScale = new Vector2(-1f, 1f);
            }
        }

    }

    void OnTriggerEnter2D(Collider2D other) {

        if (other.CompareTag("Player"))Explode();

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
        //for mario
        float distance = (float)Math.Sqrt(Math.Pow(marioBodyTransform.position.x - transform.parent.transform.position.x, 2) + Math.Pow(marioBodyTransform.position.y - transform.parent.transform.position.y, 2));
        marioMove.TakeDamage(explosionDamage);
        CameraShaker.Instance.ShakeOnce(explosionMagnitude, explosionRoughness, explosionFadeInTime, explosionFadeOutTime);
        GameObject explosionEffectInstance = Instantiate(explosionEffect, transform.position, Quaternion.identity); //meaning no rotation
        Destroy(explosionEffectInstance, 10);
        Destroy(transform.parent.gameObject);
    }

}