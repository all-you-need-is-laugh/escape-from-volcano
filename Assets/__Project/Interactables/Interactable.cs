using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public abstract class Interactable : MonoBehaviour {
    protected abstract void OnInteract(GameObject actor);

    private void OnTriggerEnter2D(Collider2D collider) {
        if (collider.tag == "Player") {
            OnInteract(collider.gameObject);
        }
    }
}
