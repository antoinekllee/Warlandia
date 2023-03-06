using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class Barrak : MonoBehaviour
{
    public Button playerOneToggleButton; 
    public Button playerTwoToggleButton; 

    public Button[] playerOneItemButtons;
    public BarrakItem[] playerOneBarrakItems;

    public Button[] playerTwoItemButtons;
    public BarrakItem[] playerTwoBarrakItems;

    public GameObject playerOneMenu; 
    public GameObject playerTwoMenu; 

    GameManager gameManager; 

    private AudioSource audioSource;

    bool shouldSpawn = false; 

    void Start()
    {
        gameManager = GetComponent<GameManager>();

        audioSource = GetComponent<AudioSource>(); 
    }

    void Update()
    {
        if (gameManager.playerTurn == 1)
        {
            playerOneToggleButton.interactable = true; 
            playerTwoToggleButton.interactable = false; 
        }
        else
        {
            playerOneToggleButton.interactable = false;
            playerTwoToggleButton.interactable = true;
        }
    }

    public void ToggleMenu(GameObject menu)
    {
        menu.SetActive (!menu.activeSelf); 
        audioSource.Play(); 

        ResetItemButtons(); 
    }

    public void ResetItemButtons()
    {
        if (gameManager.playerTurn == 1)
        {
            for (int i = 0; i < playerOneItemButtons.Length; i++)
            {
                if (gameManager.playerOneGold >= playerOneBarrakItems[i].cost)
                {
                    playerOneItemButtons[i].interactable = true; 
                }
                else
                {
                    playerOneItemButtons[i].interactable = false;
                }
            }
        }
        else
        {
            for (int i = 0; i < playerTwoItemButtons.Length; i++)
            {
                if (gameManager.playerTwoGold >= playerTwoBarrakItems[i].cost)
                {
                    playerTwoItemButtons[i].interactable = true;
                }
                else
                {
                    playerTwoItemButtons[i].interactable = false;
                }
            }
        }
    }

    public void CloseMenus()
    {
        playerOneMenu.SetActive(false); 
        playerTwoMenu.SetActive(false); 
    }

    public void BuyItem(BarrakItem item)
    {
        if (!shouldSpawn && (gameManager.playerTurn == 1 && item.cost <= gameManager.playerOneGold || gameManager.playerTurn == 2 && item.cost <= gameManager.playerTwoGold))
        {
            gameManager.itemCost = item.cost;
            shouldSpawn = true; 
        }
        else
        {
            return; 
        }

        audioSource.Play(); 

        gameManager.purchasedItem = item; 

        if (gameManager.selectedUnit != null)
        {
            gameManager.selectedUnit.selected = false; 
            gameManager.selectedUnit = null; 
        }

        shouldSpawn = false; 

        GetCreatableTiles(); 
    }

    void GetCreatableTiles()
    {
        foreach (Tile tile in FindObjectsOfType<Tile>())
        {
            if (tile.IsClear())
            {
                tile.SetCreatable(); 
            }
        }
    }
}
