using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotZoneCheckScript : MonoBehaviour
{
    private EnemyScript enemy;
    private bool inRange;
    private Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        enemy = GetComponentInParent<EnemyScript>();
        anim = GetComponentInParent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (inRange && !anim.GetCurrentAnimatorStateInfo(0).IsName("enemy_1_attack_1"))
            enemy.Flip();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            inRange = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player") {
            inRange = false;
            gameObject.SetActive(false);
            enemy.triggerArea.SetActive(true);
            enemy.inRange = false;
            enemy.SelectTarget();
        }
    }
}
