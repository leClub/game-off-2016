using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rb;

    bool isAnchorable = false;
    Vector3 anchor;
    bool anchored = false;
    SpringJoint2D spring;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spring = GetComponent<SpringJoint2D>();
        rb.velocity = new Vector2(10f, 0f);
        anchor = transform.position;
    }

    void Update()
    {
    }

    void FixedUpdate()
    {
        Vector3 pos = transform.position;

        //Vector2 vel = rb.velocity + new Vector2(pos.x, pos.y);
        //Debug.DrawLine(vel, transform.position, Color.blue);
        //rb.velocity.Set(transform.right.x * 10f, transform.right.y);

        if (isAnchorable)
        {
            Debug.DrawLine(anchor, pos, Color.red);
            if (Input.GetKey("space"))
            {
                spring.enabled = true;
                anchored = true;
                isAnchorable = false;
            }
        }

        if (anchored)
        {
            Debug.DrawLine(anchor, pos, Color.blue);

            if (!Input.GetKey("space"))
            {
                spring.enabled = false;
                anchored = false;
            }
        }

        Vector3 vel = transform.position + new Vector3(rb.velocity.x, rb.velocity.y, 0);
        rb.velocity = Vector3.ClampMagnitude(rb.velocity * 9999, 10);

        Debug.DrawLine(vel, transform.position, Color.cyan);
        Debug.Log(rb.velocity);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        isAnchorable = true;
        anchor = other.transform.position;

        spring.connectedAnchor = new Vector2(anchor.x, anchor.y);
    }

    void OnTriggerStay2D(Collider2D other)
    {
        // Debug.Log( "stay" );
    }

    void OnTriggerExit2D(Collider2D other)
    {
        isAnchorable = false;
    }
}
