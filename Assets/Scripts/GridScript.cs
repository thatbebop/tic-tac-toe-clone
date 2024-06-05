using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using static StaticData;
using static UnityEditor.PlayerSettings;

public class GridScript : MonoBehaviour
{
    int width = 3, height = 3;
    Transform[,] data = null;

    [SerializeField]
    GridLayoutGroup layoutGroup;

    List<Transform> layoutChildren = new List<Transform>();

    Dictionary<Transform, SelectionTTT> selectionsData = new Dictionary<Transform, SelectionTTT>();

    public SelectionTTT selection;

    public delegate void VictoryDetected(SelectionType selectionType, string user);
    public static event VictoryDetected OnVictoryDetected;

    [SerializeField]
    AudioManager audioManager;


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GridSetUp());
        data = new Transform[width, height];
        OnVictoryDetected += VictorySetUp;
        GameManager.OnChangeOnGameState += ChangeGameState;
    }

    IEnumerator GridSetUp()
    {
        yield return new WaitForEndOfFrame();

        for (int i = 0; i < layoutGroup.transform.childCount; i++)
        {
            layoutChildren.Add(layoutGroup.transform.GetChild(i));
        }
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                data[j, i] = layoutChildren[i * height + j];
                selectionsData.Add(data[j, i], null);
            }
        }
    }

    public void EndTurn(Button btn)
    {
        SelectionType selType;
        switch (GameManager.CurrentGameState)
        {
            case GameState.PLAYER1_TURN:
                selType = PLAYER_SELECTION.Where(x => x.Value == PLAYER1).Select(x => x.Key).First();
                if (SetUpSelection(btn.transform, selType))
                {
                    if (!IsLineFormed(selType)) 
                    {
                        if (GameManager.CurrentGameState != GameState.DRAW)
                        {
                            if (GameManager.CurrentGameMode == GameMode.PLAYERVSPLAYER)
                                GameManager.CurrentGameState = GameState.PLAYER2_TURN;
                            else
                                GameManager.CurrentGameState = GameState.CPU_TURN;
                        }                            
                    }
                    else
                    {
                        GameManager.CurrentGameState = GameState.PLAYER1_GAMEWON;
                        OnVictoryDetected?.Invoke(selType, PLAYER1);
                    }
                }                    
                break;
            case GameState.PLAYER2_TURN:
                selType = PLAYER_SELECTION.Where(x => x.Value == PLAYER2).Select(x => x.Key).First();
                if (SetUpSelection(btn.transform, selType))
                {
                    if (!IsLineFormed(selType))
                    {
                        if (GameManager.CurrentGameState != GameState.DRAW)
                            GameManager.CurrentGameState = GameState.PLAYER1_TURN;
                    }                        
                    else
                    {
                        GameManager.CurrentGameState = GameState.PLAYER2_GAMEWON;
                        OnVictoryDetected?.Invoke(selType, PLAYER2);
                    }
                }                  
                break;
        }
    }

    void VictorySetUp(SelectionType selectionType, string user)
    {
        RestartGameState();
    }

    void ChangeGameState(GameState gameState)
    {
        if (gameState == GameState.DRAW || gameState == GameState.PLAYER1_VICTORY || gameState == GameState.PLAYER2_VICTORY) 
        {
            RestartGameState();
        }

        if (gameState == GameState.CPU_TURN)
        {
            var gameManager = GameManager.instance;
            var selType = PLAYER_SELECTION.Where(x => x.Value == PLAYER2).Select(x => x.Key).First();
            var pos = gameManager.cpu.MakeDecision(selectionsData);

            StartCoroutine(CPUDecisionDelay(pos, selType));
        }
    }

    IEnumerator CPUDecisionDelay(Transform pos, SelectionType selType)
    {
        var delay = Random.Range(0.6f, 1.4f);
        yield return new WaitForSeconds(delay);

        if (SetUpSelection(pos, selType))
        {
            if (!IsLineFormed(selType))
            {
                if (GameManager.CurrentGameState != GameState.DRAW)
                    GameManager.CurrentGameState = GameState.PLAYER1_TURN;
            }
            else
            {
                GameManager.CurrentGameState = GameState.PLAYER2_GAMEWON;
                OnVictoryDetected?.Invoke(selType, PLAYER2);
            }
        }
    }

    void RestartGameState()
    {
        ClearSelectionsData();
    }

    public void RematchClicked()
    {
        if (!(GameManager.CurrentGameState == GameState.PLAYER1_VICTORY || GameManager.CurrentGameState == GameState.PLAYER2_VICTORY))
        {
            if (PLAYER_SELECTION[CROSS] == PLAYER1)
            {
                PLAYER_SELECTION[CROSS] = PLAYER2;
                PLAYER_SELECTION[CIRCLE] = PLAYER1;

                if (GameManager.CurrentGameMode == GameMode.PLAYERVSPLAYER)
                {
                    GameManager.CurrentGameState = GameState.PLAYER2_TURN;
                }
                else
                    GameManager.CurrentGameState = GameState.CPU_TURN;

            }
            else
            {
                PLAYER_SELECTION[CROSS] = PLAYER1;
                PLAYER_SELECTION[CIRCLE] = PLAYER2;

                GameManager.CurrentGameState = GameState.PLAYER1_TURN;
            }
        }
        else
        {
            PLAYER_SELECTION[CROSS] = PLAYER1;
            PLAYER_SELECTION[CIRCLE] = PLAYER2;
        }
    }

    IEnumerator ChangeSelectionsDelay()
    {
        yield return null;
        if (!(GameManager.CurrentGameState == GameState.PLAYER1_VICTORY || GameManager.CurrentGameState == GameState.PLAYER2_VICTORY))
        {
            if (PLAYER_SELECTION[CROSS] == PLAYER1)
            {
                PLAYER_SELECTION[CROSS] = PLAYER2;
                PLAYER_SELECTION[CIRCLE] = PLAYER1;

                if (GameManager.CurrentGameMode == GameMode.PLAYERVSPLAYER)
                {
                    GameManager.CurrentGameState = GameState.PLAYER2_TURN;
                }
                else
                    GameManager.CurrentGameState = GameState.CPU_TURN;

            }
            else
            {
                PLAYER_SELECTION[CROSS] = PLAYER1;
                PLAYER_SELECTION[CIRCLE] = PLAYER2;

                GameManager.CurrentGameState = GameState.PLAYER1_TURN;
            }
        }
        else
        {
            PLAYER_SELECTION[CROSS] = PLAYER1;
            PLAYER_SELECTION[CIRCLE] = PLAYER2;
        }
    }

    void ClearSelectionsData()
    {
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                var pos = data[j, i];
                if (selectionsData[pos] != null)
                {
                    StartCoroutine(RemoveSelectionTransition(selectionsData[pos], pos));
                }
            }
        }
    }

    IEnumerator RemoveSelectionTransition(SelectionTTT selection, Transform pos)
    {
        var anim = selection.GetComponent<Animator>();
        anim.SetTrigger("RemoveSelection");
        yield return new WaitForSeconds(0.5f);
        Destroy(selection.gameObject);
        selectionsData[pos] = null;
    }

    bool SetUpSelection(Transform btnTransform, SelectionType selectionType)
    {
        var dataPos = GetDataPos(btnTransform.transform);

        if (selectionsData[data[(int)dataPos.x, (int)dataPos.y]] == null)
        {
            var plform = Instantiate(selection, btnTransform.position, Quaternion.identity);
            var anim = plform.GetComponent<Animator>();
            anim.SetTrigger("AddSelection");
            plform.selectionType = selectionType;
            plform.GetComponent<SpriteRenderer>().sprite = plform.selectionList[plform.selectionType];

            selectionsData[data[(int)dataPos.x, (int)dataPos.y]] = plform;

            if (selectionType == SelectionType.CROSS)
                audioManager.Play(CROSS_SND);
            else
                audioManager.Play(CIRCLE_SND);

            return true;
        }

        return false;
    }

    Vector2 GetDataPos(Transform btnTr)
    {
        for (int i = 0; i < height; ++i)
        {
            for (int j = 0; j < width; j++)
            {
                if (data[j, i] == btnTr)
                {
                    return new Vector2(j, i);
                }
            }
        }
        return Vector2.zero;
    }

    bool IsLineFormed(SelectionType selectionType)
    {
        var pLines = POSSIBLE_LINES;
        var countSelections = 0;

        for (int i = 0; i < pLines.Count; i++) 
        {
            for(int j = 0; j < pLines[i].Count; j++)
            {
                for(int xPos = 0; xPos < pLines[i][j].Length; xPos++)
                {
                    if (pLines[i][j][xPos] == 1 && selectionsData[data[xPos, j]] != null)
                        if (selectionsData[data[xPos, j]].selectionType == selectionType)
                            countSelections++;
                }
            }

            if (countSelections == 3)
                return true;
            else
                countSelections = 0;
        }

        if (selectionsData.Values.Count(x => x != null) == 9)
        {
            GameManager.CurrentGameState = GameState.DRAW;
        }
        return false;
    }
}
