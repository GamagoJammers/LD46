using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class TestHealth : MonoBehaviour
{
    public Damageable ramHealth;
    public Image healthBar;


    private void Update()
    {
        healthBar.fillAmount = (float)ramHealth.m_healthPoints / (float)ramHealth.m_maxHealth;
        transform.LookAt(Camera.main.transform.position);
    }
}
