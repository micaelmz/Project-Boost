using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour {
    
    private Movement playerMovement;
    
    [SerializeField] private float delayLoadTime;

    private void Start() {
        playerMovement = GetComponent<Movement>();
    }

    void OnCollisionEnter(Collision other) {
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
        playerMovement.enabled = false;
        Invoke("LoadNextLevel", delayLoadTime);
    }

    void StartCrashSequence() {
        // todo particles
        // todo sfx
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