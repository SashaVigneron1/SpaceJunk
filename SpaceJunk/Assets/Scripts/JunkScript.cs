using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JunkScript : MonoBehaviour
{
    [SerializeField] int _points;

    [SerializeField] float minimumMovementSpeed;
    [SerializeField] float maximumMovementSpeed;

    [SerializeField] List<GameObject> meshes;

    [SerializeField] PhysicMaterial physicMaterial;

    float _movementSpeed;
    Rigidbody _rigidBody;


    public int Points
    {
        get { return _points; }
        set { _points = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        _rigidBody = GetComponent<Rigidbody>();

        int index = Random.Range(0, meshes.Count);

        var m = Instantiate(meshes[index], this.transform);
        m.transform.parent = transform;
        m.layer = LayerMask.NameToLayer("FreeJunk");
        m.GetComponent<MeshCollider>().material = physicMaterial;

        _movementSpeed = Random.Range(minimumMovementSpeed, maximumMovementSpeed);
        //transform.rotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
        _rigidBody.AddForce(transform.forward * _movementSpeed, ForceMode.Impulse);
        _rigidBody.AddTorque(Vector3.up * Random.Range(10, 20), ForceMode.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
       
    }
}
