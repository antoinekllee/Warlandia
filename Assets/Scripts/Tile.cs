using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    private SpriteRenderer spriteRenderer; 
    public Sprite[] tileGraphics; 

    public float hoverAmount; 

    public LayerMask obstacleLayer; 

    public Color highlightedColour; 
    public bool isWalkable; 
    private GameManager gameManager; 

    public Color creatableColour; 
    public bool isCreatable; 

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>(); 
        int randomTile = Random.Range (0, tileGraphics.Length); 
        spriteRenderer.sprite = tileGraphics[randomTile]; 


        gameManager = FindObjectOfType<GameManager>(); 
    }

    private void OnMouseEnter()
    {
        if (gameManager.playerTurn != 0)
            transform.localScale += Vector3.one * hoverAmount; 
    }

    private void OnMouseExit()
    {
        if (gameManager.playerTurn != 0)
            transform.localScale -= Vector3.one * hoverAmount;
    }

    public bool IsClear()
    {
        Collider2D obstacle = Physics2D.OverlapCircle(transform.position, 0.2f, obstacleLayer); 
        if (obstacle != null)
        {
            return false; 
        }
        else
        {
            return true; 
        }
    }

    public void Highlight()
    {
        spriteRenderer.color = highlightedColour; 
        isWalkable = true; 
    }

    public void Reset()
    {
        spriteRenderer.color = Color.white; 
        isWalkable = false;
        isCreatable = false;
    }

    public void SetCreatable()
    {
        spriteRenderer.color = creatableColour; 
        isCreatable = true; 
    }

    public void OnMouseDown()
    {
        if (isWalkable && gameManager.selectedUnit != null)
        {
            gameManager.selectedUnit.Move(this.transform.position); 
        }
        else if (isCreatable)
        {
            if (gameManager.playerTurn == 1)
            {
                gameManager.playerOneGold -= gameManager.itemCost; 
            }
            else if (gameManager.playerTurn == 2)
            {
                gameManager.playerTwoGold -= gameManager.itemCost;
            }

            gameManager.gameObject.GetComponent<Barrak>().ResetItemButtons(); 

            gameManager.UpdateGoldText();

            BarrakItem item = Instantiate (gameManager.purchasedItem, new Vector2 (transform.position.x, transform.position.y), Quaternion.identity);
            gameManager.ResetTiles(); 
            Unit unit = item.GetComponent<Unit>(); 

            if (unit != null)
            {
                unit.hasMoved = true; 
                unit.hasAttacked = true; 
            }
        }
    }
}
