using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerAreaCheckScript : MonoBehaviour
{
    private EnemyScript enemy;
    // Start is called before the first frame update
    void Start()
    {
        enemy = GetComponentInParent<EnemyScript>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            gameObject.SetActive(false);
            enemy.target = collision.transform;
            enemy.inRange = true;
            enemy.hotZone.SetActive(true);
        }
    }
}
