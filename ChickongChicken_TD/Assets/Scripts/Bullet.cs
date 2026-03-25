using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody2D rb;

    [Header("Attributes")]
    [SerializeField] private float bulletSpeed = 5f;
    [SerializeField] private int bulletDamage = 1;

    private Transform target;

    private Camera mainCamera;
    private float destroyMargin = 100f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        mainCamera = Camera.main;
    }

    public void SetTarget(Transform _target)
    {
        target = _target;
    }

    private void FixedUpdate()
    {
        if (!target) return;

        Vector2 direction = (target.position - transform.position).normalized;
        rb.linearVelocity = direction * bulletSpeed;
    }

    private void Update()
    {
        if (IsOutOfCameraBounds())
        {
            Destroy(gameObject);
        }
    }

    private bool IsOutOfCameraBounds()
    {
        Vector3 screenPos = mainCamera.WorldToScreenPoint(transform.position);

        if (screenPos.x < -destroyMargin ||
            screenPos.x > Screen.width + destroyMargin ||
            screenPos.y < -destroyMargin ||
            screenPos.y > Screen.height + destroyMargin)
        {
            return true;
        }

        return false;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        Health health = other.gameObject.GetComponent<Health>();
        if (health != null)
        {
            health.TakeDamage(bulletDamage);
        }
        Destroy(gameObject);
    }
}