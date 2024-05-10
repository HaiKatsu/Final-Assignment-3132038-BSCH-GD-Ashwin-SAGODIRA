using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameUIScript : MonoBehaviour
{
    public TMP_Text lifeText;
    public TMP_Text scoreText;
    public TMP_Text cooldownText;
    public GameManagerScript gameManager;

    public CharacterControllerScript player;


    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("Game Manager").GetComponent<GameManagerScript>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterControllerScript>();
    }

    // Update is called once per frame
    void Update()
    {
        lifeText.text = "Life: " + gameManager.life;
        scoreText.text = "Score: " + gameManager.score;

        if (Time.time - player.getLastAttackTime() > player.getAttackCooldown())
            cooldownText.text = "Attack: Ready";
        else
            cooldownText.text = "Attack: " + (player.getAttackCooldown() - (Time.time - player.getLastAttackTime())).ToString("F2");

    }
}
