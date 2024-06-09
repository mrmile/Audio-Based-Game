using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testScript : MonoBehaviour
{
    Scene_Manager manager;
    void Start()
    {
        manager = FindObjectOfType<Scene_Manager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.Space))
        {
            manager.LoadScene(sceneId.GAME_OVER);
        }
        if (Input.GetKey(KeyCode.Q))
        {
            manager.SetCurrentLevelAsCompleted();
        }
    }
}
