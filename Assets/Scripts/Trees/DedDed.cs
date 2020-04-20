using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DedDed : MonoBehaviour
{
    public float timeToDed = 1.5f;
    private void OnEnable()
    {
        Destroy(gameObject, timeToDed);
    }
}
