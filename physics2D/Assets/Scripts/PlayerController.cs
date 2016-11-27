using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    // Camera
    public Camera mainCamera;
    Vector3 cameraTargetPos;
    float cameraTargetSize;

    // Rigidbody
    Rigidbody2D rb;
    SpringJoint2D spring;
    Vector2 anchor;
    bool isAnchorable = false;
    bool anchored = false;
    float anchorDist;

    public float maxSpeed = 15;

    private Vector3 refref = Vector3.zero;

    // Animation
    Animator anim;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(10f, 0f);
        spring = GetComponent<SpringJoint2D>();

        // Set camera default position
        cameraTargetPos = transform.position;
        cameraTargetSize = 50f;

        anim = GetComponent<Animator>();
    }

    void Update()
    {
    }

    void FixedUpdate()
    {
        Vector3 pos = transform.position;

        // If ship is inside orbit of planet
        if (isAnchorable)
        {
            // Attach anchor
            if (Input.GetKey("space"))
            {
                spring.enabled = true;
                anchored = true;
                isAnchorable = false;
                anchorDist = Vector3.Distance(anchor, pos);
            }
        }

        // If ship is attached to orbit of planet
        if (anchored)
        {
            // Set camera target position to planet
            cameraTargetPos = new Vector3(anchor.x, anchor.y, -10f);
            cameraTargetSize = 12f;

            // Set rotation around planet of ship
            float rotationAngle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) - Mathf.PI / 2;
            float x = anchor.x + Mathf.Cos(rotationAngle) * 5;
            float y = anchor.y + Mathf.Sin(rotationAngle) * 5;
            Debug.DrawLine(anchor, new Vector3(x, y, 0), Color.yellow);
            transform.rotation = Quaternion.Euler(0, 0, Mathf.Rad2Deg * rotationAngle);

            spring.distance = Mathf.Lerp(spring.distance, anchorDist, 0.5f);

            // Release anchor
            if (!Input.GetKey("space"))
            {
                spring.enabled = false;
                anchored = false;
            }
        }
        else
        {
            // Set camera target position to the ship
            cameraTargetPos = new Vector3(pos.x, pos.y, -10f) + new Vector3(rb.velocity.x, rb.velocity.y, 0f);
            cameraTargetSize = 20f;
        }

        // constant velocity
        Vector3 vel = transform.position + new Vector3(rb.velocity.x, rb.velocity.y, 0);
        rb.velocity = Vector3.ClampMagnitude(rb.velocity * 999, maxSpeed);
        //Debug.DrawLine(vel, transform.position, Color.cyan);

        mainCamera.transform.position = Vector3.SmoothDamp(mainCamera.transform.position, cameraTargetPos, ref refref, 0.4f);
        mainCamera.orthographicSize = Mathf.Lerp(mainCamera.orthographicSize, cameraTargetSize, 0.03f);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Collision behaviour when hit planet orbit
        if (other.tag == "Orbit") {
            anchored = true;
            anchor = other.transform.position;

            spring.connectedAnchor = new Vector2(anchor.x, anchor.y);

            Vector2 pos = new Vector2(transform.position.x, transform.position.y);
            Vector2 dir = anchor - pos;
            dir.Normalize();
            Vector2 A = new Vector2(anchor.x - dir.y, anchor.y + dir.x);
            Vector2 B = new Vector2(anchor.x + dir.y, anchor.y - dir.x);

            //Debug.DrawLine(anchor, A, Color.green, 100);
            //Debug.DrawLine(anchor, B, Color.red, 100);

            //Debug.DrawLine(pos, A, Color.yellow, 100);
            //Debug.DrawLine(pos, B, Color.yellow, 100);
            //Debug.DrawLine(pos, anchor, Color.yellow, 100);

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
                Debug.Log("Going to crash");
            }
            else
            {
                isAnchorable = true;
            }
        }
        // Collision behaviour when hit planet core
        else if ( other.tag == "Planet" )
        {
            rb.velocity = Vector3.zero;
            anim.enabled = true;
            anim.SetBool( "explode", true);
            transform.localScale = new Vector3(4f, 4f, 1f);
            Debug.Log("BADABOOM !");
            StartCoroutine(ToGameover());
        }
        // Collision behaviour when hit other objects
        else {
            Debug.Log("??");
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

    IEnumerator ToGameover() {
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene("Gameover", LoadSceneMode.Single);
    }
}
