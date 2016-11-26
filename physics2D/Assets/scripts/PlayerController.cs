using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public Camera mainCamera;
    Rigidbody2D rb;

    bool isAnchorable = false;
    Vector2 anchor;
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
            //Debug.DrawLine(anchor, pos, Color.red);
            if (Input.GetKey("space"))
            {
                spring.enabled = true;
                anchored = true;
                isAnchorable = false;
            }
        }

        if (anchored)
        {
            //Debug.DrawLine(anchor, pos, Color.blue);

            if (!Input.GetKey("space"))
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

        spring.connectedAnchor = new Vector2(anchor.x, anchor.y);

        Vector2 pos = new Vector2(transform.position.x, transform.position.y);
        Vector2 dir = anchor - pos;
        dir.Normalize();
        Vector2 A = new Vector2(anchor.x - dir.y, anchor.y + dir.x);
        Vector2 B = new Vector2(anchor.x + dir.y, anchor.y - dir.x);

        Debug.DrawLine(anchor, A, Color.green, 100);
        Debug.DrawLine(anchor, B, Color.red, 100);

        Debug.DrawLine(pos, A, Color.yellow, 100);
        Debug.DrawLine(pos, B, Color.yellow, 100);
        Debug.DrawLine(pos, anchor, Color.yellow, 100);

        float velAngle = Mathf.Atan2(rb.velocity.y, rb.velocity.x);
        Vector2 posA = A - pos;
        float posAAngle = Mathf.Atan2(posA.y, posA.x);
        Vector2 posB = B - pos;
        float posBAngle = Mathf.Atan2(posB.y, posB.x);
        Vector2 posAnchor = anchor - pos;
        float posAnchorAngle = Mathf.Atan2(posAnchor.y, posAnchor.x);

        float[] dif = new float[] { Mathf.Abs(velAngle - posAAngle), Mathf.Abs(velAngle - posBAngle), Mathf.Abs(velAngle - posAnchorAngle) };
        if (Mathf.Min(dif) == Mathf.Abs(velAngle - posAnchorAngle))
        {
            Debug.Log("crash");
        }
        else
        {
            isAnchorable = true;
        }
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
