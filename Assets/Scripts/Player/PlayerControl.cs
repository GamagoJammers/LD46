using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{

    public Damageable m_damageable;

    void Start()
    {
        
    }

    void Update()
    {
        if (m_damageable.CanMove())
        {

        }
    }
}
