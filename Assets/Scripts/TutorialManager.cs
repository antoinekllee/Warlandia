using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class TutorialManager : MonoBehaviour
{
    public Text phaseNumber; 

    public GameObject[] textObjects; 
    public GameObject[] visualObjects; 

    AudioSource audioSource; 

    public GameObject rightButton; 
    public Button leftButton; 

    public GameObject homeButton; 

    void Start()
    {
        audioSource = GetComponent<AudioSource>(); 
    }

    public void Right()
    {
        audioSource.Play(); 

        for (int i = 0; i < textObjects.Length; i++)
        {
            if (textObjects[i].activeSelf)
            {
                Debug.Log("Found current tutorial phase: " + textObjects[i].name);

                textObjects[i].SetActive(false);
                visualObjects[i].SetActive(false);

                int nextIndex = 0;

                if (i + 1 < textObjects.Length)
                    nextIndex = i + 1;
                else
                    nextIndex = 0;

                textObjects[nextIndex].SetActive(true); 
                visualObjects[nextIndex].SetActive(true); 

                Debug.Log("New tutorial phase: " + textObjects[nextIndex].name);

                phaseNumber.text = (nextIndex + 1).ToString() + "/" + textObjects.Length;

                if (i + 2 == textObjects.Length)
                {
                    Debug.Log("TRUE");

                    leftButton.interactable = false;

                    rightButton.SetActive(false);
                    homeButton.SetActive(true);

                    // break;
                }

                break;
            }
        }
    }
    public void Left()
    {
        audioSource.Play();

        for (int i = textObjects.Length - 1; i >= 0; i--)
        {
            Debug.Log(textObjects[i]); 

            if (textObjects[i].activeSelf)
            {
                Debug.Log("Found current tutorial phase: " + textObjects[i].name);

                textObjects[i].SetActive(false);
                visualObjects[i].SetActive(false);

                int nextIndex = 0;

                if (i - 1 >= 0)
                    nextIndex = i - 1;
                else
                    nextIndex = textObjects.Length - 1;

                textObjects[nextIndex].SetActive(true);
                visualObjects[nextIndex].SetActive(true); 

                Debug.Log("New tutorial phase: " + textObjects[nextIndex].name);

                phaseNumber.text = (nextIndex + 1).ToString() + "/" + textObjects.Length;

                break;
            }
        }
    }
}
