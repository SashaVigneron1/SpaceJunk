using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    private CameraShake cameraShake;

    [SerializeField] private AudioSource impactSound = null;

    private void Start()
    {
        var cameraController = FindObjectOfType<CameraController>();
        cameraShake = cameraController.GetComponent<CameraShake>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            StartCoroutine(cameraShake.Shake());
            RandomizePitch();
            impactSound.Play();
        }

        if(collision.gameObject.tag == "Asteroid")
        {
            StartCoroutine(cameraShake.Shake());
            RandomizePitch();
            impactSound.Play();
        }

        if(collision.gameObject.tag == "Collector")
        {
            StartCoroutine(cameraShake.Shake());
            RandomizePitch();
            impactSound.Play();

        }
    }
    private void RandomizePitch()
    {
        impactSound.pitch = Random.Range(0.8f, 1);
    }
}
