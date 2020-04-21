using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public class ChangeSceneFromUI : MonoBehaviour
{
    public void LoadGame()
	{
		Time.timeScale = 1.0f;
		SceneManager.LoadScene("GameScene");
    }

    public void LoadMenu()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

}
