using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Level_Select_UI_Text : MonoBehaviour
{
    [SerializeField] TMP_Text text;
    Scene_Manager manager;
    // Start is called before the first frame update
    void Start()
    {
        manager = FindObjectOfType<Scene_Manager>();
        
    }

    // Update is called once per frame
    void Update()
    {
        text.text = manager.selectedLevel.ToString();
    }
}
