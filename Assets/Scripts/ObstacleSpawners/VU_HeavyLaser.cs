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

    // Start is called before the first frame update
    void Start()
    {
        level_ = GetComponentInParent<LevelsManager>();
        easings_ = GetComponent<R_Easings>();

        float Xpos = Random.Range(minRandX, maxRandX);

        obstacleWarning.transform.position = new Vector3(Xpos, -5, 0);
        obstacle.transform.position = new Vector3(Xpos, -5, 0);

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

        if (step == 2 && obstacleTime > livingTime)
        {
            //obstacleWarning.transform.localScale = new Vector3(0, 20, 0);
            step++;
            startTime = Time.time;
            obstacleTime = Time.time - startTime;
        }

        if (obstacle.transform.localScale.x > 0 && step == 3)
        {
            obstacle.transform.localScale = new Vector3(easings_.EaseExpoIn(obstacleTime, width, 0 - width, 0.25f), height, 0);
            obstacleWarning.transform.localScale = new Vector3(easings_.EaseExpoIn(obstacleTime, width, 0 - width, 0.25f), height, 0);
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
    }
}
