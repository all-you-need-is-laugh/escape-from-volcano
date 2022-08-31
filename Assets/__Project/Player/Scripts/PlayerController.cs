using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour {
    [SerializeField]
    private float _moveSpeed = 3f;
    [SerializeField]
    private float _jumpForce = 60f;

    private Rigidbody2D _rigidbody;

    private bool _canJump = false;
    private float _moveHorizontal;
    private float _moveVertical;

    void Awake() {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    void Update() {
        HandleMovementInput();
    }

    void FixedUpdate() {
        _rigidbody.velocity = new Vector2(_moveHorizontal * _moveSpeed, _rigidbody.velocity.y);

        if (_canJump && _moveVertical > 0.1f) {
            _rigidbody.AddForce(new Vector2(0, _moveVertical * _jumpForce), ForceMode2D.Impulse);
        }
    }

    void HandleMovementInput() {
        _moveHorizontal = Input.GetAxisRaw("Horizontal");
        _moveVertical = Input.GetAxisRaw("Vertical");
    }

    void OnTriggerEnter2D(Collider2D collider) {
        if (collider.gameObject.tag == "Environment") {
            _canJump = true;
        }
    }

    void OnTriggerExit2D(Collider2D collider) {
        if (collider.gameObject.tag == "Environment") {
            _canJump = false;
        }
    }
}
