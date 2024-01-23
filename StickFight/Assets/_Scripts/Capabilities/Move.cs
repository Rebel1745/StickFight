using UnityEngine;

namespace StickFight
{
    [RequireComponent(typeof(Controller), typeof(CollisionDataRetriever), typeof(Rigidbody2D))]
    public class Move : MonoBehaviour
    {
        [SerializeField, Range(0f, 100f)] private float _maxSpeed = 4f;
        [SerializeField] bool _useAcceleration = true;
        [SerializeField, Range(0f, 100f)] private float _maxAcceleration = 35f;
        [SerializeField, Range(0f, 100f)] private float _maxAirAcceleration = 20f;

        private Animator _anim;
        private Controller _controller;
        private Vector2 _direction, _desiredVelocity, _velocity;
        private Rigidbody2D _body;
        private CollisionDataRetriever _collisionDataRetriever;

        private float _maxSpeedChange, _acceleration;
        private bool _onGround, _onWall, _isFacingRight = true, _isInputMuted;

        private void Awake()
        {
            _anim = GetComponent<Animator>();
            _body = GetComponent<Rigidbody2D>();
            _collisionDataRetriever = GetComponent<CollisionDataRetriever>();
            _controller = GetComponent<Controller>();
        }

        private void Update()
        {
            _direction = _controller.input.RetrieveMoveInput(false);
            _desiredVelocity = new Vector2(_direction.x, 0f) * Mathf.Max(_maxSpeed - _collisionDataRetriever.Friction, 0f);
            _isInputMuted = _controller.input.RetrieveIsMutedInput();
        }

        private void FixedUpdate()
        {
            if (_isInputMuted)
                return;

            _onGround = _collisionDataRetriever.OnGround;

            _velocity = _body.velocity;
           
            if (_useAcceleration)
            {
                _acceleration = _onGround ? _maxAcceleration : _maxAirAcceleration;
                _maxSpeedChange = _acceleration * Time.deltaTime;
                _velocity.x = Mathf.MoveTowards(_velocity.x, _desiredVelocity.x, _maxSpeedChange);
            }                
            else
                _velocity.x = _desiredVelocity.x;

            _body.velocity = _velocity;

            _anim.SetBool("isGround", _onGround);
            _anim.SetFloat("MovementSpeed", Mathf.Abs(_velocity.x));
            _anim.SetFloat("ClimbSpeed", _direction.y);

            if (_body.velocity.x > 0f && !_isFacingRight)
                Flip();
            else if (_body.velocity.x < 0f && _isFacingRight)
                Flip();
        }

        private void Flip()
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
