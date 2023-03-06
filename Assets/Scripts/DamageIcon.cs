using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class DamageIcon : MonoBehaviour
{
    public Sprite[] blueDamageSprites; 
    public Sprite[] redDamageSprites; 

    public float lifetime; 
    public float yOffset; 

    public GameObject blueDestructionEffect;
    public GameObject redDestructionEffect;

    GameManager gameManager;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();

        Invoke ("Destruction", lifetime); 
    }

    public void Setup(int damage, int playerTurn)
    {
        if (playerTurn == 1)
            GetComponent<SpriteRenderer>().sprite = blueDamageSprites[damage - 1];
        else if (playerTurn == 2)
            GetComponent<SpriteRenderer>().sprite = redDamageSprites [damage - 1]; 
    }

    void Destruction()
    {
        if (gameManager.playerTurn == 1)
            Instantiate(blueDestructionEffect, transform.position, Quaternion.identity);
        else if (gameManager.playerTurn == 2)
            Instantiate(redDestructionEffect, transform.position, Quaternion.identity);

        Destroy(gameObject); 
    }
}
