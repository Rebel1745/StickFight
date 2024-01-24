using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StickFight
{
    public class Kick : MonoBehaviour
    {
        [SerializeField] private float _KickDuration = 0.1f;
        private float _currentDuration;

        private Animator _anim;
        private Controller _controller;

        private bool _isKicking, _isDashKicking, _isKickingInput, _isDashingInput, _isDashingMutedInput, _isKickingMutedInput;

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

            if ((!_isKicking && !_isDashKicking) && _isKickingInput && (!_isDashingInput && !_isDashingMutedInput))
            {
                print("standard Kick");
                StartStandardKick();
            }
            else if ((!_isKicking && !_isDashKicking) && _isKickingMutedInput && _isDashingInput)
            {
                print("dash Kick");
                StartDashKick();
            }

            if (_isKicking || _isDashKicking)
                UpdateKicking();
        }

        private void StartStandardKick()
        {
            _isKicking = true;
            _isDashKicking = false;

            _currentDuration = 0f;

            _anim.SetBool("isKicking", true);
        }

        private void StartDashKick()
        {
            _isKicking = false;
            _isDashKicking = true;

            _anim.SetBool("isKicking", true);
        }

        private void UpdateKicking()
        {
            if (_isKicking)
            {
                _currentDuration += Time.deltaTime;
                if (_currentDuration >= _KickDuration)
                    StopKicking();
            }

            if (_isDashKicking)
            {
                if (!_isDashingInput)
                    StopKicking();
            }
        }

        private void StopKicking()
        {
            _isKicking = false;
            _isDashKicking = false;
            _anim.SetBool("isKicking", false);
            _controller.input.KickFinished();
        }
    }
}
