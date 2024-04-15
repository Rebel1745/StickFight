using UnityEngine;

namespace StickFight
{
    [RequireComponent(typeof(Controller), typeof(CollisionDataRetriever), typeof(Rigidbody2D))]
    public class Move : MonoBehaviour
    {
        [SerializeField, Range(0f, 100f)] private float _maxSpeed = 4f;
        [SerializeField, Range(0f, 100f)] private float _maxAcceleration = 35f;
        [SerializeField, Range(0f, 100f)] private float _maxAirAcceleration = 20f;

        private Animator _anim;
        private Controller _controller;
        private Vector2 _moveInput, _desiredVelocity, _velocity, _autoMoveToWallDir;
        private Rigidbody2D _body;
        private CollisionDataRetriever _collisionDataRetriever;

        private float _maxSpeedChange, _acceleration;
        private bool _onGround, _onWall, _isFacingRight = true, _isInputMuted, _dashInput, _jumpInput, _clingingInput, _isAutoMoveToWall, _autoFlipped;
        private float _wallDirectionX;
        private bool[] _onGroundRays, _onWallRays;  // utilises the multiple rays cast in the collisiondataretriever script

        public bool IsFacingRight { get { return _isFacingRight; } }

        private void Awake()
        {
            _anim = GetComponent<Animator>();
            _body = GetComponent<Rigidbody2D>();
            _collisionDataRetriever = GetComponent<CollisionDataRetriever>();
            _controller = GetComponent<Controller>();
        }

        private void Update()
        {
            _moveInput = _controller.input.RetrieveMoveInput(false);
            _desiredVelocity = new Vector2(_moveInput.x, 0f) * Mathf.Max(_maxSpeed - _collisionDataRetriever.Friction, 0f);
            _isInputMuted = _controller.input.RetrieveIsMutedInput();
            _onGround = _collisionDataRetriever.OnGround;
            _onWall = _collisionDataRetriever.OnWall;
            _wallDirectionX = _collisionDataRetriever.ContactNormal.x;
            _dashInput = _controller.input.RetrieveDashInput(false);
            _jumpInput = _controller.input.RetrieveJumpInput(false);
            _onGroundRays = _collisionDataRetriever.OnGroundRays;
            _clingingInput = _controller.input.RetrieveWallClimbInput(false);
            _onWallRays = _collisionDataRetriever.OnWallRays;
        }

        private void FixedUpdate()
        {
            if (_isAutoMoveToWall)
                AutoMoveToWall();
            else
                CheckAutoMoveToWall();

            if (_isInputMuted)
                return;

            // if we arent doing a wall bounce (pressing jump but no direction, we ignore the input
            if(!(_jumpInput && _moveInput.x > -0.1f && _moveInput.x < 0.1f) || _onGround)
            {
                _velocity = _body.velocity;

                _acceleration = _onGround ? _maxAcceleration : _maxAirAcceleration;
                _maxSpeedChange = _acceleration * Time.deltaTime;
                _velocity.x = Mathf.MoveTowards(_velocity.x, _desiredVelocity.x, _maxSpeedChange);

                _body.velocity = _velocity;
            }

            _anim.SetBool("isGround", _onGround);
            _anim.SetFloat("MovementSpeed", Mathf.Abs(_velocity.x));
            _anim.SetFloat("ClimbSpeed", _moveInput.y);

            if(_onWall)
            {
                // wallDirectionX = 1 if wall is on players left, -1 if it is on the right
                if (_wallDirectionX == 1 && _isFacingRight)
                    Flip();
                else if (_wallDirectionX == -1 && !_isFacingRight)
                    Flip();
            }
            else
            {
                if (!_isAutoMoveToWall)
                {
                    if (_body.velocity.x > 0f && !_isFacingRight)
                        Flip();
                    else if (_body.velocity.x < 0f && _isFacingRight)
                        Flip();
                }
            }
        }

        private void CheckAutoMoveToWall()
        {
            // check to see if only the first ray is on the ground.  If so, we can transition into grabbing the wall
            if (_onGround && _onGroundRays[0] && !_onGroundRays[1] && _clingingInput)
            {
                _isAutoMoveToWall = true;
                _body.velocity = Vector2.zero;
                _autoFlipped = false;
                _autoMoveToWallDir = _isFacingRight ? transform.right : -transform.right;
                // mute the input so control is out of the players hands
                _controller.input.UpdateInputMuting(true);
            }
        }

        private void AutoMoveToWall()
        {
            Vector3 newVelocity = Vector3.zero;

            if (_onGroundRays[0] && !_autoFlipped)
            {
                // if we are still grounded move the player forward until they are fully over the edge
                _body.velocity = _autoMoveToWallDir * _maxSpeed;
                return;
            }
            if(!_onWallRays[1])
            {
                // if we arent facing the wall, flip the sprite
                if (!_autoFlipped)
                {
                    Flip();
                    _autoFlipped = true;
                }

                // if the middle ray is not touching the wall, move down until it is
                newVelocity = -transform.up * _maxSpeed;
                newVelocity.x = _autoMoveToWallDir.x * 0.1f;
                _body.velocity = newVelocity;
                return;
            }
            //Time.timeScale = 1f;
            _isAutoMoveToWall = false;
            _controller.input.UpdateInputMuting(false);
        }

        public void Flip()
        {
            // Switch the way the player is labelled as facing.
            _isFacingRight = !_isFacingRight;

            if(_isFacingRight)
                this.transform.localScale = new Vector3(1f, 1f, 1f); 
            else
                this.transform.localScale = new Vector3(-1f, 1f, 1f);
        }
    }
}
