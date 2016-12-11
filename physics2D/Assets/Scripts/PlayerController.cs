﻿using UnityEngine;
using UnityEngine.UI;
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

    private Vector3 refref = Vector3.zero;

    // Ship speed
    public float speed = 18;
    // Minimum ship speed
    public float minSpeed = 10;
    // Maximum ship speed
    public float maxSpeed = 30;
    // Speed descrease rate
    public float decreaseRate = 0.02f;
    // Spedd is decreasing
    private bool isDescreasing = false;
    // Speed Slider
    public Slider speedSlider;
    // Previous speedSlider for lerp transition
    private float lastSpeed;
    // Acceleration Particles system
    public ParticleSystem boostParticle;

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

        // Set speed slider limits
        speedSlider.maxValue = maxSpeed;
        speedSlider.minValue = minSpeed;
    }

    void Update()
    {
        //Update speed slider with lerp transition
        speedSlider.value = Mathf.Lerp(lastSpeed, speed, 0.5f);
        lastSpeed = speedSlider.value;
    }

    void FixedUpdate()
    {
        Vector3 pos = transform.position;

        // If ship is inside orbit of planet
        if (isAnchorable)
        {
            // Attach anchor
            if (Input.GetKey("space") || Input.touches.Length > 0)
            {
                spring.enabled = true;
                anchored = true;
                isAnchorable = false;
                anchorDist = Vector3.Distance(anchor, pos);

                // Set camera target position to planet
                cameraTargetPos = new Vector3(anchor.x, anchor.y, -10f);
                cameraTargetSize = 12f * anchorDist / 10;
            }

        }

        // If ship is attached to orbit of planet
        if (anchored)
        {
            // Set rotation around planet of ship
            float rotationAngle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) - Mathf.PI / 2;
            float x = anchor.x + Mathf.Cos(rotationAngle) * 5;
            float y = anchor.y + Mathf.Sin(rotationAngle) * 5;
            Debug.DrawLine(anchor, new Vector3(x, y, 0), Color.yellow);
            transform.rotation = Quaternion.Euler(0, 0, Mathf.Rad2Deg * rotationAngle);

            spring.distance = Mathf.Lerp(spring.distance, anchorDist, 0.5f);

            // Release anchor
            if (Input.GetKeyUp("space") || Input.touches.Length <= 0)
            {
                spring.enabled = false;
                anchored = false;
            }
        }
        else
        {
            // Set camera target position to the ship
            cameraTargetPos = new Vector3(pos.x, pos.y, -10f) + new Vector3(rb.velocity.x / 2, rb.velocity.y / 2, 0f);
            cameraTargetSize = 20f;
        }

        // constant velocity
        rb.velocity = Vector3.ClampMagnitude(rb.velocity * 999, speed);
        //Vector3 vel = transform.position + new Vector3(rb.velocity.x, rb.velocity.y, 0);
        //Debug.DrawLine(vel, transform.position, Color.cyan);

        mainCamera.transform.position = Vector3.SmoothDamp(mainCamera.transform.position, cameraTargetPos, ref refref, 0.4f);
        mainCamera.orthographicSize = Mathf.Lerp(mainCamera.orthographicSize, cameraTargetSize, 0.03f);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Collision behaviour when hit planet orbit
        if (other.tag == "Orbit")
        {
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
        else if (other.tag == "Planet")
        {
            rb.velocity = Vector3.zero;
            anim.enabled = true;
            anim.SetBool("explode", true);
            transform.localScale = new Vector3(4f, 4f, 1f);
            Debug.Log("BADABOOM !");
            StartCoroutine(ToGameover(2));
        }
        // Collision behaviour when leaving limits
       else if (other.tag == "OuterSpace")
        {
            Debug.Log("leaving limits");
            StartCoroutine(ToGameover(1));
        }
        // Collision behaviour when hit other objects
        else {
            Debug.Log("??");
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        //Debug.Log( "stay" );
        // When key is hold decrease the speed
        if (Input.GetKeyDown("space") || Input.touches.Length > 0) {
            isDescreasing = true;
        }
        if (Input.GetKeyUp("space") || Input.touches.Length <= 0) {
            //Debug.Log("release");
            isDescreasing = false;

            // Increase speed at new orbit
            if (speed < maxSpeed) {
                speed += 5;
                boostParticle.Play();
            }
        }

        if(isDescreasing) {
            if (speed > minSpeed) {
                speed -= decreaseRate;
            } else {
                speed = minSpeed;
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        //Debug.Log("OnTriggerExit2D");
        if (other.tag == "Orbit")
        {
            isAnchorable = false;
        }
    }

    IEnumerator ToGameover(int seconds)
    {
        yield return new WaitForSeconds(seconds);
        SceneManager.LoadScene("Gameover", LoadSceneMode.Single);
    }
}
