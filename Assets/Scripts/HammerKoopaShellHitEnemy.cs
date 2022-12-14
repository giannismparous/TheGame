using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerKoopaShellHitEnemy : MonoBehaviour
{

    public HammerKoopaShellMove hammerKoopaShellMove;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (hammerKoopaShellMove.GetMoveSpeed()>1 && other.CompareTag("Enemy"))
        {
            Destroy(other.gameObject.transform.parent.gameObject);
        }
    }
}
