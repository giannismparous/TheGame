using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerKoopaShellJumped : MonoBehaviour
{

    public HammerKoopaShellMove hammerKoopaShellMove;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<CheckEnemyHead>())
        {
            hammerKoopaShellMove.SetMoveSpeed(0);
        }
    }
}
