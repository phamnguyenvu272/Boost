using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField] float levelLoadDelay = 1f;
    [SerializeField] AudioClip crash;
    [SerializeField] AudioClip success;

    [SerializeField] ParticleSystem crashParticles;
    [SerializeField] ParticleSystem successParticles;

    AudioSource audioSource;

    bool isTransitioning = false;
    bool collisionDisabled = false;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        RespondToDebugKeys();
    }

    void RespondToDebugKeys()
    {
        if (Input.GetKey(KeyCode.L))
        {
            LoadNextLevel();
            Debug.Log("Cheat Code: Next Level");
        }
        else if (Input.GetKey(KeyCode.C))
        {
            collisionDisabled = !collisionDisabled;   //toggle collision
            Debug.Log("Cheat Code: Collision is Disabled");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isTransitioning || collisionDisabled)
        {
            return; 
        }

        switch (collision.gameObject.tag)
        {
            case "Friendly":
                Debug.Log("this thing is friendly");
                break;  
            case "Finish":
                StartSuccessSequence();
                break;
            default:
                StartCrashSequence();
                break;
        }
    }

    void StartCrashSequence()
    {
        isTransitioning = true;
        audioSource.Stop();
        audioSource.PlayOneShot(crash);
        crashParticles.Play();
        GetComponent<Movement>().enabled = false;
        Invoke("ReloadLevel", levelLoadDelay);
    }

    void StartSuccessSequence()
    {
        isTransitioning = true;
        audioSource.Stop();
        audioSource.PlayOneShot(success);
        successParticles.Play();
        GetComponent<Movement>().enabled = false;
        Invoke("LoadNextLevel", levelLoadDelay);
    }

    void ReloadLevel()
    {
        //buildIndex: returns the index of the scene in Build Settings
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);

    }

    void LoadNextLevel()
    {
        //buildIndex: returns the index of the scene in Build Settings
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        Debug.Log("next level");
        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0;
            Debug.Log("back to level 1");
        }
        SceneManager.LoadScene(nextSceneIndex);
    }
}
