using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VU_HeavyLaser : MonoBehaviour
{
    LevelsManager level_;
    R_Easings easings_;

    public GameObject obstacleWarning;
    public GameObject obstacle;

    public float width = 1;
    public float height = 1;
    public float minRandX = 0;
    public float maxRandX = 0;
    public float livingTime = 0;
    public float warningTime = 0;
    public bool allowShake = true;

    private float startTime = 0;
    private float obstacleTime = 0;

    private int step = 0;

    private SpriteRenderer[] objectsChildren;
    private float startingColorValue_r = 0;
    private float startingColorValue_g = 0;
    private float startingColorValue_b = 0;

    // Start is called before the first frame update
    void Start()
    {
        level_ = FindObjectOfType<LevelsManager>();
        easings_ = FindObjectOfType<R_Easings>();

        float Xpos = Random.Range(minRandX, maxRandX);

        obstacleWarning.transform.position = new Vector3(Xpos, -5, 0);
        obstacle.transform.position = new Vector3(Xpos, -5, 0);

        obstacleWarning.transform.localScale = new Vector3(0, 0, 0);
        obstacle.transform.localScale = new Vector3(0, 0, 0);

        startTime = Time.time;

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
        startingColorValue_r = 1 - level_.levelObstaclesColor.r;
        startingColorValue_g = 1 - level_.levelObstaclesColor.g;
        startingColorValue_b = 1 - level_.levelObstaclesColor.b;
        //-----------------------------------------------------------------------
    }

    // Update is called once per frame
    void Update()
    {
        obstacleTime = Time.time - startTime;

        if (obstacleWarning.transform.localScale.x < width)
        {
            obstacleWarning.transform.localScale = new Vector3(easings_.EaseLinearNone(obstacleTime, 0, width - 0, 0.2f), easings_.EaseLinearNone(obstacleTime, 0, height - 0, 0.2f), 0);
        }
        else if (step == 0 && obstacleTime > warningTime)
        {
            step++;
            startTime = Time.time;
            obstacleTime = Time.time - startTime;
        }

        if (obstacle.transform.localScale.x < width && step == 1)
        {
            obstacle.transform.localScale = new Vector3(easings_.EaseLinearNone(obstacleTime, 0, width - 0, 0.2f), easings_.EaseLinearNone(obstacleTime, 0, height - 0, 0.2f), 0);
        }
        else if (step == 1)
        {
            //obstacleWarning.transform.localScale = new Vector3(0, 20, 0);
            if (allowShake == true)
            {
                level_.CameraDirectionalShake(new Vector2(0, 0.1f), 0.1f, 1.0f);
            }
            step++;
            startTime = Time.time;
            obstacleTime = Time.time - startTime;
        }

        if (step == 2 && obstacleTime >= livingTime)
        {
            //obstacleWarning.transform.localScale = new Vector3(0, 20, 0);
            step++;
            startTime = Time.time;
            obstacleTime = Time.time - startTime;
        }

        if (obstacle.transform.localScale.x > 0 && step == 3)
        {
            obstacle.transform.localScale = new Vector3(easings_.EaseExpoIn(obstacleTime, width, 0 - width, 0.2f), height, 0);
            obstacleWarning.transform.localScale = new Vector3(easings_.EaseExpoIn(obstacleTime, width, 0 - width, 0.2f), height, 0);
        }
        else if (step == 3)
        {
            step++;
            startTime = Time.time;
            obstacleTime = Time.time - startTime;
        }

        if (step == 4)
        {
            Destroy(obstacleWarning);
            Destroy(obstacle);
            Destroy(gameObject);
        }

        //-----Color Setup-------------------------------------------------------
        if (startingColorValue_r > 0.01f && step >= 1) startingColorValue_r = easings_.EaseSineOut(obstacleTime, (1 - level_.levelObstaclesColor.r), 0 - (1 - level_.levelObstaclesColor.r), 0.5f);
        if (startingColorValue_g > 0.01f && step >= 1) startingColorValue_g = easings_.EaseSineOut(obstacleTime, (1 - level_.levelObstaclesColor.g), 0 - (1 - level_.levelObstaclesColor.g), 0.5f);
        if (startingColorValue_b > 0.01f && step >= 1) startingColorValue_b = easings_.EaseSineOut(obstacleTime, (1 - level_.levelObstaclesColor.b), 0 - (1 - level_.levelObstaclesColor.b), 0.5f);

        for (int i = 0; i < objectsChildren.Length; i++)
        {

            if (objectsChildren[i].gameObject.tag != "Obstacle")
            {
                objectsChildren[i].color = new Color(level_.levelObstaclesColor.r, level_.levelObstaclesColor.g, level_.levelObstaclesColor.b, 0.3f);
            }
            else if (objectsChildren[i].gameObject.tag == "Obstacle")
            {
                objectsChildren[i].color =
                    new Color(level_.levelObstaclesColor.r + startingColorValue_r,
                    level_.levelObstaclesColor.g + startingColorValue_g,
                    level_.levelObstaclesColor.b + startingColorValue_b, 1.0f);
            }

        }
        //-----------------------------------------------------------------------
    }
}
