using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class BombBullet : MonoBehaviour
{
    public float moveSpeed = 5f; // Adjust the movement speed as needed
    public float rotationSpeed = 180f; // Adjust the rotation speed as needed
    public bool disableAutoMovement = false;

    public GameObject bullet;

    LevelsManager level_;
    private SpriteRenderer[] objectsChildren;


    void Start()
    {
        level_ = FindObjectOfType<LevelsManager>();

        //-----Color Setup-------------------------------------------------------
        objectsChildren = GetComponentsInChildren<SpriteRenderer>(); ;

        float alpha = 255;
        for (int i = 0; i < objectsChildren.Length; i++)
        {

            if (objectsChildren[i].gameObject.tag != "Obstacle")
            {
                alpha = 0.3f;
            }
            else if (objectsChildren[i].gameObject.tag == "Obstacle")
            {
                alpha = 1.0f;
            }
            objectsChildren[i].color = new Color(level_.levelObstaclesColor.r, level_.levelObstaclesColor.g, level_.levelObstaclesColor.b, alpha);
        }
        //-----------------------------------------------------------------------
    }

    void Update()
    {
        if(disableAutoMovement == false)
        {
            // Move the object in the direction of its X axis
            transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);

            // Rotate the object around the Z axis
            bullet.transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
        }
    }
}
