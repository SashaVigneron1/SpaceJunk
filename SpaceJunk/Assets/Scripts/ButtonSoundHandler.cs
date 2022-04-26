using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonSoundHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] AudioClip OnPointerEnterSound;
    AudioSource source;

    private void Start()
    {
        source = GetComponent<AudioSource>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Play Button Sound
        source.clip = OnPointerEnterSound;
        source.Play();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Do Nothing
    }
}
