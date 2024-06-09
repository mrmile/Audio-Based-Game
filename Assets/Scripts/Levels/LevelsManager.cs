using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class LevelsManager : MonoBehaviour
{
    R_Easings easings_;

    public Color levelObstaclesColor = Color.white;
    public Color levelBackgroundColor = Color.white;

    public Camera camera;
    private Vector3 originalCameraPosition;

    private Color lastLevelObstaclesColor = Color.white;
    private Color lastLevelBackgroundColor = Color.white;
    private Color newLevelObstaclesColor = Color.white;
    private Color newLevelBackgroundColor = Color.white;
    private bool obstacleColorChanging = false;
    private bool bgColorChanging = false;

    public float levelTime = 0;
    private float mechanicalTime = 0;
    float startTime = 0;
    float mechanicalStartTime = 0;
    private float changesTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        easings_ = FindObjectOfType<R_Easings>();
        startTime = Time.time;
        mechanicalStartTime = Time.time;

    }

    // Update is called once per frame
    void Update()
    {
        levelTime = Time.time - startTime;
        mechanicalTime = Time.time - mechanicalStartTime;

        camera.backgroundColor = levelBackgroundColor;

        if (obstacleColorChanging == true && mechanicalTime <= changesTime)
        {
            levelObstaclesColor = new Color(easings_.EaseLinearInOut(mechanicalTime, lastLevelObstaclesColor.r, newLevelObstaclesColor.r - lastLevelObstaclesColor.r, changesTime),
                easings_.EaseLinearInOut(mechanicalTime, lastLevelObstaclesColor.g, newLevelObstaclesColor.g - lastLevelObstaclesColor.g, changesTime),
                easings_.EaseLinearInOut(mechanicalTime, lastLevelObstaclesColor.b, newLevelObstaclesColor.b - lastLevelObstaclesColor.b, changesTime));
        }
        else if (obstacleColorChanging == true && mechanicalTime > changesTime)
        {
            obstacleColorChanging = false;
        }

        if (bgColorChanging == true)
        {
            levelBackgroundColor = new Color(easings_.EaseLinearInOut(mechanicalTime, lastLevelBackgroundColor.r, newLevelBackgroundColor.r - lastLevelBackgroundColor.r, changesTime),
                easings_.EaseLinearInOut(mechanicalTime, lastLevelBackgroundColor.g, newLevelBackgroundColor.g - lastLevelBackgroundColor.g, changesTime),
                easings_.EaseLinearInOut(mechanicalTime, lastLevelBackgroundColor.b, newLevelBackgroundColor.b - lastLevelBackgroundColor.b, changesTime));
        }
        else if (bgColorChanging == true && mechanicalTime > changesTime)
        {
            bgColorChanging = false;
        }
    }

    public void CameraDirectionalShake(Vector2 directionVector, float shakeDuration, float shakeIntensity)
    {
        //StartCoroutine(EaseShake(directionVector, shakeDuration, shakeIntensity));
        StartCoroutine(Shake(directionVector, shakeDuration, shakeIntensity));

    }

    public void ShakeCamera(float duration, float intensity)
    {
        StartCoroutine(Shake(duration, intensity));
    }

    IEnumerator Shake(float duration, float intensity)
    {
        originalCameraPosition = camera.transform.position;

        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            // Calculate a random offset within the specified intensity
            Vector3 offset = new Vector3(Random.Range(-1f, 1f) * intensity, Random.Range(-1f, 1f) * intensity, 0f);

            // Apply the offset to the camera's position
            camera.transform.position = originalCameraPosition + offset;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the final position is exactly the original position
        camera.transform.position = originalCameraPosition;
    }

    IEnumerator Shake(Vector2 directionVector, float shakeDuration, float shakeIntensity)
    {
        Vector3 originalPosition = camera.transform.position;
        Vector3 targetPosition = new Vector3(originalPosition.x + directionVector.x * shakeIntensity,
                                            originalPosition.y + directionVector.y * shakeIntensity,
                                            originalPosition.z);

        float elapsedTime = 0f;

        while (elapsedTime < shakeDuration)
        {
            // Interpolate between the original position and the target position over time
            camera.transform.position = Vector3.Lerp(originalPosition, targetPosition, elapsedTime / shakeDuration);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the final position is exactly the target position
        camera.transform.position = targetPosition;

        // Wait for a short duration before moving back to the original position
        //yield return new WaitForSeconds(0.1f); <--not needed

        // Smoothly interpolate back to the original position
        elapsedTime = 0f;
        while (elapsedTime < shakeDuration)
        {
            camera.transform.position = Vector3.Lerp(targetPosition, originalPosition, elapsedTime / shakeDuration);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the final position is exactly the original position
        camera.transform.position = originalPosition;
    }

    IEnumerator EaseShake(Vector2 directionVector, float shakeDuration, float shakeIntensity)
    {
        Vector3 originalPosition = camera.transform.position;
        Vector3 targetPosition = new Vector3(originalPosition.x + directionVector.x * shakeIntensity,
                                            originalPosition.y + directionVector.y * shakeIntensity,
                                            originalPosition.z);

        float elapsedTime = 0f;

        // Smoothly interpolate back to the original position
        elapsedTime = 0f;
        while (elapsedTime < shakeDuration)
        {
            camera.transform.position = new Vector3(easings_.EaseSineOut(elapsedTime, 0, directionVector.x - 0, shakeDuration / 2), easings_.EaseLinearNone(elapsedTime, 0, directionVector.y - 0, shakeDuration / 2), 0);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the final position is exactly the original position
        camera.transform.position = originalPosition;
    }

    public void ChangeObstacleColor(Color newObstaclesColor)
    {
        lastLevelObstaclesColor = levelObstaclesColor;
        newLevelObstaclesColor = newObstaclesColor;

        obstacleColorChanging = true;


        mechanicalStartTime = Time.time;
        mechanicalTime = Time.time - mechanicalStartTime;
    }

    public void ChangeLevelColor(Color newLevelColor)
    {
        lastLevelBackgroundColor = levelBackgroundColor;
        newLevelBackgroundColor = newLevelColor;

        bgColorChanging = true;


        mechanicalStartTime = Time.time;
        mechanicalTime = Time.time - mechanicalStartTime;
    }

    public void SetChangerTimer(float timeChangerSet)
    {
        changesTime = timeChangerSet;
    }
}
