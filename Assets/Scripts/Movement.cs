using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Movement : MonoBehaviour {
    [SerializeField] private float verticalThrustForce = 1000f;
    [SerializeField] private float horizontalThrustForce = 100f;

    [SerializeField] private AudioClip thrustSourceClip;

    [SerializeField] private ParticleSystem leftThrustParticle;
    [SerializeField] private ParticleSystem rightThrustParticle;
    [SerializeField] private ParticleSystem mainThrustParticle;


    private AudioSource _audioSource;
    private Rigidbody _rigidBody;

    // Start is called before the first frame update
    void Start() {
        _rigidBody = GetComponent<Rigidbody>();
        _audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update() {
        ProcessVerticalInput();
        ProcessHorizontalInput();
    }


    void ProcessVerticalInput() {
        bool isJumping = Input.GetAxis("Jump") != 0;

        if (isJumping) 
            HandleThrust();
        
        else StopThrust();
    }

    void HandleThrust() {
        _rigidBody.AddRelativeForce(Time.deltaTime * verticalThrustForce * Vector3.up);

        // Play SFX
        if (!_audioSource.isPlaying)
            _audioSource.PlayOneShot(thrustSourceClip);

        // Play Particles
        if (!mainThrustParticle.isPlaying) 
            mainThrustParticle.Play();
    }

    void StopThrust() {
        // Stop SFX
        if (_audioSource.isPlaying)
            _audioSource.Stop();

        // Stop Particles
        if (mainThrustParticle.isPlaying)
            mainThrustParticle.Stop();
    }

    
    void ProcessHorizontalInput() {
        var horizontalAxis = Input.GetAxis("Horizontal");
        var isRotating = horizontalAxis != 0;

        if (isRotating) {
            HandlePlayerDirection(horizontalAxis);
            HandleThrustParticles(horizontalAxis);
        }
        else StopPlayerDirection();
    }

    void HandlePlayerDirection(float horizontalAxis) {
        _rigidBody.freezeRotation = true;
        transform.Rotate(
            0,
            0,
            Time.deltaTime * horizontalThrustForce * horizontalAxis
        );
        _rigidBody.freezeRotation = false;
    }

    void HandleThrustParticles(float horizontalAxis) {
        var isGoingRight = horizontalAxis > 0;

        if (isGoingRight && !rightThrustParticle.isPlaying)
            rightThrustParticle.Play();

        else if (!isGoingRight && !leftThrustParticle.isPlaying)
            leftThrustParticle.Play();
    }

    void StopPlayerDirection() {
        leftThrustParticle.Stop();
        rightThrustParticle.Stop();
    }
}