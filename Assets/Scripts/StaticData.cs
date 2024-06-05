using System.Collections.Generic;

public class StaticData
{

    public static SelectionType CROSS = SelectionType.CROSS;
    public static SelectionType CIRCLE = SelectionType.CIRCLE;
    public static string TITLE = "Tic Tac Toe";
    public static string PLAYER1 = "Player 1";
    public static string PLAYER2 = "Player 2";
    public static int DEFAULTCONDWINS = 3;
    public static string MENU_BTN_SND = "MenuBtn";
    public static string CROSS_SND = "Cross";
    public static string CIRCLE_SND = "Circle";

    public static Dictionary<SelectionType, string> PLAYER_SELECTION = new Dictionary<SelectionType, string> { { CROSS, PLAYER1 }, { CIRCLE, PLAYER2 } };

    public static List<List<int[]>> POSSIBLE_LINES = new List<List<int[]>> {
        new List<int[]> 
        {
            new int[] { 1, 1, 1 },
            new int[] { 0, 0, 0 },
            new int[] { 0, 0, 0 }
        },
        new List<int[]>
        {
            new int[] { 0, 0, 0 },
            new int[] { 1, 1, 1 },
            new int[] { 0, 0, 0 }
        },
        new List<int[]>
        {
            new int[] { 0, 0, 0 },
            new int[] { 0, 0, 0 },
            new int[] { 1, 1, 1 }
        },
        new List<int[]>
        {
            new int[] { 1, 0, 0 },
            new int[] { 1, 0, 0 },
            new int[] { 1, 0, 0 }
        },
        new List<int[]>
        {
            new int[] { 0, 1, 0 },
            new int[] { 0, 1, 0 },
            new int[] { 0, 1, 0 }
        },
        new List<int[]>
        {
            new int[] { 0, 0, 1 },
            new int[] { 0, 0, 1 },
            new int[] { 0, 0, 1 }
        },
        new List<int[]>
        {
            new int[] { 1, 0, 0 },
            new int[] { 0, 1, 0 },
            new int[] { 0, 0, 1 }
        },
        new List<int[]>
        {
            new int[] { 0, 0, 1 },
            new int[] { 0, 1, 0 },
            new int[] { 1, 0, 0 }
        }

        };

}