using UnityEngine;

public class CoinController : Pickupable {
    protected override void OnPickUp(GameObject actor) {
        Debug.Log($"{actor.name} picked up the coin!");
    }
}
