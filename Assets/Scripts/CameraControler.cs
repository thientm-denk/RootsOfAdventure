using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CameraControler : MonoBehaviour
{
    [FormerlySerializedAs("gm")] public GameManager gameManager;

    public Camera mainCamera;
    public float lerpSpeed;
    public Vector2 border;

    public MeshRenderer soilMesh;
    public Gradient soilGradient;

    private void Update()
    {
        if (gameManager.rootControler.dead)
        {
        }
        else
        {
            Vector3 targetPos =
                gameManager.rootControler.root.GetPosition(gameManager.rootControler.rootPointIndex - 1);
            targetPos = new Vector3(Mathf.Clamp(targetPos.x, border.x, border.y), targetPos.y,
                            mainCamera.transform.position.z) + Vector3.down * 3f;
            mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, targetPos, lerpSpeed);
        }

        float depthSoil = mainCamera.transform.position.y / -200f;
        soilMesh.material.color = soilGradient.Evaluate(depthSoil);
    }

    public void Death()
    {
        StartCoroutine(DeathCam());
    }


    IEnumerator DeathCam()
    {
        float t = 0;
        while (t < 1f)
        {
            t += Time.deltaTime;
            Vector3 targetPos =
                gameManager.rootControler.root.GetPosition(gameManager.rootControler.rootPointIndex - 1);
            targetPos = new Vector3(Mathf.Clamp(targetPos.x, border.x, border.y), targetPos.y,
                mainCamera.transform.position.z);
            mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, targetPos, lerpSpeed);
            yield return null;
        }
        

        for (var i = 0;
            i < gameManager.rootControler.rootPointIndex;
            i -= (int) (gameManager.rootControler.depth * 5f / 100f) - 1)
        {
            Vector3 targetPos =
                gameManager.rootControler.root.GetPosition(gameManager.rootControler.rootPointIndex - i);
            targetPos = new Vector3(Mathf.Clamp(targetPos.x, border.x, border.y), targetPos.y,
                mainCamera.transform.position.z);
            mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, targetPos, lerpSpeed * 5f);
            float index = (float) gameManager.rootControler.rootPointIndex;
            float alpha = 1.0f;
            Gradient gradient = new Gradient();
            gradient.SetKeys(
                new GradientColorKey[]
                {
                    new GradientColorKey(gameManager.rootControler.rootBase, 0.0f),
                    new GradientColorKey(gameManager.rootControler.rootBase, (index - i) / index - 1f / index),
                    new GradientColorKey(gameManager.rootControler.rootEnd, (index - i) / index),
                    new GradientColorKey(gameManager.rootControler.rootTip, (index - i) / index + 1f / index),
                    new GradientColorKey(gameManager.rootControler.rootTip, 1.0f)
                },
                new GradientAlphaKey[] {new GradientAlphaKey(alpha, 0.0f), new GradientAlphaKey(alpha, 1.0f)}
            );
            gameManager.rootControler.root.colorGradient = gradient;
            yield return null;
        }

        Gradient endGradient = new Gradient();
        endGradient.SetKeys(
            new GradientColorKey[]
            {
                new GradientColorKey(gameManager.rootControler.rootTip, 0.0f),
                new GradientColorKey(gameManager.rootControler.rootTip, 1.0f)
            },
            new GradientAlphaKey[] {new GradientAlphaKey(1f, 0.0f), new GradientAlphaKey(1f, 1.0f)}
        );
        gameManager.rootControler.root.colorGradient = endGradient;
        gameManager.ui.ShowDeathUI();

        while (gameManager.rootControler.dead)
        {
            t += Time.deltaTime;
            Vector3 targetPos = gameManager.rootControler.root.GetPosition(0);
            targetPos = new Vector3(Mathf.Clamp(targetPos.x, border.x, border.y), targetPos.y,
                mainCamera.transform.position.z);
            mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, targetPos + Vector3.up * 4.2f,
                lerpSpeed * 2f);
            yield return null;
        }
    }

    public void End()
    {
        StartCoroutine(EndCam());
    }

    IEnumerator EndCam()
    {
        float t = 0;
        while (t < 1f)
        {
            t += Time.deltaTime;
            Vector3 targetPos =
                gameManager.rootControler.root.GetPosition(gameManager.rootControler.rootPointIndex - 1);
            targetPos = new Vector3(Mathf.Clamp(targetPos.x, border.x, border.y), targetPos.y,
                mainCamera.transform.position.z);
            mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, targetPos, lerpSpeed);
            yield return null;
        }

        t = 0;
        // Shake
        while (t < 1f)
        {
            t += Time.deltaTime;
            Vector3 targetPos =
                gameManager.rootControler.root.GetPosition(gameManager.rootControler.rootPointIndex - 1);
            targetPos = new Vector3(Mathf.Clamp(targetPos.x, border.x, border.y), targetPos.y,
                mainCamera.transform.position.z);
            Vector3 shake = Random.insideUnitCircle * 0.1f;
            targetPos += shake;
            mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, targetPos, 1f);
            yield return null;
        }
        

        for (var i = 0;
            i < gameManager.rootControler.rootPointIndex;
            i -= (int) (gameManager.rootControler.depth * 5f / 100f) - 1)
        {
            Vector3 targetPos =
                gameManager.rootControler.root.GetPosition(gameManager.rootControler.rootPointIndex - i);
            targetPos = new Vector3(Mathf.Clamp(targetPos.x, border.x, border.y), targetPos.y,
                mainCamera.transform.position.z);
            mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, targetPos, lerpSpeed * 5f);
            float index = (float) gameManager.rootControler.rootPointIndex;
            float alpha = 1.0f;
            Gradient gradient = new Gradient();
            gradient.SetKeys(
                new GradientColorKey[]
                {
                    new GradientColorKey(gameManager.rootControler.rootBase, 0.0f),
                    new GradientColorKey(gameManager.rootControler.rootBase, (index - i) / index - 1f / index),
                    new GradientColorKey(gameManager.rootControler.rootMagic, (index - i) / index),
                    new GradientColorKey(gameManager.rootControler.rootMagic, (index - i) / index + 1f / index),
                    new GradientColorKey(gameManager.rootControler.rootMagic, 1.0f)
                },
                new GradientAlphaKey[] {new GradientAlphaKey(alpha, 0.0f), new GradientAlphaKey(alpha, 1.0f)}
            );
            gameManager.rootControler.root.colorGradient = gradient;
            yield return null;
        }

        Gradient endGradient = new Gradient();
        endGradient.SetKeys(
            new GradientColorKey[]
            {
                new GradientColorKey(gameManager.rootControler.rootMagic, 0.0f),
                new GradientColorKey(gameManager.rootControler.rootMagic, 1.0f)
            },
            new GradientAlphaKey[] {new GradientAlphaKey(1f, 0.0f), new GradientAlphaKey(1f, 1.0f)}
        );
        gameManager.rootControler.root.colorGradient = endGradient;
        gameManager.ui.ShowEndUI();

        while (gameManager.rootControler.dead)
        {
            t += Time.deltaTime;
            Vector3 targetPos = gameManager.rootControler.root.GetPosition(0);
            targetPos = new Vector3(Mathf.Clamp(targetPos.x, border.x, border.y), targetPos.y,
                mainCamera.transform.position.z);
            mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, targetPos + Vector3.up * 4.2f,
                lerpSpeed * 2f);
            yield return null;
        }
    }
}