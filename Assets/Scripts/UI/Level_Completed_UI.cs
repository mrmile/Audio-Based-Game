using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level_Completed_UI : MonoBehaviour
{
    [SerializeField] GameObject completedImage;
    Scene_Manager manager;
    // Start is called before the first frame update
    void Start()
    {
        manager = FindObjectOfType<Scene_Manager>();
        completedImage.SetActive(manager.GetCurrentLevelCompleted());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
