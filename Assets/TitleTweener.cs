using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleTweener : MonoBehaviour
{
    public LineRenderer line;
    public GameObject title1, title2, title3;
    // Start is called before the first frame update
    private void Start()
    {
        title1.SetActive(true);
        title2.SetActive(true);
        title3.SetActive(true);
    }    

}
