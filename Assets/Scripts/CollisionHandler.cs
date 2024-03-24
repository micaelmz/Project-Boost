using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour {
    
    [SerializeField] private float delayLoadTime;
    
    [SerializeField] private AudioClip crashSourceClip;
    [SerializeField] private AudioClip successSourceClip;
    
    [SerializeField] private ParticleSystem successParticle;
    [SerializeField] private ParticleSystem crashParticle;
    
    private Movement _movement;
    private AudioSource _audioSource;

    private GameObject[] removableParts;

    private bool isTransitioning = false;

    private void Start() {
        _movement = GetComponent<Movement>();
        _audioSource = GetComponent<AudioSource>();
        removableParts = GameObject.FindGameObjectsWithTag("RemovablePart");
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
        _movement.enabled = false;
        
        _audioSource.PlayOneShot(successSourceClip);
        successParticle.Play();
        
        Invoke(nameof(LoadNextLevel), delayLoadTime);
    }

    void StartCrashSequence() {
        isTransitioning = true;
        _movement.enabled = false;
        
        _audioSource.PlayOneShot(crashSourceClip);
        crashParticle.Play();

        RemoveRemovableParts();
        
        Invoke(nameof(ReloadLevel), delayLoadTime);
    }

    void RemoveRemovableParts() {
        // aka "disassemble rocket"
        for (int i = 0; i < removableParts.Length; i++) {
            MeshRenderer partToRemove = removableParts[i].GetComponent<MeshRenderer>();
            partToRemove.enabled = false;
        }
    }

    void ReloadLevel() {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

    void LoadNextLevel() {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int lastSceneIndex = SceneManager.sceneCountInBuildSettings;
        int nextSceneIndex = (currentSceneIndex + 1) % lastSceneIndex; // if it is the last scene, it returns 0, which implies reloading the first level again
        SceneManager.LoadScene(nextSceneIndex);
    }
}