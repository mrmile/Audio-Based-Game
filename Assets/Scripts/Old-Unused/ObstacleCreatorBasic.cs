using System.Collections;
using UnityEngine;

public class ObstacleCreatorBasic : MonoBehaviour
{
    public bool isWarning = false; // if set to true it will not have a collider that damages the player (collider component deactivates)

    public float spawnAtTime = 0.0f; // determines when the obstacle will spawn (set active) relative to the level's start time

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
            //UpdateObstacle();
        }
    }

    IEnumerator SpawnObstacle()
    {
        yield return new WaitForSeconds(spawnAtTime);

        obstacleActive = true;
    }
}
