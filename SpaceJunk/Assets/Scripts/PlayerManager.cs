using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    HUDManager hud;
    CameraController cameraController = null;
    PlayerSpawner playerSpawner = null;

    List<PlayerInput> playerInputs = new List<PlayerInput>();

    public void HandlePlayerJoin(PlayerInput pi)
    {
        playerInputs.Add(pi);

        if (cameraController == null)
            cameraController = FindObjectOfType<CameraController>();
        cameraController.AddPlayer(pi.gameObject.transform);

        if (playerSpawner == null)
            playerSpawner = FindObjectOfType<PlayerSpawner>();
        playerSpawner.AddPlayer(pi.gameObject.transform);

        if (hud == null)
            hud = FindObjectOfType<HUDManager>();
        hud.GivePlayerHUDSlot(pi.playerIndex, playerSpawner.PlayerColors()[pi.playerIndex]);

    }
    public void HandlePlayerLeave(PlayerInput pi)
    {
        if (cameraController != null)
            cameraController.RemovePlayer(pi.gameObject.transform);
    }
}
