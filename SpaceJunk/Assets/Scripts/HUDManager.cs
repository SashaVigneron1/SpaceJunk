using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI gameTimerText;
    [SerializeField] TextMeshProUGUI playerWonText;
    [SerializeField] GameObject finishScreen;
    [SerializeField] AudioClip buttonClickedSound;

    [SerializeField] Button restartButton;

    [SerializeField] GameObject pauseScreen;
    [SerializeField] Button resumeButton;

    List<GameObject> hudSlots = new List<GameObject>();

    private int currentTime;

    [SerializeField] AudioSource timerStartSound = null;
    bool hasPlayedAudio = false;
    bool doOnce = false;
  
    private void Awake()
    {
        for (int i = 0; i < transform.childCount; ++i)
        {
            hudSlots.Add(transform.GetChild(i).gameObject);
        }

        if (timerStartSound == null)
            timerStartSound = GetComponent<AudioSource>();
    }
    public void GivePlayerHUDSlot(int playerIndex, Color playerColor)
    {
        hudSlots[playerIndex].SetActive(true);
        PlayerHUD playerHUD = hudSlots[playerIndex].GetComponent<PlayerHUD>();
        playerHUD.SetColor(playerColor);
        playerHUD.SetPlayer(playerIndex);
    }

    public void SetTimer(float value)
    {
        if ((int)value == currentTime)
            return;

        currentTime = (int)value;

        int min = (int)value / 60;
        int seconds = (int)value % 60;

        if(seconds >= 10)
        {
            gameTimerText.text = min.ToString() + ":" + seconds.ToString();
        }

        else
        {
            gameTimerText.text = min.ToString() + ":0" + seconds.ToString();
        }
      

        if(min == 0 && seconds <= 30)
        {

            if( hasPlayedAudio == false)
            {
                hasPlayedAudio = true;
                timerStartSound.Play();
            }
            if(seconds <= 3
                && doOnce == false)
            {
                doOnce = true;
                timerStartSound.loop = true;
                timerStartSound.Play();
            }
            if( seconds <= 0)
            {
                timerStartSound.Stop();
            }

            StartCoroutine(BlinkTimer());
        }
    }
    public void SetTimerTextOvertime()
    {
        // Set Color To Red
        gameTimerText.color = Color.red;
        gameTimerText.text = "OVERTIME";
    }

    public void SetWinnerText(string playerId, Color playerColor)
    {
        playerWonText.text = "P" + playerId + " won!";
        var c = new Color(playerColor.r, playerColor.g, playerColor.b, 1);
        playerWonText.color = c;
    }
    public void Restart()
    {
        AudioSource.PlayClipAtPoint(buttonClickedSound, Camera.main.transform.position);
        SceneManager.LoadScene(1);
    }
    public void ReturnToMainMenu()
    {
        AudioSource.PlayClipAtPoint(buttonClickedSound, Camera.main.transform.position);
        SceneManager.LoadScene(0);
    }
    public void ShowFinishScreen()
    {
        finishScreen.SetActive(true);
        gameTimerText.SetText("");
        restartButton.Select();
    }

    IEnumerator BlinkTimer()
    {
        gameTimerText.color = Color.red;

        yield return new WaitForSeconds(0.5f);

        gameTimerText.color = Color.white;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        pauseScreen.SetActive(false);
    }

    public void ShowPauseScreen()
    {
        pauseScreen.SetActive(true);
        resumeButton.Select();

    }
}
