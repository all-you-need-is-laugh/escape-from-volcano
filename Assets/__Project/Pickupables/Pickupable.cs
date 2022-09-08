using UnityEngine;

public abstract class Pickupable : MonoBehaviour {
    public abstract void OnPickUp(GameObject actor);

    private void OnTriggerEnter2D(Collider2D collider) {
        if (collider.tag == "Player") {
            OnPickUp(collider.gameObject);
            Destroy(gameObject);
        }
    }
}
