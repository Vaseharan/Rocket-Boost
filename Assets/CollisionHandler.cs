using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] float levelLoadDelay = 1f;
    [SerializeField] AudioClip success;
    [SerializeField] AudioClip crash;
    [SerializeField] ParticleSystem successparticle;
    [SerializeField] ParticleSystem crashparticle;


    AudioSource audioSource;
    bool isTransitioning = false;
    bool collisionDisabled = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();   
    }
   void Update()
    {
        RespondTODebugKeys();//cheatcode
    }
    void RespondTODebugKeys()//cheat codes
    {
        if(Input.GetKeyUp(KeyCode.L)) {
            LoadNextLevel();
        }else if (Input.GetKeyUp(KeyCode.C))
        {
            collisionDisabled = !collisionDisabled;
        }

    }
    public void OnCollisionEnter(Collision collision)
    {
        if (isTransitioning || collisionDisabled) { return; }
        switch(collision.gameObject.tag) 

        {
            case "Friendly":
                Debug.Log("Starting point");
                break;
            case "Finish":
                StartSuccessSequence();
                Debug.Log("Finished Successfully");
                break;
            case "Fuel":
                Debug.Log("this is fuel");
                break;
            default:
                StartCrashSequence();
                break;

        }
       
        
    }
    void StartSuccessSequence()
    {
        isTransitioning = true; 
        audioSource.Stop();
        audioSource.PlayOneShot(success);
        successparticle.Play();
        GetComponent<Movement>().enabled = false;
        Invoke("LoadNextLevel", levelLoadDelay);
    }
    void StartCrashSequence()
    {
        isTransitioning = true;
        audioSource.Stop(); 
        audioSource.PlayOneShot(crash);
        crashparticle.Play();
        GetComponent<Movement>().enabled = false;
        Invoke("ReloadScene", levelLoadDelay);
    }
    void ReloadScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }
    void LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0;
        }
        SceneManager.LoadScene(nextSceneIndex);
    }
}
