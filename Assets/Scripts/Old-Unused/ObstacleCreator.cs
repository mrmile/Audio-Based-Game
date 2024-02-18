using System.Collections;
using UnityEngine;

[System.Serializable]
public struct ObstacleFrameParameters
{
    public Vector2 position; // set position of the obstacle
    public float zRotation; // set rotation of the obstacle in Z axis
    public Vector2 scale; // set scale of obstacle
    public float keyFrameTime; // sets the time to interpolate between the current and next keyFrame values
    public int easingTypeIndex; // set what type of easing will be used (from 0 to 30) to interpolate
    public Vector2 cameraShakeDistance; // make the camera move quickly the specified amount back and forth once right when the keyFrame is triggered to simulate shake
}

public class ObstacleCreator : MonoBehaviour
{
    public bool isWarning = false; // if set to true it will not have a collider that damages the player (collider component deactivates)

    public float spawnAtTime = 0.0f; // determines when the obstacle will spawn (set active) relative to the level's start time

    public ObstacleFrameParameters[] obstacleKeyframes; // A list with all the keyframes change the obstacle will be affected by since its spawn. There is interpolation between them with easings. Obstacle is deleted when the last keyFrame is reached.

    public bool hasPositionParent = false; // this will make the obstacle's position += the parent's position and rotation += the parent's rotation
    public GameObject parent; // Set the obstacle's parent. Not needed if hasPositionParent is false.

    private int currentKeyframeIndex = 0;
    private bool obstacleActive = false;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnObstacle());
    }

    // Update is called once per frame
    void Update()
    {
        if (obstacleActive)
        {
            //UpdateObstacle(); Not needed in this version
        }
    }

    IEnumerator SpawnObstacle()
    {
        yield return new WaitForSeconds(spawnAtTime);

        if (obstacleKeyframes.Length > 0)
        {
            obstacleActive = true;

            while (currentKeyframeIndex < obstacleKeyframes.Length - 1)
            {
                yield return StartCoroutine(InterpolateKeyframes());
            }

            obstacleActive = false;
            Destroy(gameObject);
        }
    }

    IEnumerator InterpolateKeyframes()
    {
        float startTime = obstacleKeyframes[currentKeyframeIndex].keyFrameTime;
        float endTime = obstacleKeyframes[currentKeyframeIndex + 1].keyFrameTime;
        float currentTime = Time.time - startTime;
        float progress = currentTime / (endTime - startTime);

        Vector2 newPosition = Vector2.Lerp(obstacleKeyframes[currentKeyframeIndex].position, obstacleKeyframes[currentKeyframeIndex + 1].position, progress);
        float newRotation = Mathf.Lerp(obstacleKeyframes[currentKeyframeIndex].zRotation, obstacleKeyframes[currentKeyframeIndex + 1].zRotation, progress);
        Vector2 newScale = Vector2.Lerp(obstacleKeyframes[currentKeyframeIndex].scale, obstacleKeyframes[currentKeyframeIndex + 1].scale, progress);

        Vector2 pivotOffset = new Vector2(0.5f, 0.5f);
        Vector2 pivotAdjustedPosition = newPosition - Vector2.Scale(pivotOffset, newScale);

        transform.position = hasPositionParent ? (Vector2)parent.transform.position + pivotAdjustedPosition : pivotAdjustedPosition;
        transform.rotation = Quaternion.Euler(0, 0, newRotation);
        transform.localScale = new Vector3(newScale.x, newScale.y, 1f);

        float shakeDuration = 0.1f;

        if (currentTime < startTime + shakeDuration)
        {
            // Camera shake logic...
        }

        if (currentTime >= endTime)
        {
            currentKeyframeIndex++;
        }

        yield return null;
    }
}
