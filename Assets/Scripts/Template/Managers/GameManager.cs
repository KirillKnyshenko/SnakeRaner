using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameStage { StartWait, Game, EndWait };
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
        
    //Þçàáåëüíîå
    public static LevelManager currentLevel { get; private set; }
    public static GameObject player { get; private set; }
    public static Canvas canvas { get; private set; }

    public GameStage gameStage;
    
    //Äàííûå
    public GameDataObject.GDOMain data;
    GameDataObject gdata;



    /// Ýâåíòû 
    public static event System.Action StartGame = delegate { }; //Êîãäà gameStage ñòàíîâèòñÿ Game
    public static event System.Action EndGame = delegate { }; //Êîãäà gameStage ñòàíîâèòñÿ EndWait

    public static event System.Action TapToPlayUI = delegate { }; //Êîãäà èãðîê òàïàåò â ïåðâûé ðàç ïðè data.startByTap

    [SerializeField] private InterstitialAd _interstitialAd;

    #region Mono
    public void Awake()
    {
        StartGame = delegate { };
        EndGame = delegate { };
        TapToPlayUI = delegate { };
        
        QualitySettings.SetQualityLevel(QualitySettings.names.Length - 1);
        _interstitialAd.LoadAd();
    }
    private void Start()
    {
        instance = this;
        gdata = GameDataObject.GetData();
        data = gdata.main;
        OnLevelStarted(data);
        LoadLevel();
        
    }
    private void Update()
    {
        if (instance == null) instance = this;
        EditorControls();
        TapToStartCheck();
    }

    #endregion

    #region Gameplay
    public void TapToStartCheck() //Ïðîâåðêà íà âåðâûé òàï 
    {
        if (data.startByTap)
        {
            if (Input.GetKey(KeyCode.Mouse0))
            {
                if (gameStage == GameStage.StartWait)
                {
                    StartGameByTap();
                    gameStage = GameStage.Game;
                    TapToPlayUI();
                    StartGame();
                }
            }
        }
    }
    public void LoadLevel() //Ñîçäàíèå óðîâíÿ 
    {
        var stdData = GameDataObject.GetMain(true);
        if (stdData.saves == null){ Debug.LogError("Yaroslav: Saves Not Found"); return; }
        if (stdData.levelList == null || stdData.levelList.Count == 0) { Debug.LogError("Yaroslav: Levels List in \"" + GameDataObject.GetData(true).name + "\" is empty"); return; }

        stdData.saves.SetLevel((int)stdData.saves.GetPref(Prefs.Level));
        currentLevel = Instantiate(stdData.levelList[(int)stdData.saves.GetPref(Prefs.Level)]);
        //Èãðîê è êàíâàñ
        SpawnPlayer();
        SpawnCanvas();
    } 

    public void SpawnPlayer()
    {
        if (data.playerPrefab)
        {
            Vector3 spawnPoint = Vector3.zero;
            if (currentLevel.playerSpawn != null)
            {
                spawnPoint = currentLevel.playerSpawn.transform.position;
                //Íàñòðîéêà èãðîêà
            }
            else
            {
                Debug.LogError("Yaroslav: Spawn Not Found");
            }
            player = Instantiate(data.playerPrefab, spawnPoint, Quaternion.identity);
        }
    }

    public void SpawnCanvas()
    {
        if (data.canvas)
            canvas = Instantiate(data.canvas.gameObject, Vector3.zero, Quaternion.identity).GetComponent<Canvas>();
    }

    public void StopGamePlay() //Îñòàíîâêà èãðû (Èãðîêà è òä.) 
    {
        //Âûêëþ÷åíèå èãðîêà è äð.
    }
    public void StartGameByTap() //Âêëþ÷åíèå ïðè òàïå (Èãðîêà è òä.)
    {
        //Âêëþ÷åíèå óïðàâëåíèÿ è äð.
    }

    #endregion

    #region Editor
    public void EditorControls()
    {
        #if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Space))
        {
            NextLevel();
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            data.saves.AddToPref(Prefs.Points, 10);
        }

        #endif
    }
    #endregion

    #region Static
    public static void OnLevelStarted(GameDataObject.GDOMain data)
    {
        if (data.startByTap)
        {
            instance.gameStage = GameStage.StartWait;
            instance.StopGamePlay();
        }
        else
        {
            instance.gameStage = GameStage.Game;
            StartGame();
        }

        //Ñòàðò óðîâàíÿ 
    }
    public static void OnLevelEnd(bool win = true)
    {
        instance.StopGamePlay();
        instance.gameStage = GameStage.EndWait;
        EndGame();
        //Ýâåíòû ìåòðèê
        //Êîíåö óðîâíÿ
    }
    public static void Restart()
    {
        instance._interstitialAd.ShowAd();
        SceneManager.LoadScene(0);
    }
    public static void NextLevel()
    {
        var data = GameDataObject.GetMain();
        data.saves.SetPref(Prefs.Level, (int)data.saves.GetPref(Prefs.Level) + 1);
        data.saves.SetLevel((int)data.saves.GetPref(Prefs.Level));
        data.saves.AddToPref(Prefs.CompletedLevels, 1);
        SceneManager.LoadScene(0);
    }
    #endregion
}
