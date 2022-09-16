using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class HealthBarController : MonoBehaviour {
    private Slider _slider;

    void Awake() {
        _slider = GetComponent<Slider>();
    }

    public void SetMaxHealth(float maxHealth) {
        _slider.maxValue = maxHealth;
    }

    public void SetHealth(float health) {
        _slider.value = health;
    }
}
