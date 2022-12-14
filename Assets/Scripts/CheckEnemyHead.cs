using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckEnemyHead : MonoBehaviour
{

    private Rigidbody2D rb;

    private void Start()
    {
        rb = this.gameObject.transform.parent.GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("EnemyHead"))
        {
            rb.velocity = new Vector2(rb.velocity.x, 0f);
            rb.AddForce(Vector2.up * 400f);
        }
    }
}
