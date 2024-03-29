using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PulsingSquare : MonoBehaviour
{
    LevelsManager level_;
    R_Easings easings_;

    public GameObject obstacle;
    public GameObject obstacleWarning;

    public Vector2 minPosXY;
    public Vector2 maxPosXY;
    public Vector2 scale;

    public float rotationSpeed = 0;
    public float livingTime = 0;
    public float warningTime = 0;
    public float shakeIntensity = 0;

    public string dontDestroyAtEndIfParentNameIs = "_null_";
    private bool dontDestroyAtEnd = false; //turn it only true if used in a snake

    private float startTime = 0;
    private float obstacleTime = 0;

    private int step = 0;

    //private SpriteRenderer warningSprite;
    //private SpriteRenderer obstacleSprite;

    // Start is called before the first frame update
    void Start()
    {
        level_ = GetComponentInParent<LevelsManager>();
        easings_ = GetComponent<R_Easings>();

        //warningSprite = GetComponentInChildren<SpriteRenderer>();
        //obstacleSprite = GetComponentInChildren<SpriteRenderer>();
        //warningSprite.color = Color.blue;
        //obstacleSprite.color = Color.blue;

        obstacleWarning.transform.localScale = new Vector3(0, 0, 0);
        obstacle.transform.localScale = new Vector3(0, 0, 0);

        obstacle.transform.localPosition = new Vector3(Random.Range(minPosXY.x, maxPosXY.x), Random.Range(minPosXY.y, maxPosXY.y), 0);
        obstacleWarning.transform.localPosition = new Vector3(Random.Range(minPosXY.x, maxPosXY.x), Random.Range(minPosXY.y, maxPosXY.y), 0);

        if (gameObject.transform.parent != null)
        {
            if (gameObject.transform.parent.name == dontDestroyAtEndIfParentNameIs)
            {
                dontDestroyAtEnd = true;
            }
        }

        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        obstacleTime = Time.time - startTime;

        if(rotationSpeed != 0)
        {
            gameObject.transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
        }

        if (obstacleWarning.transform.localScale.x < scale.x)
        {
            obstacleWarning.transform.localScale = new Vector3(easings_.EaseLinearNone(obstacleTime, 0, scale.x - 0, 0.2f), easings_.EaseLinearNone(obstacleTime, 0, scale.y - 0, 0.2f), 0);
        }
        else if (step == 0 && obstacleTime > warningTime)
        {
            if (shakeIntensity != 0)
            {
                level_.ShakeCamera(0.1f, shakeIntensity);
            }

            step++;
            startTime = Time.time;
            obstacleTime = Time.time - startTime;
        }

        if (obstacleTime < 0.15f && step == 1)
        {
            obstacle.transform.localScale = new Vector3(easings_.EaseBackOut(obstacleTime, 0, scale.x - 0, 0.15f), easings_.EaseBackOut(obstacleTime, 0, scale.y - 0, 0.15f), 0);
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
            obstacle.transform.localScale = new Vector3(easings_.EaseExpoIn(obstacleTime, scale.x, 0 - scale.x, 0.25f), easings_.EaseExpoIn(obstacleTime, scale.y, 0 - scale.y, 0.25f), 0);
            obstacleWarning.transform.localScale = new Vector3(easings_.EaseExpoIn(obstacleTime, scale.x, 0 - scale.x, 0.25f), easings_.EaseExpoIn(obstacleTime, scale.y, 0 - scale.y, 0.25f), 0);
        }
        else if (step == 3)
        {
            step++;
            startTime = Time.time;
            obstacleTime = Time.time - startTime;
        }

        if (step == 4 && dontDestroyAtEnd == false)
        {
            Destroy(obstacleWarning);
            Destroy(obstacle);
            Destroy(gameObject);
        }
        else if (step == 4 && dontDestroyAtEnd == true)
        {
            gameObject.transform.position = new Vector3(99999, 99999, 99999);
        }
    }
}
