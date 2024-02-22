using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    LevelsManager level_;
    R_Easings easings_;

    public Vector2 initPos;
    public Vector2 finalMinPos;
    public Vector2 finalMaxPos;

    public GameObject bombParent;
    public GameObject bombSquare;
    public GameObject bombAfterSquare;
    public GameObject bombBullets;

    private float startTime = 0;
    private float obstacleTime = 0;

    private int step = 0;
    private Vector2 endPose;

    private int randRotationIndex = 0;
    private float resultRotationZ = 0;

    // Start is called before the first frame update
    void Start()
    {
        level_ = GetComponentInParent<LevelsManager>();
        easings_ = GetComponent<R_Easings>();

        endPose.x = Random.Range(finalMinPos.x, finalMaxPos.x);
        endPose.y = Random.Range(finalMinPos.y, finalMaxPos.y);

        randRotationIndex = Random.Range(0, 1);
        if (randRotationIndex == 0)
        {
            resultRotationZ = Random.Range(45, 180);
        }
        else if (randRotationIndex == 1)
        {
            resultRotationZ = Random.Range(-45, -180);
        }

        bombAfterSquare.transform.localScale = new Vector3(0, 0, 0);

        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        obstacleTime = Time.time - startTime;

        if (step == 0 && bombParent.transform.position.x != endPose.x)
        {
            bombParent.transform.position = new Vector3(easings_.EaseSineOut(obstacleTime, initPos.x, endPose.x - initPos.x, 1.2f),
                                                        easings_.EaseSineOut(obstacleTime, initPos.y, endPose.y - initPos.y, 1.2f), 0);

            bombParent.transform.eulerAngles = new Vector3(0, 0, easings_.EaseSineOut(obstacleTime, 0, resultRotationZ - 0, 1.2f));
        }
        else if(step == 0)
        {
            step++;
            startTime = Time.time;
            obstacleTime = Time.time - startTime;
        }

        if (step == 1)
        {
            //spawn bomb bullets
            for (int i = 0; i < bombBullets.transform.childCount; i++)
            {
                bombBullets.transform.GetChild(i).gameObject.SetActive(true);
            }

            step++;
            startTime = Time.time;
            obstacleTime = Time.time - startTime;
        }

        if (step == 2 && bombAfterSquare.transform.localScale.x < 1.5f)
        {
            bombAfterSquare.transform.localScale = new Vector3(easings_.EaseBackOut(obstacleTime, 0, 1.5f - 0, 1.2f),
                                                               easings_.EaseBackOut(obstacleTime, 0, 1.5f - 0, 1.2f),
                                                               easings_.EaseBackOut(obstacleTime, 0, 1.5f - 0, 1.2f));
        }
        else if (step == 3)
        {
            step++;
            startTime = Time.time;
            obstacleTime = Time.time - startTime;
        }

        if (step == 3 && bombAfterSquare.transform.localScale.x > 0.0f)
        {
            bombAfterSquare.transform.localScale = new Vector3(easings_.EaseBackIn(obstacleTime, 1.5f, 0 - 1.5f, 1.2f),
                                                               easings_.EaseBackIn(obstacleTime, 1.5f, 0 - 1.5f, 1.2f),
                                                               easings_.EaseBackIn(obstacleTime, 1.5f, 0 - 1.5f, 1.2f));
        }
        else if (step == 3)
        {
            step++;
            startTime = Time.time;
            obstacleTime = Time.time - startTime;
        }
    }
}
