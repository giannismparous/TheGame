using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharizardFoundPlayer : MonoBehaviour
{
    private Rigidbody2D otherRB;
    private CharizardMove charizardMove;

    void Awake()
    {
        charizardMove = transform.parent.transform.Find("CharizardBody").GetComponent<CharizardMove>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {

        if (!charizardMove.isUsingFlamethrower() & !charizardMove.isMovingTowardsPlayer() && other.CompareTag("Player"))
        {
            charizardMove.FoundPlayer();
        }

    }

}
