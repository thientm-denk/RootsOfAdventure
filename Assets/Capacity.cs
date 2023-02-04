using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Capacity : MonoBehaviour
{
    public GameManager gM;
    public int capacityID;
    public int level;

    public List<int> cost;
    public List<float> values;
    public List<Image> slots;

    public Button button;
    public Text costText;

    public Color on, off;

    public void Initialize()
    {
        level = gM.capacities[capacityID];
        for (var i = 0; i < slots.Count; i++)
        {
            slots[i].color = i >= level ? on : off;
        }

        if (level < cost.Count)
        {
            costText.text = "" + cost[level];
        }
        else
        {
            costText.text = "MAX";
        }
    }

    private void Update()
    {
        if (level < cost.Count)
        {
            button.interactable = gM.cash >= cost[level] && level < slots.Count;
        }
        else
        {
            button.interactable = false;
        }
    }

    public void Buy()
    {
        gM.cash -= cost[level];
        level++;
        PlayerPrefs.SetInt("Cap" + capacityID, level);
        gM.capacities[capacityID]++;
        for (var i = 0; i < slots.Count; i++)
        {
            slots[i].color = i >= level ? on : off;
        }

        if (level < cost.Count)
        {
            costText.text = "" + cost[level];
        }
        else
        {
            costText.text = "MAX";
        }
    }
}