using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;
    [SerializeField] float speedRef;
    [SerializeField] float rotTime;
    [SerializeField] float dashSpeedMultiplier;
    [SerializeField] float dashTime;
    float speed;
    float dashTimer;
    bool dashing = false;
    bool lockMovement = false;

    [SerializeField] Vector2 movement;
    Vector2 lastMovement;
    Transform spr;

    float myFloat;


    [SerializeField] Queue<bool> dashInputBuffer = new Queue<bool>();
    void DequeueInputDash()
    {
        if (dashInputBuffer.Count > 0)
            dashInputBuffer.Dequeue();
    }


    // Start is called before the first frame update
    void Start()
    {
        spr = transform.GetChild(0);
        speed = speedRef;
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        //get movement input
        movement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        movement.Normalize();
        if (!lockMovement)
            lastMovement = movement;

        //get dash input
        bool tryDash = false;

        if (movement != Vector2.zero)
        {
            tryDash = Input.GetButtonDown("Jump");

            if (tryDash)
            {
                dashInputBuffer.Enqueue(true);
                Invoke("DequeueInputDash", 0.5f);
            }
        }

        if (dashInputBuffer.Count > 0 && dashInputBuffer.Peek() == true && !dashing)
        {
            dashing = true;
            dashTimer = dashTime;
            //lockMovement = true;
            dashInputBuffer.Dequeue();

        }

        //calculate sprite rotation

        Vector3 rotMovement = Vector3.zero;

        if (!lockMovement)
            rotMovement = movement;
        else
            rotMovement = lastMovement;


        float angle = Mathf.Atan2(rotMovement.x, rotMovement.y) * Mathf.Rad2Deg;
        float smoothAngle = Mathf.SmoothDampAngle(spr.eulerAngles.z, -angle, ref myFloat, rotTime);

        spr.rotation = Quaternion.Euler(0, 0, smoothAngle);



        //timers
        if (dashTimer > 0) dashTimer -= Time.deltaTime;
        else
        {
            dashing = false;
            lockMovement = false;
        }

    }



    // Update is called once per frame
    void FixedUpdate()
    {
        if (!lockMovement)
            MoveCharacter(movement);
        else
            MoveCharacter(lastMovement);
    }
    void MoveCharacter(Vector2 direction)
    {
        if (!dashing)
            rb.velocity = direction * speed * 100.0f * Time.deltaTime;
        else
            rb.velocity = direction * speed * 100.0f * dashSpeedMultiplier * Time.deltaTime;

    }
}
