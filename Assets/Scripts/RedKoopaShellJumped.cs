using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedKoopaShellJumped : MonoBehaviour
{

    public RedKoopaShellMove redKoopaShellMove;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<CheckEnemyHead>())
        {
            redKoopaShellMove.SetMoveSpeed(0);
        }
    }
}
