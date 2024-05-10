using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillZoneScript : MonoBehaviour
{
    private GameManagerScript gameManager;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("Game Manager").GetComponent<GameManagerScript>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player") && gameManager.life > 0)
        {
            if (gameManager.lastActivatedCheckpoint != null) {
                col.transform.position = gameManager.lastActivatedCheckpoint.position;
            }
            else {
                col.transform.position = gameManager.spawnPoint.position;
            }
            Instantiate(gameManager.playerScript.aparitionEffect, col.transform.position, Quaternion.identity);

            gameManager.removeLife(1);

        }
        else if (col.gameObject.CompareTag("Player") && gameManager.life <= 0)
        {
            gameManager.health = 0;
        }
    }
}
