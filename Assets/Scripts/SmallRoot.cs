using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallRoot : MonoBehaviour
{
    public LineRenderer root;
    Vector3 direction;
    float lifespan = 0.5f;
    int rootPointIndex = 2;
    float currentTimeUntilNewRootPoint = 0.08f;
    float timeUntilNewRootPoint = 0.08f;
    public float rootTipLenght = 0.1f;
    public LayerMask raycastMask;
    GameManager gM;

    Vector3 spawnPos;
    public void Initialize(Vector3 direc, Vector3 pos, GameManager _gM, float lifeSpan){
        direction = direc;
        root.SetPosition(0,pos);
        root.SetPosition(1,pos);
        root.SetPosition(2,pos);
        lifespan = lifeSpan * Random.value;
        spawnPos = pos;
        gM=_gM;
    }

    private void FixedUpdate() {
        if(lifespan<0){return;}
        Vector3 lastPosition = root.GetPosition(rootPointIndex-1);

        // check if on rock
        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(lastPosition, direction, out hit, rootTipLenght, raycastMask))
        {
            Water water = hit.collider.GetComponent<Water>();
            if(water!= null){
                water.Drink();
                gM.rootControler.health = Mathf.Min(gM.rootControler.health+water.waterAmount,gM.rootControler.maxHealth);
            }else{
                direction = -direction;
            }

        }
        else
        {
            currentTimeUntilNewRootPoint -= Time.fixedDeltaTime * 3f;
            lifespan -= Time.fixedDeltaTime;
            if(currentTimeUntilNewRootPoint <= 0){
                root.positionCount = root.positionCount + 1;
                rootPointIndex++;
                currentTimeUntilNewRootPoint = timeUntilNewRootPoint;
                Vector3 randomDirection = Random.insideUnitCircle;
                randomDirection += (root.GetPosition(root.positionCount-1)-spawnPos).normalized + Vector3.down * 1f;
                direction = Vector3.Lerp(direction, randomDirection, 0.08f);
            }
            direction = direction.normalized * rootTipLenght;
            root.SetPosition(rootPointIndex, lastPosition + direction);
        }
        
    }

    


}
