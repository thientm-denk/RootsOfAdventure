using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Pool", menuName = "ScriptableObjects/Level Pool", order = 1)]
public class LevelPool : ScriptableObject
{
    public List<GameObject> tiles;
}
