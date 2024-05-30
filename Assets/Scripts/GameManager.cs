using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState { MENU, GAME_START, PLAYER1_TURN, PLAYER2_TURN, CPU_TURN, PLAYER1_VICTORY, PLAYER2_VICTORY, CPU_VICTORY, DRAW }
public enum GameMode { PLAYERVSPLAYER, PLAYERVSCPU }
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private static GameState state;
    public static GameState CurrentGameState
    {
        get => state;
        set
        {
            if (state == value || (value != GameState.GAME_START && state == GameState.MENU))
            {
                return;
            }
            state = value;

            OnChangeOnGameState?.Invoke(state);
        }
    }
    private static GameMode gameMode;
    public static GameMode CurrentGameMode { get => gameMode; set => gameMode = value; }

    public int PointsToWin = 3;

    public delegate void ChangeOnGameState(GameState currentGameState);
    public static event ChangeOnGameState OnChangeOnGameState;

    private void Awake()
    {
        if (instance == null)
        {
            OnChangeOnGameState += GameStateChange;
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        state = GameState.PLAYER1_TURN;
        CurrentGameMode = GameMode.PLAYERVSPLAYER;
    }

    void GameStateChange(GameState currentGameState)
    {
        switch (currentGameState)
        {
            case GameState.MENU:
                SceneManager.LoadScene(0);
                break;
        }
    }
}
