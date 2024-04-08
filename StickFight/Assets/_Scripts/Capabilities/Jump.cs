using UnityEngine;

namespace StickFight
{
    [RequireComponent(typeof(Controller), typeof(CollisionDataRetriever), typeof(Rigidbody2D))]
    public class Jump : MonoBehaviour
    {
        [Header("Standard Jump")]
        [SerializeField, Range(0f, 10f)] private float _jumpHeight = 3f;
        [SerializeField, Range(0, 5)] private int _maxAirJumps = 0;
        [SerializeField, Range(0f, 5f)] private float _downwardMovementMultiplier = 3f;
        [SerializeField, Range(0f, 5f)] private float _upwardMovementMultiplier = 1.7f;
        [SerializeField, Range(0f, 0.3f)] private float _coyoteTime = 0.2f;
        [SerializeField, Range(0f, 0.3f)] private float _jumpBufferTime = 0.2f;

        [Header("Wall Jump")]
        [SerializeField] private Vector2 _wallJumpBounce = new Vector2(10.7f, 10f);
        [SerializeField] private Vector2 _wallJumpLeap = new Vector2(14f, 12f);

        private Animator _anim;
        private Controller _controller;
        private Rigidbody2D _body;
        private CollisionDataRetriever _collisionDataRetriever;
        private Vector2 _velocity;

        private int _jumpPhase;
        private float _defaultGravityScale, _jumpSpeed, _coyoteCounter, _jumpBufferCounter, _wallDirectionX;
        private bool _jumpInput, _onGround, _onWall, _isJumping, _isJumpReset, _isInputMuted, _dashInput;
        private Vector2 _moveInput;
        
        void Awake()
        {
            _anim = GetComponent<Animator>();
            _body = GetComponent<Rigidbody2D>();
            _collisionDataRetriever = GetComponent<CollisionDataRetriever>();
            _controller = GetComponent<Controller>();

            _isJumpReset = true;
            _defaultGravityScale = 1f;
        }
        
        void Update()
        {
            _jumpInput = _controller.input.RetrieveJumpInput(false);
            _dashInput = _controller.input.RetrieveDashInput(true);
            _isInputMuted = _controller.input.RetrieveIsMutedInput();
            _moveInput = _controller.input.RetrieveMoveInput(false);
            _wallDirectionX = _collisionDataRetriever.ContactNormal.x;
        }

        void ResetJump()
        {
            _jumpPhase = 0;
            _coyoteCounter = _coyoteTime;
            _isJumping = false;
            _anim.SetBool("isJumping", false);
        }

        private void FixedUpdate()
        {
            if (_isInputMuted) return;

            _onGround = _collisionDataRetriever.OnGround;
            _onWall = _collisionDataRetriever.OnWall;
            _velocity = _body.velocity;

            if ((_onGround && _body.velocity.y == 0) || _onWall)
            {
                ResetJump();
            }
            else
            {
                _coyoteCounter -= Time.deltaTime;
            }

            if(_onWall && !_onGround)
            {
                CheckForWallJump();
            }
            else
            {
                CheckForJump();
            }
            
        }

        void CheckForWallJump()
        {
            _anim.SetBool("isWall", true);
            if (_jumpInput && _isJumpReset)
            {
                if (_moveInput.x == 0)
                {
                    _velocity = new Vector2(_wallJumpBounce.x * _wallDirectionX, _wallJumpBounce.y);
                    _jumpInput = false;
                    _isJumpReset = false;
                }
                else
                {
                    _velocity = new Vector2(_wallJumpLeap.x * _wallDirectionX, _wallJumpLeap.y);
                    _jumpInput = false;
                    _isJumpReset = false;
                }

                _anim.SetBool("isWall", false);
                _anim.SetBool("isJumping", true);
            }
            else if (!_jumpInput)
            {
                _isJumpReset = true;
            }

            _body.velocity = _velocity;
        }

        void CheckForJump()
        {
            if (_jumpInput && _isJumpReset)
            {
                _isJumpReset = false;
                _jumpInput = false;
                _jumpBufferCounter = _jumpBufferTime;
            }
            else if (_jumpBufferCounter > 0)
            {
                _jumpBufferCounter -= Time.deltaTime;
            }
            else if (!_jumpInput)
            {
                _isJumpReset = true;
            }

            if (_jumpBufferCounter > 0)
            {
                JumpAction();
            }

            if (!_dashInput) { 
                if (_jumpInput && _body.velocity.y > 0)
                {
                    _body.gravityScale = _upwardMovementMultiplier;
                }
                else if ((!_controller.input.RetrieveJumpInput(false) || _body.velocity.y < 0) && !_controller.input.RetrieveDashInput(false))
                {
                    _body.gravityScale = _downwardMovementMultiplier;
                }
                else if (_body.velocity.y == 0)
                {
                    _body.gravityScale = _defaultGravityScale;
                }
            }

            _body.velocity = _velocity;
        }

        private void JumpAction()
        {
            if (_coyoteCounter > 0f || (_jumpPhase < _maxAirJumps))
            {
                if(_isJumping)
                {
                    _jumpPhase += 1;
                }
                //Time.timeScale = 0.1f;
                _jumpBufferCounter = 0;
                _coyoteCounter = 0;
                _jumpSpeed = Mathf.Sqrt(-2f * Physics2D.gravity.y * _jumpHeight * _upwardMovementMultiplier);
                _isJumping = true;
                
                if (_velocity.y > 0f)
                {
                    _jumpSpeed = Mathf.Max(_jumpSpeed - _velocity.y, 0f);
                }
                else if (_velocity.y < 0f)
                {
                    _jumpSpeed += Mathf.Abs(_body.velocity.y);
                }
                _velocity.y += _jumpSpeed;
                
                _anim.SetBool("isJumping", true);
            }
        }
    }
}

