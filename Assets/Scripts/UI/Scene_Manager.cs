using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum sceneId
{
    TITLE,
    LEVEL_SELECTION,
    GAME_OVER,
    GAMEPLAY
}
public enum levelId
{
    LEVEL01,
    LEVEL02,
    LEVEL03
}
public class Scene_Manager : MonoBehaviour
{
    
    public sceneId selectedScene;
    levelId selectedLevel;
    int currentSelectedLevel = 0;
    string sceneName;


    private static Scene_Manager _instance;
    public static Scene_Manager Instance { get { return _instance; } }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
        DontDestroyOnLoad(Instance);
    }
   
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyUp(KeyCode.Escape) && selectedScene != sceneId.TITLE) LoadScene(sceneId.TITLE);
        
        else Application.Quit();

        switch(selectedScene)
        {
            case sceneId.TITLE:
                {
                    if(Input.GetKey(KeyCode.Space))
                    {
                        LoadScene(sceneId.LEVEL_SELECTION);
                    }
                    break;
                }
            case sceneId.LEVEL_SELECTION:
                {
                    if (Input.GetKeyDown(KeyCode.A))
                    {
                        ChangeGameplayLevel(false);
                    }
                    if (Input.GetKeyDown(KeyCode.D))
                    {
                        ChangeGameplayLevel(true);
                    }
                    if(Input.GetKey(KeyCode.Space))
                    {
                        LoadGameplayLevel();                  
                    }
                    break;
                }
            case sceneId.GAME_OVER:
                {
                    if(Input.GetKey(KeyCode.Space))
                    {
                        LoadGameplayLevel();
                    }
                    break;
                }
            default: break;
        }
        
        
    }
    
    public void LoadScene(sceneId id)
    {
        switch (id)
        {
            case sceneId.TITLE:
                {
                    sceneName = "Title_Scene";
                    selectedScene = sceneId.TITLE;
                    break;
                }
            case sceneId.LEVEL_SELECTION:
                {
                    sceneName = "Level_Selection";
                    selectedScene = sceneId.LEVEL_SELECTION;
                    break;
                }
            case sceneId.GAME_OVER:
                {
                    sceneName = "Game_Over_Scene";
                    selectedScene = sceneId.GAME_OVER;
                    break;
                }
            case sceneId.GAMEPLAY:
                {                  
                    selectedScene = sceneId.GAMEPLAY;
                    break;
                }

                
            default: break;
        }

        SceneManager.LoadScene(sceneName);
    }

    void ChangeGameplayLevel(bool right)
    {
        if (right)
        {
            if (currentSelectedLevel < System.Enum.GetValues(typeof(levelId)).Length - 1)
            {
                currentSelectedLevel++;
            }
            else currentSelectedLevel = 0;
        }
        else if (!right)
        {
            if (currentSelectedLevel > 0)
            {
                currentSelectedLevel--;
            }
            else currentSelectedLevel = System.Enum.GetValues(typeof(levelId)).Length - 1;
        }
    }

    void LoadGameplayLevel()
    {
        switch ((levelId)currentSelectedLevel)
        {
            case levelId.LEVEL01:
                sceneName = "Level_01";
                break;

            case levelId.LEVEL02:
                sceneName = "Level_02";
                break;

            case levelId.LEVEL03:
                sceneName = "Level_03";
                break;

            default: break;
        }
        LoadScene(sceneId.GAMEPLAY);
    }

    
}
