using System.Collections.Generic;
using UnityEngine;

public enum SelectionType { CIRCLE, CROSS };
public class SelectionTTT : MonoBehaviour
{
    public SelectionType selectionType;

    [SerializeField]
    List<Sprite> selectionTypeList;
    
    public Dictionary<SelectionType, Sprite> selectionList = new Dictionary<SelectionType, Sprite>();

    private void Awake()
    {
        selectionList.Add(SelectionType.CROSS, selectionTypeList[0]);
        selectionList.Add(SelectionType.CIRCLE, selectionTypeList[1]);
    }
}