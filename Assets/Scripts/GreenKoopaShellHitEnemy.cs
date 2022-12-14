using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenKoopaShellHitEnemy : MonoBehaviour
{

    public GreenKoopaShellMove greenKoopaShellMove;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (greenKoopaShellMove.GetMoveSpeed()>1 && other.CompareTag("Enemy"))
        {
            Destroy(other.gameObject.transform.parent.gameObject);
        }
    }
}
