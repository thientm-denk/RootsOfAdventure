using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControler : MonoBehaviour
{

    public GM gm;

    public Camera mainCamera;
    public float lerpSpeed;
    public Vector2 border;
    bool dead = false;

    public MeshRenderer soilMesh;
    public Gradient soilGradient;

    private void Update() {

        if(gm.rootControler.dead){
            
        }else {
            Vector3 targetPos = gm.rootControler.root.GetPosition(gm.rootControler.rootPointIndex-1);
            targetPos = new Vector3(Mathf.Clamp(targetPos.x, border.x, border.y), targetPos.y, mainCamera.transform.position.z) + Vector3.down * 3f;
            mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, targetPos, lerpSpeed);
        }

        float depthSoil = mainCamera.transform.position.y/-200f;
        soilMesh.material.color = soilGradient.Evaluate(depthSoil);
    }

    public void Death(){
            StartCoroutine(DeathCam());
    }


    IEnumerator DeathCam(){
        float t = 0;
        gm.sfx.DieSFX();
        while (t<1f)
        {
            t+= Time.deltaTime;
            Vector3 targetPos = gm.rootControler.root.GetPosition(gm.rootControler.rootPointIndex-1);
            targetPos = new Vector3(Mathf.Clamp(targetPos.x, border.x, border.y), targetPos.y, mainCamera.transform.position.z);
            mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, targetPos, lerpSpeed);
            yield return null;
        }
        gm.sfx.StartRise();
        
        for (var i = 0; i < gm.rootControler.rootPointIndex; i-=(int)(gm.rootControler.depth*5f/100f)-1)
        {
            Vector3 targetPos = gm.rootControler.root.GetPosition(gm.rootControler.rootPointIndex-i);
            targetPos = new Vector3(Mathf.Clamp(targetPos.x, border.x, border.y), targetPos.y, mainCamera.transform.position.z);
            mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, targetPos, lerpSpeed*5f);
            float index = (float)gm.rootControler.rootPointIndex;
            float alpha = 1.0f;
            Gradient gradient = new Gradient();
            gradient.SetKeys(
                new GradientColorKey[] { new GradientColorKey(gm.rootControler.rootBase, 0.0f),
                new GradientColorKey(gm.rootControler.rootBase, (index-i)/index - 1f/index),
                new GradientColorKey(gm.rootControler.rootEnd, (index-i)/index), 
                new GradientColorKey(gm.rootControler.rootTip, (index-i)/index + 1f/index),
                new GradientColorKey(gm.rootControler.rootTip, 1.0f) },
                new GradientAlphaKey[] { new GradientAlphaKey(alpha, 0.0f), new GradientAlphaKey(alpha, 1.0f) }
            );
            gm.rootControler.root.colorGradient = gradient;
            yield return null;
        }
        Gradient endGradient = new Gradient();
        endGradient.SetKeys(
            new GradientColorKey[] { new GradientColorKey(gm.rootControler.rootTip, 0.0f), new GradientColorKey(gm.rootControler.rootTip, 1.0f)},
            new GradientAlphaKey[] { new GradientAlphaKey(1f, 0.0f), new GradientAlphaKey(1f, 1.0f) }
        );
        gm.rootControler.root.colorGradient = endGradient;
        gm.ui.ShowDeathUI();
        gm.sfx.Blossom();

        while (gm.rootControler.dead)
        {
            t+= Time.deltaTime;
            Vector3 targetPos = gm.rootControler.root.GetPosition(0);
            targetPos = new Vector3(Mathf.Clamp(targetPos.x, border.x, border.y), targetPos.y, mainCamera.transform.position.z);
            mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, targetPos + Vector3.up * 4.2f, lerpSpeed * 2f);
            yield return null;
        }

        
    }
    public void End(){
        StartCoroutine(EndCam());
    }
    IEnumerator EndCam(){
        float t = 0;
        gm.sfx.EndSfx();
        while (t<1f)
        {
            t+= Time.deltaTime;
            Vector3 targetPos = gm.rootControler.root.GetPosition(gm.rootControler.rootPointIndex-1);
            targetPos = new Vector3(Mathf.Clamp(targetPos.x, border.x, border.y), targetPos.y, mainCamera.transform.position.z);
            mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, targetPos, lerpSpeed);
            yield return null;
        }
        t = 0;
        // Shake
        while (t<1f)
        {
            t+= Time.deltaTime;
            Vector3 targetPos = gm.rootControler.root.GetPosition(gm.rootControler.rootPointIndex-1);
            targetPos = new Vector3(Mathf.Clamp(targetPos.x, border.x, border.y), targetPos.y, mainCamera.transform.position.z);
            Vector3 shake = Random.insideUnitCircle * 0.1f;
            targetPos += shake;
            mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, targetPos, 1f);
            yield return null;
        }
        gm.sfx.StartRise();
        
        for (var i = 0; i < gm.rootControler.rootPointIndex; i-=(int)(gm.rootControler.depth*5f/100f)-1)
        {
            Vector3 targetPos = gm.rootControler.root.GetPosition(gm.rootControler.rootPointIndex-i);
            targetPos = new Vector3(Mathf.Clamp(targetPos.x, border.x, border.y), targetPos.y, mainCamera.transform.position.z);
            mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, targetPos, lerpSpeed*5f);
            float index = (float)gm.rootControler.rootPointIndex;
            float alpha = 1.0f;
            Gradient gradient = new Gradient();
            gradient.SetKeys(
                new GradientColorKey[] { new GradientColorKey(gm.rootControler.rootBase, 0.0f),
                new GradientColorKey(gm.rootControler.rootBase, (index-i)/index - 1f/index),
                new GradientColorKey(gm.rootControler.rootMagic, (index-i)/index), 
                new GradientColorKey(gm.rootControler.rootMagic, (index-i)/index + 1f/index),
                new GradientColorKey(gm.rootControler.rootMagic, 1.0f) },
                new GradientAlphaKey[] { new GradientAlphaKey(alpha, 0.0f), new GradientAlphaKey(alpha, 1.0f) }
            );
            gm.rootControler.root.colorGradient = gradient;
            yield return null;
        }
        Gradient endGradient = new Gradient();
        endGradient.SetKeys(
            new GradientColorKey[] { new GradientColorKey(gm.rootControler.rootMagic, 0.0f), new GradientColorKey(gm.rootControler.rootMagic, 1.0f)},
            new GradientAlphaKey[] { new GradientAlphaKey(1f, 0.0f), new GradientAlphaKey(1f, 1.0f) }
        );
        gm.rootControler.root.colorGradient = endGradient;
        gm.ui.ShowEndUI();

        while (gm.rootControler.dead)
        {
            t+= Time.deltaTime;
            Vector3 targetPos = gm.rootControler.root.GetPosition(0);
            targetPos = new Vector3(Mathf.Clamp(targetPos.x, border.x, border.y), targetPos.y, mainCamera.transform.position.z);
            mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, targetPos + Vector3.up * 4.2f, lerpSpeed * 2f);
            yield return null;
        }

        
    }
}
