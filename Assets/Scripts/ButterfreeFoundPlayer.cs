using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButterfreeFoundPlayer : MonoBehaviour
{
    private Rigidbody2D otherRB;
    private ButterfreeMove butterfreeMove;

    void Awake()
    {
        butterfreeMove = transform.parent.transform.Find("ButterfreeBody").GetComponent<ButterfreeMove>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {

        if (!butterfreeMove.AttackIsActivated() && other.CompareTag("Player"))
        {
            butterfreeMove.Attack();
        }

    }

}
