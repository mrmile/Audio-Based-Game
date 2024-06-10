using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[System.Serializable]
public struct CustomStretch
{
    public float stretchTo;
    public int movingPieces; //0 is both, 1 is only left/up, 2 is only right/down
    public float shakeIntensity_Expand;
    public float waitTime;
}

public class Stretchers : MonoBehaviour
{
    LevelsManager level_;
    R_Easings easings_;

    public GameObject obstacleParent;
    public GameObject obstacleWarning;
    public GameObject obstacleSquare_1;
    public GameObject obstacleSquare_2;
    public GameObject obstacleStretchingBar;


    public Vector2 minPosXY;
    public Vector2 maxPosXY;
    public float stretchAmount;
    public int moveToDirection = 0; //0 is none, 1 is left, 2 is right
    public bool directionIsVertical = false;
    public float moveToDirectionTime = 10; //the higher the value the slower
    public int movingPieces = 0; //0 is both, 1 is only left/up, 2 is only right/down

    //public float livingTime = 0;
    public float warningTime = 0;
    public float waitTime = 0.5f;
    public float obstacleLivingTime = 1;
    public float shakeIntensity_Appear = 0.05f;
    public float shakeIntensity_Expand = 0.1f;

    public CustomStretch[] manualStretch;

    private float startTime = 0;
    private float obstacleTime = 0;

    private int step = 0;
    private int index = 0;
    private Vector3 lastPost_var1;
    private Vector3 lastPost_var2;
    private Vector3 lastPost_var3;
    private Vector3 lastScale_var1;

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
        obstacleSquare_1.transform.localScale = new Vector3(0, 0, 0);
        obstacleSquare_2.transform.localScale = new Vector3(0, 0, 0);
        obstacleStretchingBar.transform.localScale = new Vector3(0, 0, 0);

