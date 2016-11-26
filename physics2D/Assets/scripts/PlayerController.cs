using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public Camera mainCamera;
    Rigidbody2D rb;

    Vector2 velocity = new Vector2(10f, 0f);

    bool isAnchorable = false;
    Vector3 anchor;
    bool anchored = false;
    SpringJoint2D spring;

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
        rb.velocity = Vector3.ClampMagnitude(rb.velocity * 9999, 15);

        //transform.rotation = new Quaternion(rb.velocity.x, rb.velocity.y, 0f, 0f);

        float rotationAngle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) - Mathf.PI / 2;
        float x = anchor.x + Mathf.Cos(rotationAngle)*5;
        float y = anchor.y + Mathf.Sin(rotationAngle)*5;
        Debug.DrawLine(anchor, new Vector3(x, y, 0), Color.yellow);
        transform.rotation = Quaternion.Euler(0, 0, Mathf.Rad2Deg * rotationAngle);

        Debug.DrawLine(vel, transform.position, Color.cyan);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log("enter");
        anchored = true;
        anchor = other.transform.position;

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
