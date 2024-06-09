using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPosition : MonoBehaviour
{

    [SerializeField] Transform target;

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector2(target.position.x, target.position.y);
    }
}
