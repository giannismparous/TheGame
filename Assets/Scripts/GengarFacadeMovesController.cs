using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GengarFacadeMovesController : MonoBehaviour
{
    private int dirX;
    private Rigidbody2D rb;
    private Vector3 localScale;
    private bool facingRight;
    public MarioMove marioMove;

    void Start() {
        rb = transform.GetComponent<Rigidbody2D>();
        localScale = transform.localScale;
        GameObject player = GameObject.Find("Mario");
        marioMove = player.transform.Find("MarioBody").GetComponent<MarioMove>();
        Physics2D.IgnoreCollision(player.transform.Find("MarioBody").GetComponent<BoxCollider2D>(), transform.Find("Feet").GetComponent<BoxCollider2D>(), true);
    }

    void FixedUpdate() {
        if (marioMove.transform.parent.transform.position.x < rb.transform.position.x) facingRight = false;
        else facingRight = true;
    }

    void LateUpdate()
    {
        CheckWhereToFace();
    }

    void CheckWhereToFace()
    {

        if (dirX > 0)
            facingRight = false;
        else if (dirX < 0)
            facingRight = true;

        if (((!facingRight) && (localScale.x < 0)) || ((facingRight) && (localScale.x > 0)))
            localScale.x *= -1;

        transform.localScale = localScale;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<MarioMove>())
        {
            Destroy(transform.gameObject);
        }
    }
}
