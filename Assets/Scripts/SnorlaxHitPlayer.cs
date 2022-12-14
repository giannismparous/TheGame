using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnorlaxHitPlayer : MonoBehaviour
{
    public MarioMove marioMove;
    private SnorlaxMove snorlaxMove;
    public float KBForce;

    void Awake()
    {
        GameObject player = GameObject.Find("Mario");
        marioMove = player.transform.Find("MarioBody").GetComponent<MarioMove>();
        snorlaxMove = transform.parent.Find("SnorlaxBody").GetComponent<SnorlaxMove>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("Player"))
        {
            if (other.transform.position.x <= transform.position.x) marioMove.RegisterHitKnockback(KBForce, true);
            else marioMove.RegisterHitKnockback(KBForce, false);
            marioMove.TakeDamage(10);
            if (!snorlaxMove.IsAwake()) snorlaxMove.WakeUp();
        }

    }
}
