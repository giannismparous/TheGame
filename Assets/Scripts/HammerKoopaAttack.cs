using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HammerKoopaAttack : MonoBehaviour
{

    public Transform marioTransform;
    private float distance;
    public HammerKoopaMove hammerKoopaMove;

    void Start() {
        marioTransform = GameObject.Find("Mario").transform;
    }

    void Update()
    {

        distance = (float)Math.Sqrt(Math.Pow(marioTransform.position.x - transform.parent.transform.position.x, 2) + Math.Pow(marioTransform.position.y - transform.parent.transform.position.y, 2));

        if (distance < 5)
        {
            if (marioTransform.position.x< transform.parent.transform.position.x)hammerKoopaMove.SetDirX(-1);
            else hammerKoopaMove.SetDirX(1);
            hammerKoopaMove.Attack();
            
        }
        else {
            hammerKoopaMove.StopAttack();
        }
    }
}
