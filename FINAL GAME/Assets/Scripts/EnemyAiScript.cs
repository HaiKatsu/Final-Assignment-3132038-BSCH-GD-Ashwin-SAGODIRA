using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAiScript : MonoBehaviour
{
    public enum State
    {
        Idle,
        Patrol,
        Chasing,
        Attacking,
    }

    public float speed;
    public float patrolTime;
    public float idleTime;
    private Animator anim;
    private State state = State.Idle;
    private float nextPatrolTime; // Timestamp of when the next patrol should start
    private float nextIdleTime; // Timestamp of when the next idle should start
    private int direction; // Direction the enemy will patrol in. 1 for right, -1 for left
    public bool isIdle;
    public bool isPatrol;
    public bool isChasing;
    public bool isAttacking;
    private float attackDistance;
    private float nextIdleStartTime;
    private Collider2D col;
    private Collider2D playerCol;

    private GameManagerScript gameManager;

    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("Game Manager").GetComponent<GameManagerScript>();
        col = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();
        playerCol = GameObject.FindGameObjectWithTag("Player").GetComponent<Collider2D>();

        nextPatrolTime = Time.time;
        nextIdleTime = Time.time + patrolTime + idleTime;
        direction = 1;
        isIdle = true;
        isPatrol = false;
        isChasing = false;
        isAttacking = false;
        attackDistance = 0.5f;
    }
    
    void Update()
    {
        if (transform.rotation.z != 0)
        {
            Quaternion rotation = transform.rotation;
            rotation.z = 0;
            transform.rotation = rotation;
        }
    
        if (state == State.Idle)
            OnIdle();
        if (state == State.Patrol)
            OnPatrol();
        if (state == State.Chasing)
            OnChasing();
        if (state == State.Attacking)
            anim.Play("enemy_1_attack_1");
            //OnAttacking();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            state = State.Chasing;
            ChangeBooleanState();
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            state = State.Idle;
            ChangeBooleanState();
            nextPatrolTime = Time.time + idleTime;
        }
    }

    private void ChangeBooleanState()
    {
        isIdle = state == State.Idle;
        isPatrol = state == State.Patrol;
        isChasing = state == State.Chasing;
        isAttacking = state == State.Attacking;
    }

    private void OnIdle() {
        anim.Play("enemy_1_idle");
    
        if (Time.time >= nextPatrolTime)
        {
            state = State.Patrol;
            ChangeBooleanState();
            direction *= -1; // Flip the direction
            nextIdleTime = Time.time + patrolTime + idleTime;
        }
    }

    private void OnPatrol() {
        anim.Play("enemy_1_run");
        transform.Translate(Vector2.right * direction * speed * Time.deltaTime);

        Vector3 scale = transform.localScale;
        scale.x = direction;
        transform.localScale = scale;

        if (Time.time >= nextIdleTime)
        {
            state = State.Idle;
            ChangeBooleanState();
            nextPatrolTime = Time.time + idleTime + patrolTime;
        }
    }

    private void OnChasing()
    {
        anim.Play("enemy_1_run");
        Vector3 playerPosition = GameObject.FindGameObjectWithTag("Player").transform.position;
        if (playerPosition.x < transform.position.x)
            direction = -1;
        else
            direction = 1;
        transform.Translate(Vector2.right * direction * speed * Time.deltaTime);

        Vector3 scale = transform.localScale;
        scale.x = direction;
        transform.localScale = scale;

        // Check if the enemy is close to the player
        if (Vector3.Distance(transform.position, playerPosition) <= attackDistance)
        {
            state = State.Attacking;
            ChangeBooleanState();
        }
    }

    private void OnAttacking()
    {
        StartCoroutine(PrintYesAfterDelay());
    }

    private IEnumerator PrintYesAfterDelay()
    {
        anim.Play("enemy_1_attack_1");
        yield return new WaitForSeconds(0.4f);

        ColliderDistance2D colliderDistance = col.Distance(playerCol);
        
        if (colliderDistance.distance <= 0.2f)
        {
            Debug.Log("YES");
        }
        else
        {
            Debug.Log("Nothing");
        }
    }
}