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

public enum levelNames
{
    Audiofrec__Brain_Stomp,
    ParagonX9__Chaoz_Impact,
    Cyron_xMainly__Losing_You___Not_Implemented
}

public class Scene_Manager : MonoBehaviour
{

    public sceneId selectedScene;
    public levelId selectedLevel;
    public levelNames songCurrentNames;

    int currentSelectedLevel = 0;
    string sceneName;
    bool currentGameplayLevelCompleted;

    int score;

    int hiScore01;
    int hiScore02;
    int hiScore03;

    [SerializeField] float inputDelay;
    float currentInputTime;
    bool canEnterInput;

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

        if (currentInputTime > 0)
        {
            currentInputTime -= Time.deltaTime;
            canEnterInput = false;
        }
        else
        {
            canEnterInput = true;
        }


        if (canEnterInput && Input.GetButtonDown("Cancel") && selectedScene != sceneId.TITLE)
        {
            currentInputTime = inputDelay;
            LoadScene(sceneId.TITLE);
        }

        switch (selectedScene)
        {
            case sceneId.TITLE:
                {
                    if (canEnterInput && Input.GetButtonDown("Jump"))
                    {
                        currentInputTime = inputDelay;
                        currentGameplayLevelCompleted = false;
                        LoadScene(sceneId.LEVEL_SELECTION);
                    }
                    break;
                }
            case sceneId.LEVEL_SELECTION:
                {
                    if (canEnterInput && (Input.GetAxisRaw("Horizontal") < -0.9f))
                    {
                        currentInputTime = inputDelay;
                        ChangeGameplayLevel(false);
                    }
                    if (canEnterInput && (Input.GetAxisRaw("Horizontal") > 0.9f))
                    {
                        currentInputTime = inputDelay;
                        ChangeGameplayLevel(true);
                    }
                    if (canEnterInput && Input.GetButtonDown("Jump"))
                    {
                        currentInputTime = inputDelay;
                        LoadGameplayLevel();
                    }
                    break;
                }
            case sceneId.GAME_OVER:
                {
                    if (canEnterInput && Input.GetButtonDown("Jump"))
                    {
                        currentInputTime = inputDelay;
                        currentGameplayLevelCompleted = false;
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
                    score = 0;
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

        selectedLevel = (levelId)currentSelectedLevel;
        songCurrentNames = (levelNames)currentSelectedLevel;

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

    public void SetCurrentLevelAsCompleted()
    {
        currentGameplayLevelCompleted = true;
    }
    public bool GetCurrentLevelCompleted()
    {
        return currentGameplayLevelCompleted;
    }
    public void CheckHiScore()
    {
        switch (selectedLevel)
        {
            case levelId.LEVEL01:
                {
                    if (score > hiScore01) hiScore01 = score;
                    break;
                }
            case levelId.LEVEL02:
                {
                    if (score > hiScore02) hiScore02 = score;
                    break;
                }
            case levelId.LEVEL03:
                {
                    if (score > hiScore03) hiScore03 = score;
                    break;
                }
            default: break;
        }
    }

    public void SetScore(int score)
    {
        this.score = score;
    }
    public int GetScore()
    {
        return score;
    }
    public int GetHiScore(levelId level)
    {
        switch(level)
        {
            case levelId.LEVEL01: return hiScore01;
            case levelId.LEVEL02: return hiScore02;
            case levelId.LEVEL03: return hiScore03;
            default: return 0;


        }

    }
}
