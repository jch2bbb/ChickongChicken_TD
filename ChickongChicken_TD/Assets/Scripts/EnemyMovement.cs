using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody2D rb;

    [Header("Attributes")]
    [SerializeField] private float moveSpeed = 2f;

    private Transform target;
    private int pathIndex = 0;
    private float currentMoveSpeed;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        currentMoveSpeed = moveSpeed;
        target = LevelManager.main.path[pathIndex];
    }

    private void Update()
    {
        if (Vector2.Distance(target.position, transform.position) <= 0.1f)
        {
            pathIndex++;

            if (pathIndex >= LevelManager.main.path.Length)
            {
                EnemySpawner.onEnemyDestroy.Invoke();
                Destroy(gameObject);
                return;
            }

            target = LevelManager.main.path[pathIndex];
        }
    }

    private void FixedUpdate()
    {
        Vector2 direction = (target.position - transform.position).normalized;
        rb.linearVelocity = direction * currentMoveSpeed;
    }

    public void ApplySlow(float slowMultiplier, float duration)
    {
        StopAllCoroutines();
        StartCoroutine(SlowCoroutine(slowMultiplier, duration));
    }

    private IEnumerator SlowCoroutine(float slowMultiplier, float duration)
    {
        currentMoveSpeed = moveSpeed * slowMultiplier;
        yield return new WaitForSeconds(duration);
        currentMoveSpeed = moveSpeed;
    }

    public void UpdateSpeed(float newSpeed)
    {
        currentMoveSpeed = newSpeed;
    }

    public void ResetSpeed()
    {
        currentMoveSpeed = moveSpeed;
    }

    public void ReachedBase()
    {
        // Notify spawner enemy is gone but do NOT count as kill
        EnemySpawner.onEnemyDestroy.Invoke();
        Destroy(gameObject);
    }
}