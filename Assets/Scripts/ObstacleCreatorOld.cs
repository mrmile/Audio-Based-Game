using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[System.Serializable]
public struct ObstacleFrameParametersOld
{
    public Vector2 position; // set position of the obstacle
    public float zRotation; // set rotation of the obstacle in Z axis
    public Vector2 scale; // set scale of obstacle
    public float keyFrameTime; // sets the time to interpolate between the current and next keyFrame values
    public int easingTypeIndex; // set what type of easing will be used (from 0 to 30) to interpolate
    public Vector2 cameraShakeDistance; // make the camera move quickly the specified amount back and forth once right when the keyFrame is triggered to simulate shake
}

public class ObstacleCreatorOld : MonoBehaviour
{
    public bool isWarning = false; // if set to true it will not have a collider that damages the player (collider component deactivates)

    public float spawnAtTime = 0.0f; // determines when the obstacle will spawn (set active) relative to the level's start time

    public ObstacleFrameParameters[] obstacleKeyframes; // A list with all the keyframes change the obstacle will be affected by since its spawn. There is interpolation between them with easings. Obstacle is deleted when the last keyFrame is reached.

    public bool hasPositionParent = false; // this will make the obstacle's position += the parent's position and rotation += the parent's rotation
    public GameObject parent; // Set the obstacle's parent. Not needed if hasPositionParent is false.



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
