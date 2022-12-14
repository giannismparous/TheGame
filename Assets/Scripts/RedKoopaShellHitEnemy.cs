using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedKoopaShellHitEnemy : MonoBehaviour
{

    public RedKoopaShellMove redKoopaShellMove;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (redKoopaShellMove.GetMoveSpeed() > 1 && other.CompareTag("Enemy"))
        {
            Destroy(other.gameObject.transform.parent.gameObject);
        }
    }
}
