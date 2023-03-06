using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorFollow : MonoBehaviour
{
    public Vector2 offset; 

    private void Start()
    {
        Cursor.visible = false; 
    }

    public void Update()
    {
        transform.position = (Vector2) Input.mousePosition + offset; 

        if (Cursor.visible)
            Cursor.visible = false; 
    }
}
