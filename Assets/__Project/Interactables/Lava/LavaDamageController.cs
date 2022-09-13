using System;
using UnityEngine;

public class LavaDamageController : Interactable {
    [SerializeField]
    private float _damageAmount = 1;

    protected override void OnInteract(GameObject actor) {
        if (actor.TryGetComponent<HealthSystem>(out HealthSystem healthSystem)) {
            healthSystem.TakeDamage(_damageAmount);
        }
        else {
            throw new Exception($"Unexpectedly picked up by actor without {nameof(HealthSystem)} component: {actor.name} ({actor.GetInstanceID()})");
        }
    }
}
