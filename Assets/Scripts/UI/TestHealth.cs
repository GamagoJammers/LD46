using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class TestHealth : MonoBehaviour
{
    public int health = 10;
    public int maxHealth = 10;
    public UI healthbar;
    // Start is called before the first frame update
    void Start()
    {
        healthbar.setMaxHealth(maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        healthbar.setHealth(health);
    }
}
