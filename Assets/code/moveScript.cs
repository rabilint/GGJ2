using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveScript : MonoBehaviour
{
    private float posY;

    void Start()
    {
        posY = transform.position.y;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            float posX = transform.position.x;
            
            transform.position = new Vector2(posX - 3, 0);
        }
    }
}
