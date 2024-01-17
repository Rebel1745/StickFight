using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StickFight
{
    public class Dash : MonoBehaviour
    {
        [SerializeField] [Range(20f, 100f)] private float _dashSpeed = 30f;
        [SerializeField] [Range(0.1f, 0.5f)] private float _dashDuration = 0.2f;
        [SerializeField] [Range(0, 5)] private int _maxDashes = 1;
        private bool _isDashing;
        private float _originalGravity;
        private int _currentDashNumber = 0;

        private Controller _controller;
        private Rigidbody2D _body;
        private CollisionDataRetriever _collisionDataRetriever;
        private Vector2 _velocity;

        private void Awake()
        {
            _body = GetComponent<Rigidbody2D>();
            _collisionDataRetriever = GetComponent<CollisionDataRetriever>();
            _controller = GetComponent<Controller>();
        }

        private void Start()
        {
            _originalGravity = _body.gravityScale;
            _currentDashNumber = 0;
        }

        private void Update()
        {
            // if we are grounded or on a wall, reset our dashes so we can dash again
            if (_collisionDataRetriever.OnGround || _collisionDataRetriever.OnWall)
            {
                _currentDashNumber = 0;
            }

            if(_controller.input.RetrieveDashInput(this.gameObject))
            {
                if (!_isDashing && _currentDashNumber <= _maxDashes)
                    StartCoroutine(DoDash());
                else
                    _controller.input.DashFinished();
            }
        }

        void ResetDash()
        {
            _body.gravityScale = _originalGravity;
            _isDashing = false;
            _body.velocity = Vector2.zero;
            _controller.input.DashFinished();
        }

        private IEnumerator DoDash()
        {
            _currentDashNumber++;
            _isDashing = true;

            _body.gravityScale = 0f;

            int dashDir = _controller.input.RetrieveDashDirection(this.gameObject);
            _body.velocity = Vector2.right * dashDir * _dashSpeed;

            yield return new WaitForSeconds(_dashDuration);

            ResetDash();
        }
    }
}
