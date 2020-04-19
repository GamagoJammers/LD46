﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UI : MonoBehaviour
{
    private float Timer = 0.0f;
    float minutes;
    float seconds;
    public Text timerText;
    public GameObject pauseMenu;
    public Slider slider;
    // Start is called before the first frame update
    void Start()
    {
        pauseMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.instance.isPaused)
        {
            Timer += Time.deltaTime;
            seconds = Timer % 60;
            minutes = Timer / 60;
        }
        timerText.text = minutes.ToString("00") + ":" + seconds.ToString("00");
        PauseMenu();
        if (pauseMenu.activeSelf)
        {
            GameManager.instance.isPaused = true;
        }
        else if (!pauseMenu.activeSelf)
        {
            GameManager.instance.isPaused = false;
        }
    }

    private void PauseMenu()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && GameManager.instance.isPaused)
        {
            pauseMenu.SetActive(false);
        }

        else if (!GameManager.instance.isPaused && Input.GetKeyDown(KeyCode.Escape))
        {
            pauseMenu.SetActive(true);
        }

    }
    public void setMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;
    }
    public void setHealth(int health)
    {
        slider.value = health;
    }
}
