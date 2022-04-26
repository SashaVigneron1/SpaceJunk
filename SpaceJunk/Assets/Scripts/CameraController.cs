using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Camera mainCamera;
    [SerializeField] float speed;
    [SerializeField] float defaultDistanceX;
    [SerializeField] float defaultDistanceZ;
    [SerializeField] float minHeight;
    [SerializeField] bool shouldLerp;

    List<Transform> targets = new List<Transform>();

    private void Start()
    {
        targets.Add(this.transform);
        targets.Add(this.transform);
        targets.Add(this.transform);
    }

    private void Update()
    {
        Vector3 averagePos = Vector3.zero;
        foreach(var target in targets)
        {
            averagePos += target.position;
        }
        averagePos /= targets.Count;
        //Debug.Log(averagePos);

        // default height = 10
        // default distance = 5;

        float maxHeight = 0;
        foreach (var target in targets)
        {
            float distanceX = Mathf.Abs(target.position.x - mainCamera.transform.position.x); 
            float distanceZ = Mathf.Abs(target.position.z - mainCamera.transform.position.z);
            
            if (distanceX > defaultDistanceX)
            {
                float height = minHeight * distanceX / defaultDistanceX;
                if (height > maxHeight) maxHeight = height;
            }
            if (distanceZ > defaultDistanceZ)
            {
                float height = minHeight * distanceZ / defaultDistanceZ;
                if (height > maxHeight) maxHeight = height;
            }
        }

        if (maxHeight < minHeight)
            maxHeight = minHeight;

        var targetPos = new Vector3(averagePos.x, maxHeight, averagePos.z);

        if (shouldLerp)
            mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, targetPos, speed * Time.deltaTime);
        else
            mainCamera.transform.position = targetPos;
    }

    public void AddPlayer(Transform player)
    {
        targets.Add(player);
    }

    public void RemovePlayer(Transform player)
    {
        targets.Remove(player);
    }
}

