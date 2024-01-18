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
        private AnimationController _playerAnimationController;
        private Rigidbody2D _body;
        private Controller _controller;

        private Vector2 _velocity;
        private bool _onWall, _onGround, _onCeiling, _desiredJump, _isJumpReset, _isClinging;
        private float _wallDirectionX, _wallStickCounter;

        // Start is called before the first frame update
        void Start()
        {
            _anim = GetComponent<Animator>();
            _collisionDataRetriever = GetComponent<CollisionDataRetriever>();
            _body = GetComponent<Rigidbody2D>();
            _controller = GetComponent<Controller>();
            _playerAnimationController = GetComponent<AnimationController>();

            _isJumpReset = true;
        }

        // Update is called once per frame
        void Update()
        {
            _desiredJump = _controller.input.RetrieveJumpInput(this.gameObject);
        }

        private void FixedUpdate()
        {
            _onWall = _collisionDataRetriever.OnWall;
            _velocity = _body.velocity;
            _onGround = _collisionDataRetriever.OnGround;
            _onCeiling = _collisionDataRetriever.OnCeiling;
            _wallDirectionX = _collisionDataRetriever.ContactNormal.x;
            _isClinging = _controller.input.RetrieveWallClimbInput(this.gameObject);

            _anim.SetBool("isWall", _onWall);
            _anim.SetBool("isClinging", _isClinging);

            if (!_onWall)
            {
                _anim.SetFloat("ClimbSpeed", 0f);
            }

            #region Wall Climb
            float gravity = _body.gravityScale;

            if (_onWall && _isClinging)
            {
                _body.gravityScale = 0f;
                _velocity.x = 0f;
                _velocity.y = _controller.input.RetrieveMoveInput(this.gameObject).y * _wallClimbMaxSpeed;

                if (_velocity.y >= 0)
                    _playerAnimationController.ChangeAnimationState(AnimationToPlay.WallClimb);
                else
                    _playerAnimationController.ChangeAnimationState(AnimationToPlay.WallSlide);
            }
            else
            {
                _body.gravityScale = gravity;
            }
            #endregion

            #region Wall Slide
            if (_onWall && !_isClinging)
            {
                if(_velocity.y < -_wallSlideMaxSpeed)
                {
                    _velocity.y = -_wallSlideMaxSpeed;
                    _playerAnimationController.ChangeAnimationState(AnimationToPlay.WallSlide);
                }
            }
            #endregion

            #region Wall Stick
            if (_onWall && !_onGround && !WallJumping && !_controller.input.RetrieveDashInput(this.gameObject))
            {
                if (_wallStickCounter > 0)
                {
                    _velocity.x = 0;

                    if (_controller.input.RetrieveMoveInput(this.gameObject).x != 0 &&
                        Mathf.Sign(_controller.input.RetrieveMoveInput(this.gameObject).x) == Mathf.Sign(_collisionDataRetriever.ContactNormal.x))
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
            }
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
                    if (_controller.input.RetrieveMoveInput(this.gameObject).x == 0)
                    {
                        _velocity = new Vector2(_wallJumpBounce.x * _wallDirectionX, _wallJumpBounce.y);
                        WallJumping = true;
                        _desiredJump = false;
                        _isJumpReset = false;
                    }
                    else if (Mathf.Sign(-_wallDirectionX) == Mathf.Sign(_controller.input.RetrieveMoveInput(this.gameObject).x))
                    {
                        _velocity = new Vector2(_wallJumpClimb.x * _wallDirectionX, _wallJumpClimb.y);
                        WallJumping = true;
                        _desiredJump = false;
                        _isJumpReset = false;
                    }
                    else
                    {
                        _velocity = new Vector2(_wallJumpLeap.x * _wallDirectionX, _wallJumpLeap.y);
                        WallJumping = true;
                        _desiredJump = false;
                        _isJumpReset = false;
                    }
                    _anim.SetBool("isJumping", true);
                    _playerAnimationController.ChangeAnimationState(AnimationToPlay.Jump);
                }
                else if (!_desiredJump)
                {
                    _isJumpReset = true;
                }
            }
            #endregion

            #region On Ceiling
            if(_onCeiling && _controller.input.RetrieveWallClimbInput(this.gameObject))
            {
                _body.gravityScale = 0f;
                _velocity.x = _controller.input.RetrieveMoveInput(this.gameObject).x * _wallClimbMaxSpeed;
                _velocity.y = 0f;
                _playerAnimationController.ChangeAnimationState(AnimationToPlay.CeilingMove);
            }
            else
            {
                _body.gravityScale = gravity;
            }

            #endregion

            _body.velocity = _velocity;
            _anim.SetFloat("ClimbSpeed", _velocity.y);
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
