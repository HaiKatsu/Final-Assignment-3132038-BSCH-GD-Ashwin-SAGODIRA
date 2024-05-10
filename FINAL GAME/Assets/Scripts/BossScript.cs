using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossScript : MonoBehaviour {

    public float attackDistance;
    public float moveSpeed;
    public float timer;
    public Transform leftLimit;
    public Transform rightLimit;
    [HideInInspector] public Transform target;
    [HideInInspector] public bool inRange;
    public GameObject hotZone;
    public GameObject triggerArea;
    public GameObject hitBox;
    public bool isInvincible;
    public float health;
    public GameObject col;
    public GameObject[] hearts;

    private Animator anim;
    private float distance;
    private bool attackMode;
    private bool cooling;
    private float intTimer;
    private CharacterControllerScript player;
    private GameManagerScript gameManager;
    private float invincibleTime;
    private float invincibleStartTime;
    private Rigidbody2D rb;

    void Start()
    {   
        SelectTarget();
        intTimer = timer;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterControllerScript>();
        gameManager = GameObject.FindGameObjectWithTag("Game Manager").GetComponent<GameManagerScript>();
        health = 10;
        timer = 3;
        isInvincible = false;
        invincibleTime = 2.0f;
    }

    void Update () {
        if (health <= 0)
            anim.SetBool("IsDead", true);

        if (isInvincible && Time.time - invincibleStartTime >= invincibleTime)
            isInvincible = false;

        if (attackMode && player.getCollider().IsTouching(hitBox.GetComponent<Collider2D>()))
            gameManager.removeHealth(2);


        if (player.getHitbox().IsTouching(col.GetComponent<Collider2D>()) && player.isAttacking && !isInvincible)
            removeHealth(1);


        if (!attackMode)
            Move();

        if (!InsideofLimites() && !inRange && !anim.GetCurrentAnimatorStateInfo(0).IsName("boss_attack"))
            SelectTarget();

        if (inRange)
            EnemyLogic();
	}

    void EnemyLogic()
    {
        distance = Vector2.Distance(transform.position, target.position);

        if (distance > attackDistance)
            StopAttack();
        else if (attackDistance >= distance && cooling == false && player.anim.GetBool("IsDead") == false)
            Attack();

        if (cooling) {
            Cooldown();
            anim.SetBool("Attack", false);
        }
    }

    void Move()
    {
        if (anim.GetBool("IsDead") == false && anim.GetBool("Attack") == false && player.anim.GetBool("IsDead") == false) {
            anim.SetBool("canWalk", true);

            if (!anim.GetCurrentAnimatorStateInfo(0).IsName("boss_attack")) {
                Vector2 targetPosition = new Vector2(target.position.x, transform.position.y);
                transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            }
        }
    }

    void Attack()
    {
        timer = intTimer;
        attackMode = true;

        anim.SetBool("canWalk", false);
        anim.SetBool("Attack", true);        
    }

    void StopAttack()
    {
        cooling = false;
        attackMode = false;
        anim.SetBool("Attack", false);
    }

    void Cooldown()
    {
        timer -= Time.deltaTime;

        if (timer <= 0 && cooling && attackMode) {
            cooling = false;
            timer = intTimer;
        }
    }    

    public void TriggerCooling()
    {
        cooling = true;
    }

    private bool InsideofLimites()
    {
        return transform.position.x > leftLimit.position.x && transform.position.x < rightLimit.position.x;
    }

    public void SelectTarget()
    {
        float distanceToLeft = Vector2.Distance(transform.position, leftLimit.position);
        float distanceToRight = Vector2.Distance(transform.position, rightLimit.position);

        if (distanceToLeft > distanceToRight)
            target = leftLimit;
        else
            target = rightLimit;
        
        Flip();
    }

    public void Flip()
    {
        Vector3 rotation = transform.eulerAngles;

        if (transform.position.x > target.position.x)
            rotation.y = 180;
        else
            rotation.y = 0;

        transform.eulerAngles = rotation;
    }

    public void removeHealth(float healthToRemove)
    {
        if (!isInvincible) {
            health -= healthToRemove;
            BecomeInvincible();
        }
    }

    public void BecomeInvincible()
    {
        isInvincible = true;
        invincibleStartTime = Time.time;
    }

    public void DestroyEnemy()
    {
        Destroy(gameObject);
        gameManager.addScore(1);
    }
}
