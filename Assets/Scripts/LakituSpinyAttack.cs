using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LakituSpinyAttack : MonoBehaviour
{

    private float distance;
    public LakituMove lakituMove;
    private float attackTimer;
    public float attackTimerValue;
    private System.Random rand;

    void Start() {
        rand = new System.Random();
        attackTimer = (float)(attackTimerValue + rand.NextDouble() * 10 - 5);
    }

    void Update()
    {

        attackTimer -= Time.deltaTime;

        if (attackTimer <= 0 && !lakituMove.GetIsFallingAttacking()) {
            lakituMove.SpinyAttack();
            attackTimer = (float)(attackTimerValue + rand.NextDouble() * 10 - 5);
        }

    }
}
