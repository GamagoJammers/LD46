using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public class ChangeSceneFromUI : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void goPlay()
    {
        SceneManager.LoadScene("PauseMenu");
    }
    void goMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
