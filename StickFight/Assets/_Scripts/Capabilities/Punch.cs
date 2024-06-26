using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StickFight
{
    public class Punch : MonoBehaviour
    {
        [Header("Punching")]
        [SerializeField] private float _punchDuration = 0.1f;
        private float _currentDuration;
        [SerializeField] private LayerMask _whatIsEnemy;

        [Header("Standard Punch")]
        [SerializeField, Tooltip("Location of the center of the box that checks hits for standard punches")] private Transform _hitCheckOriginStandardPunch;
        [SerializeField] private Vector2 _hitBoxSizeStandardPunch;
        [SerializeField] private bool _showStandardPunchGizmo = false;

        [Header("Air Punch")]
        [SerializeField, Tooltip("Location of the center of the box that checks hits for standard punches while jumping")] private Transform _hitCheckOriginAirPunch;
        [SerializeField] private Vector2 _hitBoxSizeAirPunch;
        [SerializeField] private bool _showAirPunchGizmo = false;
        [SerializeField] private int _maximumAirPunches = 2;
        private int _currentAirPunches = 0;

        [Header("Dash Punch")]
        [SerializeField, Tooltip("Location of the center of the box that checks hits for dash punches")] private Transform _hitCheckOriginDashPunch;
        [SerializeField] private Vector2 _hitBoxSizeDashPunch;
        [SerializeField] private bool _showDashPunchGizmo = false;

        private Animator _anim;
        private Controller _controller;
        private Gravity _gravity;
        private CollisionDataRetriever _collisionDataRetriever;

        private bool _isPunching, _isPunchingInput, _isDashingInput, _isDashingMutedInput, _isPunchingMutedInput, _onGround;
        private PunchType _punchType;

        private void Awake()
        {
            _anim = GetComponent<Animator>();
            _controller = GetComponent<Controller>();
            _gravity = GetComponent<Gravity>();
            _collisionDataRetriever = GetComponent<CollisionDataRetriever>();
        }

        private void Update()
        {
            _isPunchingInput = _controller.input.RetrievePunchInput(false);
            _isDashingInput = _controller.input.RetrieveDashInput(false);
            _isDashingMutedInput = _controller.input.RetrieveDashInput(true);
            _isPunchingMutedInput = _controller.input.RetrievePunchInput(true);
            _onGround = _collisionDataRetriever.OnGround;

            if (_onGround)
                _currentAirPunches = 0;

            if (!_isPunching && _isPunchingInput && (!_isDashingInput && !_isDashingMutedInput))
            {
                if (_onGround)
                    StartStandardPunch();
                else if (_currentAirPunches < _maximumAirPunches)
                    StartAirPunch();
            }
            else if (!_isPunching && _isPunchingMutedInput && _isDashingInput)
            {
                StartDashPunch();
            }

            if (_isPunching)
                UpdatePunching();
        }

        private void StartAirPunch()
        {
            _currentAirPunches++;
            _isPunching = true;
            _punchType = PunchType.Air;

            _currentDuration = 0f;

            _anim.SetBool("isPunching", true);
        }

        private void StartStandardPunch()
        {
            _isPunching = true;
            _punchType = PunchType.Standard;

            _currentDuration = 0f;

            _anim.SetBool("isPunching", true);
        }

        private void StartDashPunch()
        {
            _isPunching = true;
            _punchType = PunchType.Dash;

            _anim.SetBool("isPunching", true);
        }

        private void UpdatePunching()
        {
            if (_punchType == PunchType.Standard || _punchType == PunchType.Air)
            {
                _currentDuration += Time.deltaTime;
                if (_currentDuration >= _punchDuration)
                    StopPunching();
            }

            if (_punchType == PunchType.Dash)
            {
                if (!_isDashingInput)
                    StopPunching();
            }

            CheckForHit();
        }

        private void StopPunching()
        {
            _isPunching = false;
            _anim.SetBool("isPunching", false);
            _gravity.ResetToDefaultGravity(true, false, "Punch::StopPunching()");
            _controller.input.PunchFinished();
        }

        private void CheckForHit()
        {
            Collider2D[] hits;

            if (_punchType == PunchType.Standard)
                hits = Physics2D.OverlapBoxAll(_hitCheckOriginStandardPunch.position, _hitBoxSizeStandardPunch, 0f, _whatIsEnemy);
            else if (_punchType == PunchType.Air)
                hits = Physics2D.OverlapBoxAll(_hitCheckOriginAirPunch.position, _hitBoxSizeAirPunch, 0f, _whatIsEnemy);
            else
                hits = Physics2D.OverlapBoxAll(_hitCheckOriginDashPunch.position, _hitBoxSizeDashPunch, 0f, _whatIsEnemy);

            if (hits.Length > 0)
            {
                // if we hit something, suspend gravity so we can keep hitting
                _gravity.ZeroGravity(true, true, "Punch::CheckForHit()");
                foreach (Collider2D c in hits)
                {
                    print("Collided with " + c.name);
                    StopPunching();
                }
            }

        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            if (_showStandardPunchGizmo)
                Gizmos.DrawWireCube(_hitCheckOriginStandardPunch.position, new Vector3(_hitBoxSizeStandardPunch.x, _hitBoxSizeStandardPunch.y, 1f));
            if (_showAirPunchGizmo)
                Gizmos.DrawWireCube(_hitCheckOriginAirPunch.position, new Vector3(_hitBoxSizeAirPunch.x, _hitBoxSizeAirPunch.y, 1f));
            if (_showDashPunchGizmo)
                Gizmos.DrawWireCube(_hitCheckOriginDashPunch.position, new Vector3(_hitBoxSizeDashPunch.x, _hitBoxSizeDashPunch.y, 1f));
        }
    }

    public enum PunchType
    {
        Standard,
        Dash,
        Air
    }
}
