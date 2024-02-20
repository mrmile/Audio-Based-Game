using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;
    [SerializeField] float speedRef;
    [SerializeField] float rotTime;
    float speed;

    [SerializeField] Vector2 movement;
    Transform spr;

    float myFloat;
    // Start is called before the first frame update
    void Start()
    {
        spr = transform.GetChild(0);
        speed = speedRef;
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        movement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        movement.Normalize();

        float angle = Mathf.Atan2(movement.x, movement.y) * Mathf.Rad2Deg;
        float smoothAngle = Mathf.SmoothDampAngle(spr.eulerAngles.z, -angle, ref myFloat, rotTime);

        spr.rotation = Quaternion.Euler(0, 0, smoothAngle);

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        MoveCharacter(movement);

    }
    void MoveCharacter(Vector2 direction)
    {
        //playerRb.MovePosition((Vector2)transform.localPosition + direction * playerSpeed * Time.deltaTime);
        rb.velocity = direction * speed * 100.0f * Time.deltaTime;
    }
}
