using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannibalHitPlayer : MonoBehaviour
{
    public CannibalMove cannibalMove;
    public MarioMove marioMove;
    public float KBForce;
    private Rigidbody2D otherRB;

    void Awake()
    {
        GameObject player = GameObject.Find("Mario");
        marioMove = player.transform.Find("MarioBody").GetComponent<MarioMove>();
    }

    void OnTriggerEnter2D(Collider2D other) {

        if (other.CompareTag("Player")) {
            cannibalMove.SetTimer(3);
            if (other.transform.position.x <= transform.position.x) marioMove.RegisterHitKnockback(KBForce, true);
            else marioMove.RegisterHitKnockback(KBForce, false);
            marioMove.TakeDamage(10);
        }

    }
}
