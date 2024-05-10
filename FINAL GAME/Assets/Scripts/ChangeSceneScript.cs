using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSceneScript : MonoBehaviour
{
    GameManagerScript gameManager;

    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("Game Manager").GetComponent<GameManagerScript>();
    }


    public void OnTriggerEnter2D(Collider2D other)
    {

        if (other.gameObject.tag == "Player" && UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "Scene 1") {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Scene 2");
        }
        
        if (other.gameObject.tag == "Player" && UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "Scene 2") {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Scene 3");
        }
    }
}
