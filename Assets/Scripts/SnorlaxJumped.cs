using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnorlaxJumped : MonoBehaviour
{
    public MarioMove marioMove;
    private SnorlaxMove snorlaxMove;

    void Awake()
    {
        snorlaxMove = transform.parent.Find("SnorlaxBody").GetComponent<SnorlaxMove>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("Player"))
        {
            if (!snorlaxMove.IsAwake()) snorlaxMove.WakeUp();
        }

    }
}
