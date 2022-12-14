using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicianShieldHitPlayer : MonoBehaviour
{
    private Rigidbody2D otherRB;
    private MagicianMovesController magicianMovesController;
    public float KBForce;
    public MarioMove marioMove;

    void Awake()
    {
        GameObject player = GameObject.Find("Mario");
        marioMove = player.transform.Find("MarioBody").GetComponent<MarioMove>();
        magicianMovesController = transform.parent.transform.Find("MagicianBody").GetComponent<MagicianMovesController>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("Player") && magicianMovesController.ShieldIsActivated())
        {
            if (other.transform.position.x <= transform.position.x) marioMove.RegisterHitKnockback(KBForce, true);
            else marioMove.RegisterHitKnockback(KBForce, false);
            marioMove.TakeDamage(10);
        }

    }
}
