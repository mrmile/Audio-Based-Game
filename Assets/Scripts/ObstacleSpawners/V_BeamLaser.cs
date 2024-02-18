using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class V_BeamLaser : MonoBehaviour
{
    LevelsManager level_;
    R_Easings easings_;

    public GameObject obstacleWarning;
    public GameObject obstacle;

    public float width = 1;
    public float minRandX = 0;
    public float maxRandX = 0;
    public float livingTime = 0;

    private float startTime = 0;
    private float obstacleTime = 0;

    private int step = 0;

    // Start is called before the first frame update
    void Start()
    {
        level_ = GetComponent<LevelsManager>();
        easings_ = GetComponent<R_Easings>();

        float Xpos = Random.Range(minRandX, maxRandX);
        
        obstacleWarning.transform.position = new Vector3(Xpos, 0, 0);
        obstacle.transform.position = new Vector3(Xpos, 0, 0);

        obstacleWarning.transform.localScale = new Vector3(0, 20, 0);
        obstacle.transform.localScale = new Vector3(0, 20, 0);

        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        obstacleTime = Time.time - startTime;

        Debug.Log("Width O W: " + obstacleWarning.transform.localScale.x);
        Debug.Log("Width O: " + obstacle.transform.localScale.x);

        if (obstacleWarning.transform.localScale.x < width)
        {
            obstacleWarning.transform.localScale = new Vector3(easings_.EaseExpoOut(obstacleTime, 0, width - 0, 0.5f), 20, 0);
        }
        else if (step == 0 && obstacleTime > 2)
        {
            step++;
            startTime = Time.time;
            obstacleTime = Time.time - startTime;
        }

        if (obstacle.transform.localScale.x < width && step == 1)
        {
            obstacle.transform.localScale = new Vector3(easings_.EaseExpoOut(obstacleTime, 0, width - 0, 0.5f), 20, 0);
        }
        else if (step == 1)
        {
            //obstacleWarning.transform.localScale = new Vector3(0, 20, 0);
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
            obstacle.transform.localScale = new Vector3(easings_.EaseExpoIn(obstacleTime, width, 0 - width, 0.5f), 20, 0);
            obstacleWarning.transform.localScale = new Vector3(easings_.EaseExpoIn(obstacleTime, width, 0 - width, 0.5f), 20, 0);
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
