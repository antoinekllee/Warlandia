using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class TransitionManager : MonoBehaviour
{
    public GameObject enterTransitionObject; 
    public float enterTransitionTime; 

    public GameObject exitTransitionObject; 
    public float exitTransitionTime; 

    public AudioSource audioSource; 

    public GameObject music; 

    public bool shouldInstantiateMusicOnStart = false; 

    void Start()
    {
        if (enterTransitionObject != null)
            StartCoroutine (EnterTransition());

        if (GameObject.Find("BackgroundMusic") == null && shouldInstantiateMusicOnStart)
        {
            GameObject musicGameObject = Instantiate (music, transform.position, Quaternion.identity); 
            musicGameObject.transform.name = "BackgroundMusic";
            DontDestroyOnLoad(musicGameObject);
        }

        // if (music != null)
        //     DontDestroyOnLoad(music); 
    }

    public IEnumerator EnterTransition()
    {
        audioSource.Play(); 

        enterTransitionObject.SetActive(true); 
        yield return new WaitForSeconds(enterTransitionTime); 
        enterTransitionObject.SetActive(false); 
    }

    public void ExitTransition(string sceneToLoad)
    {
        StartCoroutine(Transition(sceneToLoad)); 
    }

    public IEnumerator Transition(string sceneToLoad)
    {
        audioSource.Play();

        exitTransitionObject.SetActive(true); 
        yield return new WaitForSeconds(exitTransitionTime); 
        SceneManager.LoadScene(sceneToLoad); 
    }
}
