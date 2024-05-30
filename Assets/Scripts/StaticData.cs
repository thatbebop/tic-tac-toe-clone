using System.Collections.Generic;

public class StaticData
{

    public static SelectionType CROSS = SelectionType.CROSS;
    public static SelectionType CIRCLE = SelectionType.CIRCLE;
    public static string PLAYER1 = "player1";
    public static string PLAYER2 = "player2";

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