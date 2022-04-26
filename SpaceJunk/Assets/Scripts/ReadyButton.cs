using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReadyButton : MonoBehaviour
{

    [SerializeField] AudioClip buttonClickedSound;
    [SerializeField] Button readyButton;
    [SerializeField] GameObject playerWaitingText;

    private void Start()
    {
        readyButton.Select();
    }
    public void OnClick()
    {
        AudioSource.PlayClipAtPoint(buttonClickedSound, Camera.main.transform.position);

        if (FindObjectOfType<PlayerSpawner>().Ready())
        {
            Destroy(this.gameObject);

            //restart background music
            foreach (AudioSource audio in FindObjectsOfType<AudioSource>())
            {
                if (audio.name == "BackgroundMusic")
                {
                    audio.volume = 0.25f;
                    audio.Stop();
                    audio.Play();
                }
            }
        }
    }

    public void SetInteractible()
    {
        readyButton.interactable = true;
        playerWaitingText.SetActive(false);
    }
}
