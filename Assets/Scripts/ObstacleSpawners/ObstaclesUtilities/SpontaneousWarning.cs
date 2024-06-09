using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpontaneousWarning : MonoBehaviour
{
    LevelsManager level_;
    R_Easings easings_;

    public GameObject obstacleWarning;

    public Vector2 posXY;
    public Vector2 scaleXY;

    public float warningTime = 0;

    private float startTime = 0;
    private float obstacleTime = 0;

    private int step = 0;
    private float alpha = 0;

    private SpriteRenderer objectColor;

    // Start is called before the first frame update
    void Start()
    {
        level_ = FindObjectOfType<LevelsManager>();
        easings_ = FindObjectOfType<R_Easings>();

        obstacleWarning.transform.localScale = new Vector3(scaleXY.x, scaleXY.y, 0);
        obstacleWarning.transform.localPosition = new Vector3(posXY.x, posXY.y, 0);

        startTime = Time.time;

        //-----Color Setup-------------------------------------------------------
        objectColor = GetComponent<SpriteRenderer>();

        objectColor.color = new Color(level_.levelObstaclesColor.r, level_.levelObstaclesColor.g, level_.levelObstaclesColor.b, alpha);
        //-----------------------------------------------------------------------
    }

    // Update is called once per frame
    void Update()
    {
        obstacleTime = Time.time - startTime;

        if (step == 0 && obstacleTime <= 0.25f)
        {
            alpha = easings_.EaseSineIn(obstacleTime, 0.0f, 0.3f - 0.0f, 0.25f);
        }
        else if (step == 0 && obstacleTime > 0.25f)
        {
            step++;
            startTime = Time.time;
            obstacleTime = Time.time - startTime;
        }

        if (step == 1 && obstacleTime > warningTime)
        {
            step++;
            startTime = Time.time;
            obstacleTime = Time.time - startTime;
        }

        if (step == 2 && obstacleTime <= 0.1f)
        {
            alpha = easings_.EaseSineIn(obstacleTime, 0.3f, 0.0f - 0.3f, 0.1f);
        }
        else if (step == 2 && obstacleTime > 0.1f)
        {
            step++;
            startTime = Time.time;
            obstacleTime = Time.time - startTime;
        }

        if(step == 3)
        {
            Destroy(gameObject);
            step++;
        }

        //-----Color Setup-------------------------------------------------------
        objectColor.color = new Color(level_.levelObstaclesColor.r, level_.levelObstaclesColor.g, level_.levelObstaclesColor.b, alpha);
        //-----------------------------------------------------------------------
    }
}
