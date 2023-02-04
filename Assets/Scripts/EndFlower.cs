using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EndFlower : MonoBehaviour
{
    int petalCount = 0;
    int baseTone = 0;
    public List<Transform> flowerPetals;
    public List<Transform> flowers;
    public bool finishedGrowing = false;
    GameManager gM;

    public bool fix = false;

    public void Remove()
    {
        transform.localScale = Vector3.zero;
        foreach (Transform petal in flowerPetals)
        {
            petal.localScale = Vector3.zero;
        }
    }

    public void Initialize(int leftover, int baseT, GameManager _gM)
    {
        if (leftover == 0)
        {
            finishedGrowing = true;
            return;
        }

        baseTone = baseT;
        gM = _gM;
        petalCount = leftover;
        StartCoroutine(Grow());
        finishedGrowing = false;
        transform.forward = Vector3.forward;
        if (!fix)
            transform.Rotate(Vector3.up, Random.Range(-30f, 30f), Space.Self);
    }

    IEnumerator Grow()
    {
        flowers[0].localScale = Vector3.one;

        float t = 0;
        while (t < 1f)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one, t);
            t += Time.deltaTime * 5f * Mathf.Max(1, gM.rootControler.depth / 200f);
            yield return null;
        }

        for (var i = 0; i < petalCount; i++)
        {
            float tp = 0;
            while (tp < 1f)
            {
                flowerPetals[i].localScale = Vector3.Lerp(flowerPetals[i].localScale, Vector3.one, tp);
                tp += Time.deltaTime * 25f;
                yield return null;
            }

            if (i >= 4)
            {
                tp = 0;
                flowers[0].localScale = Vector3.one * 2f;
                gM.cash++;
                while (tp < 1f)
                {
                    flowers[0].localScale = Vector3.Lerp(flowers[0].localScale, Vector3.one, tp);
                    tp += Time.deltaTime * 10f;
                    yield return null;
                }
            }
        }

        finishedGrowing = true;
    }
}