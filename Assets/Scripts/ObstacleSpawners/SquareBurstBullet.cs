using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareBurstBullet : MonoBehaviour
{
    public float lifetime;
    public float rotationSpeed;
    Transform sprite;
    // Start is called before the first frame update
    void Start()
    {
        sprite = transform.GetChild(0);
    }

    // Update is called once per frame
    void Update()
    {
        sprite.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);

        if (lifetime > 0) lifetime -= Time.deltaTime;
        else Destroy(gameObject);
    }
}
