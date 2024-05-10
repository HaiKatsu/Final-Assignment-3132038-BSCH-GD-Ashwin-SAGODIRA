using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointScript : MonoBehaviour
{

    private GameManagerScript gameManager;
    private Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("Game Manager").GetComponent<GameManagerScript>();
        anim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            gameManager.checkPoint = transform;
            ActivateCheckpoint();
        }
    }

    public void ActivateCheckpoint()
    {
        anim.SetBool("IsActive", true);
        gameManager.lastActivatedCheckpoint = transform;
    }

    public void DeactivateCheckpoint()
    {
        anim.SetBool("IsActive", false);
    }
}
