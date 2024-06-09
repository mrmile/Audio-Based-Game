using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct CustomMovement
{
    public float timeIndex;
    public Vector2 newPosition;
}

public class ObstacleGroupMover : MonoBehaviour
{
    LevelsManager level_;
    R_Easings easings_;

    public Vector2 initialPosition;

    public CustomMovement[] manualPositionSet;

    private float startTime = 0;
    private float obstacleTime = 0;

    private int step = 0;
    private int index = 0;

    // Start is called before the first frame update
    void Start()
    {
        level_ = FindObjectOfType<LevelsManager>();
        easings_ = FindObjectOfType<R_Easings>();

        gameObject.transform.position = new Vector3(0, 0, 0);

        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        obstacleTime = Time.time - startTime;

        if(manualPositionSet.Length > 0 && index < manualPositionSet.Length)
        {
            if(index == 0)
            {
                if (obstacleTime <= manualPositionSet[index].timeIndex)
                {
                    gameObject.transform.position = new Vector3(easings_.EaseSineInOut(obstacleTime, initialPosition.x, manualPositionSet[index].newPosition.x - initialPosition.x, manualPositionSet[index].timeIndex), easings_.EaseSineInOut(obstacleTime, initialPosition.y, manualPositionSet[index].newPosition.y - initialPosition.y, manualPositionSet[index].timeIndex), 0);

                }
                else if (obstacleTime > manualPositionSet[index].timeIndex)
                {
                    index++;
                    startTime = Time.time;
                    obstacleTime = Time.time - startTime;
                }
            }
            else if (index > 0)
            {
                if (obstacleTime <= manualPositionSet[index].timeIndex)
                {
                    gameObject.transform.position = new Vector3(easings_.EaseSineInOut(obstacleTime, manualPositionSet[index - 1].newPosition.x, manualPositionSet[index].newPosition.x - manualPositionSet[index - 1].newPosition.x, manualPositionSet[index].timeIndex), easings_.EaseSineInOut(obstacleTime, manualPositionSet[index - 1].newPosition.y, manualPositionSet[index].newPosition.y - manualPositionSet[index - 1].newPosition.y, manualPositionSet[index].timeIndex), 0);

                }
                else if (obstacleTime > manualPositionSet[index].timeIndex)
                {
                    index++;
                    startTime = Time.time;
                    obstacleTime = Time.time - startTime;
                }
            }
        }
        else if(manualPositionSet.Length > 0 && index >= manualPositionSet.Length)
        {
            step++;
            startTime = Time.time;
            obstacleTime = Time.time - startTime;
        }

        if(step == 1)
        {
            if (transform.childCount > 0)
            {
                step++;
            } 
        }

        if (step == 2)
        {
            Destroy(gameObject);
        }
    }
}
