using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StickFight
{
    public class Dash : MonoBehaviour
    {
        [SerializeField] [Range(20f, 100f)] private float _dashSpeed = 30f;
        [SerializeField] [Range(0.1f, 5f)] private float _dashDuration = 0.2f;
        [SerializeField] [Range(0, 5)] private int _maxDashes = 1;
        private bool _isDashing, _isInputMuted;
        private float _originalGravity;
        private int _currentDashNumber = 0;

        private Animator _anim;
        private Controller _controller;
        private Rigidbody2D _body;
        private CollisionDataRetriever _collisionDataRetriever;
        private Vector2 _velocity;

        private void Awake()
        {
            _anim = GetComponent<Animator>();
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
            _isInputMuted = _controller.input.RetrieveIsMutedInput();
            if (_isInputMuted) return;

            // if we are grounded or on a wall, reset our dashes so we can dash again
            if (_collisionDataRetriever.OnGround || _collisionDataRetriever.OnWall)
            {
                _currentDashNumber = 0;
            }

            if(_controller.input.RetrieveDashInput(false))
            {
                if (!_isDashing && _currentDashNumber <= _maxDashes)
                    StartCoroutine(DoDash());
                //else
                  //  _controller.input.DashFinished();
            }
        }

        void ResetDash()
        {
            _controller.input.DashFinished();
            _controller.input.UpdateInputMuting(false);
            _body.gravityScale = _originalGravity;
            _isDashing = false;
            _body.velocity = Vector2.zero;
            _anim.SetBool("isDashing", false);
        }

        private IEnumerator DoDash()
        {
            _anim.SetBool("isDashing", true);
            _currentDashNumber++;
            _isDashing = true;
            _body.gravityScale = 0f;

            int dashDir = _controller.input.RetrieveDashDirection(false);
            _body.velocity = Vector2.right * dashDir * _dashSpeed;

            // mute the input so nothing can happen when we are dashing
            _controller.input.UpdateInputMuting(true);

            yield return new WaitForSeconds(_dashDuration);

            ResetDash();
        }
    }
}
