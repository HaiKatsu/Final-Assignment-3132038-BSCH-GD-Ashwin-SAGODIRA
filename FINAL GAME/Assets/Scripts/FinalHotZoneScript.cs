using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalBossHotZoneScript : MonoBehaviour
{
    private FinalBossScript boss;
    private bool inRange;
    private Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        boss = GetComponentInParent<FinalBossScript>();
        anim = GetComponentInParent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (inRange && !anim.GetCurrentAnimatorStateInfo(0).IsName("boss_attack"))
            boss.Flip();
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
            boss.triggerArea.SetActive(true);
            boss.inRange = false;
            boss.SelectTarget();
        }
    }
}
