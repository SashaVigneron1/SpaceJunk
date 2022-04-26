using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerMagnet : MonoBehaviour
{
    [SerializeField] private float magnetRadius = 5f;
    [SerializeField] private float magnetForce = 10f;
    
    [SerializeField] private float depletionSpeed = 10f;
    [SerializeField] private float refillSpeed = 1f;

    [SerializeField] private Vector3 magnetOffset = new Vector3(0, 0, 0);
    [SerializeField] private ParticleSystem particles;

    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private int colorMaterialIndex;

    [HideInInspector] public float Charge = 0.5f;
    private bool usingMagnet;
    [HideInInspector] public int playerIndex;
    private Color color;

    [SerializeField] AudioSource forcefieldSound = null;
    bool isPlayingAudio = false;
    
    private void Start()
    {
        Charge = 0.5f;
    }
    public void SetPlayerIndex(int index)
    {
        playerIndex = index;

        color = FindObjectOfType<PlayerSpawner>().PlayerColors()[index];
    }

    private void SetUsing(bool isUsed)
    {
        usingMagnet = isUsed;
        if (usingMagnet)
        {
            particles.Play();
        }
        else
        {
            particles.Stop();
        }
    }

    private void Update()
    {
        if (usingMagnet)
        {
            if (isPlayingAudio == false)
            {
                isPlayingAudio = true;
                forcefieldSound.Play();
            }

            Charge -= Time.deltaTime * depletionSpeed;

            if (Charge < 0)
            {
                Charge = 0;
                SetUsing(false);
            }
        }
        else
        {
            if (isPlayingAudio == true)
            {
                isPlayingAudio = false;
                forcefieldSound.Stop();
            }

            Charge += Time.deltaTime * refillSpeed;

            if (Charge > 1)
                Charge = 1;
        }

        meshRenderer.materials[colorMaterialIndex].color = Color.Lerp(Color.grey, color, Charge / 2 + 0.5f);
    }

    public void FixedUpdate()
    {
        if (!usingMagnet)
            return;

        var overlapping = Physics.OverlapSphere(transform.TransformPoint(magnetOffset), magnetRadius);
        foreach (var col in overlapping)
        {
            var rb = col.attachedRigidbody;
            if (rb == null)
                continue;

            if (rb.tag == "Junk")
            {
                Vector3 dir = transform.TransformPoint(magnetOffset) - col.attachedRigidbody.position;
                col.attachedRigidbody.AddForce(dir.normalized * magnetForce);
            }
        }
    }

    public void OnMagnet(InputAction.CallbackContext ctx)
    {
        SetUsing(ctx.ReadValue<float>() > 0.1f && Charge > 0);
    }
}
