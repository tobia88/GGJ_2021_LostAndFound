using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    TurnEnd
}

public class GameMng : MonoBehaviour
{
    public float timePassCurrentTurn = 0.0f;
    public float timeDurationPerTurn = 5.0f;

    public static GameMng Instance;

    public GameStates gameState;
    public GamePhases gamePhase;

    public Transform startPoint;
    public CharacterRecorder playerChar;

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
        playerChar.Reset();
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
                recorderMng.EnqueueSnaps(playerChar.snapDatas);
                SetGamePhase(GamePhases.TurnStart);
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
    }

    private void OnGUI()
    {
        if( !Application.isPlaying )
            return;

        float labelHeight = 25.0f;
        float spacing = 5.0f;
        
        GUI.Label(new Rect(0, 0, Screen.width, labelHeight ),"Game States: " + gameState);
        GUI.Label(new Rect(0, labelHeight + spacing, Screen.width, labelHeight ),"Time Passed: " + timePassCurrentTurn);
    }
}