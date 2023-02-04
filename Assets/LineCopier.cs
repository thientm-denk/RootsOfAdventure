using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineCopier : MonoBehaviour
{
    public GameManager gM;
    public LineRenderer myLine, modelLine;
    public float thickness;

    // Update is called once per frame
    void Update()
    {
        //Get old Position Length
        Vector3[] newPos = new Vector3[modelLine.positionCount];
        //Get old Positions
        for (var i = 0; i < newPos.Length; i++)
        {
            newPos[i] = modelLine.GetPosition(i);
        }
        myLine.positionCount = modelLine.positionCount;
        myLine.SetPositions(newPos);
        myLine.widthCurve = modelLine.widthCurve;
        myLine.widthMultiplier = modelLine.widthMultiplier * thickness + 0.05f * (float)gM.CapStrength;
    }
}
