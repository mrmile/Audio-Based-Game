using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Amplifiers : MonoBehaviour
{
    LevelsManager level_;
    R_Easings easings_;

    public GameObject obstacle;
    public GameObject obstacleWarning;

    public Vector2 minPosXY;
    public Vector2 maxPosXY;
    public Vector2 initialScale;
    public Vector2 finalScale;

    public float rotationSpeed = 0;
    public float expandingTime = 10.0f;
    public float warningTime = 0;
    public bool hasNoWarning = true;

    private float startTime = 0;
    private float obstacleTime = 0;

    private int step = 0;

    private SpriteRenderer[] objectsChildren;

    // Start is called before the first frame update
    void Start()
    {
        level_ = FindObjectOfType<LevelsManager>();
        easings_ = FindObjectOfType<R_Easings>();

        obstacleWarning.transform.localScale = new Vector3(0, 0, 0);
        obstacle.transform.localScale = new Vector3(0, 0, 0);

        obstacle.transform.localPosition = new Vector3(Random.Range(minPosXY.x, maxPosXY.x), Random.Range(minPosXY.y, maxPosXY.y), 0);
        obstacleWarning.transform.localPosition = new Vector3(Random.Range(minPosXY.x, maxPosXY.x), Random.Range(minPosXY.y, maxPosXY.y), 0);

        startTime = Time.time;

        //-----Color Setup-------------------------------------------------------
        objectsChildren = GetComponentsInChildren<SpriteRenderer>();

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

    // Update is called once per frame
    void Update()
    {
        obstacleTime = Time.time - startTime;

        if (rotationSpeed != 0)
        {
            gameObject.transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
        }

        if(step == 0 && hasNoWarning == true)
        {
            step = 2;
            startTime = Time.time;
            obstacleTime = Time.time - startTime;
        }

        if (step == 0 && obstacleTime <= expandingTime)
        {
            obstacleWarning.transform.localScale = new Vector3(easings_.EaseSineIn(obstacleTime, initialScale.x, finalScale.x - initialScale.x, expandingTime), easings_.EaseSineIn(obstacleTime, initialScale.y, finalScale.y - initialScale.y, expandingTime), 0);
        }
        if (step == 0 && obstacleTime > warningTime)
        {
            step = 2;
            startTime = Time.time;
            obstacleTime = Time.time - startTime;
        }

        if (step == 2 && obstacleTime <= expandingTime)
        {
            obstacle.transform.localScale = new Vector3(easings_.EaseSineIn(obstacleTime, initialScale.x, finalScale.x - initialScale.x, expandingTime), easings_.EaseSineIn(obstacleTime, initialScale.y, finalScale.y - initialScale.y, expandingTime), 0);
        }
        else if(step == 2 && obstacleTime > expandingTime)
        {
            step++;
            startTime = Time.time;
            obstacleTime = Time.time - startTime;
        }

        if(step == 3)
        {
            if(hasNoWarning != true) Destroy(obstacleWarning);
            Destroy(obstacle);
            Destroy(gameObject);

            step++;
        }
    }
}
