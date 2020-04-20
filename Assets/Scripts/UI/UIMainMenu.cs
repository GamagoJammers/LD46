using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMainMenu : MonoBehaviour
{

    public GameObject canvasTuto;
    public ChangeSceneFromUI goGame;

    void Start()
    {
        canvasTuto.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        if ((Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.JoystickButton1)) && canvasTuto.activeSelf)
        {
            canvasTuto.SetActive(false);
            goGame.LoadGame();
        }
    }


    public void DisplayTuto()
    {
        canvasTuto.SetActive(true);
    }
}
