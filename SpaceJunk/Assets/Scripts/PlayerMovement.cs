using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float forwardForce = 5.0f;
    [SerializeField] private float rotationTorque = 1.0f;

    private float forwardInput = 0; 
    private float rotationInput = 0;

    private Rigidbody rb;

    private AudioSource engineAudio = null;
    bool isPlayingAudio = false;
    bool isAudioFadingOut = false;
    [SerializeField] private float fadeTime = 1;
    [SerializeField] private float minVolume = 0;
    [SerializeField] private float maxVolume = 1;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();

        engineAudio = GetComponent<AudioSource>();
        engineAudio.volume = maxVolume;
    }

    void FixedUpdate()
    {
        rb.AddForce(transform.forward * forwardInput * forwardForce);
        rb.AddTorque(new Vector3(0, 1, 0) * rotationInput * rotationTorque);
        
        rb.rotation = Quaternion.Euler(0, rb.rotation.eulerAngles.y, 0);
        rb.position = new Vector3(rb.position.x, 0, rb.position.z);
    }
    private void Update()
    {
        if (isAudioFadingOut == false)
        {
            if (forwardInput > 0)
            {
                if (isPlayingAudio == false)
                {
                    engineAudio.Play();
                    isPlayingAudio = true;
                }
            }
            else
            {
                if (isPlayingAudio == true)
                {
                    isAudioFadingOut=true;
                    isPlayingAudio = false;
                }
            }
        }
        else
        {
            if (forwardInput > 0)
            {
                isAudioFadingOut = false;
                engineAudio.volume = maxVolume;
            }
            //
            if (engineAudio.volume > minVolume)
            {
                engineAudio.volume -= Time.deltaTime / fadeTime;
            }
            else
            {
                engineAudio.volume = maxVolume;
                isAudioFadingOut = false;
                engineAudio.Stop();
            }
        }
    }
    public void OnForward(InputAction.CallbackContext ctx)
    {
        forwardInput = ctx.ReadValue<float>(); 
    }

    public void OnRotate(InputAction.CallbackContext ctx)
    {
        rotationInput = ctx.ReadValue<float>();
    }
    public void StunAudio()
    {
        engineAudio.Stop();
        engineAudio.volume = maxVolume;
        isAudioFadingOut = false;
        isPlayingAudio = false;
    }

    
}
