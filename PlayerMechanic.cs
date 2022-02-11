using System;
using UnityEngine;

public class PlayerMechanic : MonoBehaviour
{
    public Material deadMat;

    public static Action EventDroneTurnsOff;

    private Material defaultMat;
    private bool isDead;

    // Debug
    private Vector3 initial_position;

    void Start()
    {
        defaultMat = GetComponent<SpriteRenderer>().material;
        
        // Debug
        initial_position = transform.position;
    }

    void Update()
    {
        // Debug
        Respawn();
    }
    
    private void PlayerDies()
    {
        // turn off simulation
        // change color to red for indicator
        isDead = true;
        GetComponent<Rigidbody2D>().simulated = false;
        GetComponent<SpriteRenderer>().material = deadMat;
    }

    // Debug
    private void Respawn()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            transform.position = initial_position;

            if(isDead)
            {
                isDead = false;
                GetComponent<Rigidbody2D>().simulated = true;
                GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                GetComponent<SpriteRenderer>().material = defaultMat;
            }
        }
    }
}
