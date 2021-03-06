﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIMainMenu : MonoBehaviour
{

    public GameObject canvasTuto;
    public ChangeSceneFromUI goGame;
    public EventSystem m_eventSys;
    public GameObject firstSelMainMenu;
    bool canPass = false;

    void Start()
    {
        canvasTuto.SetActive(false);
        m_eventSys.SetSelectedGameObject(firstSelMainMenu);
    }
	
    void Update()
    {
        if ((Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.JoystickButton0)) && canvasTuto.activeSelf && canPass)
        {
            goGame.LoadGame();
        }
    }

    public void DisplayTuto()
    {
        canvasTuto.SetActive(true);
        StartCoroutine(WaitToPass());
    }

    IEnumerator WaitToPass()
    {
        yield return new WaitForSeconds(2);
        canPass = true;
    }
}
