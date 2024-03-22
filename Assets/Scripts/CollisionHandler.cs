using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour {
    
    [SerializeField] private float delayLoadTime;
    [SerializeField] private AudioClip crashSourceClip;
    [SerializeField] private AudioClip successSourceClip;
    [SerializeField] private ParticleSystem successParticle;
    [SerializeField] private ParticleSystem crashParticle;
    
    private Movement playerMovement;
    private AudioSource _audioSource;

    private GameObject[] removableParts;

    private bool isTransitioning = false;

    private void Start() {
        playerMovement = GetComponent<Movement>();
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
        _audioSource.PlayOneShot(successSourceClip);
        successParticle.Play();
        playerMovement.enabled = false;
        Invoke("LoadNextLevel", delayLoadTime);
    }

    void StartCrashSequence() {
        isTransitioning = true;
        _audioSource.PlayOneShot(crashSourceClip);
        crashParticle.Play();

        for (int i = 0; i < removableParts.Length; i++) {
            MeshRenderer toRemove = removableParts[i].GetComponent<MeshRenderer>();
            toRemove.enabled = false;
        }
        
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