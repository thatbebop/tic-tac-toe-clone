using TMPro;
using UnityEngine;
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

    private void Start()
    {
        ResetValues();

        GridScript.OnVictoryDetected += AddPointsAndCheck;
        GameManager.OnChangeOnGameState += UnderlinePlayerTurn;

        var gameManager = GameManager.instance;
        pointsToWin = gameManager.PointsToWin;

        player1txt = player1NameTxt.text;
        rivaltxt = rivalNameTxt.text;

        UnderlinePlayerTurn(GameState.PLAYER1_TURN);
    }

    void AddPointsAndCheck(SelectionType selectionType)
    {
        var userName = PLAYER_SELECTION[selectionType];
        if (userName == PLAYER1)
        {
            if (++player1Points == pointsToWin)
            {
                ResetValues();
            } else
                player1PointsTxt.text = player1Points.ToString();
        } else
        {
            if (++rivalPoints == pointsToWin)
            {
                ResetValues();
            } else
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

    void UnderlinePlayerTurn(GameState currentState)
    {
        switch (currentState)
        {
            case GameState.PLAYER1_TURN:
                player1NameTxt.text = "<u>" + player1txt + "</u>";
                rivalNameTxt.text = rivaltxt;
                break;
            case GameState.PLAYER2_TURN:
                rivalNameTxt.text = "<u>" + rivaltxt + "</u>";
                player1NameTxt.text = player1txt;
                break;
        }
    }
}