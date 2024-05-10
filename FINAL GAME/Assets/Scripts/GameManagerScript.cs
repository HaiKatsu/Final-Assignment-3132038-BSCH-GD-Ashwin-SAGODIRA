using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerScript : MonoBehaviour
{

    public float health;

    public float score;

    public float life;

    public Transform checkPoint;
    public Transform spawnPoint;
    public Transform lastActivatedCheckpoint;
    public Transform player;
    public GameObject tryAgainButton;
    public GameObject exitButton;
    public GameObject endingText;
    public CharacterControllerScript playerScript;

    public float finalBossHealth;

    public bool isInvincible;
    private float invincibleTime;
    private float invincibleStartTime;


    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;

        checkPoint = GameObject.FindGameObjectWithTag("CheckPoint").transform;
        spawnPoint = GameObject.FindGameObjectWithTag("SpawnPoint").transform;

        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerScript = player.GetComponent<CharacterControllerScript>();

        health = 3;
        score = 0;
        life = 3;
        isInvincible = false;
        invincibleTime = 2.0f;
        finalBossHealth = 10;
    }

    void Update()
    {
        PlayerHealth();

        if (health > 0 && life >= 0 && playerScript.anim.GetBool("IsDead") == false) {
            endingText.SetActive(false);
            tryAgainButton.SetActive(false); 
            exitButton.SetActive(false);
        }

        if (isInvincible && Time.time - invincibleStartTime >= invincibleTime)
            isInvincible = false;

        if (Application.loadedLevelName == "Scene 1") {
            if (health <= 0 && life > 0) {
                playerScript.anim.SetBool("IsDead", true);
                StartCoroutine(WaitForDeathAnimationScene1());
                life -= 1;
                health = 3;
            }
            else if (health <= 0 && life <= 0) {
                playerScript.anim.SetBool("IsDead", true);
                StartCoroutine(DefinitiveDeathScene1());

            }
        }

        if (Application.loadedLevelName == "Scene 2") {
            if (health <= 0 && life > 0) {
                playerScript.anim.SetBool("IsDead", true);
                StartCoroutine(WaitForDeathAnimationScene2());
                life -= 1;
                health = 3;
            }

            else if (health <= 0 && life <= 0) {
                playerScript.anim.SetBool("IsDead", true);
                StartCoroutine(DefinitiveDeathScene2());
            }
        }

        if (Application.loadedLevelName == "Scene 3") {
            if (health <= 0 && life > 0) {
                playerScript.anim.SetBool("IsDead", true);
                StartCoroutine(WaitForDeathAnimationScene3());
                life -= 1;
                health = 3;
            }

            else if (health <= 0 && life <= 0) {
                playerScript.anim.SetBool("IsDead", true);
                StartCoroutine(DefinitiveDeathScene3());
            }
        }

        if (finalBossHealth <= 0) {
            StartCoroutine(WaitForDeathAnimationFinalBoss());
        }


    }

    void PlayerHealth()
    {
        if (health == 2) {
            playerScript.healths[0].SetActive(true);
            playerScript.healths[1].SetActive(true);
            playerScript.healths[2].SetActive(false);
        }
        else if (health == 1) {
            playerScript.healths[0].SetActive(true);
            playerScript.healths[1].SetActive(false);
            playerScript.healths[2].SetActive(false);
        }
        else if (health == 0) {
            playerScript.healths[0].SetActive(false);
            playerScript.healths[1].SetActive(false);
            playerScript.healths[2].SetActive(false);
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Reassign the player reference when a new scene is loaded
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerScript = player.GetComponent<CharacterControllerScript>();
        tryAgainButton = GameObject.FindGameObjectWithTag("TryAgainButton");
        exitButton = GameObject.FindGameObjectWithTag("ExitButton");
        endingText = GameObject.FindGameObjectWithTag("EndingText");
    }

    public void addScore(float scoreToAdd)
    {
        score += scoreToAdd;
    }

    public void addHealth(float healthToAdd)
    {
        health += healthToAdd;
    }

    public void addLife(float lifeToAdd)
    {
        life += lifeToAdd;
    }

    public void removeLife(float lifeToRemove)
    {
        life -= lifeToRemove;
    }

    public void removeHealth(float healthToRemove)
    {
        if (!isInvincible) {
            health -= healthToRemove;
            BecomeInvincible();
        }
    }

    public void removeScore(float scoreToRemove)
    {
        score -= scoreToRemove;
    }

    public void BecomeInvincible()
    {
        isInvincible = true;
        invincibleStartTime = Time.time;
    }

    public void ActivateCheckpoint(Transform checkpoint)
    {
        lastActivatedCheckpoint = checkpoint;
    }

    public IEnumerator WaitForDeathAnimationScene1()
    {
        yield return new WaitForSeconds(2);

        if (lastActivatedCheckpoint != null)
            player.position = lastActivatedCheckpoint.position;
        else
            player.position = spawnPoint.position;
        Instantiate(playerScript.aparitionEffect, player.position, player.rotation);
        playerScript.anim.SetBool("IsDead", false);
        playerScript.healths[0].SetActive(true);
        playerScript.healths[1].SetActive(true);
        playerScript.healths[2].SetActive(true);

    }

    public IEnumerator DefinitiveDeathScene1()
    {
        yield return new WaitForSeconds(2);
        tryAgainButton.SetActive(true); 
        exitButton.SetActive(true);
        playerScript.anim.enabled = false;
    }

    public IEnumerator WaitForDeathAnimationScene2()
    {
        yield return new WaitForSeconds(2);

        SceneManager.LoadScene("Scene 2");
        playerScript.anim.SetBool("IsDead", false);
    }

    public IEnumerator DefinitiveDeathScene2()
    {
        yield return new WaitForSeconds(2);

        tryAgainButton.SetActive(true); 
        exitButton.SetActive(true);
        playerScript.anim.enabled = false;
    }

    public IEnumerator WaitForDeathAnimationScene3()
    {
        // Wait for the length of the death animation
        yield return new WaitForSeconds(2);

        SceneManager.LoadScene("Scene 3");
        playerScript.anim.SetBool("IsDead", false);
        playerScript.healths[0].SetActive(true);
        playerScript.healths[1].SetActive(true);
        playerScript.healths[2].SetActive(true);

    }

    public IEnumerator DefinitiveDeathScene3()
    {
        // Wait for the length of the death animation
        yield return new WaitForSeconds(2);
        tryAgainButton.SetActive(true); 
        exitButton.SetActive(true);
        playerScript.anim.enabled = false;
    }

    public IEnumerator WaitForDeathAnimationFinalBoss()
    {
        // Wait for the length of the death animation
        yield return new WaitForSeconds(1f);

        endingText.SetActive(true);
        tryAgainButton.SetActive(true); 
        exitButton.SetActive(true);

    }
}
