using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStun : MonoBehaviour
{
    [SerializeField] float stunTime = 1.0f;
    [SerializeField] private ParticleSystem smoke;
    PlayerMovement _playerMovement = null;
    PlayerMagnet _playerMagnet = null;

    private void Start()
    {
        _playerMovement = GetComponent<PlayerMovement>();
        _playerMagnet = GetComponent<PlayerMagnet>();
    }

    public void StunPlayer()
    {
        if (_playerMovement != null)
        {
            _playerMovement.enabled = false;
            _playerMovement.StunAudio();
        }
        if (_playerMagnet != null) _playerMagnet.enabled = false;
        if (smoke != null) smoke.Play();

        Invoke("EnableScriptsBack", stunTime);
    }

    private void EnableScriptsBack()
    {
        if (_playerMovement != null) _playerMovement.enabled = true;
        if (_playerMagnet != null) _playerMagnet.enabled = true;
        if (smoke != null) smoke.Stop();
    }
}
