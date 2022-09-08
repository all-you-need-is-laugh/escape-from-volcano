using UnityEngine;

public class CoinController : Pickupable {
    public override void OnPickUp(GameObject actor) {
        Debug.Log($"{actor.name} picked up the coin!");
    }
}
