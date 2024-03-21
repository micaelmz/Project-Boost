using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour {
    
    [SerializeField] private float delayLoadTime;
    [SerializeField] private AudioClip crashSourceClip;
    [SerializeField] private AudioClip successSourceClip;
    
    private Movement playerMovement;
    private AudioSource _audioSource;

    private bool isTransitioning = false;

    private void Start() {
        playerMovement = GetComponent<Movement>();
        _audioSource = GetComponent<AudioSource>();
    }

    void OnCollisionEnter(Collision other) {
        
        if (isTransitioning) return;
        
        switch (other.gameObject.tag) {
            case "Friendly":
                Debug.Log("We're fine");
                break;
            case "Finish":
                StartNextLevelSequence();
                break;
            default:
                StartCrashSequence();
                break;
        }
    }

    void StartNextLevelSequence() {
        isTransitioning = true;
        _audioSource.PlayOneShot(successSourceClip);
        playerMovement.enabled = false;
        Invoke("LoadNextLevel", delayLoadTime);
    }

    void StartCrashSequence() {
        // todo particles
        isTransitioning = true;
        _audioSource.PlayOneShot(crashSourceClip);
        playerMovement.enabled = false;
        Invoke("ReloadLevel", delayLoadTime);
    }

    void ReloadLevel() {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

    void LoadNextLevel() {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = (currentSceneIndex + 1) % SceneManager.sceneCountInBuildSettings;
        SceneManager.LoadScene(nextSceneIndex);
    }
}