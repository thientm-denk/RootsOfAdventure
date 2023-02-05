using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootControler : MonoBehaviour
{
    public GameManager gM;
    public float health = 10f;
    public float maxHealth = 10f;
    public LineRenderer root;
    public GameObject smallRootPrefab;
    public Camera mainCamera;
    public RootTarget rootTarget;
    public float rootTipLenght;
    public float timeUntilNewRootPoint;
    public float currentTimeUntilNewRootPoint;
    public int rootPointIndex = 1;

    public LayerMask raycastMask;

    public bool dead = true;
    public bool growing = false;
    public bool isSpeedBuff = false;
    public bool isPowerBuff = false;
    public List<GameObject> smallRoots;

    public Color rootBase, rootTip, rootWater, rootEnd, rootHurt, rootMagic;

    public float poisonned = 0f;

    public float depth = 0;
    Vector3[] startPositions;
    private void Start() {
        currentTimeUntilNewRootPoint = timeUntilNewRootPoint;
        health = maxHealth;
        startPositions = new Vector3[] {root.GetPosition(0), root.GetPosition(1), root.GetPosition(2)};
    }

    public void StartGame()
    {
        dead = false;
        growing = false;
        currentTimeUntilNewRootPoint = timeUntilNewRootPoint + 1f;
        health = maxHealth;
        root.positionCount = 3;
        rootPointIndex = 2;
        depth = 0;
        root.SetPositions(startPositions);
        gM.ui.HideDeathUI();
        gM.timer = 0;
        foreach (GameObject smallRoot in smallRoots)
        {
            Destroy(smallRoot);
        }

        smallRoots.Clear();
    }

    void FixedUpdate()
    {
        if (dead)
        {
            return;
        }

        UpdateRootTarget();
        UpdateRoot();
        UpdateRootWidth();
    }

    private void Update()
    {
        if (dead || !growing)
        {
            return;
        }

        UpdateHealth();
    }

    private void UpdateHealth()
    {
        gM.timer += Time.deltaTime;
        if (poisonned > 0f)
        {
            health -= Time.deltaTime * gM.CapStrength/2;
            poisonned -= Time.deltaTime;
        }

        health -= Time.deltaTime * gM.CapWater;
        if (health <= 0 && !dead)
        {
            dead = true;
            gM.cameraControler.Death();
        }
    }

    void UpdateRootTarget()
    {
        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);

        worldPosition = new Vector3(worldPosition.x, worldPosition.y, 0);

        rootTarget.transform.position = worldPosition;
    }


    void UpdateRoot()
    {
        UpdateRootTip();

        rootTarget.PowerIcon.SetActive(isPowerBuff);
        rootTarget.SpeedIcon.SetActive(isSpeedBuff);
        Vector3 lastPosition = root.GetPosition(rootPointIndex - 1);
        Vector3 direction = rootTarget.transform.position - lastPosition;
        direction = direction.normalized * rootTipLenght;

        // check if on rock
        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(lastPosition, direction, out hit, rootTipLenght, raycastMask))
        {
            if (hit.collider.tag == "Core")
            {
                dead = true;
                if (gM.timer < gM.maxTime || !gM.newGamePlus)
                {
                    gM.maxTime = gM.timer;
                    PlayerPrefs.SetFloat("MaxTime", gM.maxTime);
                }

                gM.cameraControler.End();
                return;
            }

            Water water = hit.collider.GetComponent<Water>();
            if (water != null)
            {
                water.Drink();
                health = Mathf.Min(health + water.waterAmount, maxHealth);
                Move();
                for (var i = 0; i < 3; i++)
                {
                    SmallRoot smallRoot = Instantiate(smallRootPrefab, transform).GetComponent<SmallRoot>();
                    smallRoot.Initialize(direction, root.GetPosition(root.positionCount - 3), gM,
                        0.5f + gM.CapRoot / 5f);
                    smallRoots.Add(smallRoot.gameObject);
                }
            }

            Poison poison = hit.collider.GetComponent<Poison>();
            if (poison != null)
            {
                poisonned = Mathf.Min(4f, poisonned + 1f);
                Move();
            }

            if (hit.collider.CompareTag("Booster"))
            {
                Buff buff = hit.collider.GetComponent<Buff>();
                if (buff.Type == Buff.BuffType.Speed)
                {
                    StopCoroutine(StartSpeedBuff());
                    StartCoroutine(StartSpeedBuff());
                }
                if (buff.Type == Buff.BuffType.Power)
                {
                    StopCoroutine(StartPowerBuff());
                    StartCoroutine(StartPowerBuff());
                }
                Destroy(hit.collider.gameObject);
            }

            if (isPowerBuff)
            {
                Destroy(hit.collider.gameObject);
            }
        }
        else
        {
            Move();
        }
    }

    void UpdateRootTip()
    {
        Vector3 lastPosition = root.GetPosition(rootPointIndex - 1);
        Vector3 direction = rootTarget.transform.position - lastPosition;
        direction = direction.normalized * rootTipLenght;
        root.SetPosition(rootPointIndex, lastPosition + direction);
    }

    void UpdateRootWidth()
    {
        float rootIndex = (float) rootPointIndex;
        AnimationCurve curve = new AnimationCurve();
        curve.AddKey(0, 1);
        curve.AddKey((rootIndex - 7f) / rootIndex, 0.9f);
        curve.AddKey((rootIndex - 6f) / rootIndex, 0.9f);
        curve.AddKey((rootIndex) / rootIndex, 0.2f);
        curve.AddKey(1, 0);
        curve.SmoothTangents(1, 1f);
        curve.SmoothTangents(2, 1f);
        root.widthCurve = curve;
        root.widthMultiplier = Mathf.Min(0.2f,root.positionCount * 0.0002f + 0.1f);


        float index = (float)rootPointIndex;
        float alpha = 1.0f;
        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[]
            {
                new GradientColorKey(rootBase, 0.0f),
                new GradientColorKey(rootBase, (index - health * 2f) / index - 1f / index),
                new GradientColorKey(poisonned <= 0 ? rootWater : rootHurt, (index - health * 2f) / index),
                new GradientColorKey(poisonned <= 0 ? rootWater : rootHurt, (index - health * 2f) / index + 1f / index),
                new GradientColorKey(poisonned <= 0 ? rootWater : rootHurt, 1.0f)
            },
            new GradientAlphaKey[] {new GradientAlphaKey(alpha, 0.0f), new GradientAlphaKey(alpha, 1.0f)}
        );
        root.colorGradient = gradient;
    }

    void Move()
    {
        growing = true;
        Vector3 lastPosition = root.GetPosition(rootPointIndex - 1);
        if (lastPosition.y < depth)
        {
            depth = lastPosition.y;
        }

        Vector3 direction = rootTarget.transform.position - lastPosition;
        if (direction.normalized.y < -gM.CapUpward)
        {
            currentTimeUntilNewRootPoint -= Time.fixedDeltaTime;
        }
        else
        {
            currentTimeUntilNewRootPoint -=
                Time.fixedDeltaTime * (Mathf.Max(0, (-direction.normalized.y + gM.CapUpward)));
        }


            if(currentTimeUntilNewRootPoint <= 0 && Vector3.Distance(rootTarget.transform.position, lastPosition)>rootTipLenght*5f){
                root.positionCount = root.positionCount + 1;
                rootPointIndex++;
                if(rootPointIndex%(int)(40f/gM.CapRoot+3f) == 0){
                    SmallRoot smallRoot = Instantiate(smallRootPrefab, transform).GetComponent<SmallRoot>();
                    smallRoot.Initialize(direction, root.GetPosition(root.positionCount-3), gM, 0.3f + gM.CapRoot/5f);
                    smallRoots.Add(smallRoot.gameObject);

                }
                currentTimeUntilNewRootPoint = timeUntilNewRootPoint * ( isSpeedBuff ? gM.CapSpeed /10 : gM.CapSpeed/2 ) ;
                UpdateRootTip();
                //gM.sfx.PlayGrowingSFX();
            }
    }

    public IEnumerator StartSpeedBuff()
    {
        isSpeedBuff = true;
        yield return new WaitForSeconds(2f);
        isSpeedBuff = false;
    }

    public IEnumerator StartPowerBuff()
    {
        isPowerBuff = true;
        yield return new WaitForSeconds(10f);
        isPowerBuff = false;
    }
}
