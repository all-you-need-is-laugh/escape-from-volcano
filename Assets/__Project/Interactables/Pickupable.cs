using UnityEngine;

public abstract class Pickupable : Interactable {
    protected abstract void OnPickUp(GameObject actor);

    protected override void OnInteract(GameObject actor) {
        OnPickUp(actor);
        Destroy(gameObject);
    }
}
