using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class SquareBurst : MonoBehaviour
{

    LevelsManager level_;
    R_Easings easings_;

    [Header("Debug")]
    [SerializeField] float debugLineLength = 1.0f;

    [Header("Spawner Settings")]
    [SerializeField] float angleDirection;
    [SerializeField] float angleOffset;
    [SerializeField] Vector2 spawnArea = new Vector2(1,1);
    [SerializeField] int bulletCount;
    [SerializeField] float timeBetweenBullets;
    [Header("Bullet Settings")]
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Vector2 bulletSize = new Vector2(1,1);
    [SerializeField] float bulletSpeed;
    [SerializeField] [Tooltip("This will override Bullet Speed and use a random value between the values specified below")] bool randomBulletSpeed;
    [SerializeField] float minVal;
    [SerializeField] float maxVal;

    float currentTime;
    int currentBulletCount;
    Vector2 defaultDir = Vector2.up;

    // Start is called before the first frame update
    void Start()
    {
        currentTime = 0.1f;
    }

    private void OnDrawGizmos()
    {
        Vector2 pos = transform.position;

        Vector2 dir = Quaternion.AngleAxis(angleDirection, Vector3.forward) * defaultDir;

        Vector2 offset1 = Quaternion.AngleAxis(-angleOffset, Vector3.forward) * dir;
        Vector2 offset2 = Quaternion.AngleAxis(angleOffset, Vector3.forward) * dir;

        Gizmos.color = Color.blue;

        Gizmos.DrawLine(pos, pos + dir * debugLineLength);
        Gizmos.DrawLine(pos, pos + offset1 * debugLineLength);
        Gizmos.DrawLine(pos, pos + offset2 * debugLineLength);

        
        Gizmos.color = Color.magenta;
        Vector2 corner1 = new Vector2(pos.x - spawnArea.x, pos.y + spawnArea.y);
        Vector2 corner2 = new Vector2(pos.x + spawnArea.x, pos.y + spawnArea.y);
        Vector2 corner3 = new Vector2(pos.x + spawnArea.x, pos.y - spawnArea.y);
        Vector2 corner4 = new Vector2(pos.x - spawnArea.x, pos.y - spawnArea.y);
        Gizmos.DrawLine(corner1, corner2);
        Gizmos.DrawLine(corner2, corner3);
        Gizmos.DrawLine(corner3, corner4);
        Gizmos.DrawLine(corner4, corner1);

    }
    // Update is called once per frame
    void Update()
    {
        if (currentTime > 0) currentTime -= Time.deltaTime;
        else
        {
            float xOffset = spawnArea.x;
            float yOffset = spawnArea.y;

            Vector3 spawnPos = transform.position + new Vector3(Random.Range(-xOffset, xOffset), Random.Range(-yOffset, yOffset), 0);
            GameObject bullet = Instantiate(bulletPrefab,spawnPos, Quaternion.identity);
            bullet.transform.localScale = new Vector3(bulletSize.x, bulletSize.y, 1);

            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

            if (rb == null)
            {
                Debug.LogError("bullet prefab doesn't have rigidbody2d");
                return;
            }

            Vector2 dir = Quaternion.AngleAxis(angleDirection, Vector3.forward) * defaultDir;
            Vector2 rotatedDir = Quaternion.AngleAxis(Random.Range(-angleOffset, angleOffset), Vector3.forward) * dir;


            float bSpeed = (!randomBulletSpeed) ? bulletSpeed : Random.Range(minVal, maxVal);
            rb.velocity = rotatedDir.normalized * bSpeed;
            

            currentBulletCount++;
            currentTime = timeBetweenBullets;
        }

        if (currentBulletCount >= bulletCount) 
            Destroy(gameObject);
    }
}
