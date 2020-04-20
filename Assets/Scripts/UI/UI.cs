﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UI : MonoBehaviour
{
    private float Timer = 0.0f;
    int minutes;
    int seconds;
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

        Timer += Time.deltaTime;
        seconds = (int)(Timer % 60);
        minutes = (int)(Timer / 60);

        timerText.text = minutes.ToString("00") + ":" + seconds.ToString("00");

        CheckPause();
    }

    private void CheckPause()
    {

        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.JoystickButton7))
        {
            if (GameManager.instance.isPaused)
                ResumeGame();
            else 
                PauseGame();
        }
       
    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        GameManager.instance.isPaused = false;
        Time.timeScale = 1;
        timerText.gameObject.SetActive(true);
    }

    public void PauseGame()
    {
        pauseMenu.SetActive(true);
        GameManager.instance.isPaused = true;
        Time.timeScale = 0;
        timerText.gameObject.SetActive(false);
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
