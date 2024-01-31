using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StickFight
{
    public class Kick : MonoBehaviour
    {
        [Header("Kicking")]
        [SerializeField] private float _KickDuration = 0.2f;
        private float _currentDuration;
        [SerializeField] private LayerMask _whatIsEnemy;

        [Header("Standard Kick")]
        [SerializeField, Tooltip("Location of the center of the box that checks hits for standard Kickes")] private Transform _hitCheckOriginStandardKick;
        [SerializeField] private Vector2 _hitBoxSizeStandardKick;
        [SerializeField] private bool _showStandardKickGizmo = false;

        [Header("Air Kick")]
        [SerializeField, Tooltip("Location of the center of the box that checks hits for standard Kickes while jumping")] private Transform _hitCheckOriginAirKick;
        [SerializeField] private Vector2 _hitBoxSizeAirKick;
        [SerializeField] private bool _showAirKickGizmo = false;

        [Header("Dash Kick")]
        [SerializeField, Tooltip("Location of the center of the box that checks hits for dash Kickes")] private Transform _hitCheckOriginDashKick;
        [SerializeField] private Vector2 _hitBoxSizeDashKick;
        [SerializeField] private bool _showDashKickGizmo = false;

        private Animator _anim;
        private Controller _controller;

        private bool _isKicking, _isKickingInput, _isDashingInput, _isDashingMutedInput, _isKickingMutedInput;
        private KickType _KickType;

        private void Awake()
        {
            _anim = GetComponent<Animator>();
            _controller = GetComponent<Controller>();
        }

        private void Update()
        {
            _isKickingInput = _controller.input.RetrieveKickInput(false);
            _isDashingInput = _controller.input.RetrieveDashInput(false);
            _isDashingMutedInput = _controller.input.RetrieveDashInput(true);
            _isKickingMutedInput = _controller.input.RetrieveKickInput(true);

            if (!_isKicking && _isKickingInput && (!_isDashingInput && !_isDashingMutedInput))
            {
                StartStandardKick();
            }
            else if (!_isKicking && _isKickingMutedInput && _isDashingInput)
            {
                StartDashKick();
            }

            if (_isKicking)
                UpdateKicking();
        }

        private void StartStandardKick()
        {
            _isKicking = true;
            _KickType = KickType.Standard;

            _currentDuration = 0f;

            _anim.SetBool("isKicking", true);
        }

        private void StartDashKick()
        {
            _isKicking = true;
            _KickType = KickType.Dash;

            _anim.SetBool("isKicking", true);
        }

        private void UpdateKicking()
        {
            if (_KickType == KickType.Standard || _KickType == KickType.Air)
            {
                _currentDuration += Time.deltaTime;
                if (_currentDuration >= _KickDuration)
                    StopKicking();
            }

            if (_KickType == KickType.Dash)
            {
                if (!_isDashingInput)
                    StopKicking();
            }

            CheckForHit();
        }

        private void StopKicking()
        {
            _isKicking = false;
            _anim.SetBool("isKicking", false);
            _controller.input.KickFinished();
        }

        private void CheckForHit()
        {
            Collider2D[] hits;

            if (_KickType == KickType.Standard)
                hits = Physics2D.OverlapBoxAll(_hitCheckOriginStandardKick.position, _hitBoxSizeStandardKick, 0f, _whatIsEnemy);
            else if (_KickType == KickType.Air)
                hits = Physics2D.OverlapBoxAll(_hitCheckOriginAirKick.position, _hitBoxSizeAirKick, 0f, _whatIsEnemy);
            else
                hits = Physics2D.OverlapBoxAll(_hitCheckOriginDashKick.position, _hitBoxSizeDashKick, 0f, _whatIsEnemy);

            if (hits.Length > 0)
            {
                foreach (Collider2D c in hits)
                {
                    print("Collided with " + c.name);
                }
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            if (_showStandardKickGizmo)
                Gizmos.DrawWireCube(_hitCheckOriginStandardKick.position, new Vector3(_hitBoxSizeStandardKick.x, _hitBoxSizeStandardKick.y, 1f));
            if (_showAirKickGizmo)
                Gizmos.DrawWireCube(_hitCheckOriginAirKick.position, new Vector3(_hitBoxSizeAirKick.x, _hitBoxSizeAirKick.y, 1f));
            if (_showDashKickGizmo)
                Gizmos.DrawWireCube(_hitCheckOriginDashKick.position, new Vector3(_hitBoxSizeDashKick.x, _hitBoxSizeDashKick.y, 1f));
        }
    }

    public enum KickType
    {
        Standard,
        Dash,
        Air
    }
}
