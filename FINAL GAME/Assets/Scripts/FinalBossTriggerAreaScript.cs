using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalBossTriggerAreaScript : MonoBehaviour
{
    private FinalBossScript boss;
    // Start is called before the first frame update
    void Start()
    {
        boss = GetComponentInParent<FinalBossScript>();
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
            boss.target = collision.transform;
            boss.inRange = true;
            boss.hotZone.SetActive(true);
        }
    }
}
