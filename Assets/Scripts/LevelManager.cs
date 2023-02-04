using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public List<LevelPool> levelPools;
    public Transform ldParent;

    GameObject lastTile;

    public List<GameObject> currentLD;
    public void Initialize(){
        StartCoroutine(InitRoutine());
    }

    IEnumerator InitRoutine(){
        foreach (GameObject oldTile in currentLD)
        {
            Destroy(oldTile);
        }
        
        float depth = -1f;

        foreach (LevelPool levelPool in levelPools)
        {
            GameObject randomTile = levelPool.tiles[Random.Range(0, levelPool.tiles.Count)];
            while (lastTile == randomTile)
            {
                yield return null;
                randomTile = levelPool.tiles[Random.Range(0, levelPool.tiles.Count)];
            }
            lastTile = randomTile;
            GameObject newTile = Instantiate(randomTile);
            newTile.transform.localPosition -= Vector3.down *depth;
            currentLD.Add(newTile);
            depth -= 5f;
        }
    }

}
