using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CPUStrategyEasy : CPUStrategy
{
    public Transform SelectPosition(Dictionary<Transform, SelectionTTT> selections)
    {
        var nullPos = selections.Where(x => x.Value == null).Select(x => x.Key).ToList();
        var pos = Random.Range(0, nullPos.Count);

        return nullPos[pos];
    }
}