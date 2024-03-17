using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Movement : MonoBehaviour {
    [SerializeField] private float verticalThrustForce = 1000f;
    [SerializeField] private float horizontalThrustForce = 100f;

    // Components
    private AudioSource audioSource;
    private Rigidbody rocketRigidyBody;
    
    // States
    private bool previousJumpState = false;

    void Start() {
        rocketRigidyBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update() {
        ProcessVerticalInput();
        ProcessHorizontalInput();
    }
    
    void ProcessVerticalInput() {
        bool currentJumpState = Input.GetAxis("Jump") > 0.1f;
        
        // Can be jumping (true) or not jumping (false)
        if (currentJumpState != previousJumpState) {
            // is jumping
            if (currentJumpState) {
                rocketRigidyBody.AddRelativeForce(Time.deltaTime * verticalThrustForce * Vector3.up);
                if (!audioSource.isPlaying) {
                    audioSource.Play();
                }
            }
            // Is not jumping
            else {
                audioSource.Stop();
            }
            
            previousJumpState = currentJumpState;
        }
    }

    void ProcessHorizontalInput() {
        var horizontalAxis = Input.GetAxis("Horizontal");

        if (horizontalAxis != 0) {
            rocketRigidyBody.freezeRotation = true; // travando o controle para que manualmente seja alterado
            transform.Rotate(
                0,
                0,
                Time.deltaTime * horizontalThrustForce * horizontalAxis
            );
            rocketRigidyBody.freezeRotation = false; // liberando para que a fisica do jogo possa modificar como quiser novamente
        }
    }
}