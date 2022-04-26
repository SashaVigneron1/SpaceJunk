using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{

    [SerializeField] Button playButton;
    [SerializeField] Button creditsReturnButton;

    [SerializeField] AudioClip buttonClickedClip;
    AudioSource source;
    Animator animator;

    private void Start()
    {
        Time.timeScale = 1.0f;

        playButton.Select();
        source = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
    }

    public void StartGame()
    {
        source.clip = buttonClickedClip;
        source.Play();
        SceneManager.LoadScene(1);
    }
    public void QuitGame()
    {
        source.clip = buttonClickedClip;
        source.Play();
        Application.Quit();
    }
    
    public void ShowCredits()
    {
        creditsReturnButton.Select();

        source.clip = buttonClickedClip;
        source.Play();

        // Start Animation
        animator.SetTrigger("Show");
    }
    public void ExitCredits()
    {
        playButton.Select();

        source.clip = buttonClickedClip;
        source.Play();

        // Exit Animation
        animator.SetTrigger("Hide");
    }
}
