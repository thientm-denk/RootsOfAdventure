using System.Collections;
using UnityEngine;

namespace Thien
{
    public class CameraController : MonoBehaviour
    {

        [SerializeField] private RootController rootController;
        
        public Camera mainCamera;
        public float lerpSpeed;
        public Vector2 border;


        private void Update()
        {
            if (rootController.dead)
            {
            }
            else
            {
                Vector3 targetPos = rootController.root.GetPosition(rootController.rootPointIndex - 1);
                targetPos = new Vector3(Mathf.Clamp(targetPos.x, border.x, border.y), targetPos.y,
                                mainCamera.transform.position.z) + Vector3.down * 3f;
                mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, targetPos, lerpSpeed);
            }

         
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
                Vector3 targetPos = rootController.root.GetPosition(rootController.rootPointIndex - 1);
                targetPos = new Vector3(Mathf.Clamp(targetPos.x, border.x, border.y), targetPos.y,
                    mainCamera.transform.position.z);
                mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, targetPos, lerpSpeed);
                yield return null;
            }

           

            for (var i = 0; i < rootController.rootPointIndex; i -= (int) (rootController.depth * 5f / 100f) - 1)
            {
                Vector3 targetPos = rootController.root.GetPosition(rootController.rootPointIndex - i);
                targetPos = new Vector3(Mathf.Clamp(targetPos.x, border.x, border.y), targetPos.y,
                    mainCamera.transform.position.z);
                mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, targetPos, lerpSpeed * 5f);
                float index = (float) rootController.rootPointIndex;
                float alpha = 1.0f;
                Gradient gradient = new Gradient();
                gradient.SetKeys(
                    new GradientColorKey[]
                    {
                        new GradientColorKey(rootController.rootBase, 0.0f),
                        new GradientColorKey(rootController.rootBase, (index - i) / index - 1f / index),
                        new GradientColorKey(rootController.rootEnd, (index - i) / index),
                        new GradientColorKey(rootController.rootTip, (index - i) / index + 1f / index),
                        new GradientColorKey(rootController.rootTip, 1.0f)
                    },
                    new GradientAlphaKey[] {new GradientAlphaKey(alpha, 0.0f), new GradientAlphaKey(alpha, 1.0f)}
                );
                rootController.root.colorGradient = gradient;
                yield return null;
            }

            Gradient endGradient = new Gradient();
            endGradient.SetKeys(
                new GradientColorKey[]
                {
                    new GradientColorKey(rootController.rootTip, 0.0f),
                    new GradientColorKey(rootController.rootTip, 1.0f)
                },
                new GradientAlphaKey[] {new GradientAlphaKey(1f, 0.0f), new GradientAlphaKey(1f, 1.0f)}
            );
            rootController.root.colorGradient = endGradient;
            

            while (rootController.dead)
            {
                t += Time.deltaTime;
                Vector3 targetPos = rootController.root.GetPosition(0);
                targetPos = new Vector3(Mathf.Clamp(targetPos.x, border.x, border.y), targetPos.y,
                    mainCamera.transform.position.z);
                mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position,
                    targetPos + Vector3.up * 4.2f, lerpSpeed * 2f);
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
                Vector3 targetPos = rootController.root.GetPosition(rootController.rootPointIndex - 1);
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
                Vector3 targetPos = rootController.root.GetPosition(rootController.rootPointIndex - 1);
                targetPos = new Vector3(Mathf.Clamp(targetPos.x, border.x, border.y), targetPos.y,
                    mainCamera.transform.position.z);
                Vector3 shake = Random.insideUnitCircle * 0.1f;
                targetPos += shake;
                mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, targetPos, 1f);
                yield return null;
            }

           

            for (var i = 0; i < rootController.rootPointIndex; i -= (int) (rootController.depth * 5f / 100f) - 1)
            {
                Vector3 targetPos = rootController.root.GetPosition(rootController.rootPointIndex - i);
                targetPos = new Vector3(Mathf.Clamp(targetPos.x, border.x, border.y), targetPos.y,
                    mainCamera.transform.position.z);
                mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, targetPos, lerpSpeed * 5f);
                float index = (float) rootController.rootPointIndex;
                float alpha = 1.0f;
                Gradient gradient = new Gradient();
                gradient.SetKeys(
                    new GradientColorKey[]
                    {
                        new GradientColorKey(rootController.rootBase, 0.0f),
                        new GradientColorKey(rootController.rootBase, (index - i) / index - 1f / index),
                        new GradientColorKey(rootController.rootMagic, (index - i) / index),
                        new GradientColorKey(rootController.rootMagic, (index - i) / index + 1f / index),
                        new GradientColorKey(rootController.rootMagic, 1.0f)
                    },
                    new GradientAlphaKey[] {new GradientAlphaKey(alpha, 0.0f), new GradientAlphaKey(alpha, 1.0f)}
                );
                rootController.root.colorGradient = gradient;
                yield return null;
            }

            Gradient endGradient = new Gradient();
            endGradient.SetKeys(
                new GradientColorKey[]
                {
                    new GradientColorKey(rootController.rootMagic, 0.0f),
                    new GradientColorKey(rootController.rootMagic, 1.0f)
                },
                new GradientAlphaKey[] {new GradientAlphaKey(1f, 0.0f), new GradientAlphaKey(1f, 1.0f)}
            );
            rootController.root.colorGradient = endGradient;
            
            while (rootController.dead)
            {
                t += Time.deltaTime;
                Vector3 targetPos = rootController.root.GetPosition(0);
                targetPos = new Vector3(Mathf.Clamp(targetPos.x, border.x, border.y), targetPos.y,
                    mainCamera.transform.position.z);
                mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position,
                    targetPos + Vector3.up * 4.2f, lerpSpeed * 2f);
                yield return null;
            }
        }
    }
}