using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class SquareBurstBullet : MonoBehaviour
{
    LevelsManager level_;
    R_Easings easings_;

    public float lifetime;
    public float rotationSpeed;
    Transform sprite;

    private float startTime = 0;
    private float obstacleTime = 0;

    private SpriteRenderer[] objectsChildren;
    private float startingColorValue_r = 0;
    private float startingColorValue_g = 0;
    private float startingColorValue_b = 0;

    // Start is called before the first frame update
    void Start()
    {
        level_ = FindObjectOfType<LevelsManager>();
        easings_ = FindObjectOfType<R_Easings>();

        sprite = transform.GetChild(0);

        startTime = Time.time;

        //-----Color Setup-------------------------------------------------------
        objectsChildren = GetComponentsInChildren<SpriteRenderer>();

        for (int i = 0; i < objectsChildren.Length; i++)
        {

            objectsChildren[i].color = new Color(level_.levelObstaclesColor.r, level_.levelObstaclesColor.g, level_.levelObstaclesColor.b);
        }
        startingColorValue_r = 1 - level_.levelObstaclesColor.r;
        startingColorValue_g = 1 - level_.levelObstaclesColor.g;
        startingColorValue_b = 1 - level_.levelObstaclesColor.b;
        //-----------------------------------------------------------------------
    }

    // Update is called once per frame
    void Update()
    {
        obstacleTime = Time.time - startTime;

        sprite.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);

        if (lifetime > 0) lifetime -= Time.deltaTime;
        else Destroy(gameObject);

        //-----Color Setup-------------------------------------------------------
        if (startingColorValue_r > 0.01f) startingColorValue_r = easings_.EaseSineOut(obstacleTime, (1 - level_.levelObstaclesColor.r), 0 - (1 - level_.levelObstaclesColor.r), 0.75f);
        if (startingColorValue_g > 0.01f) startingColorValue_g = easings_.EaseSineOut(obstacleTime, (1 - level_.levelObstaclesColor.g), 0 - (1 - level_.levelObstaclesColor.g), 0.75f);
        if (startingColorValue_b > 0.01f) startingColorValue_b = easings_.EaseSineOut(obstacleTime, (1 - level_.levelObstaclesColor.b), 0 - (1 - level_.levelObstaclesColor.b), 0.75f);

        for (int i = 0; i < objectsChildren.Length; i++)
        {

            objectsChildren[i].color =
                    new Color(level_.levelObstaclesColor.r + startingColorValue_r,
                    level_.levelObstaclesColor.g + startingColorValue_g,
                    level_.levelObstaclesColor.b + startingColorValue_b);

        }
        //-----------------------------------------------------------------------
    }
}
