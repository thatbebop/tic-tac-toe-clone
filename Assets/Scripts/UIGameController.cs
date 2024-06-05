using System.Collections;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static StaticData;

public class UIGameController : MonoBehaviour
{
    [SerializeField]
    TMP_Text player1PointsTxt;
    [SerializeField]
    TMP_Text player1NameTxt;

    int player1Points = 0;
    string player1txt;

    [SerializeField]
    TMP_Text rivalPointsTxt;
    [SerializeField]
    TMP_Text rivalNameTxt;

    int rivalPoints = 0;
    string rivaltxt;

    int pointsToWin = 0;

    [SerializeField]
    GameObject popupPnl;

    [SerializeField]
    GameObject elementsBeforeMatch;

    [SerializeField]
    GameObject elementsAfterMatch;

    [SerializeField]
    AudioClip menu_btn_snd;

    [SerializeField]
    AudioManager audioManager;


    private void Start()
    {
        ResetValues();

        GridScript.OnVictoryDetected += AddPointsAndCheck;
        GameManager.OnChangeOnGameState += UnderlinePlayerTurn;
        GameManager.OnChangeOnGameState += GameStateChange;

        var gameManager = GameManager.instance;
        pointsToWin = gameManager.PointsToWin;

        player1txt = player1NameTxt.text;
        rivaltxt = rivalNameTxt.text;

        GameStateChange(GameManager.CurrentGameState);
    }

    void GameStateChange(GameState currentGameState)
    {
        switch (currentGameState)
        {
            case GameState.MENU:
                StartCoroutine(PopupPanelTransitionOpen(""));
                break;
            case GameState.GAME_START:
                SetUpGame();
                StartCoroutine(PopupPanelTransitionClose());
                break;
            case GameState.PLAYER1_GAMEWON:
                break;
            case GameState.PLAYER2_GAMEWON:
                break;
            case GameState.CPU_VICTORY:
                break;
        }
    }

    void SetUpGame()
    {
        var gameManager = GameManager.instance;

        if (int.TryParse(elementsBeforeMatch.GetComponentInChildren<TMP_InputField>().text, out var inputField))
        {
            gameManager.PointsToWin = inputField;
            pointsToWin = inputField;
        } else
        {
            gameManager.PointsToWin = DEFAULTCONDWINS;
            pointsToWin = DEFAULTCONDWINS;
        }

        var drpVal = elementsBeforeMatch.GetComponentInChildren<TMP_Dropdown>().value;

        if (drpVal == 0)
        {
            GameManager.CurrentGameMode = GameMode.PLAYERVSPLAYER;
        } else
            GameManager.CurrentGameMode = GameMode.PLAYERVSCPU;
    }

    void AddPointsAndCheck(SelectionType selectionType, string user)
    {
        if (user == PLAYER1)
        {
            if (++player1Points == pointsToWin)
            {
                StartCoroutine(PopupPanelTransitionOpen(PLAYER1));
                GameManager.CurrentGameState = GameState.PLAYER1_VICTORY;
            }
            player1PointsTxt.text = player1Points.ToString();
        } else
        {
            if (++rivalPoints == pointsToWin)
            {
                StartCoroutine(PopupPanelTransitionOpen(PLAYER2));
                if (GameManager.CurrentGameMode == GameMode.PLAYERVSPLAYER)
                    GameManager.CurrentGameState = GameState.PLAYER2_VICTORY;
                else
                    GameManager.CurrentGameState = GameState.CPU_VICTORY;
            }
            rivalPointsTxt.text = rivalPoints.ToString();
        }
    }

    void ResetValues()
    {
        player1PointsTxt.text = "0";
        rivalPointsTxt.text = "0";
        player1Points = 0;
        rivalPoints = 0;
    }

    IEnumerator PopupPanelTransitionOpen(string winner)
    {
       if (winner.Length > 0)
       {
            popupPnl.transform.GetChild(0).GetComponent<TMP_Text>().text = winner + " wins!";
            popupPnl.SetActive(true);
            elementsBeforeMatch.SetActive(false);
            elementsAfterMatch.SetActive(true);
            yield return new WaitForSeconds(0.5f);
       } 
       else
       {
            popupPnl.transform.GetChild(0).GetComponent<TMP_Text>().text = TITLE;
            popupPnl.SetActive(true);
            elementsAfterMatch.SetActive(false);
            elementsBeforeMatch.SetActive(true);
            yield return new WaitForSeconds(0.5f);
       }       
    }

    void UnderlinePlayerTurn(GameState currentState)
    {
        switch (currentState)
        {
            case GameState.PLAYER1_TURN:
                player1NameTxt.text = "<u>" + player1txt + "</u>";
                rivalNameTxt.text = rivaltxt;
                break;
            case GameState.CPU_TURN:
            case GameState.PLAYER2_TURN:
                rivalNameTxt.text = "<u>" + rivaltxt + "</u>";
                player1NameTxt.text = player1txt;
                break;
            default:
                player1NameTxt.text = player1txt;
                rivalNameTxt.text = rivaltxt;
                break;
        }
    }

    public void RematchSelected()
    {
        StartCoroutine(PopupPanelTransitionClose());
        audioManager.Play(MENU_BTN_SND);
    }

    public void MenuSelected()
    {
        GameManager.CurrentGameState = GameState.MENU;
        audioManager.Play(MENU_BTN_SND);
    }

    public void StartSelected()
    {
        GameManager.CurrentGameState = GameState.GAME_START;
        audioManager.Play(MENU_BTN_SND);
    }

    IEnumerator PopupPanelTransitionClose()
    {
        if (popupPnl.activeSelf)
        {
            popupPnl.GetComponent<Animator>().SetTrigger("VictPanelRem");
            yield return new WaitForSeconds(0.5f);
            popupPnl.SetActive(false);
            yield return new WaitForSeconds(0.2f);
            GameManager.CurrentGameState = GameState.PLAYER1_TURN;
            ResetValues();
        }
    }
}