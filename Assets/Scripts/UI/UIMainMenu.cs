using System.Collections;
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
    bool canPass =false;

    void Start()
    {
        canvasTuto.SetActive(false);
        m_eventSys.SetSelectedGameObject(firstSelMainMenu);
    
    }

    // Update is called once per frame
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
        canPass = true;
    }
}
