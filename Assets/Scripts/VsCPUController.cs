using System.Collections.Generic;
using UnityEngine;

public enum CPUDifficulty { EASY, HARD }
public class VsCPUController : MonoBehaviour
{
    CPUDifficulty CPUDifficulty;
    CPUStrategy strategy;

    private void Start()
    {
        CPUDifficulty = CPUDifficulty.EASY;
        switch (CPUDifficulty)
        {
            case CPUDifficulty.EASY:
                strategy = new CPUStrategyEasy();
                break;
            default:
                strategy = new CPUStrategyEasy();
                break;
        }
    }

    public Transform MakeDecision(Dictionary<Transform, SelectionTTT> selectionsData)
    {
        return strategy.SelectPosition(selectionsData);
    }
}