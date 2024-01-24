using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StickFight
{
    public class Punch : MonoBehaviour
    {
        [SerializeField] private float _punchDuration = 0.1f;
        private float _currentDuration;

        private Animator _anim;
        private Controller _controller;

        private bool _isPunching, _isDashPunching, _isPunchingInput, _isDashingInput, _isDashingMutedInput, _isPunchingMutedInput;

        private void Awake()
        {
            _anim = GetComponent<Animator>();
            _controller = GetComponent<Controller>();
        }

        private void Update()
        {
            _isPunchingInput = _controller.input.RetrievePunchInput(false);
            _isDashingInput = _controller.input.RetrieveDashInput(false);
            _isDashingMutedInput = _controller.input.RetrieveDashInput(true);
            _isPunchingMutedInput = _controller.input.RetrievePunchInput(true);

            if ((!_isPunching && !_isDashPunching) && _isPunchingInput && (!_isDashingInput && !_isDashingMutedInput))
            {
                print("standard punch");
                StartStandardPunch();
            }
            else if((!_isPunching && !_isDashPunching) && _isPunchingMutedInput && _isDashingInput)
            {
                print("dash punch");
                StartDashPunch();
            }

            if(_isPunching || _isDashPunching)
                UpdatePunching();
        }

        private void StartStandardPunch()
        {
            _isPunching = true;
            _isDashPunching = false;

            _currentDuration = 0f;

            _anim.SetBool("isPunching", true);
        }

        private void StartDashPunch()
        {
            _isPunching = false;
            _isDashPunching = true;

            _anim.SetBool("isPunching", true);
        }

        private void UpdatePunching()
        {
            if (_isPunching)
            {
                _currentDuration += Time.deltaTime;
                if (_currentDuration >= _punchDuration)
                    StopPunching();
            }

            if (_isDashPunching)
            {
                if (!_isDashingInput)
                    StopPunching();
            }
        }

        private void StopPunching()
        {
            _isPunching = false;
            _isDashPunching = false;
            _anim.SetBool("isPunching", false);
            _controller.input.PunchFinished();
        }
    }
}
