using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DustHitPlayer : MonoBehaviour
{

    private Rigidbody2D otherRB;
    public MarioMove marioMove;
    public float KBForce;
    public int damage;
    public float stunPlayerDuration;

    void Awake()
    {
        GameObject player = GameObject.Find("Mario");
        marioMove = player.transform.Find("MarioBody").GetComponent<MarioMove>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("Player"))
        {
            if (other.transform.position.x <= transform.position.x) marioMove.RegisterHitKnockback(KBForce, true);
            else marioMove.RegisterHitKnockback(KBForce, false);
            marioMove.TakeDamage(damage);
            marioMove.Stun(stunPlayerDuration);
            Destroy(transform.parent.transform.gameObject);
        }

    }
}
