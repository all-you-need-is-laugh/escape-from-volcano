using UnityEngine;

public class HeartController : Pickupable {
    [SerializeField]
    private float _healAmount = 1;

    public override void OnPickUp(GameObject actor) {
        if (actor.TryGetComponent<HealthSystem>(out HealthSystem healthSystem)) {
            healthSystem.TakeHeal(_healAmount);
        }
    }
}
