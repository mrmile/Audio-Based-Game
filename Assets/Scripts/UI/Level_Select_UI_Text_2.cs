using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Level_Select_UI_Text_2 : MonoBehaviour
{
    [SerializeField] TMP_Text nametext;
    Scene_Manager manager;
    // Start is called before the first frame update
    void Start()
    {
        manager = FindObjectOfType<Scene_Manager>();
        
    }

    // Update is called once per frame
    void Update()
    {
        nametext.text = manager.songCurrentNames.ToString();

    }
}
