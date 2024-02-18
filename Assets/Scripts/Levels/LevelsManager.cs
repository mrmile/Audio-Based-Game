using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class LevelsManager : MonoBehaviour
{
    R_Easings easings_;

    public Color levelObstaclesColor;
    public Color levelBackgroundColor;

    public Camera camera;

    public float levelTime = 0;
    float startTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        easings_ = GetComponent<R_Easings>();
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        levelTime = Time.time - startTime;

        camera.backgroundColor = levelBackgroundColor;
    }

    public void CameraDirectionalShake(Vector2 directionVector, float shakeDuration, float shakeIntensity)
    {
        //StartCoroutine(EaseShake(directionVector, shakeDuration, shakeIntensity));
        StartCoroutine(Shake(directionVector, shakeDuration, shakeIntensity));

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
}
