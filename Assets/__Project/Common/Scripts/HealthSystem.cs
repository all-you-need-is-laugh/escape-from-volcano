using UnityEngine;

public class HealthSystem : MonoBehaviour {
    private float _currentHealth = 0;

    [field: SerializeField]
    public float maxHealth { get; private set; }

    // [SerializeField]
    // private float _testDelta = 0;

    public event System.Action<float> OnHealthChanged;

    public void TakeDamage(float damage) {
        ChangeValue(_currentHealth, Mathf.Max(_currentHealth - Mathf.Abs(damage), 0));
    }

    public void TakeHeal(float healAmount) {
        ChangeValue(_currentHealth, Mathf.Min(_currentHealth + Mathf.Abs(healAmount), maxHealth));
    }

    private void ChangeValue(float prevValue, float newValue) {
        _currentHealth = newValue;

        if (prevValue != newValue) {
            OnHealthChanged?.Invoke(newValue);
        }
    }

    void Awake() {
        _currentHealth = maxHealth;
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
