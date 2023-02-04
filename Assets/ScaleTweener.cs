using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleTweener : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale = Vector3.one * (1f+ (Mathf.Sin(Time.time*10f) + 1f) * 0.03f);
    }
}
