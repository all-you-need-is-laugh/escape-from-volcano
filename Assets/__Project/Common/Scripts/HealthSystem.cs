using UnityEngine;

public class HealthSystem : MonoBehaviour {
    private float _currentHealth = 0;

    [SerializeField]
    private float _maxHealth = 3;

    // [SerializeField]
    // private float _testDelta = 0;

    public event System.Action OnDeath;

    public void TakeDamage(float damage) {
        if (_currentHealth <= 0) return;

        _currentHealth = Mathf.Max(_currentHealth - Mathf.Abs(damage), 0);

        if (_currentHealth == 0) {
            OnDeath?.Invoke();
        }
    }

    public void TakeHeal(float healAmount) {
        _currentHealth = Mathf.Min(_currentHealth + Mathf.Abs(healAmount), _maxHealth);
    }

    void Awake() {
        _currentHealth = _maxHealth;
    }

    // void Update() {
    //     if (_testDelta > 0) {
    //         TakeHeal(_testDelta);
    //     }
    //     else if (_testDelta < 0) {
    //         TakeDamage(_testDelta);
    //     }

    //     _testDelta = 0;
    // }
}
