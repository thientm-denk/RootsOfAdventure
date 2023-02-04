using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleTweener : MonoBehaviour
{
    public LineRenderer line;
    public GameObject title1, title2, title3;
    // Start is called before the first frame update
    IEnumerator Start()
    {
        for (var i = 0; i < line.positionCount; i++)
        {
            float t = 0;
            while (t < 1)
            {
                float rootIndex = (float)i + t;
                AnimationCurve curve = new AnimationCurve();
                curve.AddKey(0, 1);
                curve.AddKey((rootIndex-1f)/line.positionCount, 0.9f);
                curve.AddKey((rootIndex)/line.positionCount, 0.2f);
                curve.AddKey((rootIndex+1f)/line.positionCount, 0f);
                curve.AddKey((rootIndex+2f)/line.positionCount, 0f);
                curve.AddKey(1, 0);
                curve.SmoothTangents(1, 1f);
                curve.SmoothTangents(2, 1f);
                curve.SmoothTangents(3, 1f);
                curve.SmoothTangents(4, 1f);
                line.widthCurve = curve;
                yield return null;
                t+= Time.deltaTime * 40f;
            }
        }
        title1.SetActive(true);
        yield return new WaitForSeconds(1f);
        title2.SetActive(true);
        yield return new WaitForSeconds(1f);
        title3.SetActive(true);
    }

}
