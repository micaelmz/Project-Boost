using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Movement : MonoBehaviour {
    
    [SerializeField] private float verticalThrustForce = 1000f;
    [SerializeField] private float horizontalThrustForce = 100f;
    [SerializeField] private AudioClip thrustSourceClip; 

    private AudioSource _audioSource;
    private Rigidbody rocketRigidyBody;

    // Start is called before the first frame update
    void Start() {
        rocketRigidyBody = GetComponent<Rigidbody>();
        _audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update() {
        ProcessVerticalInput();
        ProcessHorizontalInput();
    }


    void ProcessVerticalInput() {
        if (Input.GetAxis("Jump") != 0) {
            rocketRigidyBody.AddRelativeForce(Time.deltaTime * verticalThrustForce * Vector3.up);

            if (!_audioSource.isPlaying) {
                _audioSource.PlayOneShot(thrustSourceClip);
            }
        }

        if (Input.GetAxis("Jump") == 0 && _audioSource.isPlaying) {
            _audioSource.Stop();
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