using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private GameObject collectorParentPrefab;
    [SerializeField] private GameObject collectorPrefab;
    [SerializeField] private float playerRadius = 5;
    [SerializeField] private float collectorRadius = 5;

    [SerializeField] private List<Color> playerColors;

    private List<Transform> playerTransforms = new List<Transform>();
    private List<Transform> collectorTransforms = new List<Transform>();
    private Transform collectorParentTransform;

    public List<Color> PlayerColors()
    {
        return playerColors;
    }

    void Start()
    {
        collectorParentTransform = Instantiate(collectorParentPrefab).transform;
    }

    public bool Ready()
    {
        if (playerTransforms.Count < 2)
            return false;

        foreach (var player in playerTransforms)
        {
            player.GetComponent<PlayerMovement>().enabled = true;
            player.GetComponent<PlayerMagnet>().enabled = true;
        }

        FindObjectOfType<GameManagerScript>().SpawnStartJunk();
        collectorParentTransform.GetComponent<CollectorParentRotation>().enabled = true;

        FindObjectOfType<PlayerInputManager>().DisableJoining();


        return true;
    }

    public void AddPlayer(Transform player)
    {

        Color color = playerColors[playerTransforms.Count];

        // PLAYER
        //-------

        var playerColorChanger = player.GetComponent<ColorChanger>();
        if (playerColorChanger != null)
            playerColorChanger.SetColor(color);

        playerTransforms.Add(player);
        player.GetComponent<PlayerMagnet>().SetPlayerIndex(collectorTransforms.Count);
        for (int i = 0; i < playerTransforms.Count; i++)
        {
            var angle = i * Mathf.PI * 2 / playerTransforms.Count;
            var x = Mathf.Cos(angle) * playerRadius;
            var z = Mathf.Sin(angle) * playerRadius;
            playerTransforms[i].position = new Vector3(x, 0, z);
            
            playerTransforms[i].rotation = Quaternion.Euler(0, 180 - angle * Mathf.Rad2Deg + 90, 0);
        }

        // COLLECTOR
        //----------

        GameObject collector = Instantiate(collectorPrefab, collectorParentTransform);
        collector.GetComponentInChildren<JunkCollector>().SetPlayerIndex(collectorTransforms.Count);
        Transform ct = collector.transform;
        collectorTransforms.Add(ct);

        var CollectorColorChanger = ct.GetComponent<ColorChanger>();
        if (CollectorColorChanger != null)
            CollectorColorChanger.SetColor(color);

        for (int i = 0; i < collectorTransforms.Count; i++)
        {
            var angle = i * Mathf.PI * 2 / collectorTransforms.Count;
            var x = Mathf.Cos(angle) * collectorRadius;
            var z = Mathf.Sin(angle) * collectorRadius;
            collectorTransforms[i].position = new Vector3(x, 0, z);
            
            collectorTransforms[i].rotation = Quaternion.Euler(0, -angle * Mathf.Rad2Deg, 0);
        }

        // READY
        if (playerTransforms.Count >= 2)
            FindObjectOfType<ReadyButton>().SetInteractible();
    }
}
