using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerKoopaJumped : MonoBehaviour
{

    public GameObject hammerKoopaShellPrefab;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<CheckEnemyHead>())
        {
            Rigidbody2D temp1 = transform.parent.GetComponent<Rigidbody2D>();
            GameObject temp2 = Instantiate(hammerKoopaShellPrefab) as GameObject;
            temp2.transform.position = new Vector2(temp1.position.x,temp1.position.y);
            Destroy(transform.parent.gameObject);
        }
    }
}
