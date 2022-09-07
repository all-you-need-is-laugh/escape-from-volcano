using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour {
    [SerializeField]
    private float _moveSpeed = 3f;
    [SerializeField]
    private float _jumpForce = 60f;

    private Rigidbody2D _rigidbody;
    private Collider2D _collider;
    private Animator _animator;

    private bool _canJump = false;
    private float _moveHorizontal;
    private float _moveVertical;
    private bool _facingRight = true;

    void Awake() {
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();

        var rigidbodyColliders = new List<Collider2D>();
        var rigidbodyCollidersNumber = _rigidbody.GetAttachedColliders(rigidbodyColliders);
        if (rigidbodyCollidersNumber != 1) {
            throw new Exception($"Unexpected number of attached colliders to {name}: {rigidbodyCollidersNumber} (expected 1)");
        }
        _collider = rigidbodyColliders[0];
    }

    void FixedUpdate() {
        _rigidbody.velocity = new Vector2(_moveHorizontal * _moveSpeed, _rigidbody.velocity.y);
        _animator.SetFloat("Speed", Mathf.Abs(_moveHorizontal));

        if (_canJump && _moveVertical > 0.1f) {
            _rigidbody.AddForce(new Vector2(0, _moveVertical * _jumpForce), ForceMode2D.Impulse);
            _canJump = false;
        }
    }

    void HandleFlip(float direction) {
        if (direction == 0) return;

        float directionSign = Mathf.Sign(direction);
        if (_facingRight != directionSign > 0) {
            Vector2 localScale = transform.localScale;
            localScale.x = directionSign;
            transform.localScale = localScale;
            _facingRight = !_facingRight;
        }
    }

    public void HandleJump(InputAction.CallbackContext context) {
        _moveVertical = context.ReadValue<float>();
    }

    public void HandleMovement(InputAction.CallbackContext context) {
        _moveHorizontal = context.ReadValue<float>();
        HandleFlip(_moveHorizontal);
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.collider.tag == "Environment") {
            _canJump = _canJump || GetColliderBelow() == collision.collider;
        }
    }

    Collider2D GetColliderBelow() {
        var colliderSize = _collider.bounds.size;

        RaycastHit2D hit = Physics2D.BoxCast(
            _collider.bounds.center - new Vector3(0, colliderSize.y / 2 + 0.02f, 0),
            new Vector2(colliderSize.x, 0.01f),
            transform.eulerAngles.z,
            Vector2.down,
            0
        );

        return hit.collider;
    }
}
