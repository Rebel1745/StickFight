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
        private bool _isDashing, _isInputMuted, _isDashingInput, _isPunchingInputMuted, _isKickingInputMuted;
        private float _originalGravity, _currentDashDuration = 0f, _wallDirectionX;
        private int _currentDashNumber = 0, _dashDirection;

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
            _isDashingInput = _controller.input.RetrieveDashInput(false);
            _isPunchingInputMuted = _controller.input.RetrievePunchInput(true);
            _isKickingInputMuted = _controller.input.RetrieveKickInput(true);

            // if we are grounded or on a wall, reset our dashes so we can dash again
            if (_collisionDataRetriever.OnGround || _collisionDataRetriever.OnWall)
            {
                _currentDashNumber = 0;
            }

            // if we are not currently dashing, and we are pressing dash, start dashing
            if(!_isDashing && _isDashingInput && _currentDashNumber <= _maxDashes)
            {
                StartDash();
            }

            UpdateDash();
        }

        void StartDash()
        {
            _anim.SetBool("isDashing", true);
            _currentDashNumber++;
            _isDashing = true;
            _body.gravityScale = 0f;
            _currentDashDuration = 0;
            _dashDirection = _controller.input.RetrieveDashDirection(false);

            _body.velocity = Vector2.right * _dashDirection * _dashSpeed;

            // mute the input so nothing can happen when we are dashing
            _controller.input.UpdateInputMuting(true);
        }

        void UpdateDash()
        {
            if (!_isDashing)
                return;

            _currentDashDuration += Time.deltaTime;

            if (_isPunchingInputMuted)
                _anim.SetBool("isPunching", true);

            if (_isKickingInputMuted)
                _anim.SetBool("isKicking", true);

            // if we have reached or surpassed the intedend duration of the dash, stop it
            if (_currentDashDuration >= _dashDuration)
                StopDash();

            // if we hit a wall, stop dashing
            if (_collisionDataRetriever.OnWall)
            {
                // wallDirectionX = 1 if wall is on players left, -1 if it is on the right
                _wallDirectionX = _collisionDataRetriever.ContactNormal.x;

                // if the dash direction IS in the direction of the wall, stop the dash
                if(Mathf.Sign(_wallDirectionX) != Mathf.Sign(_dashDirection))
                    StopDash();
            }
        }

        void StopDash()
        {
            _controller.input.DashFinished();
            _controller.input.UpdateInputMuting(false);
            _body.gravityScale = _originalGravity;
            _isDashing = false;
            _body.velocity = Vector2.zero;
            _anim.SetBool("isDashing", false);
            _anim.SetBool("isPunching", false);
            _anim.SetBool("isKicking", false);
        }
    }
}
