using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
// using UnityEngine.SceneManagement; 

public class GameManager : MonoBehaviour
{
    public Unit selectedUnit; 

    public int playerTurn = 1; 

    public GameObject selectedUnitSquare; 

    public Image turnIndicator; 
    public Sprite playerOneIndicator; 
    public Sprite playerTwoIndicator;

    public GameObject playerOneEndTurnButton;  
    public GameObject playerTwoEndTurnButton;

    public GameObject gameOverPanel; 
    public GameObject drawPanel; 

    private Animator turnIndicatorAnimator; 

    public int playerOneGold = 100;
    public int playerTwoGold = 100;

    public Text playerOneGoldText;
    public Text playerTwoGoldText;

    public BarrakItem purchasedItem; 

    public GameObject statsPanel; 
    public Vector2 statsPanelOffset; 
    public Unit viewedUnit; 

    public Text healthText; 
    public Text armourText; 
    public Text attackDamageText; 
    public Text defenceDamageText; 

    public int itemCost; 

    private Animator cameraAnimator; 

    public AudioSource endTurnAudioSource; 
    public AudioSource statsPanelAudioSource; 

    private void Start()
    {
        GetGoldIncome(1); 

        cameraAnimator = Camera.main.GetComponent<Animator>();

        turnIndicatorAnimator = turnIndicator.gameObject.GetComponent<Animator>(); 
    }

    public void ToggleStatsPanel (Unit unit)
    {
        statsPanelAudioSource.Play(); 

        if (!unit.Equals(viewedUnit))
        {
            statsPanel.SetActive(true); 
            statsPanel.transform.position = (Vector2)unit.transform.position + statsPanelOffset; 
            viewedUnit = unit; 

            UpdateStatsPanel(); 
        }
        else
        {
            statsPanel.SetActive(false); 
            viewedUnit = null; 
        }
    }

    public void UpdateStatsPanel()
    {
        if (viewedUnit != null)
        {
            healthText.text = viewedUnit.health.ToString(); 
            armourText.text = viewedUnit.armour.ToString();
            attackDamageText.text = viewedUnit.attackDamage.ToString();
            defenceDamageText.text = viewedUnit.defenceDamage.ToString();
        }
    }

    public void MoveStatsPanel(Unit unit)
    {
        if (unit.Equals(viewedUnit))
        {
            statsPanel.transform.position = (Vector2)unit.transform.position + statsPanelOffset;
        }
    }

    public void HideStatsPanel(Unit unit)
    {
        if (unit.Equals(viewedUnit))
        {
            statsPanel.SetActive(false); 
            viewedUnit = null; 
        }
    }

    public void UpdateGoldText()
    {
        playerOneGoldText.text = playerOneGold.ToString();
        playerTwoGoldText.text = playerTwoGold.ToString();
    }

    void GetGoldIncome(int playerTurn)
    {
        foreach (Village village in FindObjectsOfType<Village>())
        {
            if (village.playerNumber == playerTurn)
            {
                if (playerTurn == 1)
                {
                    playerOneGold += village.goldPerTurn; 
                }
                else
                {
                    playerTwoGold += village.goldPerTurn;
                }
            }
        }

        UpdateGoldText(); 
    }

    public void ResetTiles()
    {
        foreach (Tile tile in FindObjectsOfType<Tile>())
        {
            tile.Reset(); 
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            EndTurn(); 
        }

        if (selectedUnit != null)
        {
            selectedUnitSquare.SetActive(true); 
            selectedUnitSquare.transform.position = selectedUnit.transform.position; 
        }
        else
        {
            selectedUnitSquare.SetActive(false); 
        }
    }

    public void EndTurn()
    {
        endTurnAudioSource.Play(); 
        
        if (playerTurn == 1)
        {
            playerTurn = 2; 
            turnIndicator.sprite = playerTwoIndicator; 
            playerOneEndTurnButton.SetActive(false); 
            playerTwoEndTurnButton.SetActive(true); 
        }
        else if (playerTurn == 2)
        {
            playerTurn = 1;
            turnIndicator.sprite = playerOneIndicator;
            playerOneEndTurnButton.SetActive(true);
            playerTwoEndTurnButton.SetActive(false);
        }

        GetGoldIncome (playerTurn); 

        if (selectedUnit != null)
        {
            selectedUnit.selected = false; 
            selectedUnit = null; 
        }

        ResetTiles(); 

        foreach (Unit unit in FindObjectsOfType<Unit>())
        {
            unit.hasMoved = false; 
            unit.weaponIcon.SetActive(false); 
            unit.hasAttacked = false; 
        }

        turnIndicatorAnimator.SetTrigger("Expand"); 
        cameraAnimator.SetTrigger("Shake");

        GetComponent<Barrak>().CloseMenus(); 
    }

    // public void RestartGame()
    // {
    //     // SceneManager.LoadScene (SceneManager.GetActiveScene().buildIndex); 
    //     gameObject.GetComponent<TransitionManager>().StartCoroutine(gameObject.GetComponent<TransitionManager>().ExitTransition("Main")); 
    // }
}
