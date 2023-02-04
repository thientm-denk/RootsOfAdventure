using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{
    public float waterAmount;
    public Collider myCollider;
    public MeshRenderer myRenderer;
    public AnimationCurve drinkCurve;

    private void OnEnable()
    {
        waterAmount = transform.localScale.magnitude / 30f;
    }

    public void Drink()
    {
        myCollider.enabled = false;
        StartCoroutine(DrinkRoutine());
    }

    IEnumerator DrinkRoutine()
    {
        Material waterMaterial = myRenderer.material;
        float waterContent = 1;
        while (waterContent > 0)
        {
            waterMaterial.color = new Color(waterMaterial.color.r, waterMaterial.color.g, waterMaterial.color.b,
                drinkCurve.Evaluate(waterContent));
            waterContent -= Time.deltaTime;
            yield return null;
        }
    }
}