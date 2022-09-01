using UnityEngine;
using UnityEngine.InputSystem;

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

    void FixedUpdate() {
        _rigidbody.velocity = new Vector2(_moveHorizontal * _moveSpeed, _rigidbody.velocity.y);

        if (_canJump && _moveVertical > 0.1f) {
            _rigidbody.AddForce(new Vector2(0, _moveVertical * _jumpForce), ForceMode2D.Impulse);
            _canJump = false;
        }
    }

    public void HandleJump(InputAction.CallbackContext context) {
        _moveVertical = context.ReadValue<float>();
    }

    public void HandleMovement(InputAction.CallbackContext context) {
        _moveHorizontal = context.ReadValue<float>();
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.collider.tag == "Environment") {
            _canJump = GetColliderBelow() == collision.collider;
        }
    }

    Collider2D GetColliderBelow() {
        RaycastHit2D hit = Physics2D.BoxCast(
            transform.position - new Vector3(0, transform.localScale.y / 2 + 0.02f, 0),
            new Vector2(transform.localScale.x, 0.01f),
            transform.eulerAngles.z,
            Vector2.down,
            0
        );

        return hit.collider;
    }
}
