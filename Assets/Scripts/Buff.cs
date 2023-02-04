using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff : MonoBehaviour
{
    public enum BuffType
    {
        Speed,Power
    }
    [SerializeField] private BuffType type;

    public BuffType Type => type;
}
