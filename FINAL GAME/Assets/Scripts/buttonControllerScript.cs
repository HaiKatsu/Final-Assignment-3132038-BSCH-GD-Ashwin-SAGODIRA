using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class buttonControllerScript : MonoBehaviour
{
    public GameManagerScript gameManager;
    // Start is called before the first frame update

    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("Game Manager").GetComponent<GameManagerScript>();
    }

    public void RestartGame()
    {
        Destroy(gameManager.gameObject);
        SceneManager.LoadScene("Scene 1");
    }

    public void QuitGame()
    {
        UnityEditor.EditorApplication.isPlaying = false;
        Application.Quit();
    }
}
