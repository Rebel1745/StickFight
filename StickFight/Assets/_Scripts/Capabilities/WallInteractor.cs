using UnityEngine;

namespace StickFight
{
    [RequireComponent(typeof(Controller), typeof(CollisionDataRetriever), typeof(Rigidbody2D))]
    public class WallInteractor : MonoBehaviour
    {
        public bool WallJumping { get; private set; }

        [Header("Wall Slide")]
        [SerializeField][Range(0.1f, 5f)] private float _wallSlideMaxSpeed = 2f;
        [Header("Wall Jump")]
        [SerializeField] private Vector2 _wallJumpClimb = new Vector2(4f, 12f);
        [SerializeField] private Vector2 _wallJumpBounce = new Vector2(10.7f, 10f);
        [SerializeField] private Vector2 _wallJumpLeap = new Vector2(14f, 12f);
        [Header("Wall Stick")]
        [SerializeField, Range(0.05f, 0.5f)] private float _wallStickTime = 0.25f;
        [Header("Wall Climb")]
        [SerializeField][Range(2f, 10f)] private float _wallClimbMaxSpeed = 4f;
        
        private Animator _anim;
        private CollisionDataRetriever _collisionDataRetriever;
        private Rigidbody2D _body;
        private Controller _controller;

        private Vector2 _velocity, _moveInput;
        private bool _onWall, _onGround, _onCeiling, _desiredJump, _isJumpReset, _isClinging, _isDashing, _isInputMuted;
        private float _wallDirectionX, _wallStickCounter;
        
        void Start()
        {
            _anim = GetComponent<Animator>();
            _collisionDataRetriever = GetComponent<CollisionDataRetriever>();
            _body = GetComponent<Rigidbody2D>();
            _controller = GetComponent<Controller>();

            _isJumpReset = true;
        }
        
        void Update()
        {
            _isInputMuted = _controller.input.RetrieveIsMutedInput();
            _desiredJump = _controller.input.RetrieveJumpInput(false);
            _isClinging = _controller.input.RetrieveWallClimbInput(false);
            _isDashing = _controller.input.RetrieveDashInput(false);
            _moveInput = _controller.input.RetrieveMoveInput(false);
        }

        private void FixedUpdate()
        {
            _onWall = _collisionDataRetriever.OnWall;
            _anim.SetBool("isWall", _onWall);

            _velocity = _body.velocity;
            _onGround = _collisionDataRetriever.OnGround;
            _onCeiling = _collisionDataRetriever.OnCeiling;
            _wallDirectionX = _collisionDataRetriever.ContactNormal.x;

            if (_onWall || _onCeiling)
                _anim.SetBool("isClinging", _isClinging);
            else
                _anim.SetBool("isClinging", false);

            if (_isInputMuted) return;

            #region Wall Climb
            float gravity = _body.gravityScale;

            if (_onWall && _isClinging)
            {
                _anim.SetBool("isCeiling", false);
                _body.gravityScale = 0f;
                _velocity.x = 0f;
                _velocity.y = _moveInput.y * _wallClimbMaxSpeed;
            }
            #endregion

            #region On Ceiling
            if (_onCeiling && _isClinging)
            {
                _anim.SetBool("isCeiling", true);
                _anim.SetBool("isJumping", false);
                _body.gravityScale = 0f;
                _velocity.x = _moveInput.x * _wallClimbMaxSpeed;
                _velocity.y = 0f;

                if (_onWall)
                {
                    // we are at the top of a wall, also touching the ceiling.  This should also allow movement on the y axis
                    _velocity.y = _moveInput.y * _wallClimbMaxSpeed;
                }
            }
            #endregion

            if (!_isClinging)
            {
                _anim.SetBool("isCeiling", false);
                _body.gravityScale = gravity;
                if (!_onWall && !_onGround)
                    _anim.SetBool("isJumping", true);
            }

            #region Wall Slide
            if (_onWall && !_isClinging)
            {
                if(_velocity.y < -_wallSlideMaxSpeed)
                {
                    _velocity.y = -_wallSlideMaxSpeed;
                }
            }
            #endregion

            #region Wall Stick
            /*if (_onWall && !_onGround && !WallJumping && !_isDashing)
            {
                if (_wallStickCounter > 0)
                {
                    _velocity.x = 0;

                    if (__moveInput.x != 0 &&
                        Mathf.Sign(_moveInput.x) == Mathf.Sign(_collisionDataRetriever.ContactNormal.x))
                    {
                        _wallStickCounter -= Time.deltaTime;
                    }
                    else
                    {
                        _wallStickCounter = _wallStickTime;
                    }
                }
                else
                {
                    _wallStickCounter = _wallStickTime;
                }
            }*/
            #endregion

            #region Wall Jump

            if ((_onWall && _velocity.x == 0) || _onGround)
            {
                WallJumping = false;
            }

            if (_onWall && !_onGround)
            {
                if (_desiredJump && _isJumpReset)
                {
                    if (_moveInput.x == 0)
                    {
                        print("Bounce");
                        _velocity = new Vector2(_wallJumpBounce.x * _wallDirectionX, _wallJumpBounce.y);
                        WallJumping = true;
                        _desiredJump = false;
                        _isJumpReset = false;
                    }
                    else if (Mathf.Sign(-_wallDirectionX) == Mathf.Sign(_moveInput.x))
                    {
                        print("Climb");
                        _velocity = new Vector2(_wallJumpClimb.x * _wallDirectionX, _wallJumpClimb.y);
                        WallJumping = true;
                        _desiredJump = false;
                        _isJumpReset = false;
                    }
                    else
                    {
                        print("Leap");
                        _velocity = new Vector2(_wallJumpLeap.x * _wallDirectionX, _wallJumpLeap.y);
                        WallJumping = true;
                        _desiredJump = false;
                        _isJumpReset = false;
                    }

                    _anim.SetBool("isJumping", true);
                }
                else if (!_desiredJump)
                {
                    _isJumpReset = true;
                }
            }
            #endregion

            _body.velocity = _velocity;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            _collisionDataRetriever.EvaluateCollision(collision);
            _isJumpReset = false;

            if(_collisionDataRetriever.OnWall && !_collisionDataRetriever.OnGround && WallJumping)
            {
                _body.velocity = Vector2.zero;
            }
        }
    }
}
