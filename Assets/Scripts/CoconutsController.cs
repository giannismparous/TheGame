using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CoconutsController : MonoBehaviour
{

    private float distance;
    private Rigidbody2D rb;
    private Animator anim;
    private AnimatorClipInfo[] animatorinfo;
    private string current_animation;
    private bool facingRight = false;
    private Vector3 parentLocalScale;
    private bool isThrowingBanana;
    private bool flip;
    private bool isFlipping;
    private float throwBananaTimer;
    private List<CoconutsPointAvailability> points;
    public float dirX;
    public float distanceValue;
    public float upwardForce;
    public float sidewardForce;
    public float throwBananaTimerValue;
    public Transform marioBodyTransform;
    public GameObject bananaPrefab;


    void Start()
    {
        parentLocalScale = transform.parent.gameObject.transform.localScale;
        rb = transform.parent.GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        throwBananaTimer = 0;
        isThrowingBanana = false;
        isFlipping = false;
        GameObject player = GameObject.Find("Mario");
        marioBodyTransform = player.transform.Find("MarioBody").transform;
        points = new List<CoconutsPointAvailability>();
        points.Add(transform.parent.transform.Find("Point1").GetComponent<CoconutsPointAvailability>());
        points.Add(transform.parent.transform.Find("Point2").GetComponent<CoconutsPointAvailability>());
        points.Add(transform.parent.transform.Find("Point3").GetComponent<CoconutsPointAvailability>());
        points.Add(transform.parent.transform.Find("Point4").GetComponent<CoconutsPointAvailability>());
    }


    void FixedUpdate()
    {

        animatorinfo = anim.GetCurrentAnimatorClipInfo(0);
        current_animation = animatorinfo[0].clip.name;

        if (current_animation.Equals("coconuts_attack") && isThrowingBanana && anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
        {
            anim.SetTrigger("throwed_banana");
            isThrowingBanana = false;
            Rigidbody2D temp1 = transform.parent.GetComponent<Rigidbody2D>();
            GameObject temp2 = Instantiate(bananaPrefab) as GameObject;
            temp2.transform.position = new Vector2(temp1.position.x, temp1.position.y);
            temp2.GetComponent<BananaMove>().SetDirX(dirX);
            throwBananaTimer = throwBananaTimerValue;
            isThrowingBanana = false;
        }
        else if (current_animation.Equals("coconuts_flip") && isFlipping && anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1 && rb.velocity.Equals(Vector2.zero))
        {
            anim.SetTrigger("flipped");
            isFlipping = false;
            Debug.Log(2);
        }

        distance = (float)Math.Sqrt(Math.Pow(marioBodyTransform.parent.transform.position.x - transform.parent.transform.position.x, 2) + Math.Pow(marioBodyTransform.parent.transform.position.y - transform.parent.transform.position.y, 2));

        if (distance <= distanceValue)
        {
            anim.SetBool("is_aware", true);

            if (!isThrowingBanana)
            {
                if (marioBodyTransform.position.x < transform.position.x) dirX = -1;
                else dirX = 1;

                if (!isFlipping && !isThrowingBanana && distance <= distanceValue / 2 && rb.velocity.Equals(Vector2.zero) && CanFlip()) Flip();
                else if ((distance > distanceValue / 2 || !CanFlip()) && rb.velocity.Equals(Vector2.zero) && !isFlipping && !isThrowingBanana)
                {

                    if (throwBananaTimer <= 0 )
                    {
                        anim.SetTrigger("throw_banana");
                        isThrowingBanana = true;

                    }
                    else throwBananaTimer -= Time.deltaTime;
                }
            }
        }
        else anim.SetBool("is_aware", false);

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

    void Flip()
    {
        rb.AddForce(Vector2.up * upwardForce);
        if (dirX < 0) rb.AddForce(Vector2.right * sidewardForce);
        else if (dirX > 0) rb.AddForce(Vector2.left * sidewardForce);
        anim.SetTrigger("flip");
        isFlipping = true;
    }

    bool CanFlip() {

        flip = true;
        foreach (CoconutsPointAvailability point in points)
        {
            if (!point.IsAvailable())
            {
                flip = false;
                break;
            }
        }

        return flip;
    }
}