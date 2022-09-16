using System;
using UnityEngine;

[RequireComponent(typeof(HealthSystem))]
public class EnemyController : Interactable {
    [SerializeField]
    private GameObject _healthBar;
    [SerializeField]
    private float _playerDamageAmount = 0.5f;

    private HealthSystem _healthSystem;
    private HealthBarController _healthBarController;

    void Awake() {
        _healthSystem = GetComponent<HealthSystem>();

        _healthBarController = _healthBar.GetComponent<HealthBarController>();
        _healthBarController.SetMaxHealth(_healthSystem.maxHealth);
        _healthBarController.SetHealth(_healthSystem.maxHealth);
    }

    void OnEnable() {
        _healthSystem.OnHealthChanged += OnHealthChanged;
    }

    void OnDisable() {
        _healthSystem.OnHealthChanged -= OnHealthChanged;
    }

    void Die() {
        Debug.Log("Enemy died!");
        Destroy(gameObject);
    }

    void OnHealthChanged(float currentHealth) {
        _healthBarController.SetHealth(currentHealth);

        if (currentHealth == 0) {
            Die();
        }
    }

    protected override void OnInteract(GameObject actor) {
        if (actor.TryGetComponent<HealthSystem>(out HealthSystem healthSystem)) {
            healthSystem.TakeDamage(_playerDamageAmount);
        }
        else {
            throw new Exception($"Unexpectedly interacted with actor without {nameof(HealthSystem)} component: {actor.name} ({actor.GetInstanceID()})");
        }
    }
}
