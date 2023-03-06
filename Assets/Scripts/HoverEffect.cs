using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverEffect : MonoBehaviour
{
    public float hoverAmount; 

    private GameManager gameManager; 

    void Start()
    {
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
}
