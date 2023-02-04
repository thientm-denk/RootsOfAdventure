using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public RootControler rootControler;
    public CameraControler cameraControler;
    public UIManager ui;

    public int cash;
    public int maxDepth;
    public float timer;
    public float maxTime;
    public bool newGamePlus;

    public List<int> capacities;
    public List<Capacity> capacitiyManagers;

    public float CapSpeed{get{return capacitiyManagers[0].values[capacities[0]];}}
    public float CapWater{get{return capacitiyManagers[1].values[capacities[1]];}}
    public float CapStrength{get{return capacitiyManagers[2].values[capacities[2]];}}
    public float CapUpward{get{return capacitiyManagers[3].values[capacities[3]];}}
    public float CapRoot{get{return capacitiyManagers[4].values[capacities[4]];}}

    private void Awake() {
        Load();
    }

    public void Load(){
        cash = PlayerPrefs.GetInt("Cash", 0);
        maxDepth = PlayerPrefs.GetInt("MaxDepth", 0);
        newGamePlus = PlayerPrefs.GetInt("NewGamePlus", 0) == 1;
        maxTime = PlayerPrefs.GetFloat("MaxTime", 10000);
        capacities[0] = PlayerPrefs.GetInt("Cap0", 0);
        capacities[1] = PlayerPrefs.GetInt("Cap1", 0);
        capacities[2] = PlayerPrefs.GetInt("Cap2", 0);
        capacities[3] = PlayerPrefs.GetInt("Cap3", 0);
        capacities[4] = PlayerPrefs.GetInt("Cap4", 0);
    }

    public void ResetProgress(){
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene(0);
    }
}
