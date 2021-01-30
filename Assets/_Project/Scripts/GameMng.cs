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
    public  float             timePassCurrentTurn = 0.0f;
    public  float             timeDurationPerTurn = 5.0f;

    public static GameMng Instance;

    public GameStates gameState;
    public GamePhases gamePhase;

    public Transform startPoint;
    

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
    }

    private void Start()
    {
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
        if( this.gameState == gameState )
            return;

        this.gameState = gameState;
        
        switch (gameState)
        {
            case GameStates.GameStart:
                SetGamePhase(GamePhases.TurnStart);
                SetGameState(GameStates.GamePlaying);
                break;
        }
    }

    public void SetGamePhase(GamePhases gamePhase)
    {
        if( this.gamePhase == gamePhase )
            return;

        this.gamePhase = gamePhase;

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
                SetGameState(GameStates.GameOver);
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
        
        if( Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
    }
}