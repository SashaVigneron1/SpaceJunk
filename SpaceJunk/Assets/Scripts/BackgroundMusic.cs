using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    [SerializeField] private float fadeTime = 0;
    [SerializeField] private float maxVolume = 0;
    AudioSource audioSource = null;

    bool fadeIn = true;
    void Start()
    {
        DontDestroyOnLoad(this);

        if(audioSource == null)
            audioSource = GetComponent<AudioSource>();
        audioSource.Play();
        audioSource.volume = 0f;
    }
    void Update()
    {
        if (fadeIn == false)
            return;
        if (audioSource.volume < maxVolume)
        {
            audioSource.volume += Time.deltaTime / fadeTime;
        }
        else
        {
            fadeIn = false;
        }
    }

    void SetVolume(float volumeValue)
    {
        audioSource.volume = volumeValue;
    }
}