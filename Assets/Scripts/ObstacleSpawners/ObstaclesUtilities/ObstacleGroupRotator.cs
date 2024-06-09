using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleGroupRotator : MonoBehaviour
{
    LevelsManager level_;
    R_Easings easings_;

    public float rotationSpeed;

    // Start is called before the first frame update
    void Start()
    {
        level_ = FindObjectOfType<LevelsManager>();
        easings_ = FindObjectOfType<R_Easings>();
    }

    // Update is called once per frame
    void Update()
    {
        if (rotationSpeed != 0)
        {
            gameObject.transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
        }
    }
}
