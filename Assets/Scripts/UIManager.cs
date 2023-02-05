using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameManager gM;

    public Image healthBar;
    public Text depth;
    public Text max;
    public Text cashText;
    public GameObject deathUI;
    public GameObject upgradeUI;
    public GameObject restartButton;
    public GameObject scoreUI;
    public GameObject endUI;
    public GameObject tree;
    public Animator animator;
    public Text maxTime;
    public int lastCash = 0;
    public List<EndFlower> tiges;

    public List<Capacity> capacities;

    private void Update()
    {
        healthBar.fillAmount = gM.rootControler.health / gM.rootControler.maxHealth;
        if (gM.newGamePlus)
        {
            max.text = (float) ((int) (gM.timer * 100f)) / 100f + " s";
            maxTime.text = "Fastest : " + (float) ((int) (gM.maxTime * 100f)) / 100f + " s";
        }
        else
        {
            max.text = "max : " + (int) (gM.maxDepth) + " cm";
            maxTime.text = "";
        }

        depth.text = "" + (int) (gM.rootControler.depth * 10f) + " cm";
        if (gM.cash != lastCash)
        {
            cashText.text = "" + gM.cash;
            StartCoroutine(BopObject(cashText.transform));
            PlayerPrefs.SetInt("Cash", gM.cash);

            lastCash = gM.cash;
        }
    }

    public IEnumerator BopObject(Transform t)
    {
        float tp = 0;
        t.localScale = Vector3.one * 1.2f;
        while (tp < 1f)
        {
            t.localScale = Vector3.Lerp(t.localScale, Vector3.one, tp);
            tp += Time.deltaTime * 10f;
            yield return null;
        }
    }

    public void StartGame()
    {
        tree.transform.localScale = Vector3.one;
        animator.ResetTrigger("Re");
        animator.SetTrigger("Re");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ShowDeathUI()
    {
        StartCoroutine(ShowDeathUIRoutine());
    }

    public void ShowEndUI()
    {
        StartCoroutine(ShowEndUIRoutine());
    }

    public void HideDeathUI()
    {
        foreach (EndFlower tige in tiges)
        {
            tige.Remove();
        }

        upgradeUI.SetActive(false);
        restartButton.SetActive(false);
        endUI.SetActive(false);
    }

    IEnumerator ShowDeathUIRoutine()
    {
        deathUI.SetActive(true);
        tree.transform.localScale = Vector3.one * Mathf.Clamp( Mathf.Abs(gM.rootControler.depth/50) ,0.2f,1f);
        animator.ResetTrigger("Grow");
        animator.SetTrigger("Grow");

        for (var i = 0; i <= -(int) (gM.rootControler.depth / 20f); i++)
        {
            int leftover = Mathf.Min(20, -(int) (gM.rootControler.depth) - i * 20);
            tiges[0].Initialize(leftover, i * 20, gM);
            while (!tiges[0].finishedGrowing)
            {
                yield return null;
            }
        }

        foreach (Capacity cap in capacities)
        {
            cap.Initialize();
        }

        yield return new WaitForSeconds(1f);
        gM.maxDepth = Mathf.Min(gM.maxDepth, (int) gM.rootControler.depth * 10);
        restartButton.SetActive(true);
        upgradeUI.SetActive(true);
    }

    IEnumerator ShowEndUIRoutine()
    {
        scoreUI.SetActive(false);

        

        for (var i = 0; i <= 10; i++)
        {
            int leftover = Mathf.Min(20, 200 - i *5);
            tiges[i].Initialize(leftover, i * 5, gM);
            while (!tiges[i].finishedGrowing)
            {
                yield return null;
            }
        }

        foreach (Capacity cap in capacities)
        {
            cap.Initialize();
        }

        tiges[11].Initialize(1, 0, gM);
        if (!gM.newGamePlus)
        {
            yield return new WaitForSeconds(1f);
            endUI.SetActive(true);
            gM.newGamePlus = true;
            PlayerPrefs.SetInt("NewGamePlus", 1);
        }

        yield return new WaitForSeconds(1f);
        deathUI.SetActive(true);
        upgradeUI.SetActive(true);
        restartButton.SetActive(true);
    }
}