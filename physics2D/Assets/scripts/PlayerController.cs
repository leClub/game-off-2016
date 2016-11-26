using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rb;
    SpringJoint2D spring;
    Vector3 anchor;
    bool anchored = false;
    Vector2 velocity = new Vector2(10f, 0f);

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spring = GetComponent<SpringJoint2D>();
        rb.velocity = velocity;
        anchor = transform.position;
    }

    void Update()
    {
    }

    void FixedUpdate()
    {
        Vector3 pos = transform.position;
        Debug.DrawLine(pos, pos + transform.up, Color.blue);
        Debug.DrawLine(pos, pos + transform.right, Color.red);

        //Vector2 vel = rb.velocity + new Vector2(pos.x, pos.y);
        //Debug.DrawLine(vel, transform.position, Color.blue);
        //rb.velocity.Set(transform.right.x * 10f, transform.right.y);

        if (anchored)
        {
            Debug.DrawLine(anchor, pos, Color.red);

            if (Input.GetKey("space"))
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
        //Debug.Log("enter");
        anchored = true;
        anchor = other.transform.position;
        Debug.Log(anchor.ToString());

        spring.enabled = true;
        spring.connectedAnchor = new Vector2(anchor.x, anchor.y);
    }

    void OnTriggerStay2D(Collider2D other)
    {
        // Debug.Log( "stay" );
    }

    void OnTriggerExit2D(Collider2D other)
    {
        //Debug.Log("exit");
    }
}
