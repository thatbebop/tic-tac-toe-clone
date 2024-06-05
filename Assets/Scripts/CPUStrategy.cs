using System.Collections.Generic;
using UnityEngine;

public interface CPUStrategy
{
    public Transform SelectPosition(Dictionary<Transform, SelectionTTT> selections); 
}