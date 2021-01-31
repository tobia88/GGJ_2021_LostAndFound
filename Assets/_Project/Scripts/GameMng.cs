using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameStates
{
    Null,
    GameStart,
    GamePlaying,
    NextLevel,
    GameOver
}

public enum GamePhases
{
    Null,
    TurnStart,
    TurnProgress,
    TurnEnd,
    TurnClean
}


public class GameMng : MonoBehaviour
{
    private CharacterRecorder _playerChar;
    public float timePassCurrentTurn = 0.0f;
    public float timeDurationPerTurn = 5.0f;

    public float TimeLeft => Mathf.Clamp(timeDurationPerTurn - timePassCurrentTurn, 0.0f, timeDurationPerTurn);

    public static GameMng Instance;

    public GameStates gameState;
    public GamePhases gamePhase;

    public Transform startPoint => levels[levelIndex].startPoint;

    public GameLevel[] levels;
    public int levelIndex;

    [Header("UI")] public GameObject creditAnim;

    public CharacterRecorder PlayerChar
    {
        get
        {
            if (_playerChar == null)
                _playerChar = FindObjectOfType<CharacterRecorder>();

            return _playerChar;
        }
    }

    [Header("Manager")] public RecorderManager recorderMng;

    private void Awake()
    {
        Instance = this;
        
        creditAnim.SetActive(false);
    }

    private void Start()
    {
        foreach (var level in levels)
        {
            level.gameObject.SetActive(false);
        }
        
        SetGameState(GameStates.GameStart);
    }

    public void Reset()
    {
        timePassCurrentTurn = 0.0f;
        FootprintStamper.ClearAll();
        PlayerChar.Reset();
    }

    public void SetGameState(GameStates gameState)
    {
        if (this.gameState == gameState)
            return;

        this.gameState = gameState;

        Debug.Log($"GameMng::Game State::{gameState}");

        switch (gameState)
        {
            case GameStates.GameStart:
                
                levels[levelIndex].gameObject.SetActive(true);
                timeDurationPerTurn = levels[levelIndex].timeDurationPerTurn;
                
                SetGamePhase(GamePhases.TurnStart);
                SetGameState(GameStates.GamePlaying);
                break;

            case GameStates.NextLevel:
                levels[levelIndex].gameObject.SetActive(false);
                
                levelIndex++;

                if (levelIndex >= levels.Length)
                {
                    SetGameState(GameStates.GameOver);
                }
                else
                {
                    // StartCoroutine(ScreenTransition());
                    SetGameState(GameStates.GameStart);
                }

                break;

            case GameStates.GameOver:
                creditAnim.SetActive(true);
                PlayerChar.DisableControl();
                break;
        }
    }

    public void SetGamePhase(GamePhases gamePhase)
    {
        if (this.gamePhase == gamePhase)
            return;

        this.gamePhase = gamePhase;

        Debug.Log($"GameMng::Game Phase::{gamePhase}");

        switch (gamePhase)
        {
            case GamePhases.TurnStart:
                Reset();
                recorderMng.SetupGhosts();
                SetGamePhase(GamePhases.TurnProgress);
                break;

            case GamePhases.TurnEnd:
                recorderMng.EnqueueSnaps(PlayerChar.snapDatas);
                SetGamePhase(GamePhases.TurnStart);
                break;

            case GamePhases.TurnClean:
                SetGameState(GameStates.NextLevel);
                break;
        }
    }

    private void Update()
    {
        if (gamePhase == GamePhases.TurnProgress)
        {
            timePassCurrentTurn += Time.deltaTime;

            if (timePassCurrentTurn >= timeDurationPerTurn)
            {
                SetGamePhase(GamePhases.TurnEnd);
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
            SceneManager.LoadScene(0);

        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
    }

    IEnumerator ScreenTransition()
    {
        yield return new WaitForSeconds(2.0f);
    }
}