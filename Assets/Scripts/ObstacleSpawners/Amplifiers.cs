using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[System.Serializable]
public struct CustomScale
{
    public float timeIndex;
    public Vector2 newScale;
}

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

    public CustomScale[] manualScaleSet;

    private float startTime = 0;
    private float obstacleTime = 0;

    private int step = 0;
    private int index = 0;

    private SpriteRenderer[] objectsChildren;
    private float startingColorValue_r = 0;
    private float startingColorValue_g = 0;
    private float startingColorValue_b = 0;

    // Start is called before the first frame update
    void Start()
    {
        level_ = FindObjectOfType<LevelsManager>();
        easings_ = FindObjectOfType<R_Easings>();

        obstacleWarning.transform.localScale = new Vector3(0, 0, 0);
        obstacle.transform.localScale = new Vector3(0, 0, 0);

        obstacle.transform.localPosition = new Vector3(UnityEngine.Random.Range(minPosXY.x, maxPosXY.x), UnityEngine.Random.Range(minPosXY.y, maxPosXY.y), 0);
        obstacleWarning.transform.localPosition = new Vector3(obstacle.transform.localPosition.x, obstacle.transform.localPosition.y, 0);

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
            startingColorValue_r = 1 - level_.levelObstaclesColor.r;
            startingColorValue_g = 1 - level_.levelObstaclesColor.g;
            startingColorValue_b = 1 - level_.levelObstaclesColor.b;
        }
        //-----------------------------------------------------------------------
    }

    // Update is called once per frame
    void Update()
    {
        obstacleTime = Time.time - startTime;

        if (rotationSpeed != 0)
        {
            obstacleWarning.transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
            obstacle.transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
        }

        if(manualScaleSet.Length == 0)
        {
            if (step == 0 && hasNoWarning == true)
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
                //obstacleWarning.transform.localScale = new Vector3(0, 0, 0);
                step = 2;
                startTime = Time.time;
                obstacleTime = Time.time - startTime;
            }

            if (step == 2 && obstacleTime <= expandingTime)
            {
                obstacleWarning.transform.localScale = new Vector3(0, 0, 0);
                obstacle.transform.localScale = new Vector3(easings_.EaseSineIn(obstacleTime, initialScale.x, finalScale.x - initialScale.x, expandingTime), easings_.EaseSineIn(obstacleTime, initialScale.y, finalScale.y - initialScale.y, expandingTime), 0);
            }
            else if (step == 2 && obstacleTime > expandingTime)
            {
                step++;
                startTime = Time.time;
                obstacleTime = Time.time - startTime;
            }

            if (step == 3)
            {
                if (hasNoWarning != true) Destroy(obstacleWarning);
                Destroy(obstacle);
                Destroy(gameObject);

                step++;
            }

            //-----Color Setup-------------------------------------------------------
            for (int i = 0; i < objectsChildren.Length; i++)
            {

                if (objectsChildren[i].gameObject.tag != "Obstacle")
                {
                    objectsChildren[i].color = new Color(level_.levelObstaclesColor.r, level_.levelObstaclesColor.g, level_.levelObstaclesColor.b, 0.3f);
                }
                else if (objectsChildren[i].gameObject.tag == "Obstacle")
                {
                    objectsChildren[i].color =
                        new Color(level_.levelObstaclesColor.r, level_.levelObstaclesColor.g, level_.levelObstaclesColor.b, 1.0f);
                }

            }
            //-----------------------------------------------------------------------
        }
        else if (manualScaleSet.Length > 0 && index < manualScaleSet.Length)
        {
            if(hasNoWarning == true)
            {
                if (index == 0)
                {
                    if (obstacleTime <= manualScaleSet[index].timeIndex)
                    {
                        obstacle.transform.localScale = new Vector3(easings_.EaseSineInOut(obstacleTime, initialScale.x, manualScaleSet[index].newScale.x - initialScale.x, manualScaleSet[index].timeIndex),
                        easings_.EaseSineInOut(obstacleTime, initialScale.y, manualScaleSet[index].newScale.y - initialScale.y, manualScaleSet[index].timeIndex), 0);
                    }
                    else if (obstacleTime > manualScaleSet[index].timeIndex)
                    {
                        index++;
                        startTime = Time.time;
                        obstacleTime = Time.time - startTime;
                    }
                }
                else if (index > 0)
                {
                    if (obstacleTime <= manualScaleSet[index].timeIndex)
                    {
                        obstacle.transform.localScale = new Vector3(easings_.EaseSineInOut(obstacleTime, manualScaleSet[index - 1].newScale.x, manualScaleSet[index].newScale.x - manualScaleSet[index - 1].newScale.x, manualScaleSet[index].timeIndex),
                        easings_.EaseSineInOut(obstacleTime, manualScaleSet[index - 1].newScale.y, manualScaleSet[index].newScale.x - manualScaleSet[index - 1].newScale.y, manualScaleSet[index].timeIndex), 0);
                    }
                    else if (obstacleTime > manualScaleSet[index].timeIndex)
                    {
                        index++;
                        startTime = Time.time;
                        obstacleTime = Time.time - startTime;
                    }
                }

                //-----Color Setup-------------------------------------------------------
                for (int i = 0; i < objectsChildren.Length; i++)
                {

                    if (objectsChildren[i].gameObject.tag != "Obstacle")
                    {
                        objectsChildren[i].color = new Color(level_.levelObstaclesColor.r, level_.levelObstaclesColor.g, level_.levelObstaclesColor.b, 0.3f);
                    }
                    else if (objectsChildren[i].gameObject.tag == "Obstacle")
                    {
                        objectsChildren[i].color =
                            new Color(level_.levelObstaclesColor.r, level_.levelObstaclesColor.g, level_.levelObstaclesColor.b, 1.0f);
                    }

                }
                //-----------------------------------------------------------------------
            }
            else if (hasNoWarning != true)
            {
                if (index == 0)
                {
                    if (obstacleTime <= manualScaleSet[index].timeIndex)
                    {
                        obstacleWarning.transform.localScale = new Vector3(easings_.EaseSineInOut(obstacleTime, initialScale.x, manualScaleSet[index].newScale.x - initialScale.x, manualScaleSet[index].timeIndex),
                        easings_.EaseSineInOut(obstacleTime, initialScale.y, manualScaleSet[index].newScale.y - initialScale.y, manualScaleSet[index].timeIndex), 0);
                    }
                    else if (obstacleTime > manualScaleSet[index].timeIndex)
                    {
                        obstacle.transform.localScale = new Vector3(manualScaleSet[index].newScale.x, manualScaleSet[index].newScale.y, 0);
                        obstacleWarning.transform.localScale = new Vector3(0, 0, 0);

                        index++;
                        startTime = Time.time;
                        obstacleTime = Time.time - startTime;
                    }
                }
                else if (index > 0)
                {
                    if (obstacleTime <= manualScaleSet[index].timeIndex)
                    {
                        obstacle.transform.localScale = new Vector3(easings_.EaseSineInOut(obstacleTime, manualScaleSet[index - 1].newScale.x, manualScaleSet[index].newScale.x - manualScaleSet[index - 1].newScale.x, manualScaleSet[index].timeIndex),
                        easings_.EaseSineInOut(obstacleTime, manualScaleSet[index - 1].newScale.y, manualScaleSet[index].newScale.x - manualScaleSet[index - 1].newScale.y, manualScaleSet[index].timeIndex), 0);
                    }
                    else if (obstacleTime > manualScaleSet[index].timeIndex)
                    {
                        index++;
                        startTime = Time.time;
                        obstacleTime = Time.time - startTime;
                    }
                }

                //-----Color Setup-------------------------------------------------------
                if (startingColorValue_r > 0.01f && index > 0) startingColorValue_r = easings_.EaseSineOut(obstacleTime, (1 - level_.levelObstaclesColor.r), 0 - (1 - level_.levelObstaclesColor.r), 1.0f);
                if (startingColorValue_g > 0.01f && index > 0) startingColorValue_g = easings_.EaseSineOut(obstacleTime, (1 - level_.levelObstaclesColor.g), 0 - (1 - level_.levelObstaclesColor.g), 1.0f);
                if (startingColorValue_b > 0.01f && index > 0) startingColorValue_b = easings_.EaseSineOut(obstacleTime, (1 - level_.levelObstaclesColor.b), 0 - (1 - level_.levelObstaclesColor.b), 1.0f);

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
        else if (manualScaleSet.Length > 0 && index >= manualScaleSet.Length)
        {
            if (hasNoWarning != true) Destroy(obstacleWarning);
            Destroy(obstacle);
            Destroy(gameObject);
        }
    }
}
