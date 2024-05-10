using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplePickUpScript : MonoBehaviour
{

    public float scoreValue;
    public GameManagerScript gameManager;

    public GameObject collectedEffect;

    // Start is called before the first frame update
    void Start()
    {
        scoreValue = 1;
        gameManager = GameObject.FindGameObjectWithTag("Game Manager").GetComponent<GameManagerScript>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            gameManager.addScore(scoreValue);
            Instantiate(collectedEffect, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}
