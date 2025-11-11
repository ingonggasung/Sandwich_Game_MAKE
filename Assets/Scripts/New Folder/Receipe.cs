using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Receipe : MonoBehaviour
{
    public ReceipeSet[] SandwichReceipe; // 샌드위치 레시피들
}

[System.Serializable]
public class ReceipeSet
{
    public string stepDescription; // 레시피 명 
    public GameObject[] ingredients; // 재료들

    public string Receipe { get; internal set; }
}
