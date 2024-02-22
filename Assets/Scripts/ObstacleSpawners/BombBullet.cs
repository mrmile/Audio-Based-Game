using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombBullet : MonoBehaviour
{
    public float moveSpeed = 5f; // Adjust the movement speed as needed
    public float rotationSpeed = 180f; // Adjust the rotation speed as needed

    public GameObject bullet;

    void Update()
    {
        // Move the object in the direction of its X axis
        transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);

        // Rotate the object around the Z axis
        bullet.transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
    }
}
