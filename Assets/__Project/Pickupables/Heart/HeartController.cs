using UnityEngine;

public class HeartController : Pickupable {
    public override void OnPickUp(GameObject actor) {
        Debug.Log($"{actor.name} picked up the heart!");
    }
}
