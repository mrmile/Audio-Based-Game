using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HL_StompingLaser : MonoBehaviour
{
    LevelsManager level_;
    R_Easings easings_;

    public GameObject obstacleWarning;
    public GameObject obstacle;

    public float width = 1;
    public float height = 1;
    public float minRandY = 0;
    public float maxRandY = 0;
    public float livingTime = 0;
    public float warningTime = 0;
    public float initialTravelTime = 2.0f;
    public float initialTravelDistance = 2.0f;

    private float startTime = 0;
    private float obstacleTime = 0;

    private int step = 0;

    // Start is called before the first frame update
    void Start()
    {
        level_ = GetComponentInParent<LevelsManager>();
        easings_ = GetComponent<R_Easings>();

        float Ypos = Random.Range(minRandY, maxRandY);

        obstacleWarning.transform.position = new Vector3(10.5f, Ypos, 0);
        obstacle.transform.position = new Vector3(10.5f, Ypos, 0);

        obstacleWarning.transform.localScale = new Vector3(0, 0, 0);
        obstacle.transform.localScale = new Vector3(0, 0, 0);

        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        obstacleTime = Time.time - startTime;

        if (obstacleWarning.transform.localScale.x < width)
        {
            obstacleWarning.transform.localScale = new Vector3(easings_.EaseExpoOut(obstacleTime, 0, width - 0, 0.2f), height, 0);
        }
        else if (step == 0 && obstacleTime > warningTime)
        {
            step++;
            startTime = Time.time;
            obstacleTime = Time.time - startTime;
        }

        if (obstacle.transform.localScale.y < initialTravelDistance && step == 1)
        {
            obstacle.transform.localScale = new Vector3(width, easings_.EaseLinearNone(obstacleTime, 0, initialTravelDistance - 0, initialTravelTime), 0);
        }
        else if (step == 1 && obstacleTime > initialTravelTime)
        {
            step++;
            startTime = Time.time;
            obstacleTime = Time.time - startTime;
        }

        if (obstacle.transform.localScale.y < height && step == 2)
        {
            obstacle.transform.localScale = new Vector3(width, easings_.EaseLinearNone(obstacleTime, initialTravelDistance, height - initialTravelDistance, 0.2f), 0);
        }
        else if (step == 2)
        {
            //obstacleWarning.transform.localScale = new Vector3(0, 20, 0);
            level_.CameraDirectionalShake(new Vector2(-0.65f, 0), 0.06f, 1.0f);
            step++;
            startTime = Time.time;
            obstacleTime = Time.time - startTime;
        }

        if (step == 3 && obstacleTime > livingTime)
        {
            //obstacleWarning.transform.localScale = new Vector3(0, 20, 0);
            step++;
            startTime = Time.time;
            obstacleTime = Time.time - startTime;
        }

        if (obstacle.transform.localScale.y > 0 && step == 4)
        {
            obstacle.transform.localScale = new Vector3(width, easings_.EaseExpoIn(obstacleTime, height, 0 - height, 0.2f), 0);
            obstacleWarning.transform.localScale = new Vector3(width, easings_.EaseExpoIn(obstacleTime, height, 0 - height, 0.2f), 0);
        }
        else if (step == 4)
        {
            step++;
            startTime = Time.time;
            obstacleTime = Time.time - startTime;
        }

        if (step == 5)
        {
            Destroy(obstacleWarning);
            Destroy(obstacle);
            Destroy(gameObject);
        }
    }
}
