using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BombAttack : MonoBehaviour
{

    public Transform marioTransform;
    public float distanceValue;
    public BombMove bombMove;
    private float distance;
    private bool triggered;

    void Start() {
        marioTransform = GameObject.Find("Mario").transform;
        triggered = false;
    }

    void Update() { 
    

        if (!triggered)
        {
            distance = (float) Math.Sqrt(Math.Pow(marioTransform.position.x - transform.parent.transform.position.x, 2) + Math.Pow(marioTransform.position.y - transform.parent.transform.position.y, 2));
            if (distance < distanceValue)
            {
                bombMove.Trigger();
                triggered = true;
            }
        }
    }
}
