using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField] float delayValue = 1f;
    [SerializeField] ParticleSystem explosionParticle;
    [SerializeField] AudioClip explosionSound;

    AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {

        switch (other.gameObject.tag)
        {
            case "Finish":
                StartLoadSequence();
                break;
            default:
                StartCrashSequence();

                break;

        }
    }

    void StartLoadSequence()
    {
        
        
        GetComponent<PlayerController>().enabled = false;
        Invoke("FinishLevel", delayValue);
    }

    void StartCrashSequence()
    {
        explosionParticle.Play();
        audioSource.Stop();
        audioSource.PlayOneShot(explosionSound);
        GetComponent<MeshRenderer>().enabled=false;
        GetComponent<BoxCollider>().enabled = false;
        GetComponent<PlayerController>().enabled = false;
        Invoke("ReloadLevel", delayValue);
    }

    void ReloadLevel()
    {
        int curentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(curentSceneIndex);
    }

    void FinishLevel()
    {
        int curentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = curentSceneIndex + 1;
        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0;
        }
        SceneManager.LoadScene(nextSceneIndex);
    }


}
