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
    public GameObject childBullets;

    public bool throwBullets = true;
    public bool throwSquareSnakes = false;

    private float startTime = 0;
    private float obstacleTime = 0;

    private int step = 0;
    private Vector2 endPose;

    private int randRotationIndex = 0;
    private float resultRotationZ = 0;
    private Color currentLevelBgColor;

    // Start is called before the first frame update
    void Start()
    {
        level_ = FindObjectOfType<LevelsManager>();
        easings_ = FindObjectOfType<R_Easings>();

        endPose.x = Random.Range(finalMinPos.x, finalMaxPos.x);
        endPose.y = Random.Range(finalMinPos.y, finalMaxPos.y);

        randRotationIndex = Random.Range(0, 1);
        if (randRotationIndex == 0)
        {
            resultRotationZ = Random.Range(180, 360);
        }
        else if (randRotationIndex == 1)
        {
            resultRotationZ = Random.Range(-180, -360);
        }

        bombAfterSquare.transform.localScale = new Vector3(0, 0, 0);

        currentLevelBgColor = level_.levelBackgroundColor;

        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        obstacleTime = Time.time - startTime;

        if (step == 0 && obstacleTime < 0.5f)
        {
            bombParent.transform.position = new Vector3(easings_.EaseSineOut(obstacleTime, initPos.x, endPose.x - initPos.x, 0.5f),
                                                        easings_.EaseSineOut(obstacleTime, initPos.y, endPose.y - initPos.y, 0.5f), 0);

            bombParent.transform.eulerAngles = new Vector3(0, 0, easings_.EaseSineOut(obstacleTime, 0, resultRotationZ - 0, 0.5f));
        }
        else if(step == 0)
        {
            step++;
            startTime = Time.time;
            obstacleTime = Time.time - startTime;
        }

        if (step == 1 && obstacleTime > 0.5f)
        {
            if(throwBullets == true)
            {
                //spawn bomb bullets
                for (int i = 0; i < childBullets.transform.childCount; i++)
                {
                    childBullets.transform.GetChild(i).gameObject.SetActive(true);
                }
            }
            else if (throwSquareSnakes == true)
            {
                //spawn square snakes
                for (int i = 0; i < childBullets.transform.childCount; i++)
                {
                    childBullets.transform.GetChild(i).gameObject.SetActive(true);
                }
            }

            bombSquare.transform.localScale = new Vector3(0, 0, 0);

            level_.levelBackgroundColor = new Color(150,150,150,255);
            Invoke("restablishBgColorFromFlashback", 0.025f);

            step++;
            startTime = Time.time;
            obstacleTime = Time.time - startTime;
        }

        if (step == 2 && obstacleTime < 0.25f)
        {
            bombAfterSquare.transform.localScale = new Vector3(easings_.EaseBackOut(obstacleTime, 0, 1.5f - 0, 0.25f),
                                                               easings_.EaseBackOut(obstacleTime, 0, 1.5f - 0, 0.25f),
                                                               easings_.EaseBackOut(obstacleTime, 0, 1.5f - 0, 0.25f));
        }
        else if (step == 2 && obstacleTime > 0.5f)
        {
            step++;
            startTime = Time.time;
            obstacleTime = Time.time - startTime;
        }

        if (step == 3 && obstacleTime < 0.25f)
        {
            bombAfterSquare.transform.localScale = new Vector3(easings_.EaseExpoIn(obstacleTime, 1.5f, 0 - 1.5f, 0.25f),
                                                               easings_.EaseExpoIn(obstacleTime, 1.5f, 0 - 1.5f, 0.25f),
                                                               easings_.EaseExpoIn(obstacleTime, 1.5f, 0 - 1.5f, 0.25f));
        }
        else if (step == 3)
        {
            step++;
            startTime = Time.time;
            obstacleTime = Time.time - startTime;

            Destroy(bombSquare);
            Destroy(bombAfterSquare);

            Invoke("destroyBombParent", 5.0f);
        }
    }

    void destroyBombParent()
    {
        Destroy(bombParent);
    }

    void restablishBgColorFromFlashback()
    {
        level_.levelBackgroundColor = currentLevelBgColor;
    }
}
