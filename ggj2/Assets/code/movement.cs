using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movement : MonoBehaviour
{
    [Range(0f,0.5f)] public float speed = 0.5f;

    void FixedUpdate()
    {
        float y = transform.position.y;
        float x = transform.position.x;
        y -= speed;

        transform.position = new Vector2(x, y);
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "reiki")
        { 
            Debug.Log(">gotcha!");
        }
    }

}
