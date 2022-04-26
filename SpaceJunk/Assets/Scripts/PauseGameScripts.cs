using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseGameScripts : MonoBehaviour
{

    HUDManager hud;
    GameManagerScript gameManager;

    bool isPaused = false;

    // Start is called before the first frame update
    void Start()
    {
        hud = FindObjectOfType<HUDManager>();
        gameManager = FindObjectOfType<GameManagerScript>();
    }

    // Update is called once per frame

    public void OnPauseGame(InputAction.CallbackContext ctx)
    {
        if(gameManager.GetIsStarted())
        {
            if (!isPaused)
            {
                hud.ShowPauseScreen();
                Time.timeScale = 0;
                isPaused = true;
            }

            else
            {
                hud.ResumeGame();
                isPaused = false;
            }
        }
        
       

    }
}
