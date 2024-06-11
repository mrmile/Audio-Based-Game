using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
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

    public int hiScore01 = 0;
    public int hiScore02 = 0;
    public int hiScore03 = 0;

    [SerializeField] float inputDelay;
    float currentInputTime;
    bool canEnterInput;

    public Sprite fullStar;
    public Sprite emptyStar;

    public Image UIstar1;
    public Image UIstar2;
    public Image UIstar3;

    public TextMeshProUGUI scoreText;
    public GameObject completedLevelText;

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


        if (canEnterInput && Input.GetButtonDown("Cancel"))
        {
            if (selectedScene == sceneId.TITLE)
            {
                Application.Quit();
            }

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


        GameObject star1 = GameObject.Find("star1");
        if (selectedScene == sceneId.LEVEL_SELECTION && star1 != null)
        {
            print(star1);

            UIstar1 = star1.GetComponent<Image>();
            UIstar2 = GameObject.Find("star2").GetComponent<Image>();
            UIstar3 = GameObject.Find("star3").GetComponent<Image>();
            scoreText = GameObject.Find("scoretext").GetComponent<TextMeshProUGUI>();
            completedLevelText = GameObject.Find("completedText");

            print("completedLevelText " + completedLevelText);

            if (completedLevelText != null)
            completedLevelText.SetActive(false);


            switch (selectedLevel)
            {
                case levelId.LEVEL01:
                    {
                        if (hiScore01 >= 5)
                        {
                            UIstar1.sprite = fullStar;
                            UIstar2.sprite = fullStar;
                            UIstar3.sprite = fullStar;
                        }
                        else
                        {
                            UIstar1.sprite = fullStar;
                            UIstar2.sprite = fullStar;
                            UIstar3.sprite = emptyStar;
                        }

                        if (currentGameplayLevelCompleted)
                        {
                            scoreText.text = "You were hit " + (5 - hiScore01) + "times.";
                             if (completedLevelText != null)
                            completedLevelText.SetActive(true);

                        }
                    }

                    break;
                case levelId.LEVEL02:
                    {
                        if (hiScore02 >= 5)
                        {
                            UIstar1.sprite = fullStar;
                            UIstar2.sprite = fullStar;
                            UIstar3.sprite = fullStar;
                        }
                        else
                        {
                            UIstar1.sprite = fullStar;
                            UIstar2.sprite = fullStar;
                            UIstar3.sprite = emptyStar;
                        }
                        if (currentGameplayLevelCompleted)
                        {
                            scoreText.text = "You were hit " + (5 - hiScore03) + "times.";
                             if (completedLevelText != null)
                                completedLevelText.SetActive(true);

                        }
                    }
                    break;
                case levelId.LEVEL03:
                    {
                        if (hiScore03 >= 5)
                        {
                            UIstar1.sprite = fullStar;
                            UIstar2.sprite = fullStar;
                            UIstar3.sprite = fullStar;
                        }
                        else
                        {
                            UIstar1.sprite = fullStar;
                            UIstar2.sprite = fullStar;
                            UIstar3.sprite = emptyStar;
                        }
                        if (currentGameplayLevelCompleted)
                        {
                             if (completedLevelText != null)
                                completedLevelText.SetActive(true);
                            scoreText.text = "You were hit " + (5 - hiScore03) + "times.";

                        }
                    }
                    break;
                default:
                    break;
            }

            if (!currentGameplayLevelCompleted)
            {
                UIstar1.sprite = emptyStar;
                UIstar2.sprite = emptyStar;
                UIstar3.sprite = emptyStar;
            }
            else
                UIstar1.sprite = emptyStar;
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
        CheckHiScore();
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
