using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class Unit : MonoBehaviour
{
    public bool selected; 
    GameManager gameManager; 

    public int tileSpeed; 
    public bool hasMoved; 

    public float moveSpeed; 

    public int playerNumber; 

    public int attackRange; 
    List<Unit> enemiesInRange = new List<Unit>(); 
    public bool hasAttacked; 

    public GameObject weaponIcon; 

    public int health; 
    public int attackDamage; 
    public int defenceDamage; 
    public int armour; 

    public DamageIcon damageIcon; 
    public GameObject deathEffect;     

    private Animator cameraAnimator; 

    public AudioSource selectAudioSource;
    public AudioSource moveAudioSource;

    public Text kingHealth; 
    public bool isKing; 

    public GameObject victoryPanel; 

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>(); 

        cameraAnimator = Camera.main.GetComponent<Animator>(); 
        
        UpdateKingHealth(); 
    }

    public void UpdateKingHealth()
    {
        if (isKing)
        {
            kingHealth.text = health.ToString(); 
        }
    }

    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1))
        {
            gameManager.ToggleStatsPanel(this); 
        }
    }

    private void OnMouseDown()
    {
        ResetWeaponIcons();

        if (selected)
        {
            selected = false; 
            gameManager.selectedUnit = null; 
            gameManager.ResetTiles(); 
        }
        else
        {
            if (playerNumber == gameManager.playerTurn)
            {
                if (gameManager.selectedUnit != null)
                {
                    gameManager.selectedUnit.selected = false;
                }
                
                selectAudioSource.Play(); 

                selected = true; 
                gameManager.selectedUnit = this;

                gameManager.ResetTiles();
                GetEnemies(); 
                GetWalkableTiles(); 
            }
        }

        Collider2D col = Physics2D.OverlapCircle(Camera.main.ScreenToWorldPoint(Input.mousePosition), 0.15f); 
        Unit unit = col.GetComponent<Unit>(); 
        if (gameManager.selectedUnit != null)
        {
            if (gameManager.selectedUnit.enemiesInRange.Contains(unit) && !gameManager.selectedUnit.hasAttacked)
            {
                gameManager.selectedUnit.Attack(unit); 
            }
        }
    }

    void Attack(Unit enemy)
    {
        cameraAnimator.SetTrigger("Shake"); 

        hasAttacked = true; 

        int enemyDamage = attackDamage - armour; 
        int damageReceived = enemy.defenceDamage - armour; 

        if (enemyDamage >= 1)
        {
            DamageIcon damageIconInstance = Instantiate(damageIcon, new Vector2 (enemy.transform.position.x, enemy.transform.position.y + damageIcon.yOffset), Quaternion.identity); 
            damageIconInstance.Setup(enemyDamage, gameManager.playerTurn); 

            enemy.health -= enemyDamage; 

            enemy.UpdateKingHealth(); 
        }
        
        if (attackRange >= 2 && enemy.attackRange <= 2)
        {
            if (Mathf.Abs(transform.position.x - enemy.transform.position.x) + Mathf.Abs(transform.position.y - enemy.transform.position.y) <= 2)
            {
                if (damageReceived >= 1)
                {
                    DamageIcon damageIconInstance = Instantiate(damageIcon, new Vector2(transform.position.x, transform.position.y + damageIcon.yOffset), Quaternion.identity);
                    damageIconInstance.Setup(damageReceived, gameManager.playerTurn);
                    health -= damageReceived;
                    UpdateKingHealth(); 
                }
            }
        }
        else
        {
            if (damageReceived >= 1)
            {
                DamageIcon damageIconInstance = Instantiate(damageIcon, new Vector2(transform.position.x, transform.position.y + damageIcon.yOffset), Quaternion.identity);
                damageIconInstance.Setup(damageReceived, gameManager.playerTurn);
                health -= damageReceived;
                UpdateKingHealth();
            }
        }


        if (enemy.health <= 0)
        {
            if (enemy.isKing)
            {
                if (isKing && health <= 0)
                {
                    gameManager.drawPanel.SetActive(true);
                }
                else
                {
                    enemy.victoryPanel.SetActive(true);
                }

                enemy.kingHealth.text = "0";

                gameManager.gameOverPanel.gameObject.SetActive(true); 
                gameManager.turnIndicator.gameObject.SetActive(false);

                gameManager.playerOneEndTurnButton.SetActive(false);
                gameManager.playerTwoEndTurnButton.SetActive(false);

                gameManager.playerTurn = 0;
            }

            GameObject enemyDeathEffect = Instantiate(deathEffect, enemy.transform.position, Quaternion.identity);
            Destroy(enemyDeathEffect, 3.5f); 
            Destroy(enemy.gameObject); 
            GetWalkableTiles(); 

            gameManager.HideStatsPanel(enemy); 
        }
        if (health <= 0)
        {
            if (isKing)
            {
                if (enemy.isKing && enemy.health <= 0)
                {
                    gameManager.drawPanel.SetActive(true); 
                }
                else
                {
                    victoryPanel.SetActive(true); 
                }

                kingHealth.text = "0";

                gameManager.playerOneEndTurnButton.SetActive(false);
                gameManager.playerTwoEndTurnButton.SetActive(false);

                gameManager.gameOverPanel.gameObject.SetActive(true);
                gameManager.turnIndicator.gameObject.SetActive(false);

                gameManager.playerTurn = 0;
            }

            GameObject myDeathEffect = Instantiate(deathEffect, transform.position, Quaternion.identity);
            Destroy(myDeathEffect, 3.5f);

            gameManager.ResetTiles();

            gameManager.HideStatsPanel(this);

            Destroy(this.gameObject); 
        }

        gameManager.UpdateStatsPanel(); 
    }

    void GetWalkableTiles()
    {
        if (hasMoved)
        {
            return; 
        }

        foreach (Tile tile in FindObjectsOfType<Tile>())
        {
            if (Mathf.Abs (transform.position.x - tile.transform.position.x) + Mathf.Abs(transform.position.y - tile.transform.position.y) <= tileSpeed)
            {
                if (tile.IsClear())
                {
                    tile.Highlight(); 
                }
            }
        }
    }

    void GetEnemies()
    {
        enemiesInRange.Clear();

        foreach (Unit unit in FindObjectsOfType<Unit>())
        {
            if (Mathf.Abs(transform.position.x - unit.transform.position.x) + Mathf.Abs(transform.position.y - unit.transform.position.y) <= attackRange)
            {
                if (unit.playerNumber != gameManager.playerTurn && hasAttacked == false)
                {
                    enemiesInRange.Add (unit); 
                    unit.weaponIcon.SetActive(true); 
                }
            }
        }
    }

    public void ResetWeaponIcons()
    {
        foreach (Unit unit in FindObjectsOfType<Unit>())
        {
            unit.weaponIcon.SetActive(false); 
        }
    }

    public void Move(Vector2 tilePos)
    {
        gameManager.ResetTiles();
        
        StartCoroutine(StartMovement(tilePos)); 
    }

    IEnumerator StartMovement (Vector2 tilePos)
    {
        moveAudioSource.Play();

        while (transform.position.x != tilePos.x)
        {
            transform.position = Vector2.MoveTowards(transform.position, new Vector2 (tilePos.x, transform.position.y), moveSpeed * Time.deltaTime);
            yield return null; 
        }
        while (transform.position.y != tilePos.y)
        {
            transform.position = Vector2.MoveTowards(transform.position, new Vector2 (transform.position.x, tilePos.y), moveSpeed * Time.deltaTime);
            yield return null;
        }

        hasMoved = true; 
        ResetWeaponIcons(); 
        GetEnemies(); 
        gameManager.MoveStatsPanel(this); 
    }
}
