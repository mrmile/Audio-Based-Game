using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleSnake : MonoBehaviour
{
    LevelsManager level_;
    R_Easings easings_;

    public Vector2 minStartPos;
    public Vector2 maxStartPos;

    public float startingRotation = 0;

    public GameObject gameObjectForSnaking;
    public GameObject levelGameObject;

    public float snakeLifeTime = 10.0f;
    public float snakeSpeedDelay = 0.5f;
    public float snakeGameObjectSeparation = 1.0f;

    public float minRotatedDirectionTendency = 0.0f;
    public float maxRotatedDirectionTendency = 0.0f;
    private float rotatedDirectionTendency = 0.0f;

    private float startTime = 0;
    private float obstacleTime = 0;
    private float obstacleSpawnTime = 0;
    private float startSpawnTime = 0;

    private int step = 0;

    // Start is called before the first frame update
    void Start()
    {
        level_ = GetComponentInParent<LevelsManager>();
        easings_ = GetComponent<R_Easings>();

        gameObjectForSnaking.transform.position = new Vector3(Random.Range(minStartPos.x, maxStartPos.x), Random.Range(minStartPos.y, maxStartPos.y), 0);
        gameObject.transform.position = new Vector3(Random.Range(minStartPos.x, maxStartPos.x), Random.Range(minStartPos.y, maxStartPos.y), 0);
        gameObject.transform.eulerAngles = new Vector3(0, 0, startingRotation);

        rotatedDirectionTendency = Random.Range(minRotatedDirectionTendency, maxRotatedDirectionTendency);

        startTime = Time.time;
        startSpawnTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        obstacleTime = Time.time - startTime;
        obstacleSpawnTime = Time.time - startSpawnTime;


        if(obstacleSpawnTime > snakeSpeedDelay && step == 0)
        {
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + snakeGameObjectSeparation, gameObject.transform.position.z);
            GameObject go = (GameObject)Instantiate(gameObjectForSnaking, gameObject.transform.position, transform.rotation);
            go.transform.parent = levelGameObject.transform;
            //currentPos.x += snakeGameObjectSeparation;


            startSpawnTime = Time.time;
            obstacleSpawnTime = Time.time - startSpawnTime;

            step = 1;
        }

        if (obstacleSpawnTime > snakeSpeedDelay && step == 1)
        {
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y - snakeGameObjectSeparation, gameObject.transform.position.z);
            GameObject go = (GameObject)Instantiate(gameObjectForSnaking, gameObject.transform.position, transform.rotation);
            go.transform.parent = levelGameObject.transform;
            //currentPos.x += snakeGameObjectSeparation;


            startSpawnTime = Time.time;
            obstacleSpawnTime = Time.time - startSpawnTime;

            step = 0;
        }

        gameObject.transform.Translate(Vector3.right * (snakeGameObjectSeparation * 10) * Time.deltaTime);
        gameObject.transform.Rotate(Vector3.forward * rotatedDirectionTendency * Time.deltaTime);

        if (obstacleTime > snakeLifeTime)
        {
            Destroy(gameObject);
        }
    }
}
