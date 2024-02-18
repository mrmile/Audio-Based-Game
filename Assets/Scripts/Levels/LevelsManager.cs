using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelsManager : MonoBehaviour
{
    public Color levelObstaclesColor;
    public Color levelBackgroundColor;

    public Camera camera;

    public float levelTime = 0;
    float startTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        levelTime = Time.time - startTime;

        camera.backgroundColor = levelBackgroundColor;
    }
}
