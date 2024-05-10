using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerScript : MonoBehaviour
{
    public GameObject enemy;
    public GameObject leftLimit;
    public GameObject rightLimit;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            enemy.SetActive(true);
            leftLimit.SetActive(true);
            rightLimit.SetActive(true);
        }
    }
}
