using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField] float minimumMovementSpeed;
    [SerializeField] float maximumMovementSpeed;
    [SerializeField] float minScale = 0.5f;
    [SerializeField] float maxScale = 2.0f;
    [SerializeField] float playerCollisionExtraRestitution = 0.5f;
    [SerializeField] float minimumVelocity = 1.0f;

    [SerializeField] List<GameObject> meshes;

    float _movementSpeed;
    Rigidbody _rigidBody;

    bool isStationary = false;

    private ParticleSystem collisionParticles = null;

    private CameraShake cameraShake;


    void Start()
    {
        if (!isStationary)
        {
            _rigidBody = GetComponent<Rigidbody>();

            _movementSpeed = Random.Range(minimumMovementSpeed, maximumMovementSpeed);
            //transform.rotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
            _rigidBody.AddForce(transform.forward * _movementSpeed, ForceMode.Impulse);
            _rigidBody.AddTorque(Vector3.up * Random.Range(10, 20), ForceMode.Impulse);
        }

        int index = Random.Range(0, meshes.Count);

        var m = Instantiate(meshes[index], this.transform);
        m.transform.parent = transform;
        m.layer = LayerMask.NameToLayer("Asteroids");

        if (collisionParticles == null)
            collisionParticles = GetComponentInChildren<ParticleSystem>();

        var cameraController = FindObjectOfType<CameraController>();
        cameraShake = cameraController.GetComponent<CameraShake>();
    }

    private void Update()
    {
        if (_rigidBody.velocity.sqrMagnitude < minimumVelocity * minimumVelocity)
        {
            _rigidBody.velocity.Normalize();
            _rigidBody.velocity *= minimumVelocity;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Asteroid"))
        {
            collisionParticles.transform.position = collision.contacts[0].point;
            collisionParticles.Play();
        }

        else if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Rigidbody>().AddForce(collision.impulse * playerCollisionExtraRestitution, ForceMode.Impulse);
            collision.gameObject.GetComponent<PlayerStun>().StunPlayer();
        }

        else if(collision.gameObject.CompareTag("Collector"))
        {
            cameraShake.Shake();
        }
    }

    public void SetStationary()
    {
        _rigidBody = GetComponent<Rigidbody>();

        _rigidBody.velocity = Vector3.zero;
        _rigidBody.isKinematic = true;
        isStationary = true;
    }

    public void SetRandomScale()
    {
        float scale = Random.Range(minScale, maxScale); 
        transform.localScale = new Vector3(scale, scale, scale);
    }
}
