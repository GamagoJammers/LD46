using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroWolfRandomPosition : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.Translate(Random.Range(-GameManager.instance.zoneRadius, GameManager.instance.zoneRadius) * Vector3.right) ;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