        obstacleWarning.transform.localPosition = new Vector3(Random.Range(minPosXY.x, maxPosXY.x), Random.Range(minPosXY.y, maxPosXY.y), 0);
        obstacleSquare_1.transform.localPosition = new Vector3(obstacleWarning.transform.localPosition.x, obstacleWarning.transform.localPosition.y + 0.35f, 0);
        obstacleSquare_2.transform.localPosition = new Vector3(obstacleWarning.transform.localPosition.x, obstacleWarning.transform.localPosition.y - 0.35f, 0);
        obstacleStretchingBar.transform.localPosition = new Vector3(obstacleWarning.transform.localPosition.x, obstacleWarning.transform.localPosition.y, 0);

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
        startingColorValue_r = 1 - level_.levelObstaclesColor.r;
        startingColorValue_g = 1 - level_.levelObstaclesColor.g;
        startingColorValue_b = 1 - level_.levelObstaclesColor.b;
        //-----------------------------------------------------------------------
    }

    // Update is called once per frame
    void Update()
    {
        obstacleTime = Time.time - startTime;

        if(step == 0 && obstacleTime <= 0.2f)
        {
            obstacleWarning.transform.localScale = new Vector3(easings_.EaseLinearNone(obstacleTime, 0, 1 - 0, 0.2f), easings_.EaseLinearNone(obstacleTime, 0, 1 - 0, 0.2f), 0);
        }
        else if(step == 0 && obstacleTime > warningTime)
        {
            if (shakeIntensity_Appear != 0)
            {
                level_.ShakeCamera(0.1f, shakeIntensity_Appear);
            }
            
            startingColorValue_r = 1 - level_.levelObstaclesColor.r;
            startingColorValue_g = 1 - level_.levelObstaclesColor.g;
            startingColorValue_b = 1 - level_.levelObstaclesColor.b;

            step++;
            startTime = Time.time;
            obstacleTime = Time.time - startTime;
        }

        if(step == 1 && obstacleTime <= 0.15f)
        {
            obstacleSquare_1.transform.localScale = new Vector3(easings_.EaseElasticOut(obstacleTime, 0, 0.5f - 0, 0.15f), easings_.EaseElasticOut(obstacleTime, 0, 0.5f - 0, 0.15f), 0);
            obstacleSquare_2.transform.localScale = new Vector3(easings_.EaseElasticOut(obstacleTime, 0, 0.5f - 0, 0.15f), easings_.EaseElasticOut(obstacleTime, 0, 0.5f - 0, 0.15f), 0);
            obstacleStretchingBar.transform.localScale = new Vector3(easings_.EaseElasticOut(obstacleTime, 0, 0.15f - 0, 0.15f), easings_.EaseElasticOut(obstacleTime, 0, 0.75f - 0, 0.15f), 0);
        }
        else if(step == 1 && obstacleTime > waitTime)
        {
            obstacleWarning.transform.localScale = new Vector3(0, 0, 0);
            obstacleWarning.gameObject.SetActive(false);

            if(manualStretch.Length == 0)
            {
                if (shakeIntensity_Expand != 0)
                {
                    level_.ShakeCamera(0.1f, shakeIntensity_Expand);
                }

                startingColorValue_r = 1 - level_.levelObstaclesColor.r;
                startingColorValue_g = 1 - level_.levelObstaclesColor.g;
                startingColorValue_b = 1 - level_.levelObstaclesColor.b;
            }

            step++;
            startTime = Time.time;
            obstacleTime = Time.time - startTime;
        }

        if(manualStretch.Length == 0)
        {
            if (step == 2 && obstacleTime <= 0.15f)
            {
                if (movingPieces == 0) //both
                {
                    obstacleSquare_1.transform.localPosition = new Vector3(obstacleSquare_1.transform.localPosition.x, easings_.EaseElasticOut(obstacleTime, obstacleSquare_1.transform.localPosition.y, (stretchAmount / 2) - obstacleSquare_1.transform.localPosition.y, 0.15f), 0);
                    obstacleSquare_2.transform.localPosition = new Vector3(obstacleSquare_2.transform.localPosition.x, easings_.EaseElasticOut(obstacleTime, obstacleSquare_2.transform.localPosition.y, (-stretchAmount / 2) - obstacleSquare_2.transform.localPosition.y, 0.15f), 0);
                    obstacleStretchingBar.transform.localScale = new Vector3(obstacleStretchingBar.transform.localScale.x, easings_.EaseElasticOut(obstacleTime, obstacleStretchingBar.transform.localScale.y, stretchAmount - obstacleStretchingBar.transform.localScale.y, 0.15f), 0);
                }
                else if (movingPieces == 1) //1 is left/up
                {
                    obstacleSquare_1.transform.localPosition = new Vector3(obstacleSquare_1.transform.localPosition.x, easings_.EaseElasticOut(obstacleTime, obstacleSquare_1.transform.localPosition.y, stretchAmount - obstacleSquare_1.transform.localPosition.y, 0.15f), 0);
                    //obstacleSquare_2.transform.localPosition = new Vector3(obstacleSquare_1.transform.localPosition.x, easings_.EaseElasticOut(obstacleTime, obstacleSquare_2.transform.localPosition.y, (stretchAmount / 2) - obstacleSquare_2.transform.localPosition.y, 0.15f), 0);
                    obstacleStretchingBar.transform.localScale = new Vector3(obstacleStretchingBar.transform.localScale.x, easings_.EaseElasticOut(obstacleTime, obstacleStretchingBar.transform.localScale.y, stretchAmount - obstacleStretchingBar.transform.localScale.y, 0.15f), 0);
                    obstacleStretchingBar.transform.localPosition = new Vector3(obstacleStretchingBar.transform.localPosition.x, easings_.EaseElasticOut(obstacleTime, obstacleStretchingBar.transform.localPosition.y, (stretchAmount / 2.2f) - obstacleStretchingBar.transform.localPosition.y, 0.15f), 0);
                }
                else if (movingPieces == 2) //2 is right/down
                {
                    //obstacleSquare_1.transform.localPosition = new Vector3(obstacleSquare_1.transform.localPosition.x, easings_.EaseElasticOut(obstacleTime, obstacleSquare_1.transform.localPosition.y, (obstacleSquare_1.transform.localPosition.y + (stretchAmount / 2)) - obstacleSquare_1.transform.localPosition.y, 0.15f), 0);
                    obstacleSquare_2.transform.localPosition = new Vector3(obstacleSquare_1.transform.localPosition.x, easings_.EaseElasticOut(obstacleTime, obstacleSquare_2.transform.localPosition.y, -stretchAmount - obstacleSquare_2.transform.localPosition.y, 0.15f), 0);
                    obstacleStretchingBar.transform.localScale = new Vector3(obstacleStretchingBar.transform.localScale.x, easings_.EaseElasticOut(obstacleTime, obstacleStretchingBar.transform.localScale.y, stretchAmount - obstacleStretchingBar.transform.localScale.y, 0.15f), 0);
                    obstacleStretchingBar.transform.localPosition = new Vector3(obstacleStretchingBar.transform.localPosition.x, easings_.EaseElasticOut(obstacleTime, obstacleStretchingBar.transform.localPosition.y, (-stretchAmount / 2.2f) - obstacleStretchingBar.transform.localPosition.y, 0.15f), 0);
                }
            }
            else if (step == 2 && obstacleTime > 0.15f)
            {
                if (moveToDirection == 0)
                {
                    step = 4;
                }
                else if (moveToDirection == 1) //left
                {
                    step++;
                }
                else if (moveToDirection == 2) //right
                {
                    step++;
                }
                startTime = Time.time;
                obstacleTime = Time.time - startTime;
            }

            if (step == 3 && obstacleTime <= moveToDirectionTime)
            {
                if(directionIsVertical == false)
                {
                    if (moveToDirection == 1) //left
                    {
                        obstacleParent.transform.localPosition = new Vector3(easings_.EaseSineIn(obstacleTime, obstacleParent.transform.localPosition.x, -25 - obstacleParent.transform.localPosition.x, moveToDirectionTime), obstacleParent.transform.localPosition.y, 0);
                    }
                    else if (moveToDirection == 2) //right
                    {
                        obstacleParent.transform.localPosition = new Vector3(easings_.EaseSineIn(obstacleTime, obstacleParent.transform.localPosition.x, 25 - obstacleParent.transform.localPosition.x, moveToDirectionTime), obstacleParent.transform.localPosition.y, 0);
                    }
                }
                else if (directionIsVertical == true)
                {
                    if (moveToDirection == 1) //left
                    {
                        obstacleParent.transform.localPosition = new Vector3(obstacleParent.transform.localPosition.x, easings_.EaseSineIn(obstacleTime, obstacleParent.transform.localPosition.y, -25 - obstacleParent.transform.localPosition.y, moveToDirectionTime), 0);
                    }
                    else if (moveToDirection == 2) //right
                    {
                        obstacleParent.transform.localPosition = new Vector3(obstacleParent.transform.localPosition.x, easings_.EaseSineIn(obstacleTime, obstacleParent.transform.localPosition.y, 25 - obstacleParent.transform.localPosition.y, moveToDirectionTime), 0);
                    }
                }
            }
            else if (step == 3 && obstacleTime > moveToDirectionTime)
            {
                step++;
                startTime = Time.time;
                obstacleTime = Time.time - startTime;
            }

            if (step == 4 && obstacleTime > obstacleLivingTime)
            {
                step++;
                startTime = Time.time;
                obstacleTime = Time.time - startTime;
            }

            
        }
        else if (manualStretch.Length > 0 && index < manualStretch.Length && step == 2)
        {
            if(index == 0)
            {
                lastPost_var1 = obstacleSquare_1.transform.localPosition;
                lastPost_var2 = obstacleSquare_2.transform.localPosition;
                lastPost_var3 = obstacleStretchingBar.transform.localPosition;
                lastScale_var1 = obstacleStretchingBar.transform.localScale;
            }

            if (manualStretch[index].movingPieces == 0 && obstacleTime <= 0.15f) //both
            {
                obstacleSquare_1.transform.localPosition = new Vector3(obstacleSquare_1.transform.localPosition.x, easings_.EaseElasticOut(obstacleTime, lastPost_var1.y, (manualStretch[index].stretchTo / 2) - lastPost_var1.y, 0.15f), 0);
                obstacleSquare_2.transform.localPosition = new Vector3(obstacleSquare_2.transform.localPosition.x, easings_.EaseElasticOut(obstacleTime, lastPost_var2.y, (-manualStretch[index].stretchTo / 2) - lastPost_var2.y, 0.15f), 0);
                obstacleStretchingBar.transform.localScale = new Vector3(obstacleStretchingBar.transform.localScale.x, easings_.EaseElasticOut(obstacleTime, lastScale_var1.y, stretchAmount - lastScale_var1.y, 0.15f), 0);
            }
            else if (manualStretch[index].movingPieces == 1 && obstacleTime <= 0.15f) //1 is left/up
            {
                obstacleSquare_1.transform.localPosition = new Vector3(obstacleSquare_1.transform.localPosition.x, easings_.EaseElasticOut(obstacleTime, lastPost_var1.y, manualStretch[index].stretchTo - lastPost_var1.y, 0.15f), 0);
                //obstacleSquare_2.transform.localPosition = new Vector3(obstacleSquare_2.transform.localPosition.x, easings_.EaseElasticOut(obstacleTime, obstacleSquare_1.transform.localPosition.y, (manualStretch[index].stretchAmount / 2) - obstacleSquare_1.transform.localPosition.y, 0.15f), 0);
                obstacleStretchingBar.transform.localScale = new Vector3(obstacleStretchingBar.transform.localScale.x, easings_.EaseElasticOut(obstacleTime, lastScale_var1.y, manualStretch[index].stretchTo - lastScale_var1.y, 0.15f), 0);
                obstacleStretchingBar.transform.localPosition = new Vector3(obstacleStretchingBar.transform.localPosition.x, easings_.EaseElasticOut(obstacleTime, lastPost_var3.y, (manualStretch[index].stretchTo / 2.2f) - lastPost_var3.y, 0.15f), 0);
            }
            else if (manualStretch[index].movingPieces == 2 && obstacleTime <= 0.15f) //2 is right/down
            {
                //obstacleSquare_1.transform.localPosition = new Vector3(obstacleSquare_1.transform.localPosition.x, easings_.EaseElasticOut(obstacleTime, obstacleSquare_1.transform.localPosition.y, (obstacleSquare_1.transform.localPosition.y + (manualStretch[index].stretchAmount / 2)) - obstacleSquare_1.transform.localPosition.y, 0.15f), 0);
                obstacleSquare_2.transform.localPosition = new Vector3(obstacleSquare_2.transform.localPosition.x, easings_.EaseElasticOut(obstacleTime, lastPost_var2.y, -manualStretch[index].stretchTo - lastPost_var2.y, 0.15f), 0);
                obstacleStretchingBar.transform.localScale = new Vector3(obstacleStretchingBar.transform.localScale.x, easings_.EaseElasticOut(obstacleTime, lastScale_var1.y, manualStretch[index].stretchTo - lastScale_var1.y, 0.15f), 0);
                obstacleStretchingBar.transform.localPosition = new Vector3(obstacleStretchingBar.transform.localPosition.x, easings_.EaseElasticOut(obstacleTime, lastPost_var3.y, (-manualStretch[index].stretchTo / 2.2f) - lastPost_var3.y, 0.15f), 0);
            }

            if (obstacleTime > manualStretch[index].waitTime)
            {
                lastPost_var1 = obstacleSquare_1.transform.localPosition;
                lastPost_var2 = obstacleSquare_2.transform.localPosition;
                lastPost_var3 = obstacleStretchingBar.transform.localPosition;
                lastScale_var1 = obstacleStretchingBar.transform.localScale;

                startingColorValue_r = 1 - level_.levelObstaclesColor.r;
                startingColorValue_g = 1 - level_.levelObstaclesColor.g;
                startingColorValue_b = 1 - level_.levelObstaclesColor.b;

                startTime = Time.time;
                obstacleTime = Time.time - startTime;
                index++;
            }
        }
        else if (manualStretch.Length > 0 && index >= manualStretch.Length && step == 2)
        {
            step = 5;
            startTime = Time.time;
            obstacleTime = Time.time - startTime;
        }

        if (step == 5 && obstacleTime <= 0.15f)
        {
            obstacleParent.transform.localScale = new Vector3(easings_.EaseSineIn(obstacleTime, obstacleParent.transform.localScale.x, 0 - obstacleParent.transform.localScale.x, 0.15f), easings_.EaseSineIn(obstacleTime, obstacleParent.transform.localScale.y, 0 - obstacleParent.transform.localScale.y, 0.15f), 0);
        }
        else if (step == 5 && obstacleTime > 0.15f)
        {
            step++;
        }

        if (step == 6)
        {
            Destroy(obstacleParent);
            Destroy(obstacleWarning);
            Destroy(obstacleSquare_1);
            Destroy(obstacleSquare_2);
            Destroy(obstacleSquare_1);
            Destroy(obstacleStretchingBar);

            step++;
        }

        //-----Color Setup-------------------------------------------------------
        if (startingColorValue_r > 0.01f ) startingColorValue_r = easings_.EaseSineOut(obstacleTime, (1 - level_.levelObstaclesColor.r), 0 - (1 - level_.levelObstaclesColor.r), 0.15f);
        if (startingColorValue_g > 0.01f ) startingColorValue_g = easings_.EaseSineOut(obstacleTime, (1 - level_.levelObstaclesColor.g), 0 - (1 - level_.levelObstaclesColor.g), 0.15f);
        if (startingColorValue_b > 0.01f ) startingColorValue_b = easings_.EaseSineOut(obstacleTime, (1 - level_.levelObstaclesColor.b), 0 - (1 - level_.levelObstaclesColor.b), 0.15f);

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
